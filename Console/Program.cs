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
                    data[i, j] = int.Parse(bar[j]); // THE ARRAY OF THE DATA
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
            int[,] groups = new int[data.GetUpperBound(0) + 1, 2]; // Store the data X groups
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
                groups[kk, 0] = kk; // Assign the data index corresponding to the data array
                groups[kk, 1] = group; // Assign the group
            }
            #endregion

            #region Sort array (Optimization movement)
            int[,] tempArray = new int[data.GetUpperBound(0) + 1, 2];
            int grp = 0;
            int tempInd = 0;
            int mainInd = 0;
            while (grp <= k)
            {
                if (groups[mainInd, 1] == grp)
                {
                    tempArray[tempInd, 0] = groups[mainInd, 0];
                    tempArray[tempInd, 1] = grp;
                    groups[mainInd, 1] = k + 1;
                    tempInd++;
                }
                mainInd++;
                if (mainInd == data.GetUpperBound(0))
                {
                    grp++;
                    mainInd = 0;
                }
            }
            groups = tempArray;
            #endregion // MIGHT BE UNNECESARY

            #region Recalculate Centroids
            for (int iiii = 0; iiii < k; iiii++) // GROUP ARRAY aKa centroid
            {
                for (int dim = 0; dim < data.GetUpperBound(1); dim++) // FIRST GET THE MEAN OF THE FIRST DIMENSION
                {
                    decimal mean = 0;
                    int count = 0;
                    for (int jjj = 0; jjj < data.GetUpperBound(0); jjj++) // NAVIGATE THROUGH THE GROUP ARRAY
                    {
                        if (groups[jjj, 1] == iiii) // IS THE INDEX jjj IN THE GROUP iiii???
                        {
                            count++;
                            mean += data[groups[jjj, 0], dim];
                        }
                    }
                    mean = mean / count;
                    centroids[iiii, dim] = mean;
                }
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
        static void printArray(int[,] array)
        {
            System.Console.Clear();
            System.Console.WriteLine("d X G");
            for (int i = 0; i < array.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= array.GetUpperBound(1); j++)
                {
                    System.Console.Write(array[i, j].ToString() + " ");
                }
                System.Console.WriteLine(" ");
            }
            System.Console.ReadKey();
        }
    }
}
