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
    public partial class MatHang : Form
    {
        SqlConnection conn = new Ketnoi().getConn();//SqlConnection("Data Source=(localdb)\\mssqllocaldb;Initial Catalog=QLCH;Integrated Security=True");
        DataTable dt = new DataTable();
        public MatHang()
        {
            InitializeComponent();
            
        }
        public void reload()
        {
            SqlCommand cmd = new SqlCommand("Select * from MatHang", conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            dt = new DataTable();
            adapter.Fill(dt);
            listView1.Items.Clear();
            datatolist(dt);
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


        private void MatHang_Load(object sender, EventArgs e)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Select * from MatHang", conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                dt.Clear();
                adapter.Fill(dt);
                datatolist(dt);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("Insert into MatHang (MaMH,TenMH,DonViTinh,GiaNhap) Values (@MaMH,@TenMH,@DonViTinh,@GiaNhap);UPDATE MatHang SET Giaban = (Gianhap * 1.3);", conn);
            cmd.Parameters.AddWithValue("@MaMH", textBox1.Text);
            cmd.Parameters.AddWithValue("@TenMH", textBox2.Text);
            cmd.Parameters.AddWithValue("@DonViTinh", textBox3.Text);
            cmd.Parameters.AddWithValue("@GiaNhap", double.Parse(textBox4.Text));
            if (cmd.ExecuteNonQuery()>0)
            {

                reload();
            }
            else
            {
                MessageBox.Show("Lỗi");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("Delete from MatHang where MaMH = @MaMHCu", conn);
            cmd.Parameters.AddWithValue("@MaMHCu", listView1.SelectedItems[0].Text);
            if (cmd.ExecuteNonQuery() > 0)
            {
                reload();
            }
            else
            {
                MessageBox.Show("Lỗi");
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                textBox1.Text = listView1.SelectedItems[0].Text;
                textBox2.Text = listView1.SelectedItems[0].SubItems[1].Text;
                textBox3.Text = listView1.SelectedItems[0].SubItems[2].Text;
                textBox4.Text = listView1.SelectedItems[0].SubItems[3].Text;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("update MatHang set MaMH = @MaMH, TenMH = @TenMh, DonViTinh = @DonViTinh,GiaBan = @GiaBan where MaMH = @MaMHCu;UPDATE MatHang SET Giaban = (Gianhap * 1.3) WHERE MaMH = @MaMH;", conn);
            cmd.Parameters.AddWithValue("@MaMH", textBox1.Text);
            cmd.Parameters.AddWithValue("@TenMH", textBox2.Text);
            cmd.Parameters.AddWithValue("@DonViTinh", textBox3.Text);
            cmd.Parameters.AddWithValue("@GiaBan", double.Parse(textBox4.Text));
            cmd.Parameters.AddWithValue("@MaMHCu", listView1.SelectedItems[0].Text);
            if (cmd.ExecuteNonQuery() > 0)
            {

                reload();
            }
            else
            {
                MessageBox.Show("Lỗi");
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand($"Select * from MatHang where MaMH like '{textBox5.Text}%' or TenMH like '{textBox5.Text}%'", conn);
            if (textBox5.Text == String.Empty)
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

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
