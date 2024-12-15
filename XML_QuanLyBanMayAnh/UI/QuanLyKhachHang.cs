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
    public partial class QuanLyKhachHang : Form
    {
        private string strCon = "Data Source=localhost;Initial Catalog=QuanLyBanMayAnh2;Integrated Security=True";
        private string fileXML = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "./KhachHang.xml");
        private taoXML taoXML = new taoXML();

        public QuanLyKhachHang()
        {
            InitializeComponent();
            dgvKH.CellClick += dgvKH_CellClick; // Gắn sự kiện CellClick
            this.Load += QuanLyKhachHang_Load;
        }

        private void QuanLyKhachHang_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureDataFolderExists(); // Tạo thư mục Data nếu chưa tồn tại

                // Kiểm tra nếu file XML chưa tồn tại thì tạo mới từ CSDL
                if (!File.Exists(fileXML))
                {
                    string sql = "SELECT * FROM KhachHang";
                    taoXML.TaoXML(sql, "KhachHang", fileXML); // Gọi lớp tạo XML để tạo file
                }

                LoadData(); // Tải dữ liệu từ cơ sở dữ liệu lên DataGridView
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

        // Phương thức tải dữ liệu từ cơ sở dữ liệu lên DataGridView
        private void LoadData()
        {
            try
            {
                using (var connection = new SqlConnection(strCon))
                {
                    connection.Open();
                    string sql = "SELECT * FROM KhachHang"; // Truy vấn để lấy dữ liệu
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvKH.DataSource = dt; // Hiển thị dữ liệu lên DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            txtMaKH.Text = "";
            txtTenKh.Text = "";
            txtSDT.Text = "";
            txtDiaChi.Text = "";
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaKH.Text) ||
                    string.IsNullOrWhiteSpace(txtTenKh.Text) ||
                    string.IsNullOrWhiteSpace(txtSDT.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                using (var connection = new SqlConnection(strCon))
                {
                    connection.Open();

                    string checkExists = "SELECT COUNT(*) FROM KhachHang WHERE maKH = @maKH";
                    using (var cmdCheck = new SqlCommand(checkExists, connection))
                    {
                        cmdCheck.Parameters.AddWithValue("@maKH", txtMaKH.Text.Trim());
                        int count = (int)cmdCheck.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Mã khách hàng đã tồn tại!");
                            return;
                        }
                    }

                    string sqlInsert = "INSERT INTO KhachHang (maKH, tenKH, diaChi, SDT) " +
                                       "VALUES (@maKH, @tenKH, @diaChi, @SDT)";
                    using (var cmdInsert = new SqlCommand(sqlInsert, connection))
                    {
                        cmdInsert.Parameters.AddWithValue("@maKH", txtMaKH.Text.Trim());
                        cmdInsert.Parameters.AddWithValue("@tenKH", txtTenKh.Text.Trim());
                        cmdInsert.Parameters.AddWithValue("@diaChi", txtDiaChi.Text.Trim());
                        cmdInsert.Parameters.AddWithValue("@SDT", txtSDT.Text.Trim());
                       

                        int rowsAffected = cmdInsert.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Thêm khách hàng thành công!");
                            LoadData();
                            taoXML.TaoXML("SELECT * FROM KhachHang", "KhachHang", fileXML);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(txtMaKH.Text) ||
                    string.IsNullOrWhiteSpace(txtTenKh.Text) ||
                    string.IsNullOrWhiteSpace(txtSDT.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                using (var connection = new SqlConnection(strCon))
                {
                    connection.Open();

                    // Kiểm tra xem mã nhân viên có tồn tại trong CSDL không
                    string checkExists = "SELECT COUNT(*) FROM KhachHang WHERE maKH = @maKH";
                    using (var cmdCheck = new SqlCommand(checkExists, connection))
                    {
                        cmdCheck.Parameters.AddWithValue("@maKH", txtMaKH.Text.Trim());
                        int count = (int)cmdCheck.ExecuteScalar();
                        if (count == 0)
                        {
                            MessageBox.Show("Mã khách hàng không tồn tại!");
                            return;
                        }
                    }

                    // Cập nhật thông tin nhân viên
                    string sqlUpdate = "UPDATE KhachHang SET " +
                                       "tenKH = @tenKH, diaChi = @diaChi, SDT = @SDT " +
                                       "WHERE maKH = @maKH"; // Sử dụng WHERE để chỉ cập nhật nhân viên có mã maNV

                    using (var cmdUpdate = new SqlCommand(sqlUpdate, connection))
                    {
                        cmdUpdate.Parameters.AddWithValue("@maKH", txtMaKH.Text.Trim());
                        cmdUpdate.Parameters.AddWithValue("@tenKH", txtTenKh.Text.Trim());
                        cmdUpdate.Parameters.AddWithValue("@diaChi", txtDiaChi.Text.Trim());
                        cmdUpdate.Parameters.AddWithValue("@SDT", txtSDT.Text.Trim());
                       

                        int rowsAffected = cmdUpdate.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cập nhật thông tin khách hàng thành công!");
                            LoadData(); // Tải lại dữ liệu
                            taoXML.TaoXML("SELECT * FROM KhachHang", "KhachHang", fileXML); // Cập nhật file XML
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra nếu chưa nhập mã nhân viên cần xóa
                if (string.IsNullOrWhiteSpace(txtMaKH.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã khách hàng để xóa!");
                    return;
                }

                // Xác nhận hành động xóa
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    using (var connection = new SqlConnection(strCon))
                    {
                        connection.Open();

                        // Câu lệnh SQL xóa nhân viên theo mã nhân viên
                        string sqlDelete = "DELETE FROM KhachHang WHERE maKH = @maKH";

                        using (var cmdDelete = new SqlCommand(sqlDelete, connection))
                        {
                            cmdDelete.Parameters.AddWithValue("@maKH", txtMaKH.Text.Trim());

                            int rowsAffected = cmdDelete.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa khách hàng thành công!");
                                LoadData(); // Tải lại dữ liệu sau khi xóa
                                taoXML.TaoXML("SELECT * FROM KhachHang", "KhachHang", fileXML); // Cập nhật file XML

                                txtMaKH.Text = "";
                                txtTenKh.Text = "";
                                txtSDT.Text = "";
                                txtDiaChi.Text = "";
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy khách hàng với mã này!");
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi xóa: " + ex.Message);
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            this.Close();
            TrangChu frm = new TrangChu();
            frm.ShowDialog();
        }

        private void dgvKH_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Kiểm tra xem có dòng nào được chọn không
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvKH.Rows[e.RowIndex];

                    // Lấy giá trị từ các cột trong DataGridView
                    txtMaKH.Text = row.Cells["maKH"].Value?.ToString();
                    txtTenKh.Text = row.Cells["tenKH"].Value?.ToString();
                    txtDiaChi.Text = row.Cells["diaChi"].Value?.ToString();
                    txtSDT.Text = row.Cells["SDT"].Value?.ToString();
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi chọn dòng: " + ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
