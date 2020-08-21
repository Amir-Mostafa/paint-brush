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
    public partial class clipping_form : Form
    {
        public DataTable DDAtable = new DataTable();
        Bitmap p;
        private const byte INSIDE = 0; // 0000
        private const byte LEFT = 1;   // 0001
        private const byte RIGHT = 2;  // 0010
        private const byte BOTTOM = 4; // 0100
        private const byte TOP = 8;    // 1000
        public clipping_form()
        {
            InitializeComponent();
            DDAtable.Columns.Add("k");
            DDAtable.Columns.Add("x old");
            DDAtable.Columns.Add("x_new =x+x_inc");
            DDAtable.Columns.Add("y old");
            DDAtable.Columns.Add("y new y+y_inc");
            DDAtable.Columns.Add("(x,y)");
            p = Form1.fall.p;
        }

        private void clipping_form_Load(object sender, EventArgs e)
        {

        }
        public byte compute_code(double x,double y,int t,int b,int l,int r)
        {
            byte code = INSIDE;

            if (x < l)           // to the left of clip window
                code |= LEFT;
            else if (x > r)     // to the right of clip window
                code |= RIGHT;


            if (y < b)         // below the clip window
                code |= BOTTOM;
            else if (y > t)       // above the clip window
                code |= TOP;

            return code;
        }
        public Point[] barsky(Point p0, Point p1, int T, int B, int L, int R)
        {
            Point[] pp = new Point[2];
            int dx = Math.Abs(p1.X -p0.X);
            int dy = Math.Abs(p1.Y - p0.Y);

            int[] p = new int[4];
            int[] q = new int[4];
            double t1 = 0;
            double t2 = 1;
            p[0] = -dx;
            p[1] = dx;
            p[2] = -dy;
            p[3] = dy;
            q[0] = p0.X - L;
            q[1] = R - p0.X;
            q[2] = p0.Y - B;
            q[3] = T - p0.Y;

            for (int i = 0; i < 4; i++)
            {
                if (p[i] == 0)
                {
                    if (q[i] >= 0)
                    {
                        if (i < 2)
                        {
                            if (p0.Y < B)
                                p0.Y = B;
                            if (p1.Y > T)
                                p1.Y = T;
                        }
                        if (i > 1)
                        {
                            if (p0.X < L)
                                p0.X = L;
                            if (p1.X > R)
                                p1.X = R;
                        }
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                double u = q[i] / (double)p[i];
                if (p[i] < 0)
                {
                    if (t1 <= u)
                        t1 = u;
                }
                else
                {
                    if (t2 > u)
                        t2 = u;
                }
            }
            if (t1 < t2)
            {
                double x0 = p0.X + t1 * dx;
                double x1 = p0.X + t2 * dx;
                double y0 = p0.Y + t1 * dy;
                double y1 = p0.Y + t2 * dy;

                p0.X=(int) x0;
                p0.Y = (int)y0;

                p1.X = (int)x1;
                p1.Y = (int)y1;
            }
            pp[0] = p0;
            pp[1] = p1;
            return pp;
        }
        public Point[] cohen(Point p0,Point p1,int T,int B,int L,int R)
        {
            Point[] p = new Point[2];
            double x0 = p0.X;
            double y0 = p0.Y;
            double x1 = p1.X;
            double y1 = p1.Y;

            byte outcode0 = compute_code(x0, y0, T, B, L, R);
            byte outcode1 = compute_code(x1, y1,T,B,L,R);
            bool accept = false;

            while(true)
            {
                if ((outcode0 | outcode1) == 0)
                {
                    accept = true;
                    break;
                }

                else if ((outcode0 & outcode1) != 0)
                {
                    break;
                }
                else
                {
                    double x, y;

                    byte outcodeOut = (outcode0 != 0) ? outcode0 : outcode1;
                    if ((outcodeOut & TOP) != 0)
                    {
                        x = x0 + (x1 - x0) * (T - y0) / (y1 - y0);
                        y = T;
                    }
                    else if((outcodeOut & Bottom) !=0)
                    {
                        x = x0 + (x1 - x0) * (B - y0) / (y1 - y0);
                        y = B;
                    }
                    else if ((outcodeOut & RIGHT) != 0)
                    {  // point is to the right of clip rectangle
                        y = y0 + (y1 - y0) * (R - x0) / (x1 - x0);
                        x = R;
                    }
                    else if ((outcodeOut & LEFT) != 0)
                    {   // point is to the left of clip rectangle
                        y = y0 + (y1 - y0) * (L - x0) / (x1 - x0);
                        x = L;
                    }
                    else
                    {
                        x = double.NaN;
                        y = double.NaN;
                    }
                    if (outcodeOut == outcode0)
                    {
                        x0 = x;
                        y0 = y;
                        outcode0 = compute_code(x0, y0, T, B, L, R);
                    }
                    else
                    {
                        x1 = x;
                        y1 = y;
                        outcode1 = compute_code(x1, y1, T, B, L, R);
                    }

                }

                   

            }
            if (accept)
            {
                p[0].X = (int)x0;
                p[0].Y = (int)y0;
                p[1].X = (int)x1;
                p[1].Y = (int)y1;
                return p;
            }
            else
                return null;
        }
        public void DDA(int x1, int y1, int x2, int y2)
        {
            DDAtable.Clear();

            int xstart = 0, ystart = 0, xend = 0, yend = 0;
            int dx = x2 - x1;
            int dy = y2 - y1;

            int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));

            float xi = dx / (float)steps;
            float yi = dy / (float)steps;


            p.SetPixel(x1, y1, Color.Blue);
            float x = x1, y = y1;
            for (int i = 0; i < steps; i++)
            {
                DataRow r = DDAtable.NewRow();
                r[0] = i + 1;
                r[1] = x;
                r[3] = y;
                x += xi;
                y += yi;

                r[2] = x;
                r[4] = y;
                if (i == 0)
                {
                    xstart = (int)x;
                    ystart = (int)y;
                }
                if (i == steps - 1)
                {
                    xend = (int)x;
                    yend = (int)y;
                }
                string f = "(" + ((int)Math.Round(x)).ToString() + "," + ((int)Math.Round(y)).ToString() + ")";
                r[5] = f;
                p.SetPixel((int)Math.Round(x), (int)Math.Round(y), Color.Blue);

                DDAtable.Rows.Add(r);

                Form1.fall.dataGridView1.DataSource = DDAtable;

            }
            

         //   this.Hide();
        }
        private void button1_Click(object sender, EventArgs e)
        {

            try

            {
                p = new Bitmap(Form1.fall.pictureBox1.Width, Form1.fall.pictureBox1.Height);
                Point[] pp = new Point[2];
                pp[0].X = int.Parse(textBox1.Text);
                pp[0].Y = int.Parse(textBox2.Text);
                
                pp[1].X = int.Parse(textBox3.Text);
                pp[1].Y = int.Parse(textBox4.Text);

                int T = int.Parse(ymax.Text);
                int B = int.Parse(ymin.Text);
                int L = int.Parse(xmin.Text);
                int R = int.Parse(xmax.Text);
                
                    //pp=cohen(pp[0], pp[1],T,B,L,R);
               if (radioButton1.Checked)
                    pp=barsky(pp[0],pp[1], T, B, L, R);
              DDA(pp[0].X, pp[0].Y, pp[1].X, pp[1].Y);

              DDA(L, B, L, T);
              DDA(L, T, R, T);
              DDA(R, B, R, T);
              DDA(R, B, L, B);
              Form1.fall.pictureBox1.Image = p;
            }
            catch(Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
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

        private void ymax_TextChanged(object sender, EventArgs e)
        {

        }

        private void xmax_TextChanged(object sender, EventArgs e)
        {

        }

        private void ymin_TextChanged(object sender, EventArgs e)
        {

        }

        private void xmin_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
