using Display;
using System;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.IO;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Linq;
using System.Xml;

/// <summary >
/// This is the simulation class, used build the game of life.
/// This poject is an interpretation of Conway's Game Of Life.
/// </ summary >
/// <author > Connor Guard N10690352 </ author >
/// <date > 19th September 2020 </ date >

namespace Life
{ 
    //simulation object
    class Universe : options
    {
        //constants
        const int spaceValue = 32;
        const int alive = 1;
        const int dead = 0;
        int period =-1;
        private int[,] cells;
        /// <summary >
        /// Starts the simulation by running the main loop of the program
        /// This method calls other methods to collect the information required to use the Display API.
        /// </ summary >
        /// <param name =" grid " > Used to display the Cell matrix in the console </ param >
        public int start(Grid grid) {
            Stopwatch watch = new Stopwatch(); //initialise time  
            CellState[,] state = new CellState[getRows,getColumns];//set state for each cell
            CellState[] ghostState = {CellState.Dark, CellState.Medium, CellState.Light};   //ghost states

            for (int g = 0; g <= getGenerations; g++)   //loop per generation
            {
                watch.Restart();       
                
                for (int r = 0; r < getRows; r++) // For each of the cells...
                {
                    for (int c = 0; c < getColumns; c++)
                    {
                        state[r, c] = CellState.Blank;  //clear cell state

                        int ghosts = (previousGenerations.Count > 2) ? 2 : previousGenerations.Count-1; //ghost count

                        ghosts = (ghostMode) ? ghosts : -1; //turn on : off ghost mode

                        for (int gh = ghosts; gh >= 0; gh--)
                        {
                            state[r, c] = (previousGenerations[gh][r,c] == alive) ? ghostState[gh] : state[r, c]; //set ghosts
                        }

                        state[r,c] = (cells[r, c] == alive) ? CellState.Full : state[r, c]; //get new generation states  
                        
                        grid.UpdateCell(r, c, state[r, c]); // Update grid                                                         
                    }
                }

                while (watch.ElapsedMilliseconds < (1 / getMaxUpdate * 1000)); //Timer

                grid.SetFootnote("Generation: " + g); //prints current generation

                grid.Render(); // Render updates to the console window...

                period = steadyState(cells); //get period

                if (period > -1) { //steady state? End game if true
                    if(getOutputPath != String.Empty) outputSeed(cells); //output seed
                    return period;
                } 

                cells = this.mutation(cells); //MUTATION, following the Conways game of life...

                if (stepMode) space(); //Step Mode
            }

            if (getOutputPath != String.Empty) outputSeed(cells); //output seed
            return period;
        }

        /// <summary >
        /// Gets the first generation of cells, either a random seed, a version 1 seed or version 2 seed.
        /// Does not require a paramter or return a value, it only initalises the cell martrix, for the first generation .
        /// </ summary >
        public void setSeed() {
            string path;
            string[] readLine;
            path = (getSeedPath != string.Empty) ? File.ReadAllText(getSeedPath) : "Random";            
            readLine = path.Split("\r\n");

            if (readLine[0] == "#version=1.0")
            {
                version01 SR = new version01(getRows, getColumns, getSeedPath);
                cells = SR.getseed();
            }
            else if (readLine[0] == "#version=2.0")
            {
                version02 SR = new version02(getRows, getColumns, getSeedPath);
                cells = SR.getseed();
            }
            else
            {
                randomSeed SR = new randomSeed(getRows, getColumns, getRandomFactor);
                cells = SR.getseed();
            }
            return;
        }

        /// <summary >
        /// Applies the game of life rules to the cells on each generation.
        /// This method is called once every generation.
        /// </ summary >
        /// <param name =" cells " > The current generation of cells</ param >
        /// // <returns > This method returns the new mutated generation of cells </ returns >
        private int[,] mutation(int[,] cells)
        {
            //initialise new generation
            int[,] nextGeneration = new int[getRows, getColumns];
            int aliveNeighbours;

            for (int r = 0; r < getRows; r++) //loop through every cell
            {
                for (int c = 0; c < getColumns; c++)
                {
                    aliveNeighbours = 0;                                       
                    aliveNeighbours = getNeighbours(cells, r, c, aliveNeighbours); //count alive neighbours for a cell                                           
                    aliveNeighbours += (centreCount) ? cells[r, c] : 0; //is centre count on?                     
                    nextGeneration[r, c] = getSurvival.Contains(aliveNeighbours) ? cells[r,c] : dead; //Survival
                    nextGeneration[r, c] = getBirth.Contains(aliveNeighbours) ? alive : nextGeneration[r, c];  //birth          
                }
            }
            return nextGeneration;
        }

        /// <summary >
        ///This method counts the number of alive cells in a neighbourhood, for a specific cell.
        ///The specific cell is defined by r and c, these give the position wthin the cells, array.
        ///aliveNeighbours is the accumalated value of alive cells counted within a neigbouhood.
        /// </ summary >
        /// <param name =" cells " > The current generation of cells</ param >
        /// <param name =" r " > row location</ param >
        /// <param name =" c " > column location</ param >
        /// <param name =" aliveNeighbours " > accumalated value of alive cells </ param >
        /// // <returns > This method returns the number of alive cells </ returns >
        private int getNeighbours(int[,] cells, int r, int c, int aliveNeighbours)
        {
            //counts up alive cells
            for (int i = -getOrder; i <= getOrder; i++)
            {
                for (int j = -getOrder; j <= getOrder; j++)
                {                
                    if (Vonnuemann(i,j))  //this applies Vonnuemann rule when turned ON
                    {

                        //if cell is out of bounds
                        if (r + i < 0 || r + i > getRows - 1 || c + j < 0 || c + j > getColumns - 1)
                        {
                            //"out of bound" cells will be counted, if periodic bahaviour is on, else they are ignored
                            aliveNeighbours += (periodicBehavior) ? cells[(r + i + getRows) % getRows, (c + j + getColumns) % getColumns] : 0;
                        }
                        else
                        {
                            //if cell is in bounds, count neighbour
                            aliveNeighbours += cells[r + i, c + j];
                        }
                    }
                }
            }                 
            return aliveNeighbours -= cells[r, c]; //delete centre cell
        }

        /// <summary >
        /// Applies the vonnuemann neighbourhood rule, when turned on
        /// calculates if the cell[r,c] is within the bounds of the vonnuemann's rule.
        /// </ summary >
        /// <param name =" r " > row location</ param >
        /// /// <param name =" c " > column location</ param >
        /// // <returns > boolean value that states if its in or out </ returns >
        private bool Vonnuemann( int r, int c) {
            return (Math.Abs(r) + Math.Abs(c) <= getOrder) || !neighbourType;
        }

        /// <summary >
        /// Based on the number of previous generations saved in the List, defined by the variable "memory"
        /// Compares the latest generation, to the list. If there is a match, the game will stop and return the period.
        /// </ summary >
        /// <param name =" cells" > The latest generation being displayed</ param >
        /// // <returns > return the period when it finds a match, otherwise returns -1 </ returns >
        private int steadyState(int[,] cells) {
            bool match;

            for (int g = 0; g < previousGenerations.Count; g++) //looking for a match
            {
                match = true;
                for (int r = 0; r < getRows; r++)
                    for (int c = 0; c < getColumns; c++)
                        if (previousGenerations[g][r,c] != cells[r, c]) match = false; ; ;   //compares each cell
                
                if (match) return g+1;//match found
            }
            
            previousGenerations.Insert(0, cells); //insert at the top of the list
           
            if (previousGenerations.Count > getMemory)  //remove item if memory is maxed
            {
                previousGenerations.Remove(previousGenerations[getMemory]);
            }   
            
            return -1;
        }

        /// <summary >
        /// outputSeed method used to output the last generation to a .seed file
        /// </ summary >
        /// <param name =" cells" > The last generation of cells being displayed</ param >
        private void outputSeed(int[,] cells) {
            string seed = "#version=2.0" + Environment.NewLine;
            for (int r = 0; r < getRows; r++) //loop through every cell
            {
                for (int c = 0; c < getColumns; c++)
                {
                    if (cells[r, c] == 1) seed += "(o) cell: " + r.ToString() + ", " + c.ToString() + Environment.NewLine; //only counts alive cells                              
                }
            }
            File.WriteAllText(getOutputPath, seed); //wite to .seed file
        }

        //used to pause the program and wait for a space key press
        public void space()
        {
            // Wait for user to press the space key...
            while (true)
            {
                //checks key is a space
                var character = Console.ReadKey().KeyChar;
                if ((int)character == spaceValue)
                    break;
            }
        }
        //END OF CLASS
    }
}
