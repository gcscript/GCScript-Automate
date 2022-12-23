namespace GCScript_Automate
{
    partial class frm_Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Main));
            this.txt_Status = new System.Windows.Forms.TextBox();
            this.txt_ErrorCode = new System.Windows.Forms.TextBox();
            this.txt_Message = new System.Windows.Forms.TextBox();
            this.lbl_dev = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txt_Status
            // 
            this.txt_Status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_Status.Location = new System.Drawing.Point(5, 5);
            this.txt_Status.MaxLength = 20;
            this.txt_Status.Name = "txt_Status";
            this.txt_Status.ReadOnly = true;
            this.txt_Status.Size = new System.Drawing.Size(110, 22);
            this.txt_Status.TabIndex = 3;
            this.txt_Status.TabStop = false;
            this.txt_Status.Text = "Running";
            this.txt_Status.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt_ErrorCode
            // 
            this.txt_ErrorCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_ErrorCode.Location = new System.Drawing.Point(121, 5);
            this.txt_ErrorCode.MaxLength = 6;
            this.txt_ErrorCode.Name = "txt_ErrorCode";
            this.txt_ErrorCode.ReadOnly = true;
            this.txt_ErrorCode.Size = new System.Drawing.Size(110, 22);
            this.txt_ErrorCode.TabIndex = 4;
            this.txt_ErrorCode.TabStop = false;
            this.txt_ErrorCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt_Message
            // 
            this.txt_Message.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_Message.Location = new System.Drawing.Point(5, 33);
            this.txt_Message.MaxLength = 1000;
            this.txt_Message.Name = "txt_Message";
            this.txt_Message.ReadOnly = true;
            this.txt_Message.Size = new System.Drawing.Size(226, 22);
            this.txt_Message.TabIndex = 5;
            this.txt_Message.TabStop = false;
            this.txt_Message.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lbl_dev
            // 
            this.lbl_dev.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbl_dev.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl_dev.Location = new System.Drawing.Point(0, 59);
            this.lbl_dev.Name = "lbl_dev";
            this.lbl_dev.Size = new System.Drawing.Size(236, 20);
            this.lbl_dev.TabIndex = 6;
            this.lbl_dev.Text = "Developed by GCScript";
            this.lbl_dev.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 79);
            this.Controls.Add(this.lbl_dev);
            this.Controls.Add(this.txt_Message);
            this.Controls.Add(this.txt_ErrorCode);
            this.Controls.Add(this.txt_Status);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frm_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "GCScript Automate";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Shown += new System.EventHandler(this.frm_Main_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TextBox txt_Status;
        private TextBox txt_ErrorCode;
        private TextBox txt_Message;
        private Label lbl_dev;
    }
}