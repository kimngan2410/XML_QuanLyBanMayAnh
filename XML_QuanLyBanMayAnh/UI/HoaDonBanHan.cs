using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace XML_QuanLyBanMayAnh.UI
{
    public partial class HoaDonBanHan : Form
    {
        private string strCon = "Data Source=localhost;Initial Catalog=QuanLyBanMayAnh2;Integrated Security=True";
        private string fileXML = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "HoaDonBanHang.xml");
        private taoXML taoXML = new taoXML();
        // Biến lưu mã hóa đơn cũ khi click vào DataGridView
        private string OldMaHD;
        private string OldMaSP; // Lưu mã sản phẩm cũ
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
                // Tạo file XML
                if (!File.Exists(fileXMLChiTietHD))
                {
                    taoXML.TaoXML(sqlChiTietHD, "ChiTietHoaDon", fileXMLChiTietHD);
                    MessageBox.Show("File ChiTietHoaDon.xml đã được tạo thành công.");
                }

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
                    // Lấy mã hóa đơn cũ từ bảng và gán cho OldMaHD
                    OldMaHD = dgvHD.Rows[e.RowIndex].Cells["maHD"].Value.ToString();

                    // Hiển thị mã hóa đơn và thông tin khác
                    txtMaDH.Text = OldMaHD;
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

                    // Xóa sạch thông tin chi tiết hóa đơn
                    ClearThongTinChiTietHoaDon();

                    // Load dữ liệu ChiTietHoaDon
                    LoadChiTietHoaDon(OldMaHD);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi chọn dữ liệu: " + ex.Message);
                }
            }
        }

        private void ClearThongTinChiTietHoaDon()
        {
            cbbMaSP.SelectedIndex = -1;
            txtTenSP.Clear();
            txtHang.Clear();
            txtDonGia.Clear();
            txtSoluongDat.Clear();
            txtThanhTien.Clear();
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
                    // Lưu mã hóa đơn và mã sản phẩm cũ
                    OldMaHD = txtMaDH.Text; // Lưu mã hóa đơn
                    OldMaSP = DataCTHD.Rows[e.RowIndex].Cells["maSP"].Value.ToString(); // Lưu mã sản phẩm

                    // Hiển thị thông tin lên các trường nhập liệu
                    cbbMaSP.Text = OldMaSP;
                    txtSoluongDat.Text = DataCTHD.Rows[e.RowIndex].Cells["soLuongDat"].Value.ToString();

                    // Load thông tin sản phẩm
                    LoadSanPhamInfo(OldMaSP);
                    TinhThanhTien();
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

            // Reset mã hóa đơn cũ
            OldMaHD = string.Empty;

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
                    // Kiểm tra mã hóa đơn đã tồn tại hay chưa
                    string checkSql = "SELECT COUNT(*) FROM HoaDonBan WHERE maHD = @maHD";
                    SqlCommand checkCmd = new SqlCommand(checkSql, connection);
                    checkCmd.Parameters.AddWithValue("@maHD", txtMaDH.Text);

                    int count = (int)checkCmd.ExecuteScalar(); // Trả về số lượng bản ghi
                    if (count > 0)
                    {
                        MessageBox.Show($"Hóa đơn có mã '{txtMaDH.Text}' đã tồn tại. Vui lòng kiểm tra và thêm lại!",
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
                MessageBox.Show("Thêm hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnSuaHD_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu OldMaHD không có giá trị
            if (string.IsNullOrEmpty(OldMaHD))
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn từ danh sách trước khi sửa! Hoặc nhấn nút Lưu để tạo mới.",
                                "Chưa chọn đối tượng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Thoát khỏi sự kiện
            }

            // Kiểm tra dữ liệu nhập vào
            if (string.IsNullOrWhiteSpace(txtMaDH.Text) ||
                string.IsNullOrWhiteSpace(cbbKhachhang.Text) ||
                string.IsNullOrWhiteSpace(cbbNhanVien.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn sửa hóa đơn '{OldMaHD}' thành '{txtMaDH.Text}' không?",
                "Xác nhận sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(strCon))
                    {
                        connection.Open();

                        // Câu lệnh UPDATE để sửa maHD và các thông tin khác
                        string sql = @"UPDATE HoaDonBan 
                               SET maHD = @newMaHD, maKH = @maKH, maNV = @maNV, ngayLapDon = @ngayLapDon 
                               WHERE maHD = @oldMaHD";

                        SqlCommand cmd = new SqlCommand(sql, connection);

                        // Tham số truyền vào câu lệnh SQL
                        cmd.Parameters.AddWithValue("@newMaHD", txtMaDH.Text); // Mã hóa đơn mới
                        cmd.Parameters.AddWithValue("@oldMaHD", OldMaHD); // Mã hóa đơn cũ
                        cmd.Parameters.AddWithValue("@maKH", cbbKhachhang.SelectedValue);
                        cmd.Parameters.AddWithValue("@maNV", cbbNhanVien.SelectedValue);
                        cmd.Parameters.AddWithValue("@ngayLapDon", dtpngaydathang.Value);

                        // Thực thi lệnh UPDATE
                        cmd.ExecuteNonQuery();
                    }

                    // Cập nhật file XML từ SQL Server
                    string sqlSelect = "SELECT * FROM HoaDonBan";
                    taoXML.TaoXML(sqlSelect, "HoaDonBan", fileXML);

                    // Load lại dữ liệu
                    LoadData();
                    MessageBox.Show("Sửa hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Xóa dữ liệu trên các ô nhập liệu
                    txtMaDH.Clear();
                    txttenKH.Clear();
                    txttennhanvien.Clear();
                    dtpngaydathang.Value = DateTime.Now;
                    cbbKhachhang.SelectedIndex = -1;
                    cbbNhanVien.SelectedIndex = -1;

                    // Reset mã hóa đơn cũ
                    OldMaHD = string.Empty;

                    // Xóa DataGridView for Chi Tiết Hóa Đơn
                    DataCTHD.DataSource = null;
                    DataCTHD.Rows.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi sửa hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXoaHD_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem các ô nhập liệu có rỗng không
            if (string.IsNullOrWhiteSpace(txtMaDH.Text) ||
                string.IsNullOrWhiteSpace(cbbKhachhang.Text) ||
                string.IsNullOrWhiteSpace(cbbNhanVien.Text))
            {
                MessageBox.Show("Hãy chọn một hóa đơn để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy mã hóa đơn từ TextBox
            string maHD = txtMaDH.Text;

            // Hiển thị hộp thoại xác nhận xóa
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa hóa đơn có mã: {maHD} không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            // Nếu người dùng chọn Yes
            if (result == DialogResult.Yes)
            {
                try
                {
                    // Kết nối tới SQL Server và thực hiện xóa
                    using (SqlConnection connection = new SqlConnection(strCon))
                    {
                        connection.Open();
                        string sql = "DELETE FROM HoaDonBan WHERE maHD = @maHD";
                        SqlCommand cmd = new SqlCommand(sql, connection);
                        cmd.Parameters.AddWithValue("@maHD", maHD);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"Xóa hóa đơn có mã {maHD} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Cập nhật lại DataGridView
                            LoadData();

                            // Ghi lại dữ liệu vào XML từ SQL Server
                            string sqlSelect = "SELECT * FROM HoaDonBan";
                            taoXML.TaoXML(sqlSelect, "HoaDonBan", fileXML);
                            //MessageBox.Show("Dữ liệu XML đã được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Xóa dữ liệu nhập
                            txtMaDH.Clear();
                            txttenKH.Clear();
                            txttennhanvien.Clear();
                            dtpngaydathang.Value = DateTime.Now;
                            cbbKhachhang.SelectedIndex = -1;
                            cbbNhanVien.SelectedIndex = -1;
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy hóa đơn để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //else
            //{
            //    MessageBox.Show("Hủy xóa sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        private void btnLuuHDCT_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu chưa chọn đơn hàng (mã hóa đơn trống)
            if (string.IsNullOrWhiteSpace(txtMaDH.Text))
            {
                MessageBox.Show("Hãy chọn một đơn hàng để thêm chi tiết hóa đơn!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra dữ liệu nhập vào cho chi tiết hóa đơn
            if (string.IsNullOrWhiteSpace(cbbMaSP.Text) || // Kiểm tra mã sản phẩm
                string.IsNullOrWhiteSpace(txtSoluongDat.Text)) // Kiểm tra số lượng đặt
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin chi tiết hóa đơn!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Thêm dữ liệu vào SQL Server
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    connection.Open();

                    // Kiểm tra maHD có tồn tại trong bảng HoaDonBan không
                    string checkMaHD = "SELECT COUNT(*) FROM HoaDonBan WHERE maHD = @maHD";
                    SqlCommand cmdCheckMaHD = new SqlCommand(checkMaHD, connection);
                    cmdCheckMaHD.Parameters.AddWithValue("@maHD", txtMaDH.Text);

                    int maHDCount = (int)cmdCheckMaHD.ExecuteScalar();
                    if (maHDCount == 0)
                    {
                        MessageBox.Show($"Mã hóa đơn '{txtMaDH.Text}' không tồn tại trong bảng Hóa Đơn! " +
                                        "Vui lòng kiểm tra lại hoặc thêm mới hóa đơn trước.",
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Kiểm tra mã hóa đơn và mã sản phẩm đã tồn tại hay chưa trong bảng ChiTieHoaDon
                    string checkSql = "SELECT COUNT(*) FROM ChiTieHoaDon WHERE maHD = @maHD AND maSP = @maSP";
                    SqlCommand checkCmd = new SqlCommand(checkSql, connection);
                    checkCmd.Parameters.AddWithValue("@maHD", txtMaDH.Text);
                    checkCmd.Parameters.AddWithValue("@maSP", cbbMaSP.SelectedValue);

                    int count = (int)checkCmd.ExecuteScalar(); // Trả về số lượng bản ghi
                    if (count > 0)
                    {
                        MessageBox.Show($"Chi tiết hóa đơn với mã hóa đơn '{txtMaDH.Text}' và sản phẩm '{cbbMaSP.Text}' đã tồn tại! Vui lòng chọn sửa.",
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Nếu không trùng thì thêm dữ liệu mới vào bảng ChiTieHoaDon
                    string sql = "INSERT INTO ChiTieHoaDon (maHD, maSP, soLuongDat) " +
                                 "VALUES (@maHD, @maSP, @soLuongDat)";
                    SqlCommand cmd = new SqlCommand(sql, connection);

                    cmd.Parameters.AddWithValue("@maHD", txtMaDH.Text);
                    cmd.Parameters.AddWithValue("@maSP", cbbMaSP.SelectedValue);
                    cmd.Parameters.AddWithValue("@soLuongDat", txtSoluongDat.Text);

                    cmd.ExecuteNonQuery();
                }

                // Ghi vào file XML
                string xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ChiTietHoaDon.xml");
                XDocument xDoc;

                if (!File.Exists(xmlPath))
                {
                    xDoc = new XDocument(new XElement("Root"));
                }
                else
                {
                    xDoc = XDocument.Load(xmlPath);
                }

                xDoc.Root.Add(new XElement("ChiTietHoaDon",
                    new XElement("maHD", txtMaDH.Text),
                    new XElement("maSP", cbbMaSP.SelectedValue),
                    new XElement("soLuongDat", txtSoluongDat.Text)
                ));

                xDoc.Save(xmlPath);

                // Load lại dữ liệu ChiTietHoaDon vào DataGridView
                LoadChiTietHoaDon(txtMaDH.Text);
                MessageBox.Show("Thêm chi tiết hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearThongTinChiTietHoaDon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm chi tiết hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuaHDCT_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu chưa chọn chi tiết hóa đơn cần sửa
            if (string.IsNullOrWhiteSpace(txtMaDH.Text) || string.IsNullOrWhiteSpace(cbbMaSP.Text))
            {
                MessageBox.Show("Hãy chọn một chi tiết hóa đơn để sửa!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra dữ liệu nhập
            if (string.IsNullOrWhiteSpace(txtSoluongDat.Text))
            {
                MessageBox.Show("Vui lòng nhập số lượng đặt!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Thêm thông báo xác nhận trước khi sửa
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn sửa chi tiết hóa đơn này không?",
                                                 "Xác nhận sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                //MessageBox.Show("Hủy sửa chi tiết hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // Thoát khỏi sự kiện nếu người dùng chọn "No"
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    connection.Open();

                    // Nếu trùng lặp thì thực hiện UPDATE thay vì thông báo lỗi
                    string updateSql = @"UPDATE ChiTieHoaDon 
                                 SET soLuongDat = @soLuongDat 
                                 WHERE maHD = @newMaHD AND maSP = @newMaSP";

                    SqlCommand updateCmd = new SqlCommand(updateSql, connection);
                    updateCmd.Parameters.AddWithValue("@newMaHD", txtMaDH.Text);
                    updateCmd.Parameters.AddWithValue("@newMaSP", cbbMaSP.SelectedValue);
                    updateCmd.Parameters.AddWithValue("@soLuongDat", txtSoluongDat.Text);

                    int rowsAffected = updateCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật chi tiết hóa đơn thành công!",
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Nếu không có dữ liệu cần UPDATE, thêm mới vào
                        string insertSql = @"INSERT INTO ChiTieHoaDon (maHD, maSP, soLuongDat) 
                                     VALUES (@newMaHD, @newMaSP, @soLuongDat)";

                        SqlCommand insertCmd = new SqlCommand(insertSql, connection);
                        insertCmd.Parameters.AddWithValue("@newMaHD", txtMaDH.Text);
                        insertCmd.Parameters.AddWithValue("@newMaSP", cbbMaSP.SelectedValue);
                        insertCmd.Parameters.AddWithValue("@soLuongDat", txtSoluongDat.Text);

                        insertCmd.ExecuteNonQuery();

                        MessageBox.Show("Thêm chi tiết hóa đơn thành công!",
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    string sqlChiTietHD = "SELECT * FROM ChiTieHoaDon";
                    string fileXMLChiTietHD = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ChiTietHoaDon.xml");
                    taoXML.TaoXML(sqlChiTietHD, "ChiTietHoaDon", fileXMLChiTietHD);

                    // Load lại dữ liệu chi tiết hóa đơn
                    LoadChiTietHoaDon(txtMaDH.Text);
                    // Cập nhật lại mã hóa đơn và mã sản phẩm cũ
                    OldMaHD = txtMaDH.Text;
                    OldMaSP = cbbMaSP.SelectedValue.ToString();

                    // Xóa dữ liệu nhập
                    ClearThongTinChiTietHoaDon();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa chi tiết hóa đơn: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateXMLFile(string maHD, string maSP, string soLuongDat)
        {
            string xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ChiTietHoaDon.xml");

            if (File.Exists(xmlPath))
            {
                XDocument xDoc = XDocument.Load(xmlPath);
                var chiTiet = xDoc.Descendants("ChiTietHoaDon")
                                  .FirstOrDefault(x => x.Element("maHD")?.Value == maHD &&
                                                       x.Element("maSP")?.Value == maSP);
                if (chiTiet != null)
                {
                    chiTiet.Element("soLuongDat").Value = soLuongDat;
                    xDoc.Save(xmlPath);
                }
            }
        }


        private void btnXoaHDCT_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu chưa chọn chi tiết hóa đơn để xóa
            if (string.IsNullOrWhiteSpace(txtMaDH.Text) || string.IsNullOrWhiteSpace(cbbMaSP.Text))
            {
                MessageBox.Show("Hãy chọn một chi tiết hóa đơn để xóa!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Thông báo xác nhận xóa
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa chi tiết hóa đơn có mã hóa đơn '{txtMaDH.Text}' và sản phẩm '{cbbMaSP.Text}' không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                MessageBox.Show("Hủy xóa chi tiết hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // Thoát khỏi sự kiện nếu chọn No
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    connection.Open();

                    // Câu lệnh DELETE để xóa chi tiết hóa đơn
                    string deleteSql = @"DELETE FROM ChiTieHoaDon 
                                 WHERE maHD = @maHD AND maSP = @maSP";

                    SqlCommand deleteCmd = new SqlCommand(deleteSql, connection);
                    deleteCmd.Parameters.AddWithValue("@maHD", txtMaDH.Text);
                    deleteCmd.Parameters.AddWithValue("@maSP", cbbMaSP.SelectedValue);

                    int rowsAffected = deleteCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Xóa dòng tương ứng trong file XML
                        string sqlChiTietHD = "SELECT * FROM ChiTieHoaDon";
                        string fileXMLChiTietHD = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ChiTietHoaDon.xml");
                        taoXML.TaoXML(sqlChiTietHD, "ChiTietHoaDon", fileXMLChiTietHD);


                        MessageBox.Show("Xóa chi tiết hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Làm mới dữ liệu chi tiết hóa đơn
                        LoadChiTietHoaDon(txtMaDH.Text);

                        // Xóa dữ liệu nhập trong các ô TextBox và ComboBox
                        ClearThongTinChiTietHoaDon();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy chi tiết hóa đơn để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa chi tiết hóa đơn: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteFromXMLFile(string maHD, string maSP)
        {
            string xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ChiTietHoaDon.xml");

            if (File.Exists(xmlPath))
            {
                XDocument xDoc = XDocument.Load(xmlPath);
                var chiTiet = xDoc.Descendants("ChiTietHoaDon")
                                  .FirstOrDefault(x => x.Element("maHD")?.Value == maHD &&
                                                       x.Element("maSP")?.Value == maSP);
                if (chiTiet != null)
                {
                    chiTiet.Remove(); // Xóa phần tử khỏi XML
                    xDoc.Save(xmlPath);
                }
            }
        }


        XDocument xItem;
        private void inHoaDon()
        {
            if (string.IsNullOrWhiteSpace(OldMaHD))
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn trước khi in chi tiết hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ChiTietHoaDon.xml");
            string pathHTML = "ChiTietHoaDon.html";

            try
            {
                // Kiểm tra file tồn tại
                if (!File.Exists(xmlPath))
                {
                    MessageBox.Show("File ChiTietHoaDon.xml không tồn tại. Vui lòng tạo lại file trước khi in hóa đơn.");
                    return;
                }

                // Tải file XML
                xItem = XDocument.Load(xmlPath);
                var xI = xItem.Descendants("ChiTietHoaDon")
                              .Where(el => el.Element("maHD")?.Value == OldMaHD); // Lọc theo mã hóa đơn đang chọn

                if (!xI.Any())
                {
                    MessageBox.Show("Không có dữ liệu chi tiết hóa đơn cho hóa đơn này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Lấy đơn giá từ bảng SanPham
                Dictionary<string, string> donGiaDict = new Dictionary<string, string>();
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    connection.Open();
                    string query = "SELECT maSP, donGia FROM SanPham";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string maSP = reader["maSP"].ToString();
                        string donGia = reader["donGia"].ToString();
                        donGiaDict[maSP] = donGia; // Thêm vào Dictionary
                    }
                }

                // Tạo HTML
                var html = new XElement("html",
                    new XElement("head",
                        new XElement("style", @"
                    table { width: 100%; border: 1px solid black; border-collapse: collapse; }
                    th, td { border: 1px solid gray; padding: 8px; text-align: center; }
                    th { background-color: lightgreen; font-weight: bold; }")
                    ),
                    new XElement("body",
                        new XElement("h2", $"Chi Tiết Hóa Đơn - Mã Hóa Đơn: {OldMaHD}"),
                        new XElement("table",
                            new XElement("tr",
                                new XElement("th", "Mã Hóa Đơn"),
                                new XElement("th", "Mã Sản Phẩm"),
                                new XElement("th", "Số Lượng Đặt"),
                                new XElement("th", "Đơn Giá"),
                                new XElement("th", "Thành Tiền")
                            ),
                            from el in xI
                            let maSP = el.Element("maSP")?.Value
                            let soLuongDat = int.TryParse(el.Element("soLuongDat")?.Value, out int sl) ? sl : 0
                            let donGia = donGiaDict.ContainsKey(maSP) ? Convert.ToDecimal(donGiaDict[maSP]) : 0
                            let thanhTien = donGia * soLuongDat
                            select new XElement("tr",
                                new XElement("td", el.Element("maHD")?.Value ?? "N/A"),
                                new XElement("td", maSP ?? "N/A"),
                                new XElement("td", soLuongDat.ToString()),
                                new XElement("td", new XAttribute("style", "text-align:right"), donGia.ToString("N0")),
                                new XElement("td", new XAttribute("style", "text-align:right"), thanhTien.ToString("N0"))
                            )
                        )
                    )
                );

                // Lưu file HTML và mở trong trình duyệt
                html.Save(pathHTML);
                Process.Start(new ProcessStartInfo(pathHTML) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo hóa đơn HTML: " + ex.Message);
            }
        }

        private void btnInHD_Click(object sender, EventArgs e)
        {
            inHoaDon();
        }
    }
}
