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
            int[,] data = new int[predata.Length, 8];
            for (int i = 0; i < predata.Length; i++)
            {
                string[] bar = predata[i].Split(',');
                for (int j = 0; j < 8; j++)
                {
                    data[i, j] = Int32.Parse(bar[j]);
                }
            }
            #endregion

            #region Get Centroids
            int[,] centroids = new int[k, 8];
            if (!predef)
            {                
                for (int ii = 0; ii < k; ii++)
                {
                    Random bar = new Random();
                    int rand = bar.Next(0, data.GetUpperBound(0) + 1);
                    copyArrayRow(data, ref centroids, rand, ii);
                }
            }
            else
            {
                for (int jj = 0; jj < k; jj++)
                {
                    copyArrayRow(data, ref centroids, jj, jj);
                }
            }
            #endregion

            #region Assign To Centroids
            int[,] groups = new int[data.GetUpperBound(0) + 1, 2];
            for (int kk = 0; kk < data.GetUpperBound(0) + 1; kk++)
            {
                
            }
            #endregion
        }

        struct Point
        {
            public decimal x;
            public decimal y;
        }

        static void copyArrayRow(int[,] arrayInput, ref int[,] arrayOutput, int inRow, int outRow)
        {
            for (int i = 0; i < 8; i++)
            {
                arrayOutput[outRow, i] = arrayInput[inRow, i];
            }              
        }
        static decimal eucDist(Point inPoint1, Point inPoint2)
        {
            decimal foox = inPoint1.x - inPoint2.x;
            decimal fooy = inPoint1.y - inPoint2.y;
            decimal sqrx = foox * foox;
            decimal sqry = fooy * fooy;
            return (decimal) Math.Sqrt( (double) (sqrx + sqry));
        }
    }
}
