using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCuaHang
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        private Form formhientai;
        private void Openformchild(Form form)
        {
            if (formhientai != null)
            {
                formhientai.Close();
            }
            formhientai = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            mainpanel.Controls.Add(form);
            mainpanel.Tag = form;
            formhientai.BringToFront();
            formhientai.Show();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Openformchild(new BanHang());
            label1.Text = "Quản lý bán hàng";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Openformchild(new MatHang());
            label1.Text = "Quản lý mặt hàng";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Openformchild(new TraCuuHoaDon());
            label1.Text = "Tra cứu hóa đơn";
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Openformchild(new ThongKeDoanhSo());
            label1.Text = "Thống kê";
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        
    }
}
