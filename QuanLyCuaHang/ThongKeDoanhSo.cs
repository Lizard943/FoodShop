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

namespace QuanLyCuaHang
{
    public partial class ThongKeDoanhSo : Form
    {
        SqlConnection conn = new Ketnoi().getConn();//SqlConnection("Data Source=(localdb)\\mssqllocaldb;Initial Catalog=QLCH;Integrated Security=True");
        DataTable dt = new DataTable();
        public ThongKeDoanhSo()
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
        private void ThongKe_Load(object sender, EventArgs e)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
                String cc = "Select HoaDonMatHang.MaMH,MatHang.TenMH,Sum(SoLuong) as [Số Lượng],MatHang.GiaNhap,Sum(SoLuong)*MatHang.GiaNhap as [Vốn]" +
                    ",MatHang.GiaBan,Sum(SoLuong)*MatHang.GiaBan as [Doanh thu],Sum(SoLuong)*(MatHang.GiaBan-MatHang.GiaNhap) as [Lợi nhuận]" +
                    "from HoaDonMatHang Inner join MatHang on MatHang.MaMH = HoaDonMatHang.MaMH " +
                    "where HoaDonMatHang.MaHD in (Select MaHD from HoaDon where HoaDon.NgayMua between @BatDau and @KetThuc)" +
                    "group by HoaDonMatHang.MaMH,MatHang.TenMH,MatHang.GiaBan,MatHang.GiaNhap " +
                    "order by Sum(SoLuong)*(MatHang.GiaBan-MatHang.GiaNhap) desc";
                SqlCommand cmd = new SqlCommand(cc, conn);
                cmd.Parameters.AddWithValue("@BatDau",dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@KetThuc", dateTimePicker2.Value);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                dt.Clear();
                adapter.Fill(dt);
                datatolist(dt);


                SqlCommand cmd1 = new SqlCommand("select Sum(SoLuong) from HoaDonMatHang where HoaDonMatHang.MaHD in (Select MaHD from HoaDon where HoaDon.NgayMua between @BatDau and @KetThuc)", conn);
                cmd1.Parameters.AddWithValue("@BatDau", dateTimePicker1.Value);
                cmd1.Parameters.AddWithValue("@KetThuc", dateTimePicker2.Value);
                SqlDataReader dr = cmd1.ExecuteReader();
                if (dr.Read())
                {
                    label3.Text = dr[0].ToString();
                }
                dr.Close();

                SqlCommand cmd2 = new SqlCommand("select Sum(SoLuong*MatHang.GiaBan) from HoaDonMatHang,MatHang where HoaDonMatHang.MaMH = MatHang.MaMH and HoaDonMatHang.MaHD in (Select MaHD from HoaDon where HoaDon.NgayMua between @BatDau and @KetThuc)", conn);
                cmd2.Parameters.AddWithValue("@BatDau", dateTimePicker1.Value);
                cmd2.Parameters.AddWithValue("@KetThuc", dateTimePicker2.Value);
                SqlDataReader dr2 = cmd2.ExecuteReader();
                if (dr2.Read())
                {
                    label6.Text = dr2[0].ToString() + "đ";
                }
                dr2.Close();

                SqlCommand cmd3 = new SqlCommand("select Sum(SoLuong*MatHang.GiaNhap) from HoaDonMatHang,MatHang where HoaDonMatHang.MaMH = MatHang.MaMH and HoaDonMatHang.MaHD in (Select MaHD from HoaDon where HoaDon.NgayMua between @BatDau and @KetThuc)", conn);
                cmd3.Parameters.AddWithValue("@BatDau", dateTimePicker1.Value);
                cmd3.Parameters.AddWithValue("@KetThuc", dateTimePicker2.Value);
                SqlDataReader dr3 = cmd3.ExecuteReader();
                if (dr3.Read())
                {
                    label12.Text = dr3[0].ToString() + "đ";
                }
                dr3.Close();

                SqlCommand cmd4 = new SqlCommand("select Sum(SoLuong*(MatHang.GiaBan-MatHang.GiaNhap)) from HoaDonMatHang,MatHang where HoaDonMatHang.MaMH = MatHang.MaMH and HoaDonMatHang.MaHD in (Select MaHD from HoaDon where HoaDon.NgayMua between @BatDau and @KetThuc) ", conn);
                cmd4.Parameters.AddWithValue("@BatDau", dateTimePicker1.Value);
                cmd4.Parameters.AddWithValue("@KetThuc", dateTimePicker2.Value);
                SqlDataReader dr4 = cmd4.ExecuteReader();
                if (dr4.Read())
                {
                    label8.Text = dr4[0].ToString() + "đ";
                }
                dr4.Close();

            }
        }
        public void reload()
        {
            String cc = "Select HoaDonMatHang.MaMH,MatHang.TenMH,Sum(SoLuong) as [Số Lượng],MatHang.GiaNhap,Sum(SoLuong)*MatHang.GiaNhap as [Vốn]" +
                    ",MatHang.GiaBan,Sum(SoLuong)*MatHang.GiaBan as [Doanh thu],Sum(SoLuong)*(MatHang.GiaBan-MatHang.GiaNhap) as [Lợi nhuận]" +
                    "from HoaDonMatHang Inner join MatHang on MatHang.MaMH = HoaDonMatHang.MaMH " +
                    "where HoaDonMatHang.MaHD in (Select MaHD from HoaDon where HoaDon.NgayMua between @BatDau and @KetThuc)" +
                    "group by HoaDonMatHang.MaMH,MatHang.TenMH,MatHang.GiaBan,MatHang.GiaNhap " +
                    "order by Sum(SoLuong)*(MatHang.GiaBan-MatHang.GiaNhap) desc";
            SqlCommand cmd = new SqlCommand(cc, conn);
            cmd.Parameters.AddWithValue("@BatDau", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@KetThuc", dateTimePicker2.Value);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            dt.Clear();
            adapter.Fill(dt);
            listView1.Items.Clear();
            datatolist(dt);


            SqlCommand cmd1 = new SqlCommand("select Sum(SoLuong) from HoaDonMatHang where HoaDonMatHang.MaHD in (Select MaHD from HoaDon where HoaDon.NgayMua between @BatDau and @KetThuc)", conn);
            cmd1.Parameters.AddWithValue("@BatDau", dateTimePicker1.Value);
            cmd1.Parameters.AddWithValue("@KetThuc", dateTimePicker2.Value);
            SqlDataReader dr = cmd1.ExecuteReader();
            if (dr.Read())
            {
                label3.Text = dr[0].ToString();
            }
            dr.Close();

            SqlCommand cmd2 = new SqlCommand("select Sum(SoLuong*MatHang.GiaBan) from HoaDonMatHang,MatHang where HoaDonMatHang.MaMH = MatHang.MaMH and HoaDonMatHang.MaHD in (Select MaHD from HoaDon where HoaDon.NgayMua between @BatDau and @KetThuc)", conn);
            cmd2.Parameters.AddWithValue("@BatDau", dateTimePicker1.Value);
            cmd2.Parameters.AddWithValue("@KetThuc", dateTimePicker2.Value);
            SqlDataReader dr2 = cmd2.ExecuteReader();
            if (dr2.Read())
            {
                label6.Text = dr2[0].ToString() + "đ";
            }
            dr2.Close();

            SqlCommand cmd3 = new SqlCommand("select Sum(SoLuong*MatHang.GiaNhap) from HoaDonMatHang,MatHang where HoaDonMatHang.MaMH = MatHang.MaMH and HoaDonMatHang.MaHD in (Select MaHD from HoaDon where HoaDon.NgayMua between @BatDau and @KetThuc)", conn);
            cmd3.Parameters.AddWithValue("@BatDau", dateTimePicker1.Value);
            cmd3.Parameters.AddWithValue("@KetThuc", dateTimePicker2.Value);
            SqlDataReader dr3 = cmd3.ExecuteReader();
            if (dr3.Read())
            {
                label12.Text = dr3[0].ToString() + "đ";
            }
            dr3.Close();

            SqlCommand cmd4 = new SqlCommand("select Sum(SoLuong*(MatHang.GiaBan-MatHang.GiaNhap)) from HoaDonMatHang,MatHang where HoaDonMatHang.MaMH = MatHang.MaMH and HoaDonMatHang.MaHD in (Select MaHD from HoaDon where HoaDon.NgayMua between @BatDau and @KetThuc)", conn);
            cmd4.Parameters.AddWithValue("@BatDau", dateTimePicker1.Value);
            cmd4.Parameters.AddWithValue("@KetThuc", dateTimePicker2.Value);
            SqlDataReader dr4 = cmd4.ExecuteReader();
            if (dr4.Read())
            {
                label8.Text = dr4[0].ToString() + "đ";
            }
            dr4.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            reload();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InThongKe inThongKe = new InThongKe();
            inThongKe.LoadData(dateTimePicker1.Value, dateTimePicker2.Value);
            inThongKe.ShowDialog();
        }
    }
}
