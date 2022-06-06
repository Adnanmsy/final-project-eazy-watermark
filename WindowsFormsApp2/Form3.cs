using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // open file dialog   
            OpenFileDialog open = new OpenFileDialog();
            // image filters  
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                // display image in picture box  
                pictureBox1.Image = new Bitmap(open.FileName);
                // image file path  
                // textBox1.Text = open.FileName;
            }
        }
        public static int[,] valeurR(System.Drawing.Image b)
        {
            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(b);
            int X = bm.Width; int Y = bm.Height;
            int[,] R = new int[X, Y];

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    R[i, j] = bm.GetPixel(i, j).R;
                }
            }
            return R;
        }

        public static int[,] valeurB(System.Drawing.Image b)
        {
            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(b);
            int X = bm.Width; int Y = bm.Height;
            int[,] R = new int[X, Y];

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    R[i, j] = bm.GetPixel(i, j).B;
                }
            }
            return R;
        }

        public static int[,] valeurG(System.Drawing.Image b)
        {
            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(b);
            int X = bm.Width; int Y = bm.Height;
            int[,] G = new int[X, Y];

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    G[i, j] = bm.GetPixel(i, j).G;
                }
            }
            return G;
        }

        public static int[,] extractLSB(int[,] b)
        {
            int X = b.GetLength(0); int Y = b.GetLength(1);

            int[,] M1 = new int[X, Y];

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    string v = Convert.ToString(b[i, j], 2);

                    int l = v.Length;
                    int c = 8 - l;
                    while (c != 0)
                    {
                        v = '0' + v;
                        c--;
                    }
                    char[] bin = v.ToCharArray();
                    string h = "";
                    h = bin[7] + h;
                    //char o = bin[7];
                    int p = Convert.ToInt32(h, 2);

                    // string bin = new string(o);

                    M1[i, j] = p;
                }
            }
            return M1;
        }

        public static int[,] initialize(int[,] b)
        {
            int X = b.GetLength(0); int Y = b.GetLength(1);

            int[,] R = new int[X, Y];

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    string v = Convert.ToString(b[i, j], 2);
                    int L = v.Length;
                    int c = 8 - L;
                    while (c != 0)
                    {
                        v = '0' + v;
                        c--;
                    }
                    char[] p = v.ToCharArray();
                    p[7] = '0';
                    //p[6] = '0';

                    string bin = new string(p);

                    R[i, j] = Convert.ToInt32(bin, 2);
                }
            }
            return R;
        }

        public static int[,] tamperKey(int[,] b)
        {

            int X = b.GetLength(0);
            int Y = b.GetLength(1);
            int n = 8;
            int[,] M2 = new int[X, Y];


            for (int i = 0; i < X - n; i = i + n)
            {
                for (int j = 0; j < Y - n; j += n)
                {
                    int LR = 0;
                    for (int k = i; k < i + n; k++)
                    {
                        for (int p = j; p < j + n; p++)
                        {
                            LR += b[p, k];
                        }
                    }


                    int moyR = LR / (n * n);


                    for (int k = i; k < i + n; k++)
                    {
                        for (int p = j; p < j + n; p++)
                        {
                            if (b[k, p] < moyR)
                            {
                                M2[k, p] = 0;
                            }
                            else
                            {
                                M2[k, p] = 1;
                            }
                        }
                    }
                }
            }
            return M2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Drawing.Bitmap b = new System.Drawing.Bitmap(pictureBox1.Image);

            int X = b.Width; int Y = b.Height;
            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(X, Y);



            /* int[,] B = valeurB(b);
             int[,] MB = extractLSB(B);
             int[,] IB = initialize(B);
             int[,] TB = tamperKey(IB);
             //int[,] FB = setlsb(MB, TB);

             int[,] G = valeurG(b);
             int[,] MG = extractLSB(G);
             int[,] IG = initialize(G);
             int[,] TG = tamperKey(IG);
            // int[,] FG = setlsb(MG, TG);*/

            int[,] R = valeurR(b);
            int[,] MR = extractLSB(R);
            int[,] IR = initialize(R);
            int[,] TR = tamperKey(IR);
            //int[,] FR = setlsb(MR, TR);
            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    int D = Math.Abs(MR[i, j] - TR[i, j]);

                    if (D == 0)

                        bm.SetPixel(i, j, System.Drawing.Color.FromArgb(0, 0, 0));
                    else
                        bm.SetPixel(i, j, System.Drawing.Color.FromArgb(255, 255, 255));


                }
            }
            pictureBox2.Image = bm;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "BMP*.BMP|*.bmp";

            if (pictureBox2.Image == null)
                MessageBox.Show("No Picture added!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                saveFileDialog1.ShowDialog();
                pictureBox2.Image.Save(saveFileDialog1.FileName);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 fa = new Form1();
            fa.Show();
        }

        /*  public static int[,] setlsb(int[,] b, int[,] M2)
          {
              int X = b.GetLength(0);
              int Y = b.GetLength(1);
              int[,] W = new int[X, Y];

              for (int i = 0; i < X; i++)
              {
                  for (int j = 0; j < Y; j++)
                  {
                      int v = b[i, j] - M2[i, j];
                      int a = Math.Abs(v);

                      if (a != 0)
                          W[i, j] = 255;
                      else
                          W[i,j] = 0;
                  }
              }
              return W;
          }
          */
    }
}
