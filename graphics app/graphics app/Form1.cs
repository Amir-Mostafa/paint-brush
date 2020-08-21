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
   
    public partial class Form1 : Form
    {

        public int[] DDApoint = new int[4];
        public int[] circlepoint = new int[3];
        public int[] ellipsepoint = new int[4];
        DataTable x_y = new DataTable();
        public static Form1 fall;
        int[] arr = new int[100];
        Point[] ddap = new Point[2];
        char a;
        char last;
        string type = "";
        string op = "";
        int count = 0;
        Point[] ad = new Point[2];
        Point pp = new Point();
        public DataTable DDAtable = new DataTable();
        public Bitmap p;
        Bitmap[] changes = new Bitmap[100];
        int ch = 0;
        public Form1()
        {

            InitializeComponent();
            x_y.Columns.Add("X");
            x_y.Columns.Add("y");
            fall = this;
            p = new Bitmap(Form1.fall.pictureBox1.Width, Form1.fall.pictureBox1.Height);
            changes[0] = p;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Blue;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dataGridView1.BackgroundColor = Color.Tan;
            dataGridView1.Font = new Font("arial", 15);
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        }
       
        private void FloodFill(Bitmap bmp, Point pt,Color replacementColor)
        {
            Stack<Point> pixels = new Stack<Point>();
            Color targetColor = bmp.GetPixel(pt.X, pt.Y);
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
                x += xi;
                y += yi;
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
                p.SetPixel((int)Math.Round(x), (int)Math.Round(y), Color.Blue);
            }


            //   this.Hide();
        }
        void boundaryFill4(Bitmap bmp, Point pt, Color fill_color, Color boundary_color)
        {
            Stack<Point> pixels = new Stack<Point>();
            Color targetColor = bmp.GetPixel(pt.X, pt.Y);
            pixels.Push(pt);

            while (pixels.Count > 0)
            {
                Point a = pixels.Pop();
                if (a.X < bmp.Width && a.X > 0 &&
                        a.Y < bmp.Height && a.Y > 0)//make sure we stay within bounds
                {

                    if (bmp.GetPixel(a.X,a.Y)==targetColor&& bmp.GetPixel(a.X, a.Y) != boundary_color && bmp.GetPixel(a.X, a.Y)!=fill_color)
                    {
                        bmp.SetPixel(a.X, a.Y, fill_color);
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


        private void button2_Click(object sender, EventArgs e)
        {
            bresinham_form f = new bresinham_form();
            f.Show();
            last = 'b';
        }

        private void button1_Click(object sender, EventArgs e)
        {
             a = 'a';
            DDA_form ff = new DDA_form();
            ff.Show();
            last = 'd';
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            circle_form fc = new circle_form();
            fc.Show();
            last = 'c';
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ellipse_form f = new ellipse_form();
            f.Show();
            type = "ellipse";
            op = "";
            radioButton1.Select();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ch++;
           // changes[ch] = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            changes[ch] =(Bitmap) pictureBox1.Image;
            if(type=="dda")
            {
                if(count==0&&op=="")
                {
                    ad[0].X = Cursor.Position.X - (Location.X + pictureBox1.Location.X + 8);
                    ad[0].Y = Cursor.Position.Y - (Location.Y + pictureBox1.Location.Y + 30);
                    count = 1;
                }
                else if(count!=0)
                {
                    try
                    {
                        count = 0;
                        ad[1].X = Cursor.Position.X - (Location.X + pictureBox1.Location.X + 8);
                        ad[1].Y = Cursor.Position.Y - (Location.Y + pictureBox1.Location.Y + 30);
                        DDA_form f = new DDA_form();
                        
                       ddap= f.DDA(ad[0].X, ad[0].Y, ad[1].X, ad[1].Y);
                       
                        
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    
                }


                if (op == "translate")
                {
                    DataTable d = new DataTable();
                    d = (DataTable)dataGridView1.DataSource;

                    try
                    {
                        int xx1 = DDApoint[0];
                        int yy1 = DDApoint[1];
                        int xx2 = DDApoint[2];
                        int yy2 = DDApoint[3];

                        int pointx = Cursor.Position.X - (Location.X + pictureBox1.Location.X + 8);
                        int pointy = Cursor.Position.Y - (Location.Y + pictureBox1.Location.Y + 30);

                        int amountx = pointx - (int)xx1;
                        int amounty = pointy - (int)yy1;

                        
                        int a1 = xx1 + amountx;
                        int a2 = yy1 + amounty;
                        int a3 = xx2 + amountx;
                        int a4 = yy2 + amounty;
                        DDA_form ff = new DDA_form();
                        ff.DDA(a1, a2,a3 ,a4);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else if (op == "rotate")
                {
                    DataTable d = new DataTable();
                    d = (DataTable)dataGridView1.DataSource;
                    float x1 = DDApoint[0];
                    float y1 = DDApoint[1];

                    float xro = DDApoint[2];
                    float yro = DDApoint[3];


                    int k;
                    if (textBox3.Text == "")
                        k = 90;
                    else
                        k = int.Parse(textBox3.Text);

                    int xx = (int)Math.Round(x1 * Math.Cos((k * (Math.PI)) / 180) - y1 * Math.Sin((k * (Math.PI)) / 180));
                    int yy = (int)Math.Round(x1 * Math.Sin((k * (Math.PI)) / 180) + y1 * Math.Cos((k * (Math.PI)) / 180));

                    int xrotation = (int)x1 - xx;
                    int yrotation = (int)y1 - yy;
                    int tt1 = (int)Math.Round(xro * Math.Cos((k * (Math.PI)) / 180) - yro * Math.Sin((k * (Math.PI)) / 180));
                    int tt2 = (int)Math.Round(xro * Math.Sin((k * (Math.PI)) / 180) + yro * Math.Cos((k * (Math.PI)) / 180));

                    xro = xrotation + tt1;
                    yro = yrotation + tt2;
                    DDA_form f = new DDA_form();
                    f.DDA((int)x1, (int)y1, (int)xro, (int)yro);


                }
                else if ( op == "scal")
                {
                    DataTable d = new DataTable();
                    d = (DataTable)dataGridView1.DataSource;
                    float x1 = DDApoint[0];
                    float y1 = DDApoint[1];

                    float x2 = DDApoint[2];
                    float y2 = DDApoint[3];

                    int xscal = (int)x1, yscal = (int)y1;
                    int k = 2;

                    x1 *= k;
                    x2 *= k;

                    xscal -= (int)x1;
                    yscal -= (int)y1;

                    x1 += xscal;
                    y1 += yscal;
                    x2 += xscal;
                    y2 += yscal;
                    DDA_form f = new DDA_form();
                    f.DDA((int)x1, (int)y1, (int)x2, (int)y2);

                }
            }
            else if(type=="bresenham")
            {
                if (count == 0&&op=="")
                {
                    ad[0].X = Cursor.Position.X - (Location.X + pictureBox1.Location.X + 8);
                    ad[0].Y = Cursor.Position.Y - (Location.Y + pictureBox1.Location.Y + 30);
                    count = 1;
                }
                else if(count!=0)
                {
                    count = 0;
                    ad[1].X = Cursor.Position.X - (Location.X + pictureBox1.Location.X + 8);
                    ad[1].Y = Cursor.Position.Y - (Location.Y + pictureBox1.Location.Y + 30);
                    bresinham_form f = new bresinham_form();
                    f.BRESENHAM(ad[0].X, ad[0].Y, ad[1].X, ad[1].Y);

                    f.textBox1.Text = ad[0].X.ToString();
                    f.textBox2.Text = ad[0].Y.ToString();
                    f.textBox3.Text = ad[1].X.ToString();
                    f.textBox4.Text = ad[1].Y.ToString();

                }

                if (op == "translate")
                {
                    DataTable d = new DataTable();
                    d = (DataTable)dataGridView1.DataSource;

                    try
                    {
                        int xx1 = DDApoint[0];
                        int yy1 = DDApoint[1];
                        int xx2 = DDApoint[2];
                        int yy2 = DDApoint[3];

                        int pointx = Cursor.Position.X - (Location.X + pictureBox1.Location.X + 8);
                        int pointy = Cursor.Position.Y - (Location.Y + pictureBox1.Location.Y + 30);

                        int amountx = pointx - (int)xx1;
                        int amounty = pointy - (int)yy1;


                        int a1 = xx1 + amountx;
                        int a2 = yy1 + amounty;
                        int a3 = xx2 + amountx;
                        int a4 = yy2 + amounty;
                        bresinham_form ff = new bresinham_form();
                        ff.BRESENHAM(a1, a2, a3, a4);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else if (op == "rotate")
                {
                    DataTable d = new DataTable();
                    d = (DataTable)dataGridView1.DataSource;
                    float x1 = DDApoint[0];
                    float y1 = DDApoint[1];

                    float xro = DDApoint[2];
                    float yro = DDApoint[3];


                    int k;
                    if (textBox3.Text == "")
                        k = 90;
                    else
                        k = int.Parse(textBox3.Text);

                    int xx = (int)Math.Round(x1 * Math.Cos((k * (Math.PI)) / 180) - y1 * Math.Sin((k * (Math.PI)) / 180));
                    int yy = (int)Math.Round(x1 * Math.Sin((k * (Math.PI)) / 180) + y1 * Math.Cos((k * (Math.PI)) / 180));

                    int xrotation = (int)x1 - xx;
                    int yrotation = (int)y1 - yy;
                    int tt1 = (int)Math.Round(xro * Math.Cos((k * (Math.PI)) / 180) - yro * Math.Sin((k * (Math.PI)) / 180));
                    int tt2 = (int)Math.Round(xro * Math.Sin((k * (Math.PI)) / 180) + yro * Math.Cos((k * (Math.PI)) / 180));

                    xro = xrotation + tt1;
                    yro = yrotation + tt2;
                    bresinham_form ff = new bresinham_form();
                    ff.BRESENHAM((int)x1, (int)y1, (int)xro, (int)yro);


                }
                else if (op == "scal")
                {
                    DataTable d = new DataTable();
                    d = (DataTable)dataGridView1.DataSource;
                    float x1 = DDApoint[0];
                    float y1 = DDApoint[1];

                    float x2 = DDApoint[2];
                    float y2 = DDApoint[3];

                    int xscal = (int)x1, yscal = (int)y1;
                    int k = 2;

                    x1 *= k;
                    x2 *= k;

                    xscal -= (int)x1;
                    yscal -= (int)y1;

                    x1 += xscal;
                    y1 += yscal;
                    x2 += xscal;
                    y2 += yscal;
                    bresinham_form ff = new bresinham_form();
                    ff.BRESENHAM((int)x1, (int)y1, (int)x2, (int)y2);

                }
            }
            else if(type=="circle")
            {
                if (op=="")
                {
                    if(count==0)
                    {
                        ad[0].X = Cursor.Position.X - (Location.X + pictureBox1.Location.X + 8);
                        ad[0].Y = Cursor.Position.Y - (Location.Y + pictureBox1.Location.Y + 30);
                        count++;
                    }
                    else 
                    {
                        ad[1].X = Cursor.Position.X - (Location.X + pictureBox1.Location.X + 8);
                        ad[1].Y = Cursor.Position.Y - (Location.Y + pictureBox1.Location.Y + 30);
                        circle_form f = new circle_form();
                        int r = Math.Abs(ad[1].X - ad[0].X);
                        f.circle(ad[0].X, ad[0].Y, r);
                        count = 0;
                        f.textBox1.Text = ad[0].X.ToString();
                        f.textBox2.Text = ad[0].Y.ToString();
                        f.textBox3.Text = "50";
                    }
                    
                }
                if(op=="translate")
                {

                    int x = Cursor.Position.X - (Location.X + pictureBox1.Location.X + 8);
                    int y = Cursor.Position.Y - (Location.Y + pictureBox1.Location.Y + 30);

                    circle_form f = new circle_form();
                    f.circle(x,y, circlepoint[2]);
                }
                else if(op=="scal")
                {
                    circle_form f = new circle_form();
                    f.circle(circlepoint[0], circlepoint[1], circlepoint[2]*2);
                }
                
            }
            else if (type == "ellipse")
            {
                if (count == 0&&op=="")
                {
                    ad[0].X = Cursor.Position.X - (Location.X + pictureBox1.Location.X + 8);
                    ad[0].Y = Cursor.Position.Y - (Location.Y + pictureBox1.Location.Y + 30);
                    ellipse_form f = new ellipse_form();
                    f.ellipse(ad[0].X, ad[0].Y,100,50);
                }
                if(op=="translate")
                {
                    int x = Cursor.Position.X - (Location.X + pictureBox1.Location.X + 8);
                    int y = Cursor.Position.Y - (Location.Y + pictureBox1.Location.Y + 30);
                    ellipse_form f = new ellipse_form();
                    f.ellipse(x, y, ellipsepoint[2], ellipsepoint[3]);
                }
                else if(op=="rotate")
                {
                    DataTable d = new DataTable();
                    d = (DataTable)dataGridView1.DataSource;
                    float x1 = ellipsepoint[0];
                    float y1 = ellipsepoint[1];

                    float xro = ellipsepoint[2];
                    float yro = ellipsepoint[3];


                    int k;
                    if (textBox3.Text == "")
                        k = 90;
                    else
                        k = int.Parse(textBox3.Text);

                    int xx = (int)Math.Round(x1 * Math.Cos((k * (Math.PI)) / 180) - y1 * Math.Sin((k * (Math.PI)) / 180));
                    int yy = (int)Math.Round(x1 * Math.Sin((k * (Math.PI)) / 180) + y1 * Math.Cos((k * (Math.PI)) / 180));

                    int xrotation = (int)x1 - xx;
                    int yrotation = (int)y1 - yy;
                    int tt1 = (int)Math.Round(xro * Math.Cos((k * (Math.PI)) / 180) - yro * Math.Sin((k * (Math.PI)) / 180));
                    int tt2 = (int)Math.Round(xro * Math.Sin((k * (Math.PI)) / 180) + yro * Math.Cos((k * (Math.PI)) / 180));

                    xro = xrotation+ tt1;
                    yro = yrotation + tt2;
                    ellipse_form f = new ellipse_form();
                    
                    f.ellipse((int)x1, (int)y1, (int)Math.Abs(tt1), (int) Math.Abs(tt2));
                }
                else if(op=="scal")
                {
                    ellipse_form f = new ellipse_form();

                    f.ellipse(ellipsepoint[0], ellipsepoint[1], ellipsepoint[2] * 2, ellipsepoint[3] * 2);
                }

            }
            else if(type=="fill")
            {
                try
                {
                    Bitmap bb = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    bb = (Bitmap)pictureBox1.Image;

                    pp.X = Cursor.Position.X - (Location.X + pictureBox1.Location.X + 8);
                    pp.Y = Cursor.Position.Y - (Location.Y + pictureBox1.Location.Y + 30);
                   boundaryFill4(bb,pp, Color.Red, Color.Black);
                   // boundry(bb,pp.X,pp.Y,Color.Red.ToArgb(),Color.Black.ToArgb());
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            
            else if(type=="point")
            {
                int pointx = Cursor.Position.X - (Location.X + pictureBox1.Location.X + 8);
                int pointy = Cursor.Position.Y - (Location.Y + pictureBox1.Location.Y + 30);

                DataRow r = x_y.NewRow();

                r[0] = pointx.ToString();
                r[1] = pointy.ToString();
                x_y.Rows.Add(r);
                dataGridView2.DataSource = x_y;
                if(dataGridView2.Rows.Count==2)
                {
                    x_y = (DataTable)dataGridView2.DataSource;
                  //  p = new Bitmap(Form1.fall.pictureBox1.Width, Form1.fall.pictureBox1.Height);
                    int x = int.Parse(x_y.Rows[0][0].ToString());
                    int y = int.Parse(x_y.Rows[0][1].ToString());
                    int x1 = int.Parse(x_y.Rows[ 1][0].ToString());
                    int y1 = int.Parse(x_y.Rows[1][1].ToString());
                    DDA(x, y, x1, y1);
                    pictureBox1.Image = p;
                    dataGridView2.Rows.Remove(dataGridView2.Rows[0]);
                }

            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = (Cursor.Position.X - (Location.X + pictureBox1.Location.X + 8)) + "," + (Cursor.Position.Y - (Location.Y + pictureBox1.Location.Y + 30)); 
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            timer1.Stop();
            label1.Text="0,0";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            type = "dda";
            op = "";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            type = "bresenham";
            op = "";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            type = "circle";
            op = "";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            type = "ellipse";
            op = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                int tx = int.Parse(textBox1.Text);
                int ty = int.Parse(textBox2.Text);
                DDA_form f = new DDA_form();
                int c = ddap[0].X;
                f.DDA((ddap[0].X + tx), (ddap[0].Y + ty), (ddap[1].X + tx), (ddap[1].Y + ty));
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            type = "fill";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
          
                op = "translate";


        }

        private void button6_Click(object sender, EventArgs e)
        {
            clipping_form f = new clipping_form();
            f.Show();
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
         
                op = "rotate";
           
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
           
                op = "scal";
            
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter&&textBox4.Text!=""&&textBox5.Text!="")
            {
                DataRow r = x_y.NewRow();

                r[0] = textBox4.Text;
                r[1] = textBox5.Text;
                x_y.Rows.Add(r);
                dataGridView2.DataSource = x_y;
            }
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && textBox4.Text != "" && textBox5.Text != "")
            {
                DataRow r = x_y.NewRow();

                r[0] = textBox4.Text;
                r[1] = textBox5.Text;
                x_y.Rows.Add(r);
                dataGridView2.DataSource = x_y;
                textBox5.Text = "";
                textBox4.Text = "";
                textBox4.Focus();
            }
        }

        private void dataGridView2_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                textBox4.Text = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                textBox5.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();
                dataGridView2.Rows.Remove(dataGridView2.CurrentRow);
            }
            catch
            {

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.Rows.Count >= 2)
                {
                    x_y = (DataTable)dataGridView2.DataSource;
                    p = new Bitmap(Form1.fall.pictureBox1.Width, Form1.fall.pictureBox1.Height);
                    for (int i = 0; i < dataGridView2.Rows.Count-1; i++)
                    {

                        int x = int.Parse(x_y.Rows[i][0].ToString());
                        int y = int.Parse(x_y.Rows[i][1].ToString());
                        int x1 = int.Parse(x_y.Rows[i+1][0].ToString());
                        int y1 = int.Parse(x_y.Rows[i+1][1].ToString());
                        DDA(x, y, x1, y1);

                    }
                    pictureBox1.Image = p;
                }
            }
            catch(Exception ex)
            { MessageBox.Show(ex.Message); }

        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            type = "point";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            p = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = p;
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
           
            
            pictureBox1.Image =(Bitmap) changes[ch];
            p = (Bitmap)pictureBox1.Image;
         //   pictureBox1.Refresh();
            if (ch > 0)
                ch--;

        }
    }
}
