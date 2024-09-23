using Microsoft.Reporting.WinForms;
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

    public partial class BanHang : Form
    {
        public delegate void SendData(String data);
        public SendData send;

        SqlConnection conn = new Ketnoi().getConn();
        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();
        Random rand = new Random();
        String ma = string.Empty;
        String makh = string.Empty;
        public BanHang()
        {
            InitializeComponent();
        }
        public void updatesohoadon()
        {
            SqlCommand cmd = new SqlCommand("Select Count(*) from (Select Distinct MaHD from HoaDon) as subquery",conn);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                ma = "HD0" + dr[0].ToString();
                
            }
            dr.Close();
        }
        
        public void datatolist(ListView lv,DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                ListViewItem item = new ListViewItem(dr[0].ToString());
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    item.SubItems.Add(dr[i].ToString());
                }

                lv.Items.Add(item);
            }
        }
        public void reload1()
        {
            SqlCommand cmd = new SqlCommand("Select TenMH,GiaBan,SoLuong,SoLuong*GiaBan as Tong from HoaDonMatHang,MatHang where MaHD = @MaHD and HoaDonMatHang.MaMH = MatHang.MaMH", conn);
            cmd.Parameters.AddWithValue("@MaHD",ma);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            dt2 = new DataTable();
            listView2.Items.Clear();
            adapter.Fill(dt2);
            datatolist(listView2, dt2);

            SqlCommand cmd2 = new SqlCommand("Select Sum(GiaBan*SoLuong) from HoaDonMatHang,MatHang where MaHD = @MaHD and HoaDonMatHang.MaMH = MatHang.MaMH", conn);
            cmd2.Parameters.AddWithValue("MaHD", ma);
            SqlDataReader reader = cmd2.ExecuteReader();
            if (reader.Read())
            {
                label7.Text = reader[0].ToString() + "đ";
            }
            reader.Close();


        }
        public void reload2()
        {
            SqlCommand cmd = new SqlCommand("Select MaMH,TenMH,DonViTinh,GiaBan from MatHang",conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            dt1 = new DataTable();
            listView1.Items.Clear();
            adapter.Fill(dt1);
            datatolist(listView1, dt1);
        }


        private void BanHang_Load(object sender, EventArgs e)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
                ma = "HD" + rand.Next(100000, 999999).ToString();
                makh = "KH" + rand.Next(100000,999999).ToString();
                textBox5.Text = makh;

                SqlCommand cmd = new SqlCommand("Select MaMH,TenMH,DonViTinh,GiaBan from MatHang", conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                dt1 = new DataTable();
                listView1.Items.Clear();
                adapter.Fill(dt1);
                datatolist(listView1, dt1);


                SqlCommand cmd1 = new SqlCommand("Select TenMH,GiaBan,SoLuong,SoLuong*GiaBan as Tong from HoaDonMatHang,MatHang where MaHD = @MaHD and HoaDonMatHang.MaMH = MatHang.MaMH", conn);
                cmd1.Parameters.AddWithValue("@MaHD", ma);
                SqlDataAdapter adapter1 = new SqlDataAdapter(cmd1);
                dt2 = new DataTable();
                listView2.Items.Clear();
                adapter1.Fill(dt2);
                datatolist(listView2, dt2);


                SqlCommand cmd2 = new SqlCommand("Select Sum(GiaBan*SoLuong) from HoaDonMatHang,MatHang where MaHD = @MaHD and HoaDonMatHang.MaMH = MatHang.MaMH", conn);
                cmd2.Parameters.AddWithValue("MaHD", ma);
                SqlDataReader reader = cmd2.ExecuteReader();
                if (reader.Read())
                {
                    label7.Text = reader[0].ToString() + "đ";
                }
                reader.Close();

                


            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                textBox3.Text = listView1.SelectedItems[0].SubItems[1].Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("Insert into HoaDonMatHang Values (@MaHD,@MaMH,@SoLuong)", conn);
            cmd.Parameters.AddWithValue("@MaHD", ma);
            cmd.Parameters.AddWithValue("@MaMH", listView1.SelectedItems[0].SubItems[0].Text);
            cmd.Parameters.AddWithValue("@SoLuong", domainUpDown1.Text);
            if (cmd.ExecuteNonQuery() > 0)
            {
                reload1();
                reload2();
                
            }
            else
            {
                MessageBox.Show("Lỗi");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("Delete from HoaDonMatHang where MaMH = (select MaMH from MatHang where TenMH = @TenMH)",conn);
            cmd.Parameters.AddWithValue("@TenMH", listView2.SelectedItems[0].Text);
            if (cmd.ExecuteNonQuery() > 0)
            {
                reload1();
                reload2();
            }
            else
            {
                MessageBox.Show("Lỗi");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("Update HoaDonMatHang set SoLuong = @SoLuong where MaMH = (select MaMH from MatHang where TenMH = @TenMH)", conn);
            cmd.Parameters.AddWithValue("@SoLuong", domainUpDown1.Text);
            cmd.Parameters.AddWithValue("@TenMH", listView2.SelectedItems[0].Text);
            if (cmd.ExecuteNonQuery() > 0)
            {
                reload1();
                reload2();
            }
            else
            {
                MessageBox.Show("Lỗi");
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand($"Select * from MatHang where TenMH like N'{textBox4.Text}%' or MaMH like N'{textBox4.Text}%' ", conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            dt1 = new DataTable();
            listView1.Items.Clear();
            adapter.Fill(dt1);
            datatolist(listView1, dt1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("Delete from HoaDonMatHang where MaHD = @MaHD", conn);
            cmd.Parameters.AddWithValue("@MaHD", ma);
            if (cmd.ExecuteNonQuery() > 0)
            {
                reload1();
                reload2();
            }
            else
            {
                MessageBox.Show("Lỗi");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {


            SqlCommand cmd = new SqlCommand("Insert into KhachHang Values (@MaKH,@TenKH,@Sdt); Insert into HoaDon values (@MaHD,@MaKH,CURRENT_TIMESTAMP);", conn);
            cmd.Parameters.AddWithValue("@MaKH", textBox5.Text);
            cmd.Parameters.AddWithValue("@TenKH", textBox1.Text);
            cmd.Parameters.AddWithValue("@Sdt", textBox2.Text);
            cmd.Parameters.AddWithValue("@MaHD", ma);

            if (cmd.ExecuteNonQuery() > 0)
            {
                reload1();
                reload2();

            }
            else
            {
                MessageBox.Show("Lỗi");
            }



            BaoCao report = new BaoCao();
            report.LoadData(ma);
            report.ShowDialog();



        }

        private void BanHang_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            conn.Close();
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
