namespace WindowsInput.Tests.UnicodeText
{
    partial class UnicodeTestForm
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
            this.RecievedTextBox = new System.Windows.Forms.TextBox();
            this.ExpectedTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // RecievedTextBox
            // 
            this.RecievedTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.RecievedTextBox.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RecievedTextBox.Location = new System.Drawing.Point(0, 0);
            this.RecievedTextBox.Name = "RecievedTextBox";
            this.RecievedTextBox.Size = new System.Drawing.Size(981, 39);
            this.RecievedTextBox.TabIndex = 0;
            // 
            // ExpectedTextBox
            // 
            this.ExpectedTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ExpectedTextBox.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExpectedTextBox.Location = new System.Drawing.Point(0, 40);
            this.ExpectedTextBox.Name = "ExpectedTextBox";
            this.ExpectedTextBox.ReadOnly = true;
            this.ExpectedTextBox.Size = new System.Drawing.Size(981, 39);
            this.ExpectedTextBox.TabIndex = 1;
            // 
            // UnicodeTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(981, 79);
            this.Controls.Add(this.ExpectedTextBox);
            this.Controls.Add(this.RecievedTextBox);
            this.Name = "UnicodeTestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UnicodeTestForm";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox RecievedTextBox;
        private System.Windows.Forms.TextBox ExpectedTextBox;
    }
}