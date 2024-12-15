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
    public partial class Login : Form
    {
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

            // Khởi tạo đối tượng TaoXML
            taoXML xml = new taoXML();

            // Đường dẫn tới file XML
            string fileXML = "./NguoiDung.xml";

            // Tạo file XML nếu chưa có hoặc cập nhật dữ liệu
            if (!System.IO.File.Exists(fileXML))
            {
                string sqlQuery = "SELECT * FROM NguoiDung";  // Lệnh SQL để lấy toàn bộ dữ liệu người dùng
                string tableName = "NguoiDung";  // Tên bảng trong XML
                xml.TaoXML(sqlQuery, tableName, fileXML);  // Tạo file XML từ cơ sở dữ liệu
            }

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
            // Đọc dữ liệu từ file XML
            DataTable dt = new DataTable();
            dt = new taoXML().loadDataGridView(fileXML);

            // Kiểm tra xem có tồn tại email và mật khẩu trong file XML không
            foreach (DataRow row in dt.Rows)
            {
                if (row["tenDangNhap"].ToString() == email && row["matKhau"].ToString() == password)
                {
                    return true; // Nếu có, đăng nhập thành công
                }
            }

            return false;  // Nếu không tìm thấy, đăng nhập thất bại
        }

        private void txtTendangnhap_TextChanged(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
