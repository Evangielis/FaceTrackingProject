using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Analytics
{
    public partial class MainForm : Form
    {
        const int LEFT_PADDING = 5;
        const int TOP_PADDING = 25;
        const int RIGHT_PADDING = 30;
        const int CHART_HEIGHT = 110;
        const int CHART_SPACING = 1;

        string FileName { get; set; }
        DataSet Data { get; set; }
        string ActiveColumn { get { return this.comboBox1.SelectedItem.ToString(); } }

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Name = "Data Analyzer";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = this.CreateGraphics();
            Pen p = new Pen(Color.Black);
            Pen palt = new Pen(Color.Blue);

            g.Clear(Color.White);

            if (this.comboBox1.SelectedIndex > -1)
            {
                //Draw chart axes
                g.DrawRectangle(p, new Rectangle(LEFT_PADDING, TOP_PADDING, this.Size.Width - RIGHT_PADDING, CHART_HEIGHT));

                //Iterate through datapoints
                DataTable dt = this.Data.Tables["DATA"];
                //DataTable da = 


                Point prev = Point.Empty;
                for (int i=0; i < dt.Rows.Count; i++)
                {
                    var value = dt.Rows[i][this.ActiveColumn];
                    int xpos = LEFT_PADDING + (i * CHART_SPACING);
                    int ypos = TOP_PADDING + CHART_HEIGHT - Convert.ToInt32(value);
                    Point current = new Point(xpos, ypos);

                    if (i > 0)
                    {
                        g.DrawLine(palt, prev, current);
                    }

                    prev = current;
                }
            }

            p.Dispose();
            g.Dispose();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();

            //fd.Filter = "XML Files | (.xml)";

            //File dialog
            fd.ShowDialog();
            this.FileName = fd.FileName;

            if (File.Exists(this.FileName))
            {
                //Open and read the file
                this.Data = new DataSet();
                this.Data.ReadXml(this.FileName);

                DataTable dt = this.Data.Tables["DATA"];

                this.comboBox1.Items.Clear();
                foreach (DataColumn dc in dt.Columns)
                {
                    //Console.WriteLine(dc.ColumnName);
                    this.comboBox1.Items.Add(dc.ColumnName);
                }

                dt = this.Data.Tables["USERS"];
                this.userBox.Text = dt.Rows[0]["Name"].ToString();

                dt = this.Data.Tables["RESPONSES"];
                this.rBox.Text = dt.Rows[0]["Text"].ToString();
                this.qBox.Text = dt.Rows[0]["qID"].ToString();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void totalIntensityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TotalsForm tf = new TotalsForm(this.Data);
            tf.Show();
        }
    }
}
