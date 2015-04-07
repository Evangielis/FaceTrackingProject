using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DF_FaceTracking.cs
{
    public partial class QuizEditor : Form
    {
        QuizDataSet mdata;

        public QuizEditor()
        {
            InitializeComponent();

            FormClosing += QuizEditor_FormClosing;
        }

        private void QuizEditor_FormClosing(Object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void QuizEditor_Load(object sender, EventArgs e)
        {
            mdata = new QuizDataSet();

            Console.Out.WriteLine(mdata.Tables.Count);

            QuizDataSet.QUESTIONSDataTable table = new QuizDataSet.QUESTIONSDataTable();
            DataRow row = table.NewRow();
            row["pID"] = 1;
            row["Text"] = "This is a test question!";

            table.Rows.Add(row);
            mdata.Tables.Add(table);


        }
    }
}
