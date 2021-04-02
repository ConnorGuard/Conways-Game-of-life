using System;
using System.Collections.Generic;
using System.Text;
/// <author > Connor Guard N10690352 </ author >
/// <date > 19th September 2020 </ date >
namespace Life
{
    class version02 : seedReader
    {
        string path;
        String[] readLine;
        int brush;
        bool outBounds;
        validation validate;
        public version02(int row, int col, string seed) : base(row, col)
        {
            //The seed specifies the location of alive cells
            path = System.IO.File.ReadAllText(seed);
            readLine = path.Split("\r\n");
            brush = 1;
            validate = new validation();
        }
        /// <summary >
        /// updatecells() interprets the version2 .seed files.
        /// The seed file specifies 3 shapes: cell, rectangle and ellipse.
        /// The seed file can paint alive or dead cells.
        /// </ summary >
        /// <returns > the version2 seed as an integer matrix of 1s and 0s</ returns >
        public override int[,] updateCells(int[,] cells)
        {
            outBounds = false;
            try
            {
                for (int i = 1; i < readLine.Length; i++)
                {
                    if(readLine[i].Contains("(o)")) brush = 1;
                    else if (readLine[i].Contains("(x)")) brush = 0;

                    if (readLine[i].Contains("cell")) cell(ref cells, readLine[i].Split(" "), brush);
                    else if (readLine[i].Contains("rectangle")) rectangle(ref cells, readLine[i].Split(" "), brush);
                    else if (readLine[i].Contains("ellipse")) ellipse(ref cells, readLine[i].Split(" "), brush);
                }
            }
            catch (Exception)
            {
                outBounds = true; //out of bound cells.
            }
            if (outBounds) validate.invalid("WARNING: Some seed cells may be out of bounds");
            return cells;
        }

        /// <summary >
        /// draws individual cells alive/dead onto the cells matrix
        /// </ summary >
        /// <param name =" cells " > This is by refrence, as I want the changes to apply inside the parent method</ param >
        /// <param name =" location " > location specified by the .seed file to draw the cell</ param >
        /// <param name =" brush " > specifies weather we draw an alive or dead cell</ param >
        private void cell(ref int[,] cells, string[] location, int brush) 
        {             
            int row = int.Parse(location[2].Replace(",", string.Empty));
            int col = int.Parse(location[3]);
            cells[row, col] = brush;
            return;
        }
        /// <summary >
        /// draws a rectangle using 2 coordinate values, 
        /// The values represent the bottom left corner and the top right corner
        /// </ summary >
        /// <param name =" cells " > This is by refrence, as I want the changes to apply inside the parent method</ param >
        /// <param name =" location " > location specified by the .seed file, for where to draw the rectangle</ param >
        /// <param name =" brush " > specifies weather we draw an alive or dead cell</ param >
        private void rectangle(ref int[,] cells, string[] location, int brush)
        {
            int blr = int.Parse(location[3].Replace(",", string.Empty)); //bottom left row
            int blc = int.Parse(location[4].Replace(",", string.Empty)); //bottom left column
            int trr = int.Parse(location[5].Replace(",", string.Empty)); // top right row
            int trc = int.Parse(location[6]); //top right column

            for (int r = blr; r <= trr; r++)
            {
                for (int c = blc ; c <= trc ; c++)
                {
                    cells[r, c] = brush; // draws the filled in rectangle, based on brush value
                }
            }
            return;
        }
        /// <summary >
        /// draws an ellipse based on 2 coordinate values, the top right corner and the bottom left corner.
        /// </ summary >
        /// <param name =" cells " > This is by refrence, as I want the changes to apply inside the parent method</ param >
        /// <param name =" location " > location specified by the .seed file to draw the ellipse</ param >
        /// <param name =" brush " > specifies weather we draw an alive or dead cell</ param >
        private void ellipse(ref int[,] cells, string[] location, int brush)
        { 
            int blr = int.Parse(location[3].Replace(",", string.Empty)); //bottom left row
            int blc = int.Parse(location[4].Replace(",", string.Empty)); //bottom left column
            int trr = int.Parse(location[5].Replace(",", string.Empty)); //top right row 
            int trc = int.Parse(location[6]); //top right column

            int width = (trc+1) - blc; // gets width
            int height = (trr+1) - blr; //gets height

            double centreC = blc + (width / 2.0); //gets the centre column
            double centreR = blr + (height / 2.0); //gets the centre row

            for (int r = blr; r <= trr; r++)
            {
                for (int c = blc; c <= trc; c++)
                {
                    // calculates if the cell is in or out of the ellipse
                    double checkC = 4 * Math.Pow(c + 0.5 - centreC, 2) / Math.Pow(width, 2); 
                    double checkR = 4 * Math.Pow(r + 0.5 - centreR, 2) / Math.Pow(height, 2);
                    if (checkC + checkR <= 1) cells[r, c] = brush; //draws ellipse cells                         
                }
            }
            return;
        }

    }
}
