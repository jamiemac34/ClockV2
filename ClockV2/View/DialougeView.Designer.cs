namespace ClockV2.View
{
    partial class DialougeView
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
            this.btnSave = new System.Windows.Forms.Button();
            this.lbAlarms = new System.Windows.Forms.ListBox();
            this.Remove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(262, 107);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lbAlarms
            // 
            this.lbAlarms.FormattingEnabled = true;
            this.lbAlarms.Location = new System.Drawing.Point(12, 12);
            this.lbAlarms.Name = "lbAlarms";
            this.lbAlarms.Size = new System.Drawing.Size(244, 147);
            this.lbAlarms.TabIndex = 2;
            // 
            // Remove
            // 
            this.Remove.Location = new System.Drawing.Point(262, 136);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(88, 23);
            this.Remove.TabIndex = 4;
            this.Remove.Text = "Remove";
            this.Remove.UseVisualStyleBackColor = true;
            // 
            // DialougeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 172);
            this.Controls.Add(this.Remove);
            this.Controls.Add(this.lbAlarms);
            this.Controls.Add(this.btnSave);
            this.Name = "DialougeView";
            this.Text = "DialougeView";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ListBox lbAlarms;
        private System.Windows.Forms.Button Remove;
    }
}