﻿namespace DF_FaceTracking.cs
{
    partial class MainForm
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
            this.Start = new System.Windows.Forms.Button();
            this.Stop = new System.Windows.Forms.Button();
            this.sourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moduleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.colorResolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Live = new System.Windows.Forms.ToolStripMenuItem();
            this.Playback = new System.Windows.Forms.ToolStripMenuItem();
            this.Record = new System.Windows.Forms.ToolStripMenuItem();
            this.quizToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.questionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Status2 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.AlertsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Scale = new System.Windows.Forms.CheckBox();
            this.Panel2 = new System.Windows.Forms.PictureBox();
            this.Recognition = new System.Windows.Forms.CheckBox();
            this.RegisterUser = new System.Windows.Forms.Button();
            this.UnregisterUser = new System.Windows.Forms.Button();
            this.NumDetectionText = new System.Windows.Forms.TextBox();
            this.NumLandmarksText = new System.Windows.Forms.TextBox();
            this.NumPoseText = new System.Windows.Forms.TextBox();
            this.NumExpressionsText = new System.Windows.Forms.TextBox();
            this.Detection = new System.Windows.Forms.CheckBox();
            this.Landmarks = new System.Windows.Forms.CheckBox();
            this.Pose = new System.Windows.Forms.CheckBox();
            this.Expressions = new System.Windows.Forms.CheckBox();
            this.Pulse = new System.Windows.Forms.CheckBox();
            this.NumPulseText = new System.Windows.Forms.TextBox();
            this.annoBox = new System.Windows.Forms.CheckBox();
            this.MainMenu.SuspendLayout();
            this.Status2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Panel2)).BeginInit();
            this.SuspendLayout();
            // 
            // Start
            // 
            this.Start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Start.Location = new System.Drawing.Point(820, 225);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(80, 23);
            this.Start.TabIndex = 2;
            this.Start.Text = "Start";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // Stop
            // 
            this.Stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Stop.Enabled = false;
            this.Stop.Location = new System.Drawing.Point(820, 254);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(80, 23);
            this.Stop.TabIndex = 3;
            this.Stop.Text = "Stop";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // sourceToolStripMenuItem
            // 
            this.sourceToolStripMenuItem.Name = "sourceToolStripMenuItem";
            this.sourceToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.sourceToolStripMenuItem.Text = "Device";
            // 
            // moduleToolStripMenuItem
            // 
            this.moduleToolStripMenuItem.Name = "moduleToolStripMenuItem";
            this.moduleToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.moduleToolStripMenuItem.Text = "Module";
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sourceToolStripMenuItem,
            this.colorResolutionToolStripMenuItem,
            this.moduleToolStripMenuItem,
            this.ProfileToolStripMenuItem,
            this.modeToolStripMenuItem,
            this.quizToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.MainMenu.Size = new System.Drawing.Size(941, 24);
            this.MainMenu.TabIndex = 0;
            this.MainMenu.Text = "MainMenu";
            // 
            // colorResolutionToolStripMenuItem
            // 
            this.colorResolutionToolStripMenuItem.Name = "colorResolutionToolStripMenuItem";
            this.colorResolutionToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.colorResolutionToolStripMenuItem.Text = "Color";
            // 
            // ProfileToolStripMenuItem
            // 
            this.ProfileToolStripMenuItem.Name = "ProfileToolStripMenuItem";
            this.ProfileToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.ProfileToolStripMenuItem.Text = "Profile";
            // 
            // modeToolStripMenuItem
            // 
            this.modeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Live,
            this.Playback,
            this.Record});
            this.modeToolStripMenuItem.Name = "modeToolStripMenuItem";
            this.modeToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.modeToolStripMenuItem.Text = "Mode";
            // 
            // Live
            // 
            this.Live.Checked = true;
            this.Live.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Live.Name = "Live";
            this.Live.Size = new System.Drawing.Size(121, 22);
            this.Live.Text = "Live";
            this.Live.Click += new System.EventHandler(this.Live_Click);
            // 
            // Playback
            // 
            this.Playback.Name = "Playback";
            this.Playback.Size = new System.Drawing.Size(121, 22);
            this.Playback.Text = "Playback";
            this.Playback.Click += new System.EventHandler(this.Playback_Click);
            // 
            // Record
            // 
            this.Record.Name = "Record";
            this.Record.Size = new System.Drawing.Size(121, 22);
            this.Record.Text = "Record";
            this.Record.Click += new System.EventHandler(this.Record_Click);
            // 
            // quizToolStripMenuItem
            // 
            this.quizToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem});
            this.quizToolStripMenuItem.Name = "quizToolStripMenuItem";
            this.quizToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.quizToolStripMenuItem.Text = "Quiz";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.questionsToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // questionsToolStripMenuItem
            // 
            this.questionsToolStripMenuItem.Name = "questionsToolStripMenuItem";
            this.questionsToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.questionsToolStripMenuItem.Text = "Questions";
            this.questionsToolStripMenuItem.Click += new System.EventHandler(this.questionsToolStripMenuItem_Click);
            // 
            // Status2
            // 
            this.Status2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.AlertsLabel});
            this.Status2.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.Status2.Location = new System.Drawing.Point(0, 481);
            this.Status2.Name = "Status2";
            this.Status2.Size = new System.Drawing.Size(941, 20);
            this.Status2.TabIndex = 25;
            this.Status2.Text = "Status2";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Padding = new System.Windows.Forms.Padding(0, 0, 50, 0);
            this.StatusLabel.Size = new System.Drawing.Size(73, 15);
            this.StatusLabel.Text = "OK";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AlertsLabel
            // 
            this.AlertsLabel.AutoSize = false;
            this.AlertsLabel.Name = "AlertsLabel";
            this.AlertsLabel.Size = new System.Drawing.Size(200, 15);
            this.AlertsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Scale
            // 
            this.Scale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Scale.AutoSize = true;
            this.Scale.Checked = true;
            this.Scale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Scale.Location = new System.Drawing.Point(820, 27);
            this.Scale.Name = "Scale";
            this.Scale.Size = new System.Drawing.Size(53, 17);
            this.Scale.TabIndex = 26;
            this.Scale.Text = "Scale";
            this.Scale.UseVisualStyleBackColor = true;
            // 
            // Panel2
            // 
            this.Panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Panel2.ErrorImage = null;
            this.Panel2.InitialImage = null;
            this.Panel2.Location = new System.Drawing.Point(12, 27);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(802, 444);
            this.Panel2.TabIndex = 27;
            this.Panel2.TabStop = false;
            this.Panel2.Click += new System.EventHandler(this.Panel2_Click);
            // 
            // Recognition
            // 
            this.Recognition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Recognition.AutoSize = true;
            this.Recognition.Location = new System.Drawing.Point(820, 188);
            this.Recognition.Name = "Recognition";
            this.Recognition.Size = new System.Drawing.Size(83, 17);
            this.Recognition.TabIndex = 33;
            this.Recognition.Text = "Recognition";
            this.Recognition.UseVisualStyleBackColor = true;
            // 
            // RegisterUser
            // 
            this.RegisterUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RegisterUser.Enabled = false;
            this.RegisterUser.Location = new System.Drawing.Point(820, 283);
            this.RegisterUser.Name = "RegisterUser";
            this.RegisterUser.Size = new System.Drawing.Size(80, 23);
            this.RegisterUser.TabIndex = 34;
            this.RegisterUser.Text = "Register";
            this.RegisterUser.UseVisualStyleBackColor = true;
            this.RegisterUser.Click += new System.EventHandler(this.RegisterUser_Click);
            // 
            // UnregisterUser
            // 
            this.UnregisterUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UnregisterUser.Enabled = false;
            this.UnregisterUser.Location = new System.Drawing.Point(820, 312);
            this.UnregisterUser.Name = "UnregisterUser";
            this.UnregisterUser.Size = new System.Drawing.Size(80, 23);
            this.UnregisterUser.TabIndex = 35;
            this.UnregisterUser.Text = "Unregister";
            this.UnregisterUser.UseVisualStyleBackColor = true;
            this.UnregisterUser.Click += new System.EventHandler(this.UnregisterUser_Click);
            // 
            // NumDetectionText
            // 
            this.NumDetectionText.AccessibleName = "";
            this.NumDetectionText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NumDetectionText.Location = new System.Drawing.Point(906, 73);
            this.NumDetectionText.Name = "NumDetectionText";
            this.NumDetectionText.Size = new System.Drawing.Size(21, 20);
            this.NumDetectionText.TabIndex = 36;
            this.NumDetectionText.TextChanged += new System.EventHandler(this.NumDetectionText_TextChanged);
            // 
            // NumLandmarksText
            // 
            this.NumLandmarksText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NumLandmarksText.Location = new System.Drawing.Point(906, 96);
            this.NumLandmarksText.Name = "NumLandmarksText";
            this.NumLandmarksText.Size = new System.Drawing.Size(21, 20);
            this.NumLandmarksText.TabIndex = 37;
            // 
            // NumPoseText
            // 
            this.NumPoseText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NumPoseText.Location = new System.Drawing.Point(906, 119);
            this.NumPoseText.Name = "NumPoseText";
            this.NumPoseText.Size = new System.Drawing.Size(21, 20);
            this.NumPoseText.TabIndex = 38;
            // 
            // NumExpressionsText
            // 
            this.NumExpressionsText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NumExpressionsText.Location = new System.Drawing.Point(906, 142);
            this.NumExpressionsText.Name = "NumExpressionsText";
            this.NumExpressionsText.Size = new System.Drawing.Size(21, 20);
            this.NumExpressionsText.TabIndex = 45;
            // 
            // Detection
            // 
            this.Detection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Detection.AutoSize = true;
            this.Detection.Location = new System.Drawing.Point(820, 73);
            this.Detection.Name = "Detection";
            this.Detection.Size = new System.Drawing.Size(72, 17);
            this.Detection.TabIndex = 46;
            this.Detection.Text = "Detection";
            this.Detection.UseVisualStyleBackColor = true;
            this.Detection.CheckedChanged += new System.EventHandler(this.Detection_CheckedChanged);
            // 
            // Landmarks
            // 
            this.Landmarks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Landmarks.AutoSize = true;
            this.Landmarks.Location = new System.Drawing.Point(820, 96);
            this.Landmarks.Name = "Landmarks";
            this.Landmarks.Size = new System.Drawing.Size(78, 17);
            this.Landmarks.TabIndex = 47;
            this.Landmarks.Text = "Landmarks";
            this.Landmarks.UseVisualStyleBackColor = true;
            this.Landmarks.CheckedChanged += new System.EventHandler(this.Landmarks_CheckedChanged);
            // 
            // Pose
            // 
            this.Pose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Pose.AutoSize = true;
            this.Pose.Location = new System.Drawing.Point(820, 119);
            this.Pose.Name = "Pose";
            this.Pose.Size = new System.Drawing.Size(50, 17);
            this.Pose.TabIndex = 48;
            this.Pose.Text = "Pose";
            this.Pose.UseVisualStyleBackColor = true;
            this.Pose.CheckedChanged += new System.EventHandler(this.Pose_CheckedChanged);
            // 
            // Expressions
            // 
            this.Expressions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Expressions.AutoSize = true;
            this.Expressions.Location = new System.Drawing.Point(820, 142);
            this.Expressions.Name = "Expressions";
            this.Expressions.Size = new System.Drawing.Size(82, 17);
            this.Expressions.TabIndex = 49;
            this.Expressions.Text = "Expressions";
            this.Expressions.UseVisualStyleBackColor = true;
            this.Expressions.CheckedChanged += new System.EventHandler(this.Expressions_CheckedChanged);
            // 
            // Pulse
            // 
            this.Pulse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Pulse.AutoSize = true;
            this.Pulse.Location = new System.Drawing.Point(820, 165);
            this.Pulse.Name = "Pulse";
            this.Pulse.Size = new System.Drawing.Size(52, 17);
            this.Pulse.TabIndex = 51;
            this.Pulse.Text = "Pulse";
            this.Pulse.UseVisualStyleBackColor = true;
            this.Pulse.CheckedChanged += new System.EventHandler(this.Pulse_CheckedChanged);
            // 
            // NumPulseText
            // 
            this.NumPulseText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NumPulseText.Location = new System.Drawing.Point(906, 165);
            this.NumPulseText.Name = "NumPulseText";
            this.NumPulseText.Size = new System.Drawing.Size(21, 20);
            this.NumPulseText.TabIndex = 52;
            // 
            // annoBox
            // 
            this.annoBox.AutoSize = true;
            this.annoBox.Location = new System.Drawing.Point(821, 358);
            this.annoBox.Name = "annoBox";
            this.annoBox.Size = new System.Drawing.Size(107, 17);
            this.annoBox.TabIndex = 53;
            this.annoBox.Text = "Annotation Mode";
            this.annoBox.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 501);
            this.Controls.Add(this.annoBox);
            this.Controls.Add(this.NumPulseText);
            this.Controls.Add(this.Pulse);
            this.Controls.Add(this.Expressions);
            this.Controls.Add(this.Pose);
            this.Controls.Add(this.Landmarks);
            this.Controls.Add(this.Detection);
            this.Controls.Add(this.NumExpressionsText);
            this.Controls.Add(this.NumPoseText);
            this.Controls.Add(this.NumLandmarksText);
            this.Controls.Add(this.NumDetectionText);
            this.Controls.Add(this.UnregisterUser);
            this.Controls.Add(this.RegisterUser);
            this.Controls.Add(this.Recognition);
            this.Controls.Add(this.Panel2);
            this.Controls.Add(this.Scale);
            this.Controls.Add(this.Status2);
            this.Controls.Add(this.Stop);
            this.Controls.Add(this.Start);
            this.Controls.Add(this.MainMenu);
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.Text = "Intel(R) RealSense(TM) SDK: Face Tracking";
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.Status2.ResumeLayout(false);
            this.Status2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Panel2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.ToolStripMenuItem sourceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moduleToolStripMenuItem;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.StatusStrip Status2;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.CheckBox Scale;
        private System.Windows.Forms.PictureBox Panel2;
        private System.Windows.Forms.ToolStripMenuItem modeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Live;
        private System.Windows.Forms.ToolStripMenuItem Playback;
        private System.Windows.Forms.ToolStripMenuItem Record;
        private System.Windows.Forms.ToolStripMenuItem ProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel AlertsLabel;
        private System.Windows.Forms.CheckBox Recognition;
        private System.Windows.Forms.Button RegisterUser;
        private System.Windows.Forms.Button UnregisterUser;
        private System.Windows.Forms.ToolStripMenuItem colorResolutionToolStripMenuItem;
        private System.Windows.Forms.TextBox NumDetectionText;
        private System.Windows.Forms.TextBox NumLandmarksText;
        private System.Windows.Forms.TextBox NumPoseText;
        private System.Windows.Forms.TextBox NumExpressionsText;
        private System.Windows.Forms.CheckBox Detection;
        private System.Windows.Forms.CheckBox Landmarks;
        private System.Windows.Forms.CheckBox Pose;
        private System.Windows.Forms.CheckBox Expressions;
        private System.Windows.Forms.CheckBox Pulse;
        private System.Windows.Forms.TextBox NumPulseText;
        private System.Windows.Forms.ToolStripMenuItem quizToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem questionsToolStripMenuItem;
        private System.Windows.Forms.CheckBox annoBox;
    }
}