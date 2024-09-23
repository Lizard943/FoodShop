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
    public partial class ThongKe : Form
    {
        SqlConnection conn = new Ketnoi().getConn();//SqlConnection("Data Source=(localdb)\\mssqllocaldb;Initial Catalog=QLCH;Integrated Security=True");
        DataTable dt = new DataTable();
        public ThongKe()
        {
            InitializeComponent();
            dateTimePicker1.CustomFormat = "MM/yyyy";
            dateTimePicker2.CustomFormat = "MM/yyyy";
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
                SqlCommand cmd = new SqlCommand("Select HoaDonMatHang.MaMH,MatHang.TenMH,Sum(SoLuong),MatHang.GiaNhap,Sum(SoLuong)*MatHang.GiaNhap,MatHang.GiaBan,Sum(SoLuong)*MatHang.GiaBan,Sum(SoLuong)*(MatHang.GiaBan-MatHang.GiaNhap)\r\nfrom HoaDonMatHang Inner join MatHang on MatHang.MaMH = HoaDonMatHang.MaMH \r\ngroup by HoaDonMatHang.MaMH,MatHang.TenMH,MatHang.GiaBan,MatHang.GiaNhap order by Sum(SoLuong)*(MatHang.GiaBan-MatHang.GiaNhap) desc", conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                dt.Clear();
                adapter.Fill(dt);
                datatolist(dt);
            }
        }
        public void reload()
        {
            SqlCommand cmd = new SqlCommand("Select HoaDonMatHang.MaMH,MatHang.TenMH,Sum(SoLuong),MatHang.GiaNhap,Sum(SoLuong)*MatHang.GiaNhap,MatHang.GiaBan,Sum(SoLuong)*MatHang.GiaBan,Sum(SoLuong)*(MatHang.GiaBan-MatHang.GiaNhap)\r\nfrom HoaDonMatHang Inner join MatHang on MatHang.MaMH = HoaDonMatHang.MaMH \r\ngroup by HoaDonMatHang.MaMH,MatHang.TenMH,MatHang.GiaBan,MatHang.GiaNhap order by Sum(SoLuong)*(MatHang.GiaBan-MatHang.GiaNhap) desc", conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            dt = new DataTable();
            adapter.Fill(dt);
            listView1.Items.Clear();
            datatolist(dt);
        }
    }
}
