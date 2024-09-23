using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyCuaHang
{
    public partial class TraCuuHoaDon : Form
    {
        SqlConnection conn = new Ketnoi().getConn();//SqlConnection("Data Source=(localdb)\\mssqllocaldb;Initial Catalog=QLCH;Integrated Security=True");
        DataTable dt = new DataTable();
        public TraCuuHoaDon()
        {
            InitializeComponent();
        }
        public void datatolist(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                ListViewItem item = new ListViewItem(dr[0].ToString());
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    item.SubItems.Add(dr[i].ToString());
                }

                listView1.Items.Add(item);
            }
        }
        public void reload()
        {
            SqlCommand cmd = new SqlCommand("Select MaHD,TenKH,Sdt,NgayMua,(select Sum(GiaBan*SoLuong) from HoaDonMatHang,MatHang where HoaDonMatHang.MaMH like MatHang.MaMH and HoaDon.MaHD = HoaDonMatHang.MaHD ) from HoaDon,KhachHang where HoaDon.MaKH = KhachHang.MaKH", conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            dt.Clear();
            adapter.Fill(dt);
            listView1.Items.Clear();
            datatolist(dt);
        }
        private void TraCuuHoaDon_Load(object sender, EventArgs e)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Select MaHD,TenKH,Sdt,NgayMua,(select Sum(GiaBan*SoLuong) from HoaDonMatHang,MatHang where HoaDonMatHang.MaMH like MatHang.MaMH and HoaDon.MaHD = HoaDonMatHang.MaHD ) from HoaDon,KhachHang where HoaDon.MaKH = KhachHang.MaKH", conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                dt.Clear();
                adapter.Fill(dt);
                datatolist(dt);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BaoCao report = new BaoCao();
            report.LoadData(listView1.SelectedItems[0].Text);
            report.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("Delete from HoaDon where MaHD = @MaHDCu;Delete from HoaDonMatHang where MaHD = @MaHDCu", conn);
            cmd.Parameters.AddWithValue("@MaHDCu", listView1.SelectedItems[0].Text);
            if (cmd.ExecuteNonQuery() > 0)
            {
                reload();
            }
            else
            {
                MessageBox.Show("Lỗi");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand($"Select MaHD,TenKH,Sdt,NgayMua,(select Sum(GiaBan*SoLuong) from HoaDonMatHang,MatHang where HoaDonMatHang.MaMH like MatHang.MaMH and HoaDon.MaHD = HoaDonMatHang.MaHD ) from HoaDon,KhachHang where HoaDon.MaKH = KhachHang.MaKH and (TenKH like N'%{textBox1.Text}%' or Sdt like '{textBox1.Text}%')", conn);
            if (textBox1.Text == String.Empty)
            {
                reload();
                return;
            }
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            dt.Clear();
            adapter.Fill(dt);
            listView1.Items.Clear();
            datatolist(dt);
        }

        private void TraCuuHoaDon_FormClosed(object sender, FormClosedEventArgs e)
        {
            conn.Close();
        }
    }
}
