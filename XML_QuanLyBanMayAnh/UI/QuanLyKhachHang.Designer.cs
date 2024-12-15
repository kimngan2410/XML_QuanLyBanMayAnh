namespace XML_QuanLyBanMayAnh.UI
{
    partial class QuanLyKhachHang
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
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvKH = new System.Windows.Forms.DataGridView();
            this.txtTenKh = new System.Windows.Forms.TextBox();
            this.txtMaKH = new System.Windows.Forms.TextBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.txtSDT = new System.Windows.Forms.TextBox();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.txtDiaChi = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.Button6 = new System.Windows.Forms.Button();
            this.Button4 = new System.Windows.Forms.Button();
            this.Button3 = new System.Windows.Forms.Button();
            this.Button2 = new System.Windows.Forms.Button();
            this.Button1 = new System.Windows.Forms.Button();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.GroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKH)).BeginInit();
            this.GroupBox1.SuspendLayout();
            this.Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(44, 49);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(64, 21);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Mã KH:";
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.dgvKH);
            this.GroupBox2.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox2.Location = new System.Drawing.Point(29, 316);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(1049, 297);
            this.GroupBox2.TabIndex = 2;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "DANH SÁCH KHÁCH HÀNG";
            // 
            // dgvKH
            // 
            this.dgvKH.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKH.Location = new System.Drawing.Point(34, 40);
            this.dgvKH.Name = "dgvKH";
            this.dgvKH.RowHeadersWidth = 51;
            this.dgvKH.RowTemplate.Height = 24;
            this.dgvKH.Size = new System.Drawing.Size(982, 227);
            this.dgvKH.TabIndex = 0;
            this.dgvKH.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvKH_CellClick);
            // 
            // txtTenKh
            // 
            this.txtTenKh.Location = new System.Drawing.Point(684, 43);
            this.txtTenKh.Name = "txtTenKh";
            this.txtTenKh.Size = new System.Drawing.Size(319, 28);
            this.txtTenKh.TabIndex = 9;
            // 
            // txtMaKH
            // 
            this.txtMaKH.Location = new System.Drawing.Point(124, 46);
            this.txtMaKH.Name = "txtMaKH";
            this.txtMaKH.Size = new System.Drawing.Size(298, 28);
            this.txtMaKH.TabIndex = 8;
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.Location = new System.Drawing.Point(535, 98);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(47, 21);
            this.Label6.TabIndex = 5;
            this.Label6.Text = "SDT:";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.Location = new System.Drawing.Point(44, 98);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(67, 21);
            this.Label4.TabIndex = 3;
            this.Label4.Text = "Địa chỉ:";
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Font = new System.Drawing.Font("Tahoma", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label9.ForeColor = System.Drawing.Color.SandyBrown;
            this.Label9.Location = new System.Drawing.Point(22, 23);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(341, 34);
            this.Label9.TabIndex = 1;
            this.Label9.Text = "QUẢN LÝ KHÁCH HÀNG";
            // 
            // txtSDT
            // 
            this.txtSDT.Location = new System.Drawing.Point(684, 98);
            this.txtSDT.Name = "txtSDT";
            this.txtSDT.Size = new System.Drawing.Size(319, 28);
            this.txtSDT.TabIndex = 11;
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.txtSDT);
            this.GroupBox1.Controls.Add(this.txtDiaChi);
            this.GroupBox1.Controls.Add(this.txtTenKh);
            this.GroupBox1.Controls.Add(this.txtMaKH);
            this.GroupBox1.Controls.Add(this.Label6);
            this.GroupBox1.Controls.Add(this.Label4);
            this.GroupBox1.Controls.Add(this.Label2);
            this.GroupBox1.Controls.Add(this.Label1);
            this.GroupBox1.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox1.Location = new System.Drawing.Point(28, 96);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(1050, 169);
            this.GroupBox1.TabIndex = 0;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "THÔNG TIN KHÁCH HÀNG";
            // 
            // txtDiaChi
            // 
            this.txtDiaChi.Location = new System.Drawing.Point(124, 95);
            this.txtDiaChi.Name = "txtDiaChi";
            this.txtDiaChi.Size = new System.Drawing.Size(298, 28);
            this.txtDiaChi.TabIndex = 10;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(535, 46);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(133, 21);
            this.Label2.TabIndex = 1;
            this.Label2.Text = "Tên khách hàng:";
            // 
            // Panel1
            // 
            this.Panel1.Controls.Add(this.txtTimKiem);
            this.Panel1.Controls.Add(this.button5);
            this.Panel1.Controls.Add(this.Button6);
            this.Panel1.Controls.Add(this.Button4);
            this.Panel1.Controls.Add(this.Button3);
            this.Panel1.Controls.Add(this.Button2);
            this.Panel1.Controls.Add(this.Button1);
            this.Panel1.Controls.Add(this.GroupBox2);
            this.Panel1.Controls.Add(this.Label9);
            this.Panel1.Controls.Add(this.GroupBox1);
            this.Panel1.Location = new System.Drawing.Point(14, 31);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(1105, 730);
            this.Panel1.TabIndex = 1;
            // 
            // Button6
            // 
            this.Button6.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button6.Image = global::XML_QuanLyBanMayAnh.Properties.Resources.luu_icon;
            this.Button6.Location = new System.Drawing.Point(214, 659);
            this.Button6.Name = "Button6";
            this.Button6.Size = new System.Drawing.Size(136, 41);
            this.Button6.TabIndex = 9;
            this.Button6.Text = " Lưu";
            this.Button6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Button6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Button6.UseVisualStyleBackColor = true;
            this.Button6.Click += new System.EventHandler(this.Button6_Click);
            // 
            // Button4
            // 
            this.Button4.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button4.Image = global::XML_QuanLyBanMayAnh.Properties.Resources.exit_icon;
            this.Button4.Location = new System.Drawing.Point(910, 659);
            this.Button4.Name = "Button4";
            this.Button4.Size = new System.Drawing.Size(136, 41);
            this.Button4.TabIndex = 6;
            this.Button4.Text = " Thoát";
            this.Button4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Button4.UseVisualStyleBackColor = true;
            this.Button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // Button3
            // 
            this.Button3.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button3.Image = global::XML_QuanLyBanMayAnh.Properties.Resources.delete_icon;
            this.Button3.Location = new System.Drawing.Point(537, 659);
            this.Button3.Name = "Button3";
            this.Button3.Size = new System.Drawing.Size(136, 41);
            this.Button3.TabIndex = 5;
            this.Button3.Text = " Xoá";
            this.Button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Button3.UseVisualStyleBackColor = true;
            this.Button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // Button2
            // 
            this.Button2.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button2.Image = global::XML_QuanLyBanMayAnh.Properties.Resources.edit_icon;
            this.Button2.Location = new System.Drawing.Point(375, 659);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(136, 41);
            this.Button2.TabIndex = 4;
            this.Button2.Text = " Sửa";
            this.Button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Button2.UseVisualStyleBackColor = true;
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // Button1
            // 
            this.Button1.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button1.Image = global::XML_QuanLyBanMayAnh.Properties.Resources.add_icon;
            this.Button1.Location = new System.Drawing.Point(55, 659);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(136, 41);
            this.Button1.TabIndex = 3;
            this.Button1.Text = " Thêm";
            this.Button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimKiem.Location = new System.Drawing.Point(616, 30);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.Size = new System.Drawing.Size(328, 28);
            this.txtTimKiem.TabIndex = 15;
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.Location = new System.Drawing.Point(966, 23);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(112, 40);
            this.button5.TabIndex = 14;
            this.button5.Text = "Tìm kiếm";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // QuanLyKhachHang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1132, 793);
            this.Controls.Add(this.Panel1);
            this.Name = "QuanLyKhachHang";
            this.Text = "QuanLyKhachHang";
            this.Load += new System.EventHandler(this.QuanLyKhachHang_Load);
            this.GroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKH)).EndInit();
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button Button6;
        internal System.Windows.Forms.Button Button4;
        internal System.Windows.Forms.Button Button3;
        internal System.Windows.Forms.Button Button2;
        internal System.Windows.Forms.Button Button1;
        internal System.Windows.Forms.GroupBox GroupBox2;
        internal System.Windows.Forms.DataGridView dgvKH;
        internal System.Windows.Forms.TextBox txtTenKh;
        internal System.Windows.Forms.TextBox txtMaKH;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.TextBox txtSDT;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.TextBox txtDiaChi;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.TextBox txtTimKiem;
        internal System.Windows.Forms.Button button5;
    }
}