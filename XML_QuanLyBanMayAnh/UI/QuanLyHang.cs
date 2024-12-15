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
    public partial class QuanLyHang : Form
    {

        private string strCon = "Data Source=localhost;Initial Catalog=QuanLyBanMayAnh2;Integrated Security=True";
        private string fileXML = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "./Hang.xml");
        private taoXML taoXML = new taoXML();
        public QuanLyHang()
        {
            InitializeComponent();
            this.Load += QuanLyHang_Load;
            dgvHang.CellClick += dgvHang_CellClick; // Gắn sự kiện CellClick

        }

        private void QuanLyHang_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureDataFolderExists(); // Tạo thư mục Data nếu chưa tồn tại

                // Kiểm tra nếu file XML chưa tồn tại thì tạo mới từ CSDL
                if (!File.Exists(fileXML))
                {
                    string sql = "SELECT * FROM Hang";
                    taoXML.TaoXML(sql, "Hang", fileXML); // Gọi lớp tạo XML để tạo file
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
                    string sql = "SELECT * FROM Hang"; // Truy vấn để lấy dữ liệu
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvHang.DataSource = dt; // Hiển thị dữ liệu lên DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        private void dgvHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Kiểm tra xem có dòng nào được chọn không
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvHang.Rows[e.RowIndex];

                    // Lấy giá trị từ các cột trong DataGridView
                    txtMahang.Text = row.Cells["maHang"].Value?.ToString();
                    txtTenHang.Text = row.Cells["tenHang"].Value?.ToString();
                    txtNoiSanXuat.Text = row.Cells["noiSanXuat"].Value?.ToString();
                   

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi chọn dòng: " + ex.Message);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            txtTenHang.Text = "";
            txtMahang.Text = "";
            txtNoiSanXuat.Text = "";
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMahang.Text) ||
                    string.IsNullOrWhiteSpace(txtTenHang.Text))
                   
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                using (var connection = new SqlConnection(strCon))
                {
                    connection.Open();

                    string checkExists = "SELECT COUNT(*) FROM Hang WHERE maHang = @maHang";
                    using (var cmdCheck = new SqlCommand(checkExists, connection))
                    {
                        cmdCheck.Parameters.AddWithValue("@maHang", txtMahang.Text.Trim());
                        int count = (int)cmdCheck.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Mã hãng đã tồn tại!");
                            return;
                        }
                    }

                    string sqlInsert = "INSERT INTO Hang (maHang, tenHang, noiSanXuat) " +
                                       "VALUES (@maHang, @tenHang, @noiSanXuat)";
                    using (var cmdInsert = new SqlCommand(sqlInsert, connection))
                    {
                        cmdInsert.Parameters.AddWithValue("@maHang", txtMahang.Text.Trim());
                        cmdInsert.Parameters.AddWithValue("@tenHang", txtTenHang.Text.Trim());
                        cmdInsert.Parameters.AddWithValue("@noiSanXuat", txtNoiSanXuat.Text.Trim());


                        int rowsAffected = cmdInsert.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Thêm hãng thành công!");
                            LoadData();
                            taoXML.TaoXML("SELECT * FROM Hang", "Hang", fileXML);
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
                if (string.IsNullOrWhiteSpace(txtMahang.Text) ||
                    string.IsNullOrWhiteSpace(txtTenHang.Text))
                    
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                using (var connection = new SqlConnection(strCon))
                {
                    connection.Open();

                    // Kiểm tra xem mã nhân viên có tồn tại trong CSDL không
                    string checkExists = "SELECT COUNT(*) FROM Hang WHERE maHang = @maHang";
                    using (var cmdCheck = new SqlCommand(checkExists, connection))
                    {
                        cmdCheck.Parameters.AddWithValue("@maHang", txtMahang.Text.Trim());
                        int count = (int)cmdCheck.ExecuteScalar();
                        if (count == 0)
                        {
                            MessageBox.Show("Mã hãng không tồn tại!");
                            return;
                        }
                    }

                    // Cập nhật thông tin nhân viên
                    string sqlUpdate = "UPDATE Hang SET " +
                                       "tenHang = @tenHang, noiSanXuat = @noiSanXuat " +
                                       "WHERE maHang = @maHang"; // Sử dụng WHERE để chỉ cập nhật nhân viên có mã maNV

                    using (var cmdUpdate = new SqlCommand(sqlUpdate, connection))
                    {
                        cmdUpdate.Parameters.AddWithValue("@maHang", txtMahang.Text.Trim());
                        cmdUpdate.Parameters.AddWithValue("@tenHang", txtTenHang.Text.Trim());
                        cmdUpdate.Parameters.AddWithValue("@noiSanXuat", txtNoiSanXuat.Text.Trim());
                        


                        int rowsAffected = cmdUpdate.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cập nhật thông tin hãng thành công!");
                            LoadData(); // Tải lại dữ liệu
                            taoXML.TaoXML("SELECT * FROM Hang", "Hang", fileXML); // Cập nhật file XML
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
                if (string.IsNullOrWhiteSpace(txtMahang.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã hãng để xóa!");
                    return;
                }

                // Xác nhận hành động xóa
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa hãng này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    using (var connection = new SqlConnection(strCon))
                    {
                        connection.Open();

                        // Câu lệnh SQL xóa nhân viên theo mã nhân viên
                        string sqlDelete = "DELETE FROM Hang WHERE maHang = @maHang";

                        using (var cmdDelete = new SqlCommand(sqlDelete, connection))
                        {
                            cmdDelete.Parameters.AddWithValue("@maHang", txtMahang.Text.Trim());

                            int rowsAffected = cmdDelete.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa hãng thành công!");
                                LoadData(); // Tải lại dữ liệu sau khi xóa
                                taoXML.TaoXML("SELECT * FROM Hang", "Hang", fileXML); // Cập nhật file XML

                                txtMahang.Text = "";
                                txtTenHang.Text = "";
                                txtNoiSanXuat.Text = "";
                              
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy hãng với mã này!");
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
    }
}
