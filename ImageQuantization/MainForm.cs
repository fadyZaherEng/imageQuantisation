using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
namespace ImageQuantization
{
    public partial class MainForm : Form
    {
      
      //  public int time;
        public double B;
        public int K;
        public MainForm()
        {
            InitializeComponent();
        }

        RGBPixel[,] ImageMatrix;
        static Stopwatch st;

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
            st = Stopwatch.StartNew();
            System.Threading.Thread.Sleep(500);
        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value ;
            ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);
            //  ImageOperations.Average();
           
            ImageOperations.Split_KCluster(Convert.ToInt32(k.Text));
            ImageOperations.avg();
            ImageOperations.replace();
            ImageOperations.DisplayImage(ImageOperations.Buffer, pictureBox2);
            //  ttt.Text = Program.time.ToString();
            st.Stop();
            ttt.Text = st.ElapsedMilliseconds.ToString();
     
        }

        private void label1_Click(object sender, EventArgs e)
        {
            B = 0;
            textBox1.Clear();
        }

        private void colorbutton_Click(object sender, EventArgs e)
        {
            colortext.Clear();
            colortext.Text = ImageOperations.distinct_colour().Count.ToString();

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Prim X = new Prim();
            Prim.prim();
           textBox1.Text = Prim.mstCost.ToString();

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void k_TextChanged(object sender, EventArgs e)
        {

        }
    }
}