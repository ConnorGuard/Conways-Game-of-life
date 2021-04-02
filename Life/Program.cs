using System;
using System.Diagnostics;
using Display;
using System.IO;
/// <author > Connor Guard N10690352 </ author >
/// <date > 19th September 2020 </ date >

namespace Life
{ 
    class Program
    {
        /// <summary >
        /// The main body program that organises the program flow
        /// </ summary >
        /// <param name =" args " >String arguments that initialise the Game of Life settings</ param >
        static void Main(string[] args)
        {           
            Universe u = new Universe(); // start a new universe : inherited from options

            if (args.Length > 0) u.set(args); //update custom settings
            else centerText("Default");

            Grid grid = new Grid(u.getRows, u.getColumns); // Construct grid...

            u.setSeed(); //Initilaises the matrix, checks for out of bound cells
           
            Console.ForegroundColor = ConsoleColor.White; //initialise console colour
            
            u.printSettings(); //print settings
          
            centerText("Press the space bar to start...\t"); // Wait for user to press the space key...
            u.space();
            
            grid.InitializeWindow(); // Initialize the grid window (this will resize the window and buffer)

            grid.SetFootnote("Generation: 0"); // Set the footnote(appears in the bottom left of the screen).
          
            int period = u.start(grid); //Start Simulation
       
            grid.IsComplete = true; // Set complete marker as true
         
            grid.Render(); // Render updates to the console window...

            u.space();
          
            grid.RevertWindow(); // Revert grid window size and buffer to normal

            //print stead state status
            if (period > -1) 
            {
                Console.WriteLine("Steady state reached");
                if (period > 1) Console.WriteLine("Period: " + period); //print period if > 1  
            }
            else Console.WriteLine("Steady state NOT reached"); 
            
            u.space();
        }
         
        /// <summary >
        /// Centres text on the screen
        /// </ summary >
        /// /// <param name =" text " > The text being centred </ param >
        private static void centerText(String text)
        {          
            int align = (Console.WindowWidth + text.Length) / 2; //centre alignment of console
            string format = "{0," + align.ToString() + "}"; //initialise alignment
            Console.WriteLine(String.Format(format, text)); //print
        }

        //end of program
    }
}
