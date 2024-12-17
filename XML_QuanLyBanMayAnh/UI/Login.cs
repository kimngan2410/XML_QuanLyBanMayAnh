using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XML_QuanLyBanMayAnh.UI
{
    public partial class Login : Form
    {
        private string strCon = "Data Source=localhost;Initial Catalog=QuanLyBanMayAnh2;Integrated Security=True";
        private string fileXML = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "./NguoiDung.xml");

        private taoXML taoXML = new taoXML();
        public Login()
        {
            InitializeComponent();
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {

            string email = txtTendangnhap.Text;  // txtEmail: textbox chứa email nhập vào
            string password = txtPass.Text;  // txtPassword: textbox chứa mật khẩu nhập vào

            
            // Kiểm tra thông tin đăng nhập
            if (CheckLogin(email, password, fileXML))
            {
                // Nếu đăng nhập thành công, mở form TrangChu
                TrangChu frm = new TrangChu();
                frm.Show();
                this.Visible = false;  // Ẩn form đăng nhập
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại, vui lòng kiểm tra lại email và mật khẩu.", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CheckLogin(string email, string password, string fileXML)
        {
            try
            {
                // Tạo DataTable để chứa dữ liệu từ file XML
                DataTable dt = new DataTable();

                // Kiểm tra file XML tồn tại hay không
                if (!File.Exists(fileXML))
                {
                    MessageBox.Show("File XML không tồn tại. Đang khởi tạo dữ liệu từ SQL Server...", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Tạo file XML từ SQL Server
                    taoXML.TaoXML("SELECT * FROM NguoiDung", "NguoiDung", fileXML);
                }

                // Đọc dữ liệu từ file XML vào DataTable
                dt.ReadXml(fileXML);

                // Bỏ qua kiểm tra email và mật khẩu
                // Nếu có ít nhất một tài khoản thì cho phép đăng nhập
                if (dt.Rows.Count > 0)
                {
                    return true; // Đăng nhập thành công
                }

                // Trường hợp không có dữ liệu
                MessageBox.Show("Không có tài khoản nào trong hệ thống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kiểm tra đăng nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }



        private void txtTendangnhap_Enter(object sender, EventArgs e) 
        {
            if (txtTendangnhap.Text == "Tên đăng nhập")
            {
                txtTendangnhap.Text = "";
                txtTendangnhap.ForeColor = Color.Black;
            }
        }

        private void txtTendangnhap_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTendangnhap.Text))
            {
                txtTendangnhap.Text = "Tên đăng nhập";
                txtTendangnhap.ForeColor = Color.Gray;
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureDataFolderExists(); // Tạo thư mục Data nếu chưa tồn tại

                // Tạo file XML từ SQL nếu file chưa tồn tại
                if (!File.Exists(fileXML))
                {
                    string sql = "SELECT * FROM NguoiDung"; // Truy vấn SQL để lấy dữ liệu từ bảng
                    taoXML.TaoXML(sql, "NguoiDung", fileXML); // Tạo file XML
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }


        // Tạo thư mục Data nếu chưa tồn tại
        private void EnsureDataFolderExists()
        {
            string dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }
        }


        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Kiểm tra checkbox có được chọn không
            if (chkShowPassword.Checked)
            {
                // Hiển thị mật khẩu (chuyển sang kiểu bình thường)
                txtPass.UseSystemPasswordChar = false;
            }
            else
            {
                // Ẩn mật khẩu (chuyển về dấu chấm)
                txtPass.UseSystemPasswordChar = true;
            }
        }

        private void txtPass_Enter(object sender, EventArgs e)
        {
            if (txtPass.Text == "Mật khẩu")
            {
                txtPass.Text = "";
                txtPass.ForeColor = Color.Black;
                txtPass.UseSystemPasswordChar = true; // Bật chế độ ẩn mật khẩu
            }
        }

        private void txtPass_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPass.Text))
            {
                txtPass.UseSystemPasswordChar = false; // Hiển thị dạng văn bản cho placeholder
                txtPass.Text = "Mật khẩu";
                txtPass.ForeColor = Color.Gray;
            }
        }
    }
}
