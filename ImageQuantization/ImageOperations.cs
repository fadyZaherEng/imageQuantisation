using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
///Algorithms Project
///Intelligent Scissors
namespace ImageQuantization
{
    /// <summary>
    /// Holds the pixel color in 3 byte values: red, green and blue
    /// </summary>
    public struct RGBPixel
    {
        public byte red, green, blue;
    }
    public struct RGBPixelD
    {
        public double red, green, blue;
    }
    /// <summary>
    /// Library of static functions that deal with images
    /// </summary>
    /// 
    public class ImageOperations
    {
        public static bool[,,] visit = new bool[256, 256, 256];
        public static List<RGBPixel> dis_colors = new List<RGBPixel>();
        public static RGBPixel[,] Buffer;
        public static RGBPixelD rGB = new RGBPixelD();
        public static double count = 0;
        public static int[,,] Averags = new int[256, 256, 256];
        public static RGBPixelD[] Ks;
        public static int KCount = 0;
       /// <summary>
        /// Open an image and load it into 2D array of colors (size: Height x Width)
        /// </summary>
        /// <param name="ImagePath">Image file path</param>
        /// <returns>2D array of colors</returns>
        public static RGBPixel[,] OpenImage(string ImagePath)
        {
            Bitmap original_bm = new Bitmap(ImagePath);
            int Height = original_bm.Height;
            int Width = original_bm.Width;

            Buffer = new RGBPixel[Height, Width];

            unsafe
            {
                BitmapData bmd = original_bm.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, original_bm.PixelFormat);
                int x, y;
                int nWidth = 0;
                bool Format32 = false;
                bool Format24 = false;
                bool Format8 = false;

                if (original_bm.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    Format24 = true;
                    nWidth = Width * 3;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format32bppArgb || original_bm.PixelFormat == PixelFormat.Format32bppRgb || original_bm.PixelFormat == PixelFormat.Format32bppPArgb)
                {
                    Format32 = true;
                    nWidth = Width * 4;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    Format8 = true;
                    nWidth = Width;
                }
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (y = 0; y < Height; y++)
                {
                    for (x = 0; x < Width; x++)
                    {
                        if (Format8)
                        {
                            Buffer[y, x].red = Buffer[y, x].green = Buffer[y, x].blue = p[0];
                            p++;
                        }
                        else
                        {
                            Buffer[y, x].red = p[2];
                            Buffer[y, x].green = p[1];
                            Buffer[y, x].blue = p[0];
                            if (Format24) p += 3;
                            else if (Format32) p += 4;
                        }
                    }
                    p += nOffset;
                }
                original_bm.UnlockBits(bmd);
            }

            return Buffer;
        }
        public static List<RGBPixel> distinct_colour()
        {
            int row = GetHeight(Buffer);int col = GetWidth(Buffer);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    RGBPixel RGB=new RGBPixel();
                    if(visit[Buffer[i,j].red,Buffer[i,j].green,Buffer[i,j].blue]==false)
                    {
                        RGB.red = Buffer[i, j].red;
                        RGB.green = Buffer[i, j].green;
                        RGB.blue = Buffer[i, j].blue;
                        dis_colors.Add(RGB);
                        visit[Buffer[i, j].red, Buffer[i, j].green, Buffer[i, j].blue] = true;
                    }
                }
            }
            return dis_colors;
        }
        public static void Split_KCluster(int K)
        {
            Ks = new RGBPixelD[K];
            for (int i = 1; i < K; i++)
            {
               int source = 0; int dest = 0;
               colorProb c= Prim.edge.pop();
               source=c.colour1;dest = c.colour2;
               if (Prim.MST.ContainsKey(dest)) {
                    Prim.MST[source].Remove(dest);
                }
               else {
                    Prim.MST.Add(dest, new Dictionary<int, double>());
                    Prim.MST[source].Remove(dest);
                }
            }
        }
        public static void avg()
        {
            rGB.blue = 0; rGB.green = 0; rGB.red = 0;
            bool[] arr = new bool[dis_colors.Count];
            int c = dis_colors.Count;
            foreach (int i  in Prim.MST.Keys)
            {
                if (!arr[i])
                {
                    dfsAlgo(i, arr);
                    rGB.red = rGB.red / count;
                    rGB.blue = rGB.blue / count;
                    rGB.green = rGB.green / count;
                    Ks[KCount] = rGB;
                    rGB.blue = 0; rGB.green = 0; rGB.red = 0; count = 0; KCount++;
                }
            }

        }
        public static void dfsAlgo(int v, bool[] visited)
        {
            visited[v] = true;
            // avs.Add(dis_colors[v], KCount);
            Averags[dis_colors[v].red, dis_colors[v].blue, dis_colors[v].green] = KCount;
            rGB.red += dis_colors[v].red;
            rGB.blue += dis_colors[v].blue;
            rGB.green += dis_colors[v].green;
            count++;
            foreach (var x in Prim.MST[v])
            {
                if (!visited[x.Key])
                {
                    if(Prim.MST.ContainsKey(x.Key))
                        dfsAlgo(x.Key, visited);
                    else
                    {
                        visited[v] = true;
                        // avs.Add(dis_colors[x.Key], KCount);
                        Averags[dis_colors[x.Key].red, dis_colors[x.Key].blue, dis_colors[x.Key].green] = KCount;
                        rGB.red += dis_colors[x.Key].red;
                        rGB.blue += dis_colors[x.Key].blue;
                        rGB.green += dis_colors[x.Key].green;
                        count++;
                    }
                }
            }
        }

        public static RGBPixel[,] replace()
        {
            int row = GetHeight(Buffer);
            int col = GetWidth(Buffer);
            RGBPixel rgb = new RGBPixel();
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    rgb = Buffer[i, j];
                    int b = Averags[rgb.red,rgb.blue,rgb.green];
                    Buffer[i, j].red = (byte)Ks[b].red;
                    Buffer[i, j].blue = (byte)Ks[b].blue;
                    Buffer[i, j].green = (byte)Ks[b].green;
                }
            }

            return Buffer;
            
        }
        /// <summary>
        /// Get the height of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Height</returns>
        public static int GetHeight(RGBPixel[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(0);
        }
        /// <summary>
        /// Get the width of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Width</returns>
        public static int GetWidth(RGBPixel[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(1);
        }
        /// <summary>
        /// Display the given image on the given PictureBox object
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <param name="PicBox">PictureBox object to display the image on it</param>
        public static void DisplayImage(RGBPixel[,] ImageMatrix, PictureBox PicBox)
        {
            // Create Image:
            //==============
            int Height = ImageMatrix.GetLength(0);
            int Width = ImageMatrix.GetLength(1);

            Bitmap ImageBMP = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);

            unsafe
            {
                BitmapData bmd = ImageBMP.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, ImageBMP.PixelFormat);
                int nWidth = 0;
                nWidth = Width * 3;
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        p[2] = ImageMatrix[i, j].red;
                        p[1] = ImageMatrix[i, j].green;
                        p[0] = ImageMatrix[i, j].blue;
                        p += 3;
                    }

                    p += nOffset;
                }
                ImageBMP.UnlockBits(bmd);
            }
            PicBox.Image = ImageBMP;
        }
        /// <summary>
        /// Apply Gaussian smoothing filter to enhance the edge detection 
        /// </summary>
        /// <param name="ImageMatrix">Colored image matrix</param>
        /// <param name="filterSize">Gaussian mask size</param>
        /// <param name="sigma">Gaussian sigma</param>
        /// <returns>smoothed color image</returns>
        public static RGBPixel[,] GaussianFilter1D(RGBPixel[,] ImageMatrix, int filterSize, double sigma)
        {
            int Height = GetHeight(ImageMatrix);
            int Width = GetWidth(ImageMatrix);

            RGBPixelD[,] VerFiltered = new RGBPixelD[Height, Width];
            RGBPixel[,] Filtered = new RGBPixel[Height, Width];


            // Create Filter in Spatial Domain:
            //=================================
            //make the filter ODD size
            if (filterSize % 2 == 0) filterSize++;

            double[] Filter = new double[filterSize];

            //Compute Filter in Spatial Domain :
            //==================================
            double Sum1 = 0;
            int HalfSize = filterSize / 2;
            for (int y = -HalfSize; y <= HalfSize; y++)
            {
                //Filter[y+HalfSize] = (1.0 / (Math.Sqrt(2 * 22.0/7.0) * Segma)) * Math.Exp(-(double)(y*y) / (double)(2 * Segma * Segma)) ;
                Filter[y + HalfSize] = Math.Exp(-(double)(y * y) / (double)(2 * sigma * sigma));
                Sum1 += Filter[y + HalfSize];
            }
            for (int y = -HalfSize; y <= HalfSize; y++)
            {
                Filter[y + HalfSize] /= Sum1;
            }

            //Filter Original Image Vertically:
            //=================================
            int ii, jj;
            RGBPixelD Sum;
            RGBPixel Item1;
            RGBPixelD Item2;

            for (int j = 0; j < Width; j++)
                for (int i = 0; i < Height; i++)
                {
                    Sum.red = 0;
                    Sum.green = 0;
                    Sum.blue = 0;
                    for (int y = -HalfSize; y <= HalfSize; y++)
                    {
                        ii = i + y;
                        if (ii >= 0 && ii < Height)
                        {
                            Item1 = ImageMatrix[ii, j];
                            Sum.red += Filter[y + HalfSize] * Item1.red;
                            Sum.green += Filter[y + HalfSize] * Item1.green;
                            Sum.blue += Filter[y + HalfSize] * Item1.blue;
                        }
                    }
                    VerFiltered[i, j] = Sum;
                }

            //Filter Resulting Image Horizontally:
            //===================================
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    Sum.red = 0;
                    Sum.green = 0;
                    Sum.blue = 0;
                    for (int x = -HalfSize; x <= HalfSize; x++)
                    {
                        jj = j + x;
                        if (jj >= 0 && jj < Width)
                        {
                            Item2 = VerFiltered[i, jj];
                            Sum.red += Filter[x + HalfSize] * Item2.red;
                            Sum.green += Filter[x + HalfSize] * Item2.green;
                            Sum.blue += Filter[x + HalfSize] * Item2.blue;
                        }
                    }
                    Filtered[i, j].red = (byte)Sum.red;
                    Filtered[i, j].green = (byte)Sum.green;
                    Filtered[i, j].blue = (byte)Sum.blue;
                }

            return Filtered;
        }

    }
}
