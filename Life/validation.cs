using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

/// <author > Connor Guard N10690352 </ author >
/// <date > 19th September 2020 </ date >
namespace Life
{
    class validation 
    {
        public bool success = true;
        //validation for rows and Columns, must be an integer between 4 and 48 inclusive
        public bool dimensions(int dim) {
            return (dim >= 4 && dim <= 48) ? true : 
                invalid("[--dimensions] " + dim + " must be an integer between 4 and 48.");
        }
        //validation for --seed and --output paths, Must be a valid path to a .seed file.
        public bool seed(string seed)
        {
            return (System.IO.File.Exists(seed) && (seed).Contains(".seed")) ? true : 
                invalid( seed + " must be a valid .seed file path.");
        }
        //validation for the random factor value, must be a float between 0 and 1 inclusive.
        public bool randomFactor(double randomFactor)
        {
            return (randomFactor <= 1 && randomFactor >= 0) ? true :
                invalid("[--random] " + randomFactor + " must be a float between 1 and 0");
        }
        //validation for the update rate, must be a float between 1 and 30.
        public bool updateRate(double updateRate)
        {
            return (updateRate >= 1.0 && updateRate <= 30.0) ? true :
                invalid("[--max-update] " + updateRate + " must be a float between 1 and 30");
        }
        //validation for --generations, must be a positive integer.
        public bool generations(double generations)
        {
            return (generations > 0) ? true :
                invalid("[--generations] " + generations + " must be a positive integer");
        }
        //validation for order, must be an integer between 1 and 10.
        public bool order (int order)
        {
            return (order >= 1 && order <= 10) ? true :
                invalid("[--Order] " + order + " must be an integer between 1 and 10.");
        }
        //validation for memory, must be an integer between 4 and 512 inclusive.
        public bool memory(int memory)
        {
            return (memory <= 512 && memory >= 4) ? true :
                invalid("[--memory] "+memory + " must be: 3 < memory < 513 ");
        }
        //validation for --survival and --birth, must be a positive integer,    less than the neighbourhood size.
        public bool SB(int sb, double nSize)
        {
            return (sb > 0 && sb < nSize) ? true : false;             
        }

        /// <summary >
        /// In the Set() method, parameters are attempted to be set
        /// if the parameter is Invalid,this method is called to display an error message.
        /// </ summary >
        /// <param name =" Error " > this is dynamic as parameters have diffrent data types</ param >
        public bool invalid(String msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            success = false;
            return success;
        }
    }
}
