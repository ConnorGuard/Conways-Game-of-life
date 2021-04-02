using Display;
using System;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.IO;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
/// <author > Connor Guard N10690352 </ author >
/// <date > 19th September 2020 </ date >
namespace Life
{
    /// <summary >
    /// Version1, version2 and randomseed inherit from this class.
    /// All three return a 2 dimentional integer array named cells.
    /// They each have a diffrent mechanism for returning a seed and use,
    /// updateCells as an override.
    /// </ summary >
    public abstract class seedReader
    {
        public int[,] cells;
        public seedReader(int rows, int col)
        {           
            cells = new int[rows, col]; // create new cells
        }

        //used to get a new seed, returns modified cells
        public virtual int[,] getseed() {
            updateCells(cells);
            return cells;
        }
        public abstract int[,] updateCells(int[,] cells);
    }
}
