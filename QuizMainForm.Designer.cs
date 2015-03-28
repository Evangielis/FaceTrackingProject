namespace DF_FaceTracking.cs
{
    partial class QuizMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuizMainForm));
            this.quizStartButton = new System.Windows.Forms.Button();
            this.quizStartText = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // quizStartButton
            // 
            this.quizStartButton.Location = new System.Drawing.Point(12, 197);
            this.quizStartButton.Name = "quizStartButton";
            this.quizStartButton.Size = new System.Drawing.Size(260, 23);
            this.quizStartButton.TabIndex = 0;
            this.quizStartButton.Text = "Start the Quiz";
            this.quizStartButton.UseVisualStyleBackColor = true;
            this.quizStartButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // quizStartText
            // 
            this.quizStartText.Enabled = false;
            this.quizStartText.Location = new System.Drawing.Point(33, 29);
            this.quizStartText.Multiline = true;
            this.quizStartText.Name = "quizStartText";
            this.quizStartText.Size = new System.Drawing.Size(219, 115);
            this.quizStartText.TabIndex = 1;
            this.quizStartText.Text = resources.GetString("quizStartText.Text");
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 226);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(260, 23);
            this.progressBar1.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(56, 171);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(216, 20);
            this.textBox1.TabIndex = 3;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 174);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Name:";
            // 
            // QuizMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.quizStartText);
            this.Controls.Add(this.quizStartButton);
            this.Name = "QuizMainForm";
            this.Text = "QuizStartForm";
            this.Load += new System.EventHandler(this.QuizStartForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button quizStartButton;
        private System.Windows.Forms.TextBox quizStartText;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
    }
}