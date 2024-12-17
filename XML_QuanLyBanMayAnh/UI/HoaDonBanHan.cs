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
    public partial class HoaDonBanHan : Form
    {
        private string strCon = "Data Source=localhost;Initial Catalog=QuanLyBanMayAnh2;Integrated Security=True";
        private string fileXML = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "HoaDonBanHang.xml");
        private taoXML taoXML = new taoXML();
        public HoaDonBanHan()
        {
            InitializeComponent();
            this.Load += HoaDonBanHan_Load; // Gán sự kiện Load cho form
            dgvHD.CellClick += dgvHD_CellClick; // Gán sự kiện click trên DataGridView
            DataCTHD.CellClick += DataCTHD_CellClick;
            txtDonGia.TextChanged += (s, e) => TinhThanhTien();
            txtSoluongDat.TextChanged += (s, e) => TinhThanhTien();
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
                    string sql = "SELECT * FROM HoaDonBan"; // Truy vấn để lấy dữ liệu
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvHD.DataSource = dt; // Hiển thị dữ liệu lên DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        private void HoaDonBanHan_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureDataFolderExists(); // Tạo thư mục Data nếu chưa tồn tại
                LoadMaNV();
                LoadMaKH();
                LoadMaSP(); // Load sản phẩm
                string sql = "SELECT * FROM HoaDonBan";
                taoXML.TaoXML(sql, "HoaDonBan", fileXML); // Gọi lớp tạo XML để tạo file

                // Tạo file XML cho bảng ChiTietHoaDon
                string fileXMLChiTietHD = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ChiTietHoaDon.xml");
                string sqlChiTietHD = "SELECT * FROM ChiTieHoaDon";
                taoXML.TaoXML(sqlChiTietHD, "ChiTietHoaDon", fileXMLChiTietHD);

                LoadData(); // Tải dữ liệu từ cơ sở dữ liệu lên DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        private string LayTenNhanVien(string maNV)
        {
            string tenNV = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    connection.Open();
                    string query = "SELECT tenNV FROM NhanVien WHERE maNV = @maNV";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@maNV", maNV);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        tenNV = reader["tenNV"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy tên nhân viên: " + ex.Message);
            }
            return tenNV;
        }

        private string LayTenKhachHang(string maKH)
        {
            string tenKH = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    connection.Open();
                    string query = "SELECT tenKH FROM KhachHang WHERE maKH = @maKH";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@maKH", maKH);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        tenKH = reader["tenKH"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy tên khách hàng: " + ex.Message);
            }
            return tenKH;
        }

        private void LoadMaNV()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    string query = "SELECT maNV, tenNV FROM NhanVien";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();

                    adapter.Fill(dt);

                    // Gán dữ liệu vào ComboBox maNV
                    cbbNhanVien.DataSource = dt;
                    cbbNhanVien.DisplayMember = "maNV";
                    cbbNhanVien.ValueMember = "maNV";
                    cbbNhanVien.Tag = dt; // Lưu DataTable vào Tag
                    cbbNhanVien.SelectedIndex = -1; // Không chọn gì ban đầu
                }

                // Gắn sự kiện
                cbbNhanVien.SelectedIndexChanged += cbbNhanVien_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải mã nhân viên: " + ex.Message);
            }
        }

        private void cbbNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbNhanVien.SelectedIndex >= 0) // Kiểm tra nếu đã chọn giá trị
            {
                try
                {
                    // Lấy DataTable từ Tag của ComboBox
                    DataTable dt = cbbNhanVien.Tag as DataTable;

                    // Tìm hàng có mã nhân viên được chọn
                    if (cbbNhanVien.SelectedValue != null)
                    {
                        DataRow[] rows = dt.Select("maNV = '" + cbbNhanVien.SelectedValue.ToString() + "'");
                        if (rows.Length > 0)
                        {
                            txttennhanvien.Text = rows[0]["tenNV"].ToString(); // Gán tên nhân viên vào TextBox
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi truy xuất tên nhân viên: " + ex.Message);
                }
            }
        }


        private void LoadMaKH()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    string query = "SELECT maKH, tenKH FROM KhachHang";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();

                    adapter.Fill(dt);

                    // Gán dữ liệu vào ComboBox maKH
                    cbbKhachhang.DataSource = dt;
                    cbbKhachhang.DisplayMember = "maKH";
                    cbbKhachhang.ValueMember = "maKH";
                    cbbKhachhang.Tag = dt; // Lưu DataTable vào Tag
                    cbbKhachhang.SelectedIndex = -1; // Không chọn gì ban đầu
                }

                // Gắn sự kiện
                cbbKhachhang.SelectedIndexChanged += cbbKhachhang_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải mã khách hàng: " + ex.Message);
            }
        }

        private void cbbKhachhang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbKhachhang.SelectedIndex >= 0) // Kiểm tra nếu đã chọn giá trị
            {
                try
                {
                    // Lấy DataTable từ Tag của ComboBox
                    DataTable dt = cbbKhachhang.Tag as DataTable;

                    // Tìm hàng có mã khách hàng được chọn
                    if (cbbKhachhang.SelectedValue != null)
                    {
                        DataRow[] rows = dt.Select("maKH = '" + cbbKhachhang.SelectedValue.ToString() + "'");
                        if (rows.Length > 0)
                        {
                            txttenKH.Text = rows[0]["tenKH"].ToString(); // Gán tên khách hàng vào TextBox
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi truy xuất tên khách hàng: " + ex.Message);
                }
            }
        }


        private void dgvHD_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    string maHD = dgvHD.Rows[e.RowIndex].Cells["maHD"].Value.ToString();
                    // Hiển thị mã hóa đơn và thông tin khác
                    txtMaDH.Text = maHD;
                    txttenKH.Text = ""; // Đặt tạm để tra cứu lại
                    cbbKhachhang.Text = dgvHD.Rows[e.RowIndex].Cells["maKH"].Value.ToString();
                    cbbNhanVien.Text = dgvHD.Rows[e.RowIndex].Cells["maNV"].Value.ToString();
                    dtpngaydathang.Text = dgvHD.Rows[e.RowIndex].Cells["ngayLapDon"].Value.ToString();
                    txttennhanvien.Text = ""; // Đặt tạm để tra cứu lại

                    // Lấy tenKH từ bảng KhachHang dựa vào maKH
                    string maKH = dgvHD.Rows[e.RowIndex].Cells["maKH"].Value.ToString();
                    txttenKH.Text = LayTenKhachHang(maKH);

                    // Lấy tenNV từ bảng NhanVien dựa vào maNV
                    string maNV = dgvHD.Rows[e.RowIndex].Cells["maNV"].Value.ToString();
                    txttennhanvien.Text = LayTenNhanVien(maNV);

                    // Load dữ liệu ChiTietHoaDon
                    LoadChiTietHoaDon(maHD);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi chọn dữ liệu: " + ex.Message);
                }
            }
        }
        private void LoadChiTietHoaDon(string maHD)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    string query = "SELECT * FROM ChiTieHoaDon WHERE maHD = @maHD";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@maHD", maHD);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    DataCTHD.DataSource = dt; // Hiển thị dữ liệu lên DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải chi tiết hóa đơn: " + ex.Message);
            }
        }

        private void LoadMaSP()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    string query = "SELECT maSP, tenSP FROM SanPham";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();

                    adapter.Fill(dt);

                    // Gán dữ liệu vào ComboBox maSP
                    cbbMaSP.DataSource = dt;
                    cbbMaSP.DisplayMember = "maSP";
                    cbbMaSP.ValueMember = "maSP";
                    cbbMaSP.Tag = dt; // Lưu DataTable vào Tag
                    cbbMaSP.SelectedIndex = -1; // Không chọn gì ban đầu
                }

                // Gắn sự kiện
                cbbMaSP.SelectedIndexChanged += cbbMaSP_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải mã khách hàng: " + ex.Message);
            }
        }

        private void cbbMaSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbMaSP.SelectedIndex >= 0) // Kiểm tra nếu đã chọn giá trị
            {
                try
                {
                    //// Lấy DataTable từ Tag của ComboBox
                    //DataTable dt = cbbMaSP.Tag as DataTable;

                    //// Tìm hàng có mã khách hàng được chọn
                    //if (cbbMaSP.SelectedValue != null)
                    //{
                    //    DataRow[] rows = dt.Select("maSP = '" + cbbMaSP.SelectedValue.ToString() + "'");
                    //    if (rows.Length > 0)
                    //    {
                    //        txtTenSP.Text = rows[0]["tenSP"].ToString(); // Gán tên khách hàng vào TextBox
                    //    }
                    //}
                    string maSP = cbbMaSP.SelectedValue.ToString(); // Get selected maSP
                    LoadSanPhamInfo(maSP); // Load product information
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi truy xuất tên sản phẩm: " + ex.Message);
                }
            }
        }

        private void DataCTHD_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    string maSP = DataCTHD.Rows[e.RowIndex].Cells["maSP"].Value.ToString();
                    txtSoluongDat.Text = DataCTHD.Rows[e.RowIndex].Cells["soLuongDat"].Value.ToString();
                    cbbMaSP.Text = maSP;


                    // Truy xuất thông tin từ bảng SanPham
                    LoadSanPhamInfo(maSP);
                    TinhThanhTien(); // Gọi hàm tính thành tiền
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi chọn chi tiết hóa đơn: " + ex.Message);
                }
            }
        }
        private void LoadSanPhamInfo(string maSP)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    connection.Open();
                    string query = @"
                SELECT sp.tenSP, sp.donGia, h.tenHang
                FROM SanPham sp
                JOIN Hang h ON sp.maHang = h.maHang
                WHERE sp.maSP = @maSP";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@maSP", maSP);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtDonGia.Text = reader["donGia"].ToString();
                        txtTenSP.Text = reader["tenSP"].ToString();
                        txtHang.Text = reader["tenHang"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy thông tin sản phẩm: " + ex.Message);
            }
        }
        private void TinhThanhTien()
        {
            try
            {
                // Chuyển đổi giá trị từ TextBox sang số
                decimal donGia = string.IsNullOrEmpty(txtDonGia.Text) ? 0 : Convert.ToDecimal(txtDonGia.Text);
                int soLuong = string.IsNullOrEmpty(txtSoluongDat.Text) ? 0 : Convert.ToInt32(txtSoluongDat.Text);

                // Tính thành tiền
                decimal thanhTien = donGia * soLuong;

                // Hiển thị kết quả lên txtThanhTien
                txtThanhTien.Text = thanhTien.ToString("N0"); // Hiển thị số tiền có định dạng
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tính thành tiền: " + ex.Message);
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            TrangChu tc = new TrangChu();
            tc.Show();
            this.Close();
        }

        private void btnThemHD_Click(object sender, EventArgs e)
        {
            txtMaDH.Clear();
            txttenKH.Clear();
            txttennhanvien.Clear();
            dtpngaydathang.Value = DateTime.Now;
            cbbKhachhang.SelectedIndex = -1;
            cbbNhanVien.SelectedIndex = -1;

            cbbMaSP.SelectedIndex = -1;
            txtSoluongDat.Clear();
            txtDonGia.Clear();
            txtTenSP.Clear();
            txtHang.Clear();

            // Xóa DataGridView for Chi Tiết Hóa Đơn
            DataCTHD.DataSource = null;
            DataCTHD.Rows.Clear();
        }

        private void btnThemHDCT_Click(object sender, EventArgs e)
        {
            cbbMaSP.SelectedIndex = -1;
            txtSoluongDat.Clear();
            txtDonGia.Clear();
            txtTenSP.Clear();
            txtHang.Clear();
        }

        private void btnLuuHD_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu nhập vào
            if (string.IsNullOrWhiteSpace(txtMaDH.Text) ||
                string.IsNullOrWhiteSpace(cbbKhachhang.Text) ||
                string.IsNullOrWhiteSpace(cbbNhanVien.Text) )
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Thêm dữ liệu vào SQL Server
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    connection.Open();
                    // Kiểm tra mã sản phẩm đã tồn tại hay chưa
                    string checkSql = "SELECT COUNT(*) FROM HoaDonBan WHERE maHD = @maHD";
                    SqlCommand checkCmd = new SqlCommand(checkSql, connection);
                    checkCmd.Parameters.AddWithValue("@maHD", txtMaDH.Text);

                    int count = (int)checkCmd.ExecuteScalar(); // Trả về số lượng bản ghi
                    if (count > 0)
                    {
                        MessageBox.Show($"Sản phẩm có mã '{txtMaDH.Text}' đã tồn tại. Vui lòng kiểm tra và thêm lại!",
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Nếu không trùng thì thêm dữ liệu mới
                    string sql = "INSERT INTO HoaDonBan (maHD, maNV, maKH, ngayLapDon )" +
                                 "VALUES (@maHD, @maNV, @maKH, @ngayLapDon)";
                    SqlCommand cmd = new SqlCommand(sql, connection);

                    cmd.Parameters.AddWithValue("@maHD", txtMaDH.Text);
                    cmd.Parameters.AddWithValue("@maNV", cbbNhanVien.SelectedValue);
                    cmd.Parameters.AddWithValue("@maKH", cbbKhachhang.SelectedValue); 
                    cmd.Parameters.AddWithValue("@ngayLapDon", dtpngaydathang.Value);

                    cmd.ExecuteNonQuery();
                }

                // Load lại dữ liệu lên DataGridView
                LoadData();
                MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Ghi lại dữ liệu vào XML từ SQL Server
                string sqlSelect = "SELECT * FROM HoaDonBan";
                taoXML.TaoXML(sqlSelect, "HoaDonBan", fileXML);

                // Xóa dữ liệu nhập
                txtMaDH.Clear();
                txttenKH.Clear();
                txttennhanvien.Clear();
                dtpngaydathang.Value = DateTime.Now;
                cbbKhachhang.SelectedIndex = -1;
                cbbNhanVien.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm dữ liệu: " + ex.Message);
            }
        }
    }
}
