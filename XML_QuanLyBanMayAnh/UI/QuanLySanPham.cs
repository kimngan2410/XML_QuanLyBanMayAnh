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
    public partial class QuanLySanPham : Form
    {
        private string strCon = "Data Source=localhost;Initial Catalog=QuanLyBanMayAnh2;Integrated Security=True";
        private string fileXML = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "SanPham.xml");
        private taoXML taoXML = new taoXML();
        public QuanLySanPham()
        {
            InitializeComponent();
            this.Load += QuanLySanPham_Load; // Gán sự kiện Load cho form
            dgvSP.CellClick += dgvSP_CellClick; // Gán sự kiện click trên DataGridView
        }

        private void QuanLySanPham_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureDataFolderExists(); // Tạo thư mục Data nếu chưa tồn tại
                LoadLoaiHang(); // Load danh sách loại hàng
                string sql = "SELECT * FROM SanPham";
                taoXML.TaoXML(sql, "SanPham", fileXML); // Gọi lớp tạo XML để tạo file

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
                    string sql = "SELECT * FROM SanPham"; // Truy vấn để lấy dữ liệu
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvSP.DataSource = dt; // Hiển thị dữ liệu lên DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        private void dgvSP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    // Lấy maHang từ dòng được chọn
                    string selectedMaHang = dgvSP.Rows[e.RowIndex].Cells["maHang"].Value.ToString();

                    // Hiển thị dữ liệu khác vào các TextBox
                    txtMaSp.Text = dgvSP.Rows[e.RowIndex].Cells["maSP"].Value.ToString();
                    txttensp.Text = dgvSP.Rows[e.RowIndex].Cells["tenSP"].Value.ToString();
                    txtDongia.Text = dgvSP.Rows[e.RowIndex].Cells["donGia"].Value.ToString();
                    txtSoluongcon.Text = dgvSP.Rows[e.RowIndex].Cells["soLuongHienCon"].Value.ToString();
                    txtMota.Text = dgvSP.Rows[e.RowIndex].Cells["moTa"].Value.ToString();

                    // Tìm tenHang tương ứng với maHang
                    using (SqlConnection connection = new SqlConnection(strCon))
                    {
                        connection.Open();
                        string sql = "SELECT tenHang FROM Hang WHERE maHang = @maHang";
                        SqlCommand cmd = new SqlCommand(sql, connection);
                        cmd.Parameters.AddWithValue("@maHang", selectedMaHang);

                        object result = cmd.ExecuteScalar(); // Lấy giá trị tenHang
                        if (result != null)
                        {
                            cbbLoaiHang.Text = result.ToString(); // Hiển thị tenHang trong ComboBox
                        }
                        else
                        {
                            cbbLoaiHang.Text = ""; // Trường hợp không tìm thấy
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi chọn dữ liệu: " + ex.Message);
                }
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            TrangChu tc= new TrangChu();
            tc.Show();
            this.Close();
        }

        private void bttThem_Click(object sender, EventArgs e)
        {
            // Xóa dữ liệu nhập
            txtMaSp.Clear();
            txttensp.Clear();
            cbbLoaiHang.SelectedIndex = -1; // Xóa lựa chọn hiển thị
            cbbLoaiHang.ResetText();        // Xóa nội dung hiển thị của ComboBox
            txtDongia.Clear();
            txtSoluongcon.Clear();
            txtMota.Clear();
        }
        private void LoadLoaiHang()
        {
            try
            {
                using (var connection = new SqlConnection(strCon))
                {
                    connection.Open();
                    string sql = "SELECT maHang, tenHang FROM Hang"; // Lấy maHang và tenHang
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Gán dữ liệu vào ComboBox
                    cbbLoaiHang.DataSource = dt;
                    cbbLoaiHang.DisplayMember = "tenHang"; // Hiển thị tên hãng
                    cbbLoaiHang.ValueMember = "maHang";   // Giá trị ngầm là maHang
                    cbbLoaiHang.SelectedIndex = -1;       // Không chọn gì khi load
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải loại hàng: " + ex.Message);
            }
        }

        //nút sửa
        private void btnSua_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có sản phẩm nào được chọn chưa
            if (string.IsNullOrWhiteSpace(txtMaSp.Text) ||
                string.IsNullOrWhiteSpace(txttensp.Text) ||
                string.IsNullOrWhiteSpace(cbbLoaiHang.Text) ||
                string.IsNullOrWhiteSpace(txtDongia.Text) ||
                string.IsNullOrWhiteSpace(txtSoluongcon.Text) ||
                string.IsNullOrWhiteSpace(txtMota.Text))
            {
                MessageBox.Show("Hãy chọn một sản phẩm để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra dữ liệu nhập vào có hợp lệ không
            if (!decimal.TryParse(txtDongia.Text, out decimal donGia))
            {
                MessageBox.Show("Đơn giá phải là số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(txtSoluongcon.Text, out int soLuong))
            {
                MessageBox.Show("Số lượng hiện còn phải là số nguyên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Hiển thị hộp thoại xác nhận sửa
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn sửa sản phẩm có mã: {txtMaSp.Text} không?",
                "Xác nhận sửa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Cập nhật dữ liệu vào SQL Server
                    using (SqlConnection connection = new SqlConnection(strCon))
                    {
                        connection.Open();
                        string sql = "UPDATE SanPham SET tenSP = @tenSP, maHang = @maHang, donGia = @donGia, " +
                                     "soLuongHienCon = @soLuongHienCon, moTa = @moTa WHERE maSP = @maSP";
                        SqlCommand cmd = new SqlCommand(sql, connection);

                        cmd.Parameters.AddWithValue("@maSP", txtMaSp.Text);
                        cmd.Parameters.AddWithValue("@tenSP", txttensp.Text);
                        cmd.Parameters.AddWithValue("@maHang", cbbLoaiHang.SelectedValue);
                        cmd.Parameters.AddWithValue("@donGia", donGia);
                        cmd.Parameters.AddWithValue("@soLuongHienCon", soLuong);
                        cmd.Parameters.AddWithValue("@moTa", txtMota.Text);

                        cmd.ExecuteNonQuery();
                    }

                    // Cập nhật lại file XML từ SQL Server
                    string sqlSelect = "SELECT * FROM SanPham";
                    taoXML.TaoXML(sqlSelect, "SanPham", fileXML);

                    // Load lại dữ liệu lên DataGridView
                    LoadData();
                    MessageBox.Show("Sửa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Xóa dữ liệu trên các ô nhập liệu
                    txtMaSp.Clear();
                    txttensp.Clear();
                    cbbLoaiHang.SelectedIndex = -1;
                    txtDongia.Clear();
                    txtSoluongcon.Clear();
                    txtMota.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi sửa dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //else
            //{
            //    MessageBox.Show("Hủy sửa sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        // nút lưu
        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu nhập vào
            if (string.IsNullOrWhiteSpace(txtMaSp.Text) ||
                string.IsNullOrWhiteSpace(txttensp.Text) ||
                string.IsNullOrWhiteSpace(cbbLoaiHang.Text) ||
                string.IsNullOrWhiteSpace(txtDongia.Text) ||
                string.IsNullOrWhiteSpace(txtSoluongcon.Text) ||
                string.IsNullOrWhiteSpace(txtMota.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (!decimal.TryParse(txtDongia.Text, out decimal donGia))
                {
                    MessageBox.Show("Đơn giá phải là số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(txtSoluongcon.Text, out int soLuong))
                {
                    MessageBox.Show("Số lượng hiện còn phải là số nguyên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }

            try
            {
                // Thêm dữ liệu vào SQL Server
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    connection.Open();
                    // Kiểm tra mã sản phẩm đã tồn tại hay chưa
                    string checkSql = "SELECT COUNT(*) FROM SanPham WHERE maSP = @maSP";
                    SqlCommand checkCmd = new SqlCommand(checkSql, connection);
                    checkCmd.Parameters.AddWithValue("@maSP", txtMaSp.Text);

                    int count = (int)checkCmd.ExecuteScalar(); // Trả về số lượng bản ghi
                    if (count > 0)
                    {
                        MessageBox.Show($"Sản phẩm có mã '{txtMaSp.Text}' đã tồn tại. Vui lòng kiểm tra và thêm lại!",
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Nếu không trùng thì thêm dữ liệu mới
                    string sql = "INSERT INTO SanPham (maSP, tenSP, maHang, donGia, soLuongHienCon, moTa) " +
                                 "VALUES (@maSP, @tenSP, @maHang, @donGia, @soLuongHienCon, @moTa)";
                    SqlCommand cmd = new SqlCommand(sql, connection);

                    cmd.Parameters.AddWithValue("@maSP", txtMaSp.Text);
                    cmd.Parameters.AddWithValue("@tenSP", txttensp.Text);
                    cmd.Parameters.AddWithValue("@maHang", cbbLoaiHang.SelectedValue); // Lấy maHang từ ComboBox
                    cmd.Parameters.AddWithValue("@donGia", decimal.Parse(txtDongia.Text));
                    cmd.Parameters.AddWithValue("@soLuongHienCon", int.Parse(txtSoluongcon.Text)); // Chuyển sang int
                    cmd.Parameters.AddWithValue("@moTa", txtMota.Text);

                    cmd.ExecuteNonQuery();
                }

                // Load lại dữ liệu lên DataGridView
                LoadData();
                MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Ghi lại dữ liệu vào XML từ SQL Server
                string sqlSelect = "SELECT * FROM SanPham";
                taoXML.TaoXML(sqlSelect, "SanPham", fileXML);

                // Xóa dữ liệu nhập
                txtMaSp.Clear();
                txttensp.Clear();
                cbbLoaiHang.SelectedIndex = -1; // Xóa lựa chọn hiển thị
                cbbLoaiHang.ResetText();        // Xóa nội dung hiển thị của ComboBox
                txtDongia.Clear();
                txtSoluongcon.Clear();
                txtMota.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm dữ liệu: " + ex.Message);
            }
        }

        //nút xóa
        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem các ô nhập liệu có rỗng không
            if (string.IsNullOrWhiteSpace(txtMaSp.Text) ||
                string.IsNullOrWhiteSpace(txttensp.Text) ||
                string.IsNullOrWhiteSpace(cbbLoaiHang.Text))
            {
                MessageBox.Show("Hãy chọn một sản phẩm để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy mã sản phẩm từ TextBox
            string maSP = txtMaSp.Text;

            // Hiển thị hộp thoại xác nhận xóa
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa sản phẩm có mã: {maSP} không?",
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
                        string sql = "DELETE FROM SanPham WHERE maSP = @maSP";
                        SqlCommand cmd = new SqlCommand(sql, connection);
                        cmd.Parameters.AddWithValue("@maSP", maSP);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"Xóa sản phẩm có mã {maSP} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Cập nhật lại DataGridView
                            LoadData();

                            // Ghi lại dữ liệu vào XML từ SQL Server
                            string sqlSelect = "SELECT * FROM SanPham";
                            taoXML.TaoXML(sqlSelect, "SanPham", fileXML);
                            //MessageBox.Show("Dữ liệu XML đã được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Xóa dữ liệu nhập
                            txtMaSp.Clear();
                            txttensp.Clear();
                            cbbLoaiHang.SelectedIndex = -1; // Xóa lựa chọn hiển thị
                            cbbLoaiHang.ResetText();        // Xóa nội dung hiển thị của ComboBox
                            txtDongia.Clear();
                            txtSoluongcon.Clear();
                            txtMota.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy sản phẩm để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void txtTimKiem_Enter(object sender, EventArgs e)
        {
            if (txtTimKiem.Text == "Tìm kiếm theo mã sản phẩm")
            {
                txtTimKiem.Text = "";
                txtTimKiem.ForeColor = Color.Black; // Đổi màu chữ về màu đen
            }
        }

        private void txtTimKiem_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTimKiem.Text))
            {
                txtTimKiem.Text = "Tìm kiếm theo mã sản phẩm";
                txtTimKiem.ForeColor = Color.Gray; // Đổi màu chữ về màu xám
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string maSPCanTim = txtTimKiem.Text.Trim();

            if (string.IsNullOrWhiteSpace(maSPCanTim) || maSPCanTim == "Tìm kiếm theo mã sản phẩm")
            {
                MessageBox.Show("Vui lòng nhập mã sản phẩm để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(strCon))
                {
                    connection.Open();
                    string sql = "SELECT * FROM SanPham WHERE maSP = @maSP";
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@maSP", maSPCanTim);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        dgvSP.DataSource = dt; // Load sản phẩm được tìm thấy
                    }
                    else
                    {
                        MessageBox.Show($"Không có sản phẩm có mã: {maSPCanTim}", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData(); // Load lại toàn bộ dữ liệu
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTimKiem.Text) || txtTimKiem.Text == "Tìm kiếm theo mã sản phẩm")
            {
                LoadData(); // Load lại toàn bộ dữ liệu khi TextBox rỗng
            }
        }
    }
}
