namespace XML_QuanLyBanMayAnh.UI
{
    partial class Login
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
            this.Label1 = new System.Windows.Forms.Label();
            this.CheckBox1 = new System.Windows.Forms.CheckBox();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.txtTendangnhap = new System.Windows.Forms.TextBox();
            this.Button2 = new System.Windows.Forms.Button();
            this.Button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.BackColor = System.Drawing.Color.White;
            this.Label1.Font = new System.Drawing.Font("Tahoma", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.ForeColor = System.Drawing.Color.Orange;
            this.Label1.Location = new System.Drawing.Point(306, 167);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(248, 45);
            this.Label1.TabIndex = 22;
            this.Label1.Text = "ĐĂNG NHẬP";
            this.Label1.Click += new System.EventHandler(this.Label1_Click);
            // 
            // CheckBox1
            // 
            this.CheckBox1.AutoSize = true;
            this.CheckBox1.BackColor = System.Drawing.Color.White;
            this.CheckBox1.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckBox1.Location = new System.Drawing.Point(247, 358);
            this.CheckBox1.Name = "CheckBox1";
            this.CheckBox1.Size = new System.Drawing.Size(163, 25);
            this.CheckBox1.TabIndex = 20;
            this.CheckBox1.Text = "Hiển thị mật khẩu";
            this.CheckBox1.UseVisualStyleBackColor = false;
            // 
            // txtPass
            // 
            this.txtPass.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPass.ForeColor = System.Drawing.Color.DarkGray;
            this.txtPass.Location = new System.Drawing.Point(247, 296);
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(358, 32);
            this.txtPass.TabIndex = 19;
            this.txtPass.UseSystemPasswordChar = true;
            // 
            // txtTendangnhap
            // 
            this.txtTendangnhap.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTendangnhap.ForeColor = System.Drawing.Color.DarkGray;
            this.txtTendangnhap.Location = new System.Drawing.Point(247, 234);
            this.txtTendangnhap.Name = "txtTendangnhap";
            this.txtTendangnhap.Size = new System.Drawing.Size(358, 32);
            this.txtTendangnhap.TabIndex = 18;
            this.txtTendangnhap.TextChanged += new System.EventHandler(this.txtTendangnhap_TextChanged);
            // 
            // Button2
            // 
            this.Button2.BackColor = System.Drawing.Color.SandyBrown;
            this.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button2.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button2.ForeColor = System.Drawing.Color.White;
            this.Button2.Location = new System.Drawing.Point(460, 402);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(145, 43);
            this.Button2.TabIndex = 17;
            this.Button2.Text = "Thoát";
            this.Button2.UseVisualStyleBackColor = false;
            // 
            // Button1
            // 
            this.Button1.BackColor = System.Drawing.Color.SandyBrown;
            this.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button1.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button1.ForeColor = System.Drawing.Color.White;
            this.Button1.Location = new System.Drawing.Point(247, 402);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(145, 43);
            this.Button1.TabIndex = 16;
            this.Button1.Text = "Đăng nhập";
            this.Button1.UseVisualStyleBackColor = false;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = global::XML_QuanLyBanMayAnh.Properties.Resources.logooo;
            this.pictureBox1.Location = new System.Drawing.Point(377, 55);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(103, 91);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 23;
            this.pictureBox1.TabStop = false;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::XML_QuanLyBanMayAnh.Properties.Resources.bg_login;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(884, 500);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.CheckBox1);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.txtTendangnhap);
            this.Controls.Add(this.Button2);
            this.Controls.Add(this.Button1);
            this.DoubleBuffered = true;
            this.Name = "Login";
            this.Text = "Login";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.CheckBox CheckBox1;
        internal System.Windows.Forms.TextBox txtPass;
        internal System.Windows.Forms.TextBox txtTendangnhap;
        internal System.Windows.Forms.Button Button2;
        internal System.Windows.Forms.Button Button1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}