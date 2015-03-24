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
    public partial class QuizQuestion : Form
    {
        Question question;

        public QuizQuestion(Question q)
        {
            InitializeComponent();

            this.question = q;
        }

        private void QuizQuestion_Load(object sender, EventArgs e)
        {
            this.questionBox.Text = this.question.text;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
