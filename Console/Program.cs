using System;
using System.Windows.Forms;
using System.IO;

namespace Console
{
    class Program
    {
        private const int sigma = 1;
        private const int k = 4;
        private const bool random = true;
        private const int dimension = 8;
        private const decimal errorTolerance = 0.0000000001M;
        private bool clustered = false;

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
            decimal[,] centroids = getCentroids(data, k, random);
            #endregion
            
            decimal[] error = new decimal[0];
            decimal[,] newCentroids = centroids;

            do
            {
                centroids = newCentroids;
                newCentroids = null;
                #region Assign To Centroids
                int[,] groups = centAssign(data, centroids, k);
                #endregion

                #region Recalculate Centroids
                newCentroids = centRecalc(data, groups, k);
                #endregion

                #region Calculate Error

                addRow(ref error, centError(data, centroids, groups));

                #endregion

            } while (!compareArrays(centroids, newCentroids, errorTolerance));
        }

        static void copyArrayRow(int[,] arrayInput, ref decimal[,] arrayOutput, int inRow, int outRow)
        {
            for (int i = 0; i < 8; i++)
            {
                arrayOutput[outRow, i] = arrayInput[inRow, i];
            }              
        }
        static decimal eucDist(int[,] point1, decimal[,] point2, int row1, int row2, bool squared = true)
        {
            decimal distance = 0;
            for (int i = 0; i < point1.GetUpperBound(1); i++)
            {
                distance += square(point1[row1, i] - point2[row2, i]); // Squared
            }
            return squared ? distance : (decimal) Math.Sqrt( (double) distance);
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
        static decimal[,] getCentroids(int[,] data, int nCent, bool random = true)
        {
            decimal[,] centroids = new decimal[nCent, data.GetUpperBound(1) + 1];
            if (random)
            {                
                Random foo = new Random();
                for (int i = 0; i < k; i++)
                {
                    int rand = foo.Next(0, data.GetUpperBound(0) + 1);
                    copyArrayRow(data, ref centroids, rand, i);
                }
            }
            else
            {
                for (int i = 0; i < k; i++)
                {
                    copyArrayRow(data, ref centroids, i, i);
                }
            }

            return centroids;
        }
        static int[,] centAssign(int[,] data, decimal[,] centroids, int nCent) // The array is DATAITEMS X GROUPASSIGNED
        {
            int[,] groups = new int[data.GetUpperBound(0), 2];
            for (int i = 0; i < data.GetUpperBound(0); i++) // Navigate through all the points
            {
                decimal dist = decimal.MaxValue;
                int group = 0;
                for (int j = 0; j < nCent; j++) // Calculate all the dist to all centroids
                {
                    decimal foo = eucDist(data, centroids, i, j);
                    if (foo < dist)
                    {
                        dist = foo;
                        group = j;
                    }
                }
                groups[i, 0] = i; // Data index referred to data array
                groups[i, 1] = group; // 
            }

            return groups;
        }
        static decimal[,] centRecalc(int[,] data, int[,] groups, int nCent)
        {
            decimal[,] centroids = new decimal[nCent, data.GetUpperBound(1)];
            for (int i = 0; i < nCent; i++) // For each centroid
            {
                for (int dim = 0; dim < data.GetUpperBound(1); dim++) // Mean for each dimension
                {
                    decimal mean = 0;
                    int count = 0;
                    for (int j = 0; j < data.GetUpperBound(0); j++) // Navigate through all the data
                    {
                        if (groups[j, 1] == i) // Is data index j in the centroid i??
                        {
                            count++;
                            mean += data[groups[j, 0], dim];
                        }
                    }
                    mean = mean / count;
                    centroids[i, dim] = mean;
                }
            }

            return centroids;
        }
        static decimal centError(int[,] data, decimal[,] centroids, int[,] groups)
        {
            decimal error = 0;
            for (int i = 0; i < centroids.GetUpperBound(0); i++)
            {
                for (int j = 0; j < data.GetUpperBound(0); j++)
                {
                    if (groups[j, 1] != i)
                        continue;
                    error += eucDist(data, centroids, groups[j, 0], i, true);
                }
            }
            return error;
        }
        static bool compareArrays(decimal[,] array1, decimal[,] array2, decimal errorTol)
        {
            for (int i = 0; i < array1.GetUpperBound(0); i++)
            {
                for (int j = 0; j < array1.GetUpperBound(1); j++)
                {
                    if (Math.Abs(array1[i, j] - array2[i, j]) > errorTol)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        static void addRow(ref decimal[] input, decimal rowValue)
        {            
            decimal[] newArr = new decimal[input.Length + 1];
            if (input == null)
            {
                newArr[0] = rowValue;
            }
            else
            {
                for (int i = 0; i < input.Length; i++)
                {
                    newArr[i] = input[i];
                }
                newArr[input.Length] = rowValue;
            }
            input = newArr;
        }
        static decimal square(decimal number1)
        {
            return number1 * number1;
        }
    }
}
