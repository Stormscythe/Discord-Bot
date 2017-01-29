namespace MyAwesomeDiscordBot
{
    partial class AdminPanel
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
            this.kickButton = new System.Windows.Forms.Button();
            this.kickTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // kickButton
            // 
            this.kickButton.Location = new System.Drawing.Point(143, 43);
            this.kickButton.Name = "kickButton";
            this.kickButton.Size = new System.Drawing.Size(75, 23);
            this.kickButton.TabIndex = 0;
            this.kickButton.Text = "Kick User";
            this.kickButton.UseVisualStyleBackColor = true;
            this.kickButton.Click += new System.EventHandler(this.kickButton_Click);
            // 
            // kickTextBox
            // 
            this.kickTextBox.Location = new System.Drawing.Point(24, 45);
            this.kickTextBox.Name = "kickTextBox";
            this.kickTextBox.Size = new System.Drawing.Size(100, 20);
            this.kickTextBox.TabIndex = 1;
            this.kickTextBox.TextChanged += new System.EventHandler(this.kickTextBox_TextChanged);
            // 
            // AdminPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 437);
            this.Controls.Add(this.kickTextBox);
            this.Controls.Add(this.kickButton);
            this.Name = "AdminPanel";
            this.Text = "AdminPanel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button kickButton;
        private System.Windows.Forms.TextBox kickTextBox;
    }
}