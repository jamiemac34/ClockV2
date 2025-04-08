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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialougeAdd));
            this.DTPicker = new System.Windows.Forms.DateTimePicker();
            this.btnFormAdd = new System.Windows.Forms.Button();
            this.lblAddText = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.CBTriggerTime = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // DTPicker
            // 
            this.DTPicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DTPicker.Location = new System.Drawing.Point(12, 79);
            this.DTPicker.MinDate = new System.DateTime(2025, 4, 6, 0, 0, 0, 0);
            this.DTPicker.Name = "DTPicker";
            this.DTPicker.Size = new System.Drawing.Size(200, 20);
            this.DTPicker.TabIndex = 0;
            // 
            // btnFormAdd
            // 
            this.btnFormAdd.Location = new System.Drawing.Point(218, 103);
            this.btnFormAdd.Name = "btnFormAdd";
            this.btnFormAdd.Size = new System.Drawing.Size(75, 23);
            this.btnFormAdd.TabIndex = 1;
            this.btnFormAdd.Text = "Add";
            this.btnFormAdd.UseVisualStyleBackColor = true;
            this.btnFormAdd.Click += new System.EventHandler(this.BtnFormAddClick);
            // 
            // lblAddText
            // 
            this.lblAddText.AutoSize = true;
            this.lblAddText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblAddText.Location = new System.Drawing.Point(12, 9);
            this.lblAddText.Name = "lblAddText";
            this.lblAddText.Size = new System.Drawing.Size(101, 17);
            this.lblAddText.TabIndex = 2;
            this.lblAddText.Text = "Add new alarm";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(12, 29);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(200, 20);
            this.txtName.TabIndex = 3;
            this.txtName.Enter += new System.EventHandler(this.TxtNameFocusGot);
            this.txtName.Leave += new System.EventHandler(this.TxtNameFocusLost);
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(12, 55);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(200, 20);
            this.txtDescription.TabIndex = 4;
            this.txtDescription.Enter += new System.EventHandler(this.TxtDescriptionFocusGot);
            this.txtDescription.Leave += new System.EventHandler(this.TxtDescriptionFocusLost);
            // 
            // CBTriggerTime
            // 
            this.CBTriggerTime.FormattingEnabled = true;
            this.CBTriggerTime.Location = new System.Drawing.Point(12, 105);
            this.CBTriggerTime.Name = "CBTriggerTime";
            this.CBTriggerTime.Size = new System.Drawing.Size(200, 21);
            this.CBTriggerTime.TabIndex = 5;
            // 
            // DialougeAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 136);
            this.Controls.Add(this.CBTriggerTime);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblAddText);
            this.Controls.Add(this.btnFormAdd);
            this.Controls.Add(this.DTPicker);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DialougeAdd";
            this.Text = "Add Alarm";
            this.Activated += new System.EventHandler(this.FormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker DTPicker;
        private System.Windows.Forms.Button btnFormAdd;
        private System.Windows.Forms.Label lblAddText;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.ComboBox CBTriggerTime;
    }
}