using System;
using System.Collections.Generic;
using System.Text;
/// <author > Connor Guard N10690352 </ author >
/// <date > 19th September 2020 </ date >
/// <summary >
/// Gets the version1 .seed file and puts it into a usable format.
/// </ summary >
/// <returns >returns the seed as an integer matrix</ returns >
namespace Life
{
    class version01 : seedReader
    {
        string path;
        String[] textArray;
        bool outBounds;
        validation validate;
        public version01(int row, int col, string seed ) : base(row, col) {
            path = System.IO.File.ReadAllText(seed);
            textArray = path.Split("\r\n");
            validate = new validation();
        }

        /// <summary >
        /// read seed into the array
        /// The seed specifies the location of each alive cell
        /// updateCells interprets the .seed file and formats it into an array that can be manipulated
        /// </ summary >
        ///  /// <param name =" cells " > this is dynamic as parameters have diffrent data types</ param >
        public override int[,] updateCells(int[,] cells)
        {
            outBounds = false;
            try
            {
                for (int i = 1; i < textArray.Length - 1; i++)
                {                    
                    cells[Int32.Parse(textArray[i].Split(" ")[0]), Int32.Parse(textArray[i].Split(" ")[1])] = 1;
                }
            }
            catch (Exception)
            {
                outBounds = true;// out of bound cells.
            }
            if (outBounds) validate.invalid("WARNING: Some seed cells may be out of bounds");
            return cells;
        }
    }
}
