namespace DF_FaceTracking.cs
{
    partial class QuizStartForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuizStartForm));
            this.quizStartButton = new System.Windows.Forms.Button();
            this.quizStartText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // quizStartButton
            // 
            this.quizStartButton.Location = new System.Drawing.Point(33, 201);
            this.quizStartButton.Name = "quizStartButton";
            this.quizStartButton.Size = new System.Drawing.Size(97, 23);
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
            // QuizStartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.quizStartText);
            this.Controls.Add(this.quizStartButton);
            this.Name = "QuizStartForm";
            this.Text = "QuizStartForm";
            this.Load += new System.EventHandler(this.QuizStartForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button quizStartButton;
        private System.Windows.Forms.TextBox quizStartText;
    }
}