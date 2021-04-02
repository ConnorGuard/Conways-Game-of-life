using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
/// <author > Connor Guard N10690352 </ author >
/// <date > 19th September 2020 </ date >
namespace Life
{
    abstract class options
    {
        //Declares all variables for the simulation
        public Boolean stepMode { get; set; }
        public Boolean periodicBehavior { get; set; }
        public Boolean neighbourType { get; set; }
        public Boolean centreCount { get; set; }
        public Boolean ghostMode { get; set; }
        private Double randomFactor;
        private Double updateRate;
        private int generations;
        private int rows;
        private int columns;
        private String seed;
        private String output;
        private int order;
        private List<int> survival;
        private List<int> birth;
        private int memory;
        public List<int[,]> previousGenerations;
        //constants
        const int spaceValue = 32;
        const int alive = 1;
        const int dead = 0;

        // validation
        validation validate = new validation();

        // public Get
        public int getRows
        {
            get
            {
                return this.rows;
            }
        }
        public int getColumns
        {
            get
            {
                return this.columns;
            }
        }

        public string getSeedPath {
            get
            {
                return this.seed;
            }
        }
        public double getRandomFactor
        {
            get
            {
                return this.randomFactor;
            }
        }
        public double getMaxUpdate {
            get
            {
                return this.updateRate;
            }
        }
        public int getGenerations {
            get
            {
                return this.generations;
            }
        }
        public int getOrder
        {
            get
            {
                return this.order;
            }
        }
        public List<int> getSurvival
        {
            get
            {
                return this.survival;
            }
        }

        public List<int> getBirth 
        {
            get
            {
                return this.birth;
            }
        }
        public int getMemory
        {
            get
            {
                return this.memory;
            }
        }
        public string getOutputPath
        {
            get
            {
                return this.output;
            }
        }
        /// <summary >
        /// initialises the simulations variables with the default settings
        /// </ summary >
        public options()
        {
            //Default simulation settings
            stepMode = false;
            periodicBehavior = false;
            randomFactor = 0.5;
            updateRate = 5.0;
            generations = 50;
            rows = 16;
            columns = 16;
            seed = String.Empty;
            neighbourType = false;
            order = 1;
            centreCount = false;
            survival = new List<int>();
            survival.Add(2);
            survival.Add(3);
            birth = new List<int>();
            birth.Add(3);
            memory = 16;
            previousGenerations = new List<int[,]>();
            output = String.Empty;
            ghostMode = false;
        }

        //always reads options in this order
        private string[] Options { get;} = {
            "--dimensions",
            "--periodic",
            "--random",  
            "--seed", 
            "--generations", 
            "--max-update", 
            "--step", 
            "--neighbour", 
            "--survival",
            "--birth",
            "--memory",
            "--output",
            "--ghost"
        };


        /// <summary >
        /// Updates the game settings with user defined parameters.
        /// Iterates through the settings array and sets each one in order, 
        /// by comparing it to the arguments array.
        /// Uses an exception to catch badly formatted parameters and 
        /// The validation class to validate parameters.
        /// </ summary >
        /// <param name =" args " >String arguments that initialise the Game of Life settings</ param >
        public void set(string[] args)
        {
            int pos=-1;
            int parameterInt;
            double parameterDouble;
            foreach (string op in Options) //Sets each setting in the pre-defined order.
            { 
                pos = Array.IndexOf(args, op);
                if (pos > -1) {
                    try
                    {
                        switch (op)
                        {
                            case "--dimensions"://sets the dimensions of the universe...                     
                                parameterInt = int.Parse(args[pos + 2]);
                                if (validate.dimensions(parameterInt)) columns = parameterInt;
                                parameterInt = int.Parse(args[pos + 1]);
                                if (validate.dimensions(parameterInt)) rows = parameterInt;                     
                                break;
                            case "--periodic": //turn on periodic mode
                                periodicBehavior = true;
                                break;
                            case "--random"://sets the likelyhood that a cell is born initially...
                                parameterDouble = double.Parse(args[pos + 1]);
                                if (validate.randomFactor(parameterDouble))  randomFactor = parameterDouble;
                                break;
                            case "--seed"://sets the path to .seed file
                                if(validate.seed(args[pos + 1])) seed = args[pos + 1];
                                break;
                            case "--generations"://sets the maximum number of generations...  
                                parameterInt = int.Parse(args[pos + 1]);
                                if(validate.generations(parameterInt)) generations = parameterInt;
                                break;
                            case "--max-update"://sets the maximum rate of generations per second...             
                                parameterDouble = double.Parse(args[pos + 1]);
                                if (validate.updateRate(parameterDouble)) updateRate = parameterDouble;
                                break;
                            case "--step"://turn on step mode
                                stepMode = true;
                                break;
                            case "--neighbour": //sets neighbourhood type, order and centre cell switch
                                readNeighbour(pos, args);
                                break;
                            case "--survival": //sets number of cells for survival
                                readSB(pos, args, survival);
                                break;
                            case "--birth": //sets the number of cells for birth
                                readSB(pos, args, birth);
                                break;
                            case "--memory"://Sets memory size   
                                parameterInt = int.Parse(args[pos + 1]);
                                if(validate.memory(parameterInt)) memory = parameterInt;
                                break;
                            case "--output"://sets the path to output .seed file
                                if (validate.seed(args[pos + 1])) output = args[pos + 1];
                                break;
                            case "--ghost"://sets the path to output .seed file
                                ghostMode = true;
                                break;
                        }
                    }
                    catch (Exception) //catches badly formatted parameters
                    {
                        validate.invalid(args[pos] + " has an invalid parameter");
                    }
                }
            }
            if (validate.success) //when all arguments are read correctly
            {
                Console.ForegroundColor = ConsoleColor.Green;
                centerText("Successfully read arguments");
            }                
        }


        /// <summary >
        /// Sets the neighbour option, Which updates three variables: neighbourType, Order and centreCount.
        /// Validation is done inside this method because it is handled diffrently to other options.
        /// </ summary >
        /// <param name =" args " >String arguments that initialise the Game of Life settings</ param >
        /// /// <param name =" pos " >The position of the --neighbour option within the args array</ param >
        private void readNeighbour(int pos, string[] args) {
            //moore or vonNeumann
            if (args[pos + 1] == "moore")
            {
                neighbourType = false;
            }
            else if (args[pos + 1] == "vonNeumann")
            {
                neighbourType = true;
            }
            else
            {
                validate.invalid(args[pos + 1] + " is an invalid option");
            }
                
            if(validate.order(int.Parse(args[pos + 2]))) order = int.Parse(args[pos + 2]); //neighbourhood size 

            //count the centre?
            if (args[pos + 3] == "true")
            {
                centreCount = true;
            }
            else if (args[pos + 3] == "false")
            {
                centreCount = false;
            }
            else
            {
                validate.invalid(args[pos + 3] + " is an invalid option");
            }
        }

        /// <summary >
        /// Sets the Survival and birth values, These have identical validation and formatting so I combined them into this method
        /// You can set individual parameters or a range of values using an elipses.
        /// </ summary >
        /// <param name =" args " >String arguments that initialise the Game of Life settings</ param >
        /// <param name =" pos " >The position of the --survival or --birth option within the args array</ param >
        /// <param name =" SB " >survival or birth list address</ param >
        private void readSB(int pos, string[] args, List <int> SB)
        {
            bool invalid = false;
            try
            {
                for (int a = pos + 1; !args[a].Contains("--"); a++)     // for all parameters
                {
                    if (a == pos + 1) SB.Clear(); //initalise setting new values

                    if (args[a].Contains("..."))    // set range
                    {
                        string[] range = args[a].Split("...");

                        if (range.Length == 2 && int.Parse(range[0]) < int.Parse(range[1]))
                        { //valid format?
                            for (int i = int.Parse(range[0]); i <= int.Parse(range[1]); i++)
                            { // set parameters within range                            
                                if (validate.SB(i, neighbourhoodSize()) && !SB.Contains(i)) SB.Add(i);
                                else invalid = true;
                            }
                        }
                        else
                        {
                            validate.invalid(args[a] + " Invalid format");
                        }
                    }
                    else // set birth or survival parameter
                    {
                        if (validate.SB(int.Parse(args[a]), neighbourhoodSize()) && !SB.Contains(int.Parse(args[a]))) SB.Add(int.Parse(args[a]));                      
                        else invalid = true;
                    }
                }
            }
            catch (Exception)
            {
                //reached the end of arguments
            }
            finally {
                if (invalid) validate.invalid("Invalid [--survival or --birth] value, must be 0 < [value] < " + neighbourhoodSize());
                SB.Sort(); //sort               
            }
            return;
        }



        /// <summary >
        /// prints the settings of the simulation to the console window.
        /// </ summary >
        public void printSettings()
        {
            string status;
            centerText("");
            centerText("Game Of Life Settings");
            centerText("");
            status = (stepMode) ? "ON" : "OFF";
            centerText("Step mode: ", status);
            centerText("Random factor: ", String.Format("{0:P2}", getRandomFactor));
            status = (periodicBehavior) ? "ON" : "OFF";
            centerText("Periodic behavior: ", status);
            centerText("Update Rate: ", getMaxUpdate + " per/second");
            centerText("Generations: ", getGenerations.ToString());
            centerText("Dimensions: ", rows + " X " + columns);
            status = (getSeedPath == string.Empty) ? "Random" : getSeedPath;
            centerText("Seed path: ", status);
            status = (getOutputPath == string.Empty) ? "N/A" : getOutputPath;
            centerText("Output path: ", status);
            status = (neighbourType) ? "vonNeumann" : "moore";
            centerText("Neighbourhood Type: ", status + " " + getOrder.ToString() + " "+ centreCount);
            centerText(" Neighbours: ", neighbourhoodSize().ToString());
            status = String.Empty;
            foreach (int s in getSurvival) status += s.ToString()+" ";
            centerText("Survival: ", status);
            status = String.Empty;
            foreach (int s in getBirth) status += s.ToString() + " ";
            centerText("Birth: ", status);
            centerText("Memory: ", getMemory.ToString());
            status = (ghostMode) ? "ON" : "OFF";
            centerText("Ghost mode: ", status);
            centerText("");
        }

        //Prints text with centre format, also handles parameters format and colours
        private static void centerText(String option, String parameter = "")
        {          
            int align = Console.WindowWidth / 2; //centre alignment of console              
            align += (parameter == string.Empty) ? option.Length / 2 : 0; //if the text has no parameter, then ajust alignment               
            string format = "{0," + align.ToString() + "}"; //initialise alignment        
            Console.Write(String.Format(format, option)); //print option      
            Console.ForegroundColor = (parameter == "OFF" || parameter == "N/A" || parameter == string.Empty) ? ConsoleColor.Red : ConsoleColor.Green;
            Console.WriteLine(parameter); //print parameter
            Console.ForegroundColor = ConsoleColor.White;
        }


        /// <summary >
        /// calculates number of neigbours (includes centre cell)
        /// uses the nth term, "Order" being n
        /// moore = (2n+1)^2    
        /// vonNeumann = (moore + 1) / 2;
        /// </ summary >
        private double neighbourhoodSize() {
            double moore = Math.Pow(2 * getOrder + 1, 2);
            double vonNeumann = (moore + 1) / 2;
            return (neighbourType) ? vonNeumann : moore;
        }
    }
} 
