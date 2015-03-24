namespace DF_FaceTracking.cs
{
    partial class QuizQuestion
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
            this.questionBox = new System.Windows.Forms.TextBox();
            this.answerBox = new System.Windows.Forms.TextBox();
            this.submitButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // questionBox
            // 
            this.questionBox.Location = new System.Drawing.Point(12, 28);
            this.questionBox.Multiline = true;
            this.questionBox.Name = "questionBox";
            this.questionBox.Size = new System.Drawing.Size(377, 176);
            this.questionBox.TabIndex = 0;
            // 
            // answerBox
            // 
            this.answerBox.Location = new System.Drawing.Point(13, 230);
            this.answerBox.Multiline = true;
            this.answerBox.Name = "answerBox";
            this.answerBox.Size = new System.Drawing.Size(377, 119);
            this.answerBox.TabIndex = 1;
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(13, 355);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(376, 23);
            this.submitButton.TabIndex = 2;
            this.submitButton.Text = "Submit Answer";
            this.submitButton.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 384);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(377, 23);
            this.progressBar1.TabIndex = 3;
            // 
            // QuizQuestion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 419);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.answerBox);
            this.Controls.Add(this.questionBox);
            this.Name = "QuizQuestion";
            this.Text = "QuizQuestion";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox questionBox;
        private System.Windows.Forms.TextBox answerBox;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}