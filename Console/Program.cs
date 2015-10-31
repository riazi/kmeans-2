using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Console
{
    class Program
    {
        private const int sigma = 1;
        private const int k = 4;
        private const bool predef = true;
        private const int dimension = 8;

        [STAThread]
        static void Main(string[] args) // Entry-Point
        {
            #region Get Data Location
            OpenFileDialog foo = new OpenFileDialog();
            string route = "";
            if (foo.ShowDialog() == DialogResult.OK)
            {
                route = foo.FileName;
            }
            #endregion

            #region Parse Data
            string[] predata = File.ReadAllLines(route);
            int[,] data = new int[predata.Length, dimension];
            for (int i = 0; i < predata.Length; i++)
            {
                string[] bar = predata[i].Split(',');
                for (int j = 0; j < dimension; j++)
                {
                    data[i, j] = Int32.Parse(bar[j]); // THE ARRAY OF THE DATA
                }
            }
            #endregion

            #region Get Centroids
            decimal[,] centroids = new decimal[k, 8];
            if (!predef)
            {                
                for (int ii = 0; ii < k; ii++) // RANDOM Centroids
                {
                    Random bar = new Random();
                    int rand = bar.Next(0, data.GetUpperBound(0) + 1);
                    copyArrayRow(data, ref centroids, rand, ii);
                }
            }
            else
            {
                for (int jj = 0; jj < k; jj++) // Picking centroids for the exercise
                {
                    copyArrayRow(data, ref centroids, jj, jj);
                }
            }
            #endregion

            #region Assign To Centroids
            int[] groups = new int[data.GetUpperBound(0) + 1]; // Store the groups
            for (int kk = 0; kk < data.GetUpperBound(0) + 1; kk++) // Navigate through all the points
            {
                decimal dist = Decimal.MaxValue;
                int group = 0;
                for (int iii = 0; iii < k; iii++) // Navigate through all the centroids
                {
                    decimal qux = eucDist(data, centroids, kk, iii);
                    if (qux < dist) // Get the lower distance
                    {
                        dist = qux;
                        group = iii;
                    }
                }
                groups[kk] = group; 
            }
            #endregion
        }

        struct Point
        {
            public decimal x;
            public decimal y;
        }
        static void copyArrayRow(int[,] arrayInput, ref decimal[,] arrayOutput, int inRow, int outRow)
        {
            for (int i = 0; i < 8; i++)
            {
                arrayOutput[outRow, i] = arrayInput[inRow, i];
            }              
        }
        static decimal eucDist(int[,] p1, decimal[,] p2, int row1, int row2)
        {
            decimal foo = 0;
            for (int i = 0; i < 8; i++)
            {
                foo += (p1[row1, i] - p2[row2, i]) * (p1[row1, i] - p2[row2, i]);
            }
            return (decimal) Math.Sqrt((double)foo);
        }
    }
}
