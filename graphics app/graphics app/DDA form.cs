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
    public partial class DDA_form : Form
    {
       
        public DataTable DDAtable = new DataTable();
        Bitmap p;

        public static DDA_form dda;
        public DDA_form()
        {
            InitializeComponent();

            dda = this;
            p = Form1.fall.p;
            DDAtable.Columns.Add("k");
            DDAtable.Columns.Add("x old");
            DDAtable.Columns.Add("x_new =x+x_inc");
            DDAtable.Columns.Add("y old");
            DDAtable.Columns.Add("y new y+y_inc");
            DDAtable.Columns.Add("(x,y)");
        }

        private void DDA_form_Load(object sender, EventArgs e)
        {

        }
        
        public  Point[] DDA(int x1,int y1,int x2,int y2)
        {
                DDAtable.Clear();
                Form1.fall.DDApoint[0] = x1;
                Form1.fall.DDApoint[1] = y1;
                Form1.fall.DDApoint[2] = x2;
                Form1.fall.DDApoint[3] = y2;
                int xstart=0, ystart=0, xend=0, yend=0;
                int dx = x2 - x1;
                int dy = y2 - y1;

                int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));

                float xi = dx / (float)steps;
                float yi = dy / (float)steps;

                if (x1>0&&y1>0&&x1 < Form1.fall.pictureBox1.Width && y1 < Form1.fall.pictureBox1.Height)
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
                    if(i==0)
                    {
                        xstart = (int)x;
                        ystart = (int)y;
                    }
                    if (i == steps-1)
                    {
                        xend = (int)x;
                        yend = (int)y;
                    }
                    string f = "(" + ((int)Math.Round(x)).ToString() + "," + ((int)Math.Round(y)).ToString() + ")";
                    r[5] = f;
                    if (x > 0 && y > 0 && x < Form1.fall.pictureBox1.Width -1&& y < Form1.fall.pictureBox1.Height-1)
                    p.SetPixel((int)Math.Round(x), (int)Math.Round(y), Color.Blue);

                    DDAtable.Rows.Add(r);

                    Form1.fall.dataGridView1.DataSource = DDAtable;

                    
                    
                }
                

                Form1.fall.pictureBox1.Image = p;
                Point[] p1 = new Point[2];
                p1[0].X = xstart;
                p1[0].Y = ystart;

                p1[1].X = xend;
                p1[1].Y = yend;
              
                
                this.Hide();
              return p1;
        }
        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                int x1 = int.Parse(textBox1.Text);
                int y1 = int.Parse(textBox2.Text);
                int x2 = int.Parse(textBox3.Text);
                int y2 = int.Parse(textBox4.Text);
                 DDA(x1, y1, x2, y2);
            }
            catch
            { }
        }
    }
}
