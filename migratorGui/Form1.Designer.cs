﻿namespace migratorGui
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.exportFolder = new System.Windows.Forms.TextBox();
            this.findFilesButton = new System.Windows.Forms.Button();
            this.fileList = new System.Windows.Forms.ListBox();
            this.processFilesButton = new System.Windows.Forms.Button();
            this.processedFiles = new System.Windows.Forms.ListBox();
            this.messages = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ToRIS = new System.Windows.Forms.RadioButton();
            this.ToJSON = new System.Windows.Forms.RadioButton();
            this.runAutomatedButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(284, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Folder containing freshly-created STAR export files (HTML)";
            // 
            // exportFolder
            // 
            this.exportFolder.Location = new System.Drawing.Point(13, 30);
            this.exportFolder.Name = "exportFolder";
            this.exportFolder.Size = new System.Drawing.Size(284, 20);
            this.exportFolder.TabIndex = 1;
            this.exportFolder.TextChanged += new System.EventHandler(this.exportFolder_TextChanged);
            // 
            // findFilesButton
            // 
            this.findFilesButton.Location = new System.Drawing.Point(304, 30);
            this.findFilesButton.Name = "findFilesButton";
            this.findFilesButton.Size = new System.Drawing.Size(117, 23);
            this.findFilesButton.TabIndex = 2;
            this.findFilesButton.Text = "Find files in folder";
            this.findFilesButton.UseVisualStyleBackColor = true;
            this.findFilesButton.Click += new System.EventHandler(this.findFilesButton_Click);
            // 
            // fileList
            // 
            this.fileList.FormattingEnabled = true;
            this.fileList.Location = new System.Drawing.Point(13, 57);
            this.fileList.Name = "fileList";
            this.fileList.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.fileList.Size = new System.Drawing.Size(284, 186);
            this.fileList.TabIndex = 3;
            // 
            // processFilesButton
            // 
            this.processFilesButton.Enabled = false;
            this.processFilesButton.Location = new System.Drawing.Point(304, 57);
            this.processFilesButton.Name = "processFilesButton";
            this.processFilesButton.Size = new System.Drawing.Size(117, 23);
            this.processFilesButton.TabIndex = 4;
            this.processFilesButton.Text = "Add Bootstrap >>";
            this.processFilesButton.UseVisualStyleBackColor = true;
            this.processFilesButton.Click += new System.EventHandler(this.processFiles_Click);
            // 
            // processedFiles
            // 
            this.processedFiles.FormattingEnabled = true;
            this.processedFiles.Location = new System.Drawing.Point(427, 57);
            this.processedFiles.Name = "processedFiles";
            this.processedFiles.Size = new System.Drawing.Size(284, 186);
            this.processedFiles.TabIndex = 5;
            // 
            // messages
            // 
            this.messages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messages.Location = new System.Drawing.Point(13, 250);
            this.messages.Multiline = true;
            this.messages.Name = "messages";
            this.messages.Size = new System.Drawing.Size(698, 100);
            this.messages.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ToRIS);
            this.groupBox1.Controls.Add(this.ToJSON);
            this.groupBox1.Location = new System.Drawing.Point(304, 86);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(117, 72);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // ToRIS
            // 
            this.ToRIS.AutoSize = true;
            this.ToRIS.Location = new System.Drawing.Point(7, 43);
            this.ToRIS.Name = "ToRIS";
            this.ToRIS.Size = new System.Drawing.Size(77, 17);
            this.ToRIS.TabIndex = 0;
            this.ToRIS.Text = "Create RIS";
            this.ToRIS.UseVisualStyleBackColor = true;
            // 
            // ToJSON
            // 
            this.ToJSON.AutoSize = true;
            this.ToJSON.Checked = true;
            this.ToJSON.Location = new System.Drawing.Point(7, 19);
            this.ToJSON.Name = "ToJSON";
            this.ToJSON.Size = new System.Drawing.Size(87, 17);
            this.ToJSON.TabIndex = 0;
            this.ToJSON.TabStop = true;
            this.ToJSON.Text = "Create JSON";
            this.ToJSON.UseVisualStyleBackColor = true;
            // 
            // runAutomatedButton
            // 
            this.runAutomatedButton.Location = new System.Drawing.Point(305, 170);
            this.runAutomatedButton.Name = "runAutomatedButton";
            this.runAutomatedButton.Size = new System.Drawing.Size(117, 23);
            this.runAutomatedButton.TabIndex = 8;
            this.runAutomatedButton.Text = "Run Automated >>";
            this.runAutomatedButton.UseVisualStyleBackColor = true;
            this.runAutomatedButton.Click += new System.EventHandler(this.runAutomatedButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(726, 362);
            this.Controls.Add(this.runAutomatedButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.messages);
            this.Controls.Add(this.processedFiles);
            this.Controls.Add(this.processFilesButton);
            this.Controls.Add(this.fileList);
            this.Controls.Add(this.findFilesButton);
            this.Controls.Add(this.exportFolder);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(7420, 401);
            this.MinimumSize = new System.Drawing.Size(742, 401);
            this.Name = "Form1";
            this.Text = "STAR migrator GUI";
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox exportFolder;
        private System.Windows.Forms.Button findFilesButton;
        private System.Windows.Forms.ListBox fileList;
        private System.Windows.Forms.Button processFilesButton;
        private System.Windows.Forms.ListBox processedFiles;
        private System.Windows.Forms.TextBox messages;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton ToRIS;
        private System.Windows.Forms.RadioButton ToJSON;
        private System.Windows.Forms.Button runAutomatedButton;
    }
}

