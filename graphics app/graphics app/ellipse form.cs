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
    public partial class ellipse_form : Form
    {
        Bitmap b;
       
        
        public DataTable ellipstable=new DataTable();
        public ellipse_form()
        {
            InitializeComponent();
            b = Form1.fall.p;
            ellipstable.Columns.Add("k");
            ellipstable.Columns.Add("P");
            ellipstable.Columns.Add("(x,y)");
            ellipstable.Columns.Add("2*ry^2*x");
            ellipstable.Columns.Add("2*rx^2*y");
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
        public void ellipse(int xc,int yc,int rx,int ry)
        {
            ellipstable.Clear();

            Form1.fall.ellipsepoint[0] = xc;
            Form1.fall.ellipsepoint[1] = yc;
            Form1.fall.ellipsepoint[2] = rx;
            Form1.fall.ellipsepoint[3] = ry;
          //  b = (Bitmap)Form1.fall.pictureBox1.Image;
            int x = 0;
            int y = ry;

            double d1 = Math.Pow(ry, 2) - Math.Pow(rx, 2) * ry + 0.25 * Math.Pow(rx, 2);

            double dx = 2 * Math.Pow(rx, 2) * y;
            double dy = 2 * Math.Pow(ry, 2) * x;
            //regon 1
            int kk = 1;
            do
            {

                DataRow r = ellipstable.NewRow();
                r[0] = kk;
                kk++;
                r[1] = d1;
                string s = "(" + (xc + x).ToString() + "," + (yc + y).ToString() + ")";
                r[2] = s;

                r[3] = dy;
                r[4] = dx;
                if (xc + x > 0 && xc + x < Form1.fall.pictureBox1.Width && yc + y > 0 && yc + y < Form1.fall.pictureBox1.Height)
                    b.SetPixel(xc + x, yc + y, Color.Black);
                if (xc - x > 0 && xc - x < Form1.fall.pictureBox1.Width && yc + y > 0 && yc + y < Form1.fall.pictureBox1.Height)
                    b.SetPixel(xc - x, yc + y, Color.Black);
                if (xc - x > 0 && xc - x < Form1.fall.pictureBox1.Width && yc - y > 0 && yc - y < Form1.fall.pictureBox1.Height)
                    b.SetPixel(xc - x, yc - y, Color.Black);
                if (xc + x > 0 && xc + x < Form1.fall.pictureBox1.Width && yc - y > 0 && yc - y < Form1.fall.pictureBox1.Height)
                    b.SetPixel(xc + x, yc - y, Color.Black);

                if (d1 < 0)
                {
                    d1 = d1 + Math.Pow(ry, 2) * (2 * x + 3);
                    x++;

                    dy = 2 * Math.Pow(ry, 2) * x;

                }
                else
                {
                    d1 = d1 + Math.Pow(ry, 2) * (2 * x + 3) + Math.Pow(rx, 2) * (-2 * y + 2);
                    x++;
                    y--;
                    dy = 2 * Math.Pow(ry, 2) * x;
                    dx = 2 * Math.Pow(rx, 2) * y;
                }

                ellipstable.Rows.Add(r);
            } while (dx > dy);
            ellipstable.Rows.Add();

            //regon 1
            kk = 1;
            d1 = Math.Pow(ry, 2) * Math.Pow((x + 0.5), 2) + Math.Pow(rx, 2) * Math.Pow((y - 1), 2) - Math.Pow(ry, 2) * Math.Pow(rx, 2);
            do
            {
                DataRow r = ellipstable.NewRow();
                r[0] = kk;
                kk++;
                r[1] = d1;
                string s = "(" + (xc + x).ToString() + "," + (yc + y).ToString() + ")";
                r[2] = s;

                r[3] = "-----";
                r[4] = "-----";


                if (xc + x > 0 && xc + x < Form1.fall.pictureBox1.Width && yc + y > 0 && yc + y < Form1.fall.pictureBox1.Height)
                    b.SetPixel(xc + x, yc + y, Color.Black);
                if (xc - x > 0 && xc - x < Form1.fall.pictureBox1.Width && yc + y > 0 && yc + y < Form1.fall.pictureBox1.Height)
                    b.SetPixel(xc - x, yc + y, Color.Black);
                if (xc - x > 0 && xc - x < Form1.fall.pictureBox1.Width && yc - y > 0 && yc - y < Form1.fall.pictureBox1.Height)
                    b.SetPixel(xc - x, yc - y, Color.Black);
                if (xc + x > 0 && xc + x < Form1.fall.pictureBox1.Width && yc - y > 0 && yc - y < Form1.fall.pictureBox1.Height)
                    b.SetPixel(xc + x, yc - y, Color.Black);

                if (d1 < 0)
                {
                    d1 = d1 + Math.Pow(ry, 2) * (2 * x + 2) + Math.Pow(rx, 2) * (-2 * y + 3);
                    x++;
                    y--;
                }
                else
                {
                    d1 = d1 - Math.Pow(rx, 2) * (2 * y - 3);
                    y--;

                }

                ellipstable.Rows.Add(r);

            } while (y > 0);
            if (xc + rx > 0 && xc + rx < Form1.fall.pictureBox1.Width && yc > 0 && yc < Form1.fall.pictureBox1.Height)
            b.SetPixel(xc + rx, yc, Color.Black);
            if (xc -rx > 0 && xc - rx < Form1.fall.pictureBox1.Width && yc > 0 && yc < Form1.fall.pictureBox1.Height)
            b.SetPixel(xc - rx, yc, Color.Black);
            Form1.fall.pictureBox1.Image = b;
           // Form1 f = new Form1();
           // f.b
           // fg.b
            Form1.fall.dataGridView1.DataSource = ellipstable;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int rx = int.Parse(textBox1.Text);
                int ry = int.Parse(textBox2.Text);
                int xc = int.Parse(textBox3.Text);
                int yc = int.Parse(textBox4.Text);
                ellipse(xc, yc, rx, ry);
                Point p=new Point();
                p.X=int.Parse(textBox3.Text);
                p.Y=int.Parse(textBox4.Text);

                if (radioButton1.Checked)
                 scan(b, p, b.GetPixel(p.X,p.Y), Color.Red);
                this.Hide();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
