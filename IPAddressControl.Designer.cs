
namespace Kimono
{
    partial class IPAddressControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxIPAddrPart1 = new System.Windows.Forms.TextBox();
            this.textBoxIPAddrPart2 = new System.Windows.Forms.TextBox();
            this.textBoxIPAddrPart3 = new System.Windows.Forms.TextBox();
            this.textBoxIPAddrPart4 = new System.Windows.Forms.TextBox();
            this.textBoxDot1 = new System.Windows.Forms.TextBox();
            this.textBoxDot2 = new System.Windows.Forms.TextBox();
            this.textBoxDot3 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxIPAddrPart1
            // 
            this.textBoxIPAddrPart1.Location = new System.Drawing.Point(1, 0);
            this.textBoxIPAddrPart1.Name = "textBoxIPAddrPart1";
            this.textBoxIPAddrPart1.Size = new System.Drawing.Size(35, 20);
            this.textBoxIPAddrPart1.TabIndex = 0;
            this.textBoxIPAddrPart1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxIPAddrPart1_KeyPress);
            // 
            // textBoxIPAddrPart2
            // 
            this.textBoxIPAddrPart2.Location = new System.Drawing.Point(53, 0);
            this.textBoxIPAddrPart2.Name = "textBoxIPAddrPart2";
            this.textBoxIPAddrPart2.Size = new System.Drawing.Size(35, 20);
            this.textBoxIPAddrPart2.TabIndex = 1;
            this.textBoxIPAddrPart2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxIPAddrPart2_KeyPress);
            // 
            // textBoxIPAddrPart3
            // 
            this.textBoxIPAddrPart3.Location = new System.Drawing.Point(105, 0);
            this.textBoxIPAddrPart3.Name = "textBoxIPAddrPart3";
            this.textBoxIPAddrPart3.Size = new System.Drawing.Size(35, 20);
            this.textBoxIPAddrPart3.TabIndex = 2;
            this.textBoxIPAddrPart3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxIPAddrPart3_KeyPress);
            // 
            // textBoxIPAddrPart4
            // 
            this.textBoxIPAddrPart4.Location = new System.Drawing.Point(157, 0);
            this.textBoxIPAddrPart4.Name = "textBoxIPAddrPart4";
            this.textBoxIPAddrPart4.Size = new System.Drawing.Size(35, 20);
            this.textBoxIPAddrPart4.TabIndex = 3;
            this.textBoxIPAddrPart4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxIPAddrPart4_KeyPress);
            // 
            // textBoxDot1
            // 
            this.textBoxDot1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxDot1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDot1.Location = new System.Drawing.Point(31, -2);
            this.textBoxDot1.Name = "textBoxDot1";
            this.textBoxDot1.ReadOnly = true;
            this.textBoxDot1.Size = new System.Drawing.Size(27, 24);
            this.textBoxDot1.TabIndex = 4;
            this.textBoxDot1.Text = ".";
            this.textBoxDot1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxDot2
            // 
            this.textBoxDot2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxDot2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDot2.Location = new System.Drawing.Point(83, -2);
            this.textBoxDot2.Name = "textBoxDot2";
            this.textBoxDot2.ReadOnly = true;
            this.textBoxDot2.Size = new System.Drawing.Size(27, 24);
            this.textBoxDot2.TabIndex = 5;
            this.textBoxDot2.Text = ".";
            this.textBoxDot2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxDot3
            // 
            this.textBoxDot3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxDot3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDot3.Location = new System.Drawing.Point(134, -2);
            this.textBoxDot3.Name = "textBoxDot3";
            this.textBoxDot3.ReadOnly = true;
            this.textBoxDot3.Size = new System.Drawing.Size(27, 24);
            this.textBoxDot3.TabIndex = 6;
            this.textBoxDot3.Text = ".";
            this.textBoxDot3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // IPAddressControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxIPAddrPart4);
            this.Controls.Add(this.textBoxIPAddrPart3);
            this.Controls.Add(this.textBoxIPAddrPart2);
            this.Controls.Add(this.textBoxIPAddrPart1);
            this.Controls.Add(this.textBoxDot1);
            this.Controls.Add(this.textBoxDot2);
            this.Controls.Add(this.textBoxDot3);
            this.Name = "IPAddressControl";
            this.Size = new System.Drawing.Size(194, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxIPAddrPart1;
        private System.Windows.Forms.TextBox textBoxIPAddrPart2;
        private System.Windows.Forms.TextBox textBoxIPAddrPart3;
        private System.Windows.Forms.TextBox textBoxIPAddrPart4;
        private System.Windows.Forms.TextBox textBoxDot1;
        private System.Windows.Forms.TextBox textBoxDot2;
        private System.Windows.Forms.TextBox textBoxDot3;
    }
}
