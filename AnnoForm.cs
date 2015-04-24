using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace DF_FaceTracking.cs
{
    public partial class AnnoForm : Form
    {
        bool Confused { get; set; }
        long IntervalStart { get; set; }
        long IntervalEnd { get; set; }

        public Stopwatch Clock { get; private set; }
        StringBuilder _annoText;

        public AnnoForm()
        {
            InitializeComponent();

            this.Confused = false;
            this.IntervalStart = 0;
            this.IntervalEnd = 0;

            this.Clock = new Stopwatch();
            this._annoText = new StringBuilder();
        }

        public void ShowMe()
        {
            this.Clock.Start();
            this.Show();
        }

        private void stampButton_Click(object sender, EventArgs e)
        {
            if (Confused)
            {
                stampButton.Text = "Confused";
                IntervalEnd = Clock.ElapsedMilliseconds;
                RecordStamp();

            }
            else
            {
                stampButton.Text = "Not Confused";
                IntervalStart = Clock.ElapsedMilliseconds;
            }
            Confused ^= true;
        }

        private void RecordStamp()
        {
            _annoText.Append(IntervalStart.ToString());
            _annoText.Append(",");
            _annoText.AppendLine(IntervalEnd.ToString());

            this.stampBox.Text = _annoText.ToString();
        }

        private void AnnoForm_Load(object sender, EventArgs e)
        {

        }
    }
}
