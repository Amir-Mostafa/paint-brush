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
    public partial class bresinham_form : Form
    {

        public DataTable brestable = new DataTable();
        Bitmap p;
        public bresinham_form()
        {
            InitializeComponent();

            brestable.Columns.Add("k");
            brestable.Columns.Add("Pk");
            brestable.Columns.Add("x old");
            brestable.Columns.Add("x_new =x+x_inc");
            brestable.Columns.Add("y old");
            brestable.Columns.Add("y new y+y_inc");
            brestable.Columns.Add("(x,y)");
            p = Form1.fall.p;
        }
        public void BRESENHAM(int x1,int y1,int x2,int y2)
        {
           

            Form1.fall.DDApoint[0] = x1;
            Form1.fall.DDApoint[1] = y1;
            Form1.fall.DDApoint[2] = x2;
            Form1.fall.DDApoint[3] = y2;
            float m = (y2 - y1) / (float)(x2 - x1);

            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);

            if (Math.Abs(m) < 1)
            {
                int P = 2 * dy - dx;
                int towXY = 2 * (dy - dx);

                int xstart, ystart, xend;
                if (x1 < x2)
                {
                    xstart = x1;
                    ystart = y1;
                    xend = x2;
                }
                else
                {
                    xstart = x2;
                    ystart = y2;
                    xend = x1;
                }
                int k = 1;
                while (xstart < xend)
                {
                    DataRow r = brestable.NewRow();

                    r[0] = k;
                    k++;
                    r[1] = P;
                    r[2] = xstart;
                    xstart++;

                    r[3] = xstart;

                    r[4] = ystart;
                    if (P < 0)
                    {
                        P = P + 2 * dy;
                        if(xstart>0&&xstart<Form1.fall.pictureBox1.Width&&ystart>0&&ystart<Form1.fall.pictureBox1.Height)
                        p.SetPixel(xstart, ystart, Color.Black);
                    }
                    else
                    {
                        P = P + towXY;
                        ystart++;
                        if (xstart > 0 && xstart < Form1.fall.pictureBox1.Width && ystart > 0 && ystart < Form1.fall.pictureBox1.Height)
                        p.SetPixel(xstart, ystart, Color.Black);
                    }

                    r[5] = ystart;
                    string ss = "(" + xstart.ToString() + ystart.ToString() + ")";
                    r[6] = ss;
                    //textBox5.Text += xstart + "," + ystart + "\n";
                    brestable.Rows.Add(r);

                }
            }
            else
            {
                int P = 2 * dx - dy;
                int towXY = 2 * (dx - dy);

                int xstart, ystart, yend;
                if (y1 < y2)
                {
                    xstart = x1;
                    ystart = y1;
                    yend = y2;
                }
                else
                {
                    xstart = x2;
                    ystart = y2;
                    yend = y1;
                }
                string s = "";
                int k = 1;
                while (ystart < yend)
                {
                    DataRow r = brestable.NewRow();
                    r[0] = k;
                    k++;
                    r[1] = P.ToString();
                    r[2] = xstart;
                    r[4] = ystart;
                    ystart++;
                    r[5] = ystart;


                    if (P < 0)
                    {
                        P = P + 2 * dx;
                        if (xstart > 0 && xstart < Form1.fall.pictureBox1.Width && ystart > 0 && ystart < Form1.fall.pictureBox1.Height)
                        p.SetPixel(xstart, ystart, Color.Black);
                    }
                    else
                    {
                        P = P + towXY;
                        xstart++;
                        if (xstart > 0 && xstart < Form1.fall.pictureBox1.Width && ystart > 0 && ystart < Form1.fall.pictureBox1.Height)
                        p.SetPixel(xstart, ystart, Color.Black);
                    }
                    r[3] = xstart;
                    s = "(" + xstart + "," + ystart + ")";

                    r[6] = s;

                    brestable.Rows.Add(r);

                }

            }
            this.Hide();
            Form1.fall.pictureBox1.Image = p;
            Form1.fall.dataGridView1.DataSource = brestable;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int x1 = int.Parse(textBox1.Text);
                int y1 = int.Parse(textBox2.Text);
                int x2 = int.Parse(textBox3.Text);
                int y2 = int.Parse(textBox4.Text);
                BRESENHAM(x1, y1, x2, y2);
            }
            catch
            {

            }
        }

        private void bresinham_form_Load(object sender, EventArgs e)
        {

        }
    }
}
