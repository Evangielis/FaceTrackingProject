using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace DF_FaceTracking.cs
{
    public partial class QuizQuestion : Form
    {
        const int DEFAULT_NUM_QUESTIONS = 15;
        Question question;
        DataCollector dataCol;
        public int qID { get { return question.id; } }

        public QuizQuestion(Question q, DataCollector dc)
        {
            InitializeComponent();

            this.question = q;
            this.dataCol = dc;
        }

        private void QuizQuestion_Load(object sender, EventArgs e)
        {
            this.questionBox.Text = this.question.text;
            this.dataCol.StartSession();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            this.progressBar1.Value += 100/DEFAULT_NUM_QUESTIONS;
            this.dataCol.EndSession(this.answerBox.Text);

            while(this.dataCol.Status != CollectorStatus.Finished)
            {
                Thread.Sleep(50);
            }

            this.Close();
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
