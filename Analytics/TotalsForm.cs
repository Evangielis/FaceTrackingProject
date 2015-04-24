using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analytics
{
    public partial class TotalsForm : Form
    {
        DataSet MainData { get; set; }
        List<string> Columns { get; set; }

        public TotalsForm(DataSet main)
        {
            InitializeComponent();

            this.MainData = main;
        }

        private void TotalsForm_Load(object sender, EventArgs e)
        {
            DataTable dt = this.MainData.Tables["DATA"];
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Total Intensity for all Expressions");
            sb.AppendLine("===================================");

            foreach (DataColumn dc in dt.Columns)
                if (dc.ColumnName != "TimeStamp" && dc.ColumnName != "Pulse")
                {
                    var total = dt.AsEnumerable().Sum(x => Convert.ToInt32(x[dc.ColumnName]));
                    sb.AppendLine(dc.ColumnName.ToString() + ": " + total.ToString());
                }

            this.label1.Text = sb.ToString();
        }
    }
}
