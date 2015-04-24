namespace DF_FaceTracking.cs
{
    partial class AnnoForm
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
            this.stampLabel = new System.Windows.Forms.Label();
            this.stampButton = new System.Windows.Forms.Button();
            this.stampBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // stampLabel
            // 
            this.stampLabel.AutoSize = true;
            this.stampLabel.Location = new System.Drawing.Point(57, 9);
            this.stampLabel.Name = "stampLabel";
            this.stampLabel.Size = new System.Drawing.Size(77, 13);
            this.stampLabel.TabIndex = 0;
            this.stampLabel.Text = "TIMESTAMPS";
            // 
            // stampButton
            // 
            this.stampButton.Location = new System.Drawing.Point(15, 321);
            this.stampButton.Name = "stampButton";
            this.stampButton.Size = new System.Drawing.Size(161, 23);
            this.stampButton.TabIndex = 1;
            this.stampButton.Text = "Confused";
            this.stampButton.UseVisualStyleBackColor = true;
            this.stampButton.Click += new System.EventHandler(this.stampButton_Click);
            // 
            // stampBox
            // 
            this.stampBox.Location = new System.Drawing.Point(15, 26);
            this.stampBox.Multiline = true;
            this.stampBox.Name = "stampBox";
            this.stampBox.Size = new System.Drawing.Size(161, 289);
            this.stampBox.TabIndex = 2;
            // 
            // AnnoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(197, 356);
            this.Controls.Add(this.stampBox);
            this.Controls.Add(this.stampButton);
            this.Controls.Add(this.stampLabel);
            this.Name = "AnnoForm";
            this.Text = "Annotation Form";
            this.Load += new System.EventHandler(this.AnnoForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label stampLabel;
        private System.Windows.Forms.Button stampButton;
        private System.Windows.Forms.TextBox stampBox;
    }
}