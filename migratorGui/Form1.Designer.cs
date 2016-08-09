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
            this.fileList = new System.Windows.Forms.ListBox();
            this.processedFiles = new System.Windows.Forms.ListBox();
            this.messages = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.findFilesInFolderTree = new System.Windows.Forms.Button();
            this.RisIdentifierPattern = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.runAutomatedButton = new System.Windows.Forms.Button();
            this.JsonIdentifierPattern = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.RisJsonIdentifierPattern = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(328, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Top-level folder containing freshly-created STAR export files (HTML)";
            // 
            // exportFolder
            // 
            this.exportFolder.Font = new System.Drawing.Font("Consolas", 8F);
            this.exportFolder.Location = new System.Drawing.Point(13, 30);
            this.exportFolder.Name = "exportFolder";
            this.exportFolder.Size = new System.Drawing.Size(251, 20);
            this.exportFolder.TabIndex = 1;
            this.exportFolder.TextChanged += new System.EventHandler(this.exportFolder_TextChanged);
            // 
            // fileList
            // 
            this.fileList.Font = new System.Drawing.Font("Consolas", 8F);
            this.fileList.FormattingEnabled = true;
            this.fileList.Location = new System.Drawing.Point(13, 57);
            this.fileList.Name = "fileList";
            this.fileList.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.fileList.Size = new System.Drawing.Size(251, 290);
            this.fileList.TabIndex = 3;
            this.fileList.SelectedIndexChanged += new System.EventHandler(this.fileList_SelectedIndexChanged);
            // 
            // processedFiles
            // 
            this.processedFiles.Font = new System.Drawing.Font("Consolas", 8F);
            this.processedFiles.FormattingEnabled = true;
            this.processedFiles.Location = new System.Drawing.Point(450, 57);
            this.processedFiles.Name = "processedFiles";
            this.processedFiles.Size = new System.Drawing.Size(251, 290);
            this.processedFiles.TabIndex = 5;
            // 
            // messages
            // 
            this.messages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messages.Font = new System.Drawing.Font("Consolas", 8F);
            this.messages.Location = new System.Drawing.Point(13, 353);
            this.messages.Multiline = true;
            this.messages.Name = "messages";
            this.messages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.messages.Size = new System.Drawing.Size(698, 100);
            this.messages.TabIndex = 6;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.RisJsonIdentifierPattern);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.findFilesInFolderTree);
            this.groupBox2.Controls.Add(this.RisIdentifierPattern);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.runAutomatedButton);
            this.groupBox2.Controls.Add(this.JsonIdentifierPattern);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(270, 57);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.groupBox2.Size = new System.Drawing.Size(176, 246);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            // 
            // findFilesInFolderTree
            // 
            this.findFilesInFolderTree.Location = new System.Drawing.Point(11, 167);
            this.findFilesInFolderTree.Name = "findFilesInFolderTree";
            this.findFilesInFolderTree.Size = new System.Drawing.Size(154, 23);
            this.findFilesInFolderTree.TabIndex = 16;
            this.findFilesInFolderTree.Text = "Find files in folder tree";
            this.findFilesInFolderTree.UseVisualStyleBackColor = true;
            this.findFilesInFolderTree.Click += new System.EventHandler(this.findFilesInFolderTree_Click);
            // 
            // RisIdentifierPattern
            // 
            this.RisIdentifierPattern.Font = new System.Drawing.Font("Consolas", 8F);
            this.RisIdentifierPattern.Location = new System.Drawing.Point(11, 77);
            this.RisIdentifierPattern.Name = "RisIdentifierPattern";
            this.RisIdentifierPattern.Size = new System.Drawing.Size(154, 20);
            this.RisIdentifierPattern.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "RIS identifier pattern";
            // 
            // runAutomatedButton
            // 
            this.runAutomatedButton.Location = new System.Drawing.Point(11, 196);
            this.runAutomatedButton.Name = "runAutomatedButton";
            this.runAutomatedButton.Size = new System.Drawing.Size(154, 23);
            this.runAutomatedButton.TabIndex = 13;
            this.runAutomatedButton.Text = "Auto-process >>";
            this.runAutomatedButton.UseVisualStyleBackColor = true;
            this.runAutomatedButton.Click += new System.EventHandler(this.runAutomatedButton_Click);
            // 
            // JsonIdentifierPattern
            // 
            this.JsonIdentifierPattern.Font = new System.Drawing.Font("Consolas", 8F);
            this.JsonIdentifierPattern.Location = new System.Drawing.Point(11, 32);
            this.JsonIdentifierPattern.Name = "JsonIdentifierPattern";
            this.JsonIdentifierPattern.Size = new System.Drawing.Size(154, 20);
            this.JsonIdentifierPattern.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "JSON identifier pattern";
            // 
            // RisJsonIdentifierPattern
            // 
            this.RisJsonIdentifierPattern.Font = new System.Drawing.Font("Consolas", 8F);
            this.RisJsonIdentifierPattern.Location = new System.Drawing.Point(12, 121);
            this.RisJsonIdentifierPattern.Name = "RisJsonIdentifierPattern";
            this.RisJsonIdentifierPattern.Size = new System.Drawing.Size(153, 20);
            this.RisJsonIdentifierPattern.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "RIS-JSON identifier pattern";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(726, 464);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.messages);
            this.Controls.Add(this.processedFiles);
            this.Controls.Add(this.fileList);
            this.Controls.Add(this.exportFolder);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(7420, 503);
            this.MinimumSize = new System.Drawing.Size(742, 401);
            this.Name = "Form1";
            this.Text = "STAR migrator GUI";
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox exportFolder;
        private System.Windows.Forms.ListBox fileList;
        private System.Windows.Forms.ListBox processedFiles;
        private System.Windows.Forms.TextBox messages;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox RisIdentifierPattern;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button runAutomatedButton;
        private System.Windows.Forms.TextBox JsonIdentifierPattern;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button findFilesInFolderTree;
        private System.Windows.Forms.TextBox RisJsonIdentifierPattern;
        private System.Windows.Forms.Label label4;
    }
}

