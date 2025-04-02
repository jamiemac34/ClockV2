namespace ClockV2.View
{
    partial class DialougeAdd
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
            this.DTPicker = new System.Windows.Forms.DateTimePicker();
            this.btnFormAdd = new System.Windows.Forms.Button();
            this.lblAddText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // dateTimePicker1
            // 
            this.DTPicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DTPicker.Location = new System.Drawing.Point(12, 34);
            this.DTPicker.Name = "dateTimePicker1";
            this.DTPicker.Size = new System.Drawing.Size(200, 20);
            this.DTPicker.TabIndex = 0;
            // 
            // button1
            // 
            this.btnFormAdd.Location = new System.Drawing.Point(218, 31);
            this.btnFormAdd.Name = "button1";
            this.btnFormAdd.Size = new System.Drawing.Size(75, 23);
            this.btnFormAdd.TabIndex = 1;
            this.btnFormAdd.Text = "Add";
            this.btnFormAdd.UseVisualStyleBackColor = true;
            this.btnFormAdd.Click += new System.EventHandler(this.btnFormAddClick);
            // 
            // label1
            // 
            this.lblAddText.AutoSize = true;
            this.lblAddText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblAddText.Location = new System.Drawing.Point(12, 9);
            this.lblAddText.Name = "label1";
            this.lblAddText.Size = new System.Drawing.Size(101, 17);
            this.lblAddText.TabIndex = 2;
            this.lblAddText.Text = "Add new alarm";
            // 
            // DialougeAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 66);
            this.Controls.Add(this.lblAddText);
            this.Controls.Add(this.btnFormAdd);
            this.Controls.Add(this.DTPicker);
            this.Name = "DialougeAdd";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.DialougeAdd_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker DTPicker;
        private System.Windows.Forms.Button btnFormAdd;
        private System.Windows.Forms.Label lblAddText;
    }
}