﻿using System;
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
    public partial class QuizStartForm : Form
    {
        MainForm mform;

        public QuizStartForm(MainForm parent)
        {
            InitializeComponent();

            mform = parent;
        }

        public void ShowQuiz()
        {
            this.Show();
        }

        private void QuizStartForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            lock (mform.m_expressionStatus)
            {
                foreach (var expr in mform.m_expressionStatus)
                {
                    Console.Out.WriteLine(expr.Key + " : " + expr.Value);
                }
            }
        }
    }
}