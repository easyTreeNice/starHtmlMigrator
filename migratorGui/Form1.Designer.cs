namespace migratorGui
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(726, 362);
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
    }
}

