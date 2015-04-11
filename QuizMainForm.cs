using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace DF_FaceTracking.cs
{
    public partial class QuizMainForm : Form
    {
        MainForm mform;
        Quiz mquiz;
        string mname;
        DataCollector mdata;
        public Stopwatch Clock { get; private set; }


        public QuizEnumerator qEnum { get; private set; }

        public QuizMainForm(MainForm parent)
        {
            InitializeComponent();

            mform = parent;
            mname = String.Empty;
            Clock = new Stopwatch();

            //Event fired when form is closing
            FormClosing += QuizStartForm_FormClosing;
        }

        public void ShowQuiz()
        {
            this.Clock.Start();
            this.Show();
        }

        private void QuizStartForm_Load(object sender, EventArgs e)
        {
            quizStartButton.Enabled = false;

            if (!(Directory.Exists("Data")))
            {
                Directory.CreateDirectory("Data");
            }
        }

        private void QuizStartForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            mform.StopMe();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*lock (mform.m_expressionStatus)
            {
                foreach (var expr in mform.m_expressionStatus)
                {
                    Console.Out.WriteLine(expr.Key + " : " + expr.Value);
                }
            }*/

            //Control manipulations
            textBox1.Enabled = false;
            this.mquiz = new Quiz(mname);
            this.qEnum = this.mquiz.GetEnum();

            while((this.qEnum as IEnumerator<Question>).MoveNext())
            {
                PresentQuestion((this.qEnum as IEnumerator<Question>).Current);
            }
        }

        private void PresentQuestion(Question q)
        {
            //Pull quiz question
            mdata = new DataCollector(mname, q.id, mform, this.Clock);
            QuizQuestion qq = new QuizQuestion(q, mdata);
            qq.ShowDialog();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            quizStartButton.Enabled = (textBox1.Text != String.Empty);
            mname = textBox1.Text;
        }
    }
}
