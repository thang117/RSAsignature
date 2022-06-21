
namespace RSASignature
{
    partial class StartedForm
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
            this.btOpenTaoChuKy = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 24F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(22, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(420, 47);
            this.label1.TabIndex = 0;
            this.label1.Text = "RSA DIGITAL SIGNATURES";
            // 
            // btOpenTaoChuKy
            // 
            this.btOpenTaoChuKy.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btOpenTaoChuKy.Location = new System.Drawing.Point(140, 81);
            this.btOpenTaoChuKy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btOpenTaoChuKy.Name = "btOpenTaoChuKy";
            this.btOpenTaoChuKy.Size = new System.Drawing.Size(196, 38);
            this.btOpenTaoChuKy.TabIndex = 1;
            this.btOpenTaoChuKy.Text = "Create Signature";
            this.btOpenTaoChuKy.UseVisualStyleBackColor = true;
            this.btOpenTaoChuKy.Click += new System.EventHandler(this.btOpenTaoChuKy_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button1.Location = new System.Drawing.Point(140, 123);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(196, 38);
            this.button1.TabIndex = 2;
            this.button1.Text = "Signature Vertification ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btOpenXacThucChuKy_Click);
            // 
            // StartedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::RSASignature.Properties.Resources.Steganography04072015;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(473, 172);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btOpenTaoChuKy);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "StartedForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RSA DIGITAL SIGNATURES";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Started_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btOpenTaoChuKy;
        private System.Windows.Forms.Button button1;
    }
}