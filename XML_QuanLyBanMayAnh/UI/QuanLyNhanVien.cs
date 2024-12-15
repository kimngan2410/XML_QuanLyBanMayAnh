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
    public partial class QuanLyNhanVien : Form
    {
        private string strCon = "Data Source=localhost;Initial Catalog=QuanLyBanMayAnh2;Integrated Security=True";
        private string fileXML = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "./NhanVien.xml");
        private taoXML taoXML = new taoXML();


        public QuanLyNhanVien()
        {
            InitializeComponent();
            dgvNV.CellClick += dgvNV_CellClick; // Gắn sự kiện CellClick
            this.Load += QuanLyNhanVien_Load;
        }

        private void Label9_Click(object sender, EventArgs e)
        {

        }



        // Phương thức xử lý sự kiện khi form được load

        private void QuanLyNhanVien_Load(object sender, EventArgs e)
        {
             try
            {
                EnsureDataFolderExists(); // Tạo thư mục Data nếu chưa tồn tại

                // Kiểm tra nếu file XML chưa tồn tại thì tạo mới từ CSDL
                if (!File.Exists(fileXML))
                {
                    string sql = "SELECT * FROM NhanVien";
                    taoXML.TaoXML(sql, "NhanVien", fileXML); // Gọi lớp tạo XML để tạo file
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
                    string sql = "SELECT * FROM NhanVien"; // Truy vấn để lấy dữ liệu
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvNV.DataSource = dt; // Hiển thị dữ liệu lên DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

                

        private void Button1_Click_1(object sender, EventArgs e)
        {
            txtMaNV.Text = "";
            txtTenNV.Text = "";
            txtSDT.Text = "";
            txtChucVu.Text = "";
            txtDiachi.Text = "";
            dtpNgSinh.Value = DateTime.Now; 
            ngayLamViec.Value = DateTime.Now; 
        }

        private void dgvNV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Kiểm tra xem có dòng nào được chọn không
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvNV.Rows[e.RowIndex];

                    // Lấy giá trị từ các cột trong DataGridView
                    txtMaNV.Text = row.Cells["maNV"].Value?.ToString();
                    txtTenNV.Text = row.Cells["tenNV"].Value?.ToString();
                    txtDiachi.Text = row.Cells["diaChi"].Value?.ToString();
                    txtSDT.Text = row.Cells["SDT"].Value?.ToString();
                    txtChucVu.Text = row.Cells["chucVu"].Value?.ToString();

                    // Chọn giá trị của RadioButton
                    string gioiTinh = row.Cells["gioiTinh"].Value?.ToString();
                    if (gioiTinh == "Nam")
                    {
                        rdbNam.Checked = true;
                    }
                    else if (gioiTinh == "Nữ")
                    {
                        rdbNu.Checked = true;
                    }

                    // Hiển thị giá trị ngày vào DateTimePicker
                    if (DateTime.TryParse(row.Cells["ngaySinh"].Value?.ToString(), out DateTime ngaySinh))
                    {
                        dtpNgSinh.Value = ngaySinh;
                    }

                    if (DateTime.TryParse(row.Cells["ngayVaoLam"].Value?.ToString(), out DateTime ngayVaoLam))
                    {
                        ngayLamViec.Value = ngayVaoLam;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi chọn dòng: " + ex.Message);
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaNV.Text) ||
                    string.IsNullOrWhiteSpace(txtTenNV.Text) ||
                    string.IsNullOrWhiteSpace(txtSDT.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                using (var connection = new SqlConnection(strCon))
                {
                    connection.Open();

                    string checkExists = "SELECT COUNT(*) FROM NhanVien WHERE maNV = @maNV";
                    using (var cmdCheck = new SqlCommand(checkExists, connection))
                    {
                        cmdCheck.Parameters.AddWithValue("@maNV", txtMaNV.Text.Trim());
                        int count = (int)cmdCheck.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Mã nhân viên đã tồn tại!");
                            return;
                        }
                    }

                    string sqlInsert = "INSERT INTO NhanVien (maNV, tenNV, gioiTinh, ngaySinh, diaChi, SDT, ngayVaoLam, chucVu) " +
                                       "VALUES (@maNV, @tenNV, @gioiTinh, @ngaySinh, @diaChi, @SDT, @ngayVaoLam, @chucVu)";
                    using (var cmdInsert = new SqlCommand(sqlInsert, connection))
                    {
                        cmdInsert.Parameters.AddWithValue("@maNV", txtMaNV.Text.Trim());
                        cmdInsert.Parameters.AddWithValue("@tenNV", txtTenNV.Text.Trim());
                        if (rdbNam.Checked)
                        {
                            cmdInsert.Parameters.AddWithValue("@gioiTinh", rdbNam.Text);
                        }
                        else if (rdbNu.Checked)
                        {
                            cmdInsert.Parameters.AddWithValue("@gioiTinh", rdbNu.Text);
                        }
                        cmdInsert.Parameters.AddWithValue("@ngaySinh", dtpNgSinh.Value);
                        cmdInsert.Parameters.AddWithValue("@diaChi", txtDiachi.Text.Trim());
                        cmdInsert.Parameters.AddWithValue("@SDT", txtSDT.Text.Trim());
                        cmdInsert.Parameters.AddWithValue("@ngayVaoLam", ngayLamViec.Value);
                        cmdInsert.Parameters.AddWithValue("@chucVu", txtChucVu.Text.Trim());


                        int rowsAffected = cmdInsert.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Thêm nhân viên thành công!");
                            LoadData();
                            taoXML.TaoXML("SELECT * FROM NhanVien", "NhanVien", fileXML);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form hiện tại
            TrangChu frm = new TrangChu();
            frm.Show(); // Hiển thị form TrangChu
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(txtMaNV.Text) ||
                    string.IsNullOrWhiteSpace(txtTenNV.Text) ||
                    string.IsNullOrWhiteSpace(txtSDT.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                using (var connection = new SqlConnection(strCon))
                {
                    connection.Open();

                    // Kiểm tra xem mã nhân viên có tồn tại trong CSDL không
                    string checkExists = "SELECT COUNT(*) FROM NhanVien WHERE maNV = @maNV";
                    using (var cmdCheck = new SqlCommand(checkExists, connection))
                    {
                        cmdCheck.Parameters.AddWithValue("@maNV", txtMaNV.Text.Trim());
                        int count = (int)cmdCheck.ExecuteScalar();
                        if (count == 0)
                        {
                            MessageBox.Show("Mã nhân viên không tồn tại!");
                            return;
                        }
                    }

                    // Cập nhật thông tin nhân viên
                    string sqlUpdate = "UPDATE NhanVien SET " +
                                       "tenNV = @tenNV, gioiTinh = @gioiTinh, ngaySinh = @ngaySinh, diaChi = @diaChi, SDT = @SDT, " +
                                       "ngayVaoLam = @ngayVaoLam, chucVu = @chucVu " +
                                       "WHERE maNV = @maNV"; // Sử dụng WHERE để chỉ cập nhật nhân viên có mã maNV

                    using (var cmdUpdate = new SqlCommand(sqlUpdate, connection))
                    {
                        cmdUpdate.Parameters.AddWithValue("@maNV", txtMaNV.Text.Trim());
                        cmdUpdate.Parameters.AddWithValue("@tenNV", txtTenNV.Text.Trim());
                        cmdUpdate.Parameters.AddWithValue("@gioiTinh", rdbNam.Checked ? rdbNam.Text : rdbNu.Text);
                        cmdUpdate.Parameters.AddWithValue("@ngaySinh", dtpNgSinh.Value);
                        cmdUpdate.Parameters.AddWithValue("@diaChi", txtDiachi.Text.Trim());
                        cmdUpdate.Parameters.AddWithValue("@SDT", txtSDT.Text.Trim());
                        cmdUpdate.Parameters.AddWithValue("@ngayVaoLam", ngayLamViec.Value);
                        cmdUpdate.Parameters.AddWithValue("@chucVu", txtChucVu.Text.Trim());

                        int rowsAffected = cmdUpdate.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cập nhật thông tin nhân viên thành công!");
                            LoadData(); // Tải lại dữ liệu
                            taoXML.TaoXML("SELECT * FROM NhanVien", "NhanVien", fileXML); // Cập nhật file XML
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
                if (string.IsNullOrWhiteSpace(txtMaNV.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã nhân viên để xóa!");
                    return;
                }

                // Xác nhận hành động xóa
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    using (var connection = new SqlConnection(strCon))
                    {
                        connection.Open();

                        // Câu lệnh SQL xóa nhân viên theo mã nhân viên
                        string sqlDelete = "DELETE FROM NhanVien WHERE maNV = @maNV";

                        using (var cmdDelete = new SqlCommand(sqlDelete, connection))
                        {
                            cmdDelete.Parameters.AddWithValue("@maNV", txtMaNV.Text.Trim());

                            int rowsAffected = cmdDelete.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa nhân viên thành công!");
                                LoadData(); // Tải lại dữ liệu sau khi xóa
                                taoXML.TaoXML("SELECT * FROM NhanVien", "NhanVien", fileXML); // Cập nhật file XML

                                txtMaNV.Text = "";
                                txtTenNV.Text = "";
                                txtSDT.Text = "";
                                txtChucVu.Text = "";
                                txtDiachi.Text = "";
                                dtpNgSinh.Value = DateTime.Now;
                                ngayLamViec.Value = DateTime.Now;
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy nhân viên với mã này!");
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

        private void Button1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                string searchText = txtTimKiem.Text.Trim(); // Lấy từ TextBox tìm kiếm

                using (var connection = new SqlConnection(strCon))
                {
                    connection.Open();

                    // Tạo câu truy vấn SQL để tìm kiếm nhân viên
                    string sqlSearch = "SELECT * FROM NhanVien WHERE maNV LIKE @searchText OR tenNV LIKE @searchText OR chucVu LIKE @searchText";

                    using (var cmdSearch = new SqlCommand(sqlSearch, connection))
                    {
                        cmdSearch.Parameters.AddWithValue("@searchText", "%" + searchText + "%");

                        SqlDataAdapter adapter = new SqlDataAdapter(cmdSearch);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Hiển thị kết quả lên DataGridView
                        dgvNV.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi tìm kiếm: " + ex.Message);
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
