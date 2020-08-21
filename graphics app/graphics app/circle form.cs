using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace graphics_app
{

    
    public partial class circle_form : Form
    {
        long  xmax = 0, xmin = 0, ymax = 0, ymin = 0,cc=0;
        public DataTable circletable = new DataTable();
        Bitmap b;
        Point pp=new Point();
        

        public circle_form()
        {
            InitializeComponent();
            circletable.Columns.Add("k");
            circletable.Columns.Add("Pk");
            circletable.Columns.Add("x old");
            circletable.Columns.Add("y old");
            circletable.Columns.Add("x new");
            circletable.Columns.Add("y new");
            circletable.Columns.Add("(x,y)");
            b = Form1.fall.p;
        }

        private void FloodFill(Bitmap bmp, Point pt, Color targetColor, Color replacementColor)
        {
            Stack<Point> pixels = new Stack<Point>();
            targetColor = bmp.GetPixel(pt.X, pt.Y);
            pixels.Push(pt);

            while (pixels.Count > 0)
            {
                Point a = pixels.Pop();
                if (a.X < bmp.Width && a.X > 0 &&
                        a.Y < bmp.Height && a.Y > 0)//make sure we stay within bounds
                {

                    if (bmp.GetPixel(a.X, a.Y) == targetColor)
                    {
                        bmp.SetPixel(a.X, a.Y, replacementColor);
                        pixels.Push(new Point(a.X - 1, a.Y));
                        pixels.Push(new Point(a.X + 1, a.Y));
                        pixels.Push(new Point(a.X, a.Y - 1));
                        pixels.Push(new Point(a.X, a.Y + 1));
                    }
                }
            }
            Form1.fall.pictureBox1.Refresh(); //refresh our main picture box
            return;
        }

        private void scan(Bitmap bmp, Point pt, Color targetColor, Color replacementColor)
        {
            targetColor = bmp.GetPixel(pt.X, pt.Y);
            if (targetColor.ToArgb().Equals(replacementColor.ToArgb()))
            {
                return;
            }

            Stack<Point> pixels = new Stack<Point>();

            pixels.Push(pt);
            while (pixels.Count != 0)
            {
                Point temp = pixels.Pop();
                int y1 = temp.Y;
                while (y1 >= 0 && bmp.GetPixel(temp.X, y1) == targetColor)
                {
                    y1--;
                }
                y1++;
                bool spanLeft = false;
                bool spanRight = false;
                while (y1 < bmp.Height && bmp.GetPixel(temp.X, y1) == targetColor)
                {
                    bmp.SetPixel(temp.X, y1, replacementColor);

                    if (!spanLeft && temp.X > 0 && bmp.GetPixel(temp.X - 1, y1) == targetColor)
                    {
                        pixels.Push(new Point(temp.X - 1, y1));
                        spanLeft = true;
                    }
                    else if (spanLeft && temp.X - 1 == 0 && bmp.GetPixel(temp.X - 1, y1) != targetColor)
                    {
                        spanLeft = false;
                    }
                    if (!spanRight && temp.X < bmp.Width - 1 && bmp.GetPixel(temp.X + 1, y1) == targetColor)
                    {
                        pixels.Push(new Point(temp.X + 1, y1));
                        spanRight = true;
                    }
                    else if (spanRight && temp.X < bmp.Width - 1 && bmp.GetPixel(temp.X + 1, y1) != targetColor)
                    {
                        spanRight = false;
                    }
                    y1++;
                }

            }
            Form1.fall.pictureBox1.Refresh();

        }




        public void circle(int x0,int y0,int r)
        {


            Form1.fall.circlepoint[0] = x0;
            Form1.fall.circlepoint[1] = y0;
            Form1.fall.circlepoint[2] = r;
            int p = 1 - r;
            int x = r;
            int y = 0;
            int kk = 1;

            while (x >= y)
            {
                DataRow rk = circletable.NewRow();

                rk[0] = kk;
                kk++;
                rk[1] = p;

                rk[2] = x;
                rk[3] = y;
                if (x0 + x > 0 && x0 + x < Form1.fall.pictureBox1.Width && y0 + y > 0 && y0 + y < Form1.fall.pictureBox1.Height)
                    b.SetPixel(x0 + x, y0 + y, Color.Black);
                if (x0 - x > 0 && x0 - x < Form1.fall.pictureBox1.Width && y0 + y > 0 && y0 + y < Form1.fall.pictureBox1.Height)
                    b.SetPixel(x0 - x, y0 + y, Color.Black);
                if (x0 + x > 0 && x0 + x < Form1.fall.pictureBox1.Width && y0 - y > 0 && y0 - y < Form1.fall.pictureBox1.Height)
                    b.SetPixel(x0 + x, y0 - y, Color.Black);
                if (x0 - x > 0 && x0 - x < Form1.fall.pictureBox1.Width && y0 - y > 0 && y0 - y < Form1.fall.pictureBox1.Height)
                    b.SetPixel(x0 - x, y0 - y, Color.Black);
                if (x0 + y > 0 && x0 + y < Form1.fall.pictureBox1.Width && y0 + x > 0 && y0 + x < Form1.fall.pictureBox1.Height)
                    b.SetPixel(x0 + y, y0 + x, Color.Black);
                if (x0 - y > 0 && x0 - y < Form1.fall.pictureBox1.Width && y0 + x > 0 && y0 + x < Form1.fall.pictureBox1.Height)
                    b.SetPixel(x0 - y, y0 + x, Color.Black);
                if (x0 + y > 0 && x0 + y < Form1.fall.pictureBox1.Width && y0 - x > 0 && y0 - x < Form1.fall.pictureBox1.Height)
                    b.SetPixel(x0 + y, y0 - x, Color.Black);
                if (x0 - y > 0 && x0 - y < Form1.fall.pictureBox1.Width && y0 - x > 0 && y0 - x < Form1.fall.pictureBox1.Height)
                    b.SetPixel(x0 - y, y0 - x, Color.Black);

                rk[4] = (x0 + x).ToString();
                rk[5] = (y0 + y).ToString();
                if (p <= 0)
                {
                    p = p + 2 * (y + 1);
                }
                else
                {
                    p = p + 2 * (y + 1) - 2 * (x - 1);
                    x--;
                }
                y++;
                string g = "(" + x.ToString() + "," + y.ToString() + ")";
                rk[6] = g;

                circletable.Rows.Add(rk);

            }            
            Form1.fall.pictureBox1.Image = b;
            Form1.fall.dataGridView1.DataSource = circletable;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                int x0 = int.Parse(textBox1.Text);
                int y0 = int.Parse(textBox2.Text);
                int r = int.Parse(textBox3.Text);
                circle(x0, y0, r);
                pp.X = x0;
                pp.Y = y0;
                if(radioButton1.Checked)
                    FloodFill(b, pp, Color.Black, Color.Red);

                this.Hide();
                   
            }
            //  boundaryFill4(int.Parse(textBox1.Text), int.Parse(textBox2.Text), Color.Red, Color.Black);

            catch(Exception ex)

            {
                MessageBox.Show(ex.Message);
                
            }
           
        }

        private void circle_form_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
