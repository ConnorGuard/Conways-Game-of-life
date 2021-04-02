using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
/// <author > Connor Guard N10690352 </ author >
/// <date > 19th September 2020 </ date >
namespace Life
{
    /// <summary >
    /// Generates a random seed
    /// </ summary >
    /// <returns > the random seed as an integer matrix of 1s and 0s</ returns >
    class randomSeed : seedReader
    {
        private Random rnd;
        private double RandomFactor;
        public randomSeed(int rows, int cols, double RandomFactor) : base(rows, cols)
        {
            this.RandomFactor = RandomFactor;
        }

        //overrides the update cells method, with this pseudorandom seed generator
        public override int[,] updateCells(int[,] cells)
        {
            rnd = new Random();
            //select each cell indvidually
            for (int r = 0; r < cells.GetLength(0); r++)
            {
                for (int c = 0; c < cells.GetLength(1); c++)
                {
                    //flip a 1000 faced die
                    int dice = rnd.Next(0, 1000);
                    //is the cell alive or dead?
                    cells[r, c] = ((double)dice / 1000 <= RandomFactor) ? 1 : 0;
                }
            }       
            return cells;
        }
    }
}
