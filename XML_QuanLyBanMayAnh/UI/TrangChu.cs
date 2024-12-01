using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XML_QuanLyBanMayAnh.UI
{
    public partial class TrangChu : Form
    {
        public TrangChu()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void QuảnLýThôngTinToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void HoáĐơnBánHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HoaDonBanHan frm = new HoaDonBanHan();
            frm.Show();
            this.Visible = false;
        }

        private void QuảnLýNhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLyNhanVien frm = new QuanLyNhanVien();
            frm.Show();
            this.Visible = false;
        }

        private void QuảnLýKháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLyKhachHang frm = new QuanLyKhachHang();
            frm.Show();
            this.Visible = false;
        }

        private void QuảnLýSảnPhẩmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLySanPham frm = new QuanLySanPham();
            frm.Show();
            this.Visible = false;
        }

        private void NhàCungCấpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLyHang frm = new QuanLyHang();
            frm.Show();
            this.Visible = false;
        }
    }
}
