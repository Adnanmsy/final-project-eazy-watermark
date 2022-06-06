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
    public partial class Form2 : Form
    {
        public Form2()
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

        private void Form2_Load(object sender, EventArgs e)
        {

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
            int[,] TR = new int[X, Y];

            for (int i = 0; i < X - n; i = i + n)
            {
                for (int j = 0; j < Y - n; j += n)
                {
                    int LR = 0;
                    for (int k = i; k < i + n; k++)
                    {
                        for (int p = j; p < j + n; p++)
                        {
                            LR += b[k, p];
                        }
                    }

                    int moyR = LR / (n * n);

                    for (int k = i; k < i + n; k++)
                    {
                        for (int p = j; p < j + n; p++)
                        {
                            if (b[p, k] < moyR)
                            {
                                TR[p, k] = 0;
                            }
                            else
                            {
                                TR[p, k] = 1;
                            }
                        }
                    }
                }
            }
            return TR;
        }

        public static int[,] setlsb(int[,] b, int[,] T)
        {
            int X = b.GetLength(0);
            int Y = b.GetLength(1);
            //  int[,] NO = new int[X, Y];
            int[,] R = new int[X, Y];

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    //int L = b[i, j];
                    // int v = Convert.ToInt32(L);
                    string bin = Convert.ToString(b[i, j], 2);

                    int l = bin.Length;
                    int c = 8 - l;
                    while (c != 0)
                    {
                        bin = '0' + bin;
                        c--;
                    }
                    char[] A = bin.ToCharArray();

                    string E = Convert.ToString(T[i, j], 2);

                    int l2 = E.Length;
                    int c2 = 8 - l2;
                    while (c2 != 0)
                    {
                        E = '0' + E;
                        c2--;
                    }
                    char[] A2 = E.ToCharArray();

                    A[7] = A2[7];
                    string bF = new string(A);

                    int Ef = Convert.ToInt32(bF, 2);
                    R[i, j] = Ef;
                }
            }
            return R;
        }
        public static System.Drawing.Bitmap img_intit(System.Drawing.Image b)
        {
            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(b);
            int[,] R = valeurR(bm);
            int[,] B = valeurB(bm);
            int[,] G = valeurG(bm);

            int[,] NR = initialize(R);
            int[,] NB = initialize(B);
            int[,] NG = initialize(G);

            for (int i = 0; i < bm.Width; i++)
            {
                for (int j = 0; j < bm.Height; j++)
                {
                    bm.SetPixel(i, j, System.Drawing.Color.FromArgb(NR[i, j], NG[i, j], NB[i, j]));
                }
            }
            return bm;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            System.Drawing.Bitmap b = img_intit(pictureBox1.Image);

            int X = b.Width; int Y = b.Height;
            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(X, Y);

            int[,] R = valeurR(b);
            //int[,] IR = initialize(R);
            int[,] TR = tamperKey(R);
            int[,] FR = setlsb(R, TR);

            int[,] B = valeurB(b);
            // int[,] IB = initialize(B);
            int[,] TB = tamperKey(B);
            int[,] FB = setlsb(B, TB);

            int[,] G = valeurG(b);
            //int[,] IG = initialize(G);
            int[,] TG = tamperKey(G);
            int[,] FG = setlsb(G, TG);

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    bm.SetPixel(i, j, System.Drawing.Color.FromArgb(FR[i, j], FG[i, j], FB[i, j]));
                }
            }
            pictureBox2.Image = bm;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 fa = new Form1();
            fa.Show();
        }
    }
}
