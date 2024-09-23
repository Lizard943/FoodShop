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
    public partial class BaoCao : Form
    {   
        SqlConnection conn = new Ketnoi().getConn();
        String ma = string.Empty;
        public BaoCao()
        {
            InitializeComponent();
        }
        public void LoadData(String Ma)
        {
            ma = Ma;
        }
        private void BaoCao_Load(object sender, EventArgs e)
        {


            using (SqlConnection connection = conn)
            {
                connection.Open();

                // Truy xuất dữ liệu từ hai bảng
                SqlCommand cmd = new SqlCommand("SELECT HoaDonMatHang.MaMH, MatHang.TenMH, MatHang.GiaBan, MatHang.DonViTinh, HoaDonMatHang.SoLuong, (MatHang.GiaBan*HoaDonMatHang.SoLuong) as Tong FROM HoaDonMatHang INNER JOIN MatHang ON HoaDonMatHang.MaMH = MatHang.MaMH where MaHD = @MaHD", conn);
                cmd.Parameters.AddWithValue("@MaHD",ma);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet1 ds = new DataSet1();
                adapter.Fill(ds, ds.Tables[0].TableName);
                //adapter.Fill(ds, "HoaDonMatHang");

                // Thiết lập nguồn dữ liệu cho ReportViewer
                reportViewer1.LocalReport.ReportEmbeddedResource = "QuanLyCuaHang.rpHoaDon.rdlc";
                
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[0]));


                SqlCommand cmd1 = new SqlCommand("Select MaKH,TenKH,Sdt from KhachHang where MaKH = (Select MaKH from HoaDon where MaHD = @MaHD);", conn);
                cmd1.Parameters.AddWithValue("@MaHD", ma);
                SqlDataReader dr = cmd1.ExecuteReader();
                if (dr.Read())
                {
                    ReportParameter makh = new ReportParameter("MaKH", dr[0].ToString());
                    reportViewer1.LocalReport.SetParameters(makh);
                    ReportParameter tenkh = new ReportParameter("TenKH", dr[1].ToString());
                    reportViewer1.LocalReport.SetParameters(tenkh);
                    ReportParameter sdt = new ReportParameter("SDT", dr[2].ToString());
                    reportViewer1.LocalReport.SetParameters(sdt);

                }
                dr.Close();

                SqlCommand cmd2 = new SqlCommand("Select NgayMua from HoaDon where MaHD = @MaHD", conn);
                cmd2.Parameters.AddWithValue("@MaHD", ma);
                SqlDataReader dr1 = cmd2.ExecuteReader();
                if (dr1.Read())
                {
                    ReportParameter ngaymua = new ReportParameter("NgayMua", dr1[0].ToString());
                    reportViewer1.LocalReport.SetParameters(ngaymua);

                }
                dr1.Close();


                SqlCommand cmd3 = new SqlCommand("select Sum(GiaBan*SoLuong) from HoaDonMatHang,MatHang where HoaDonMatHang.MaMH like MatHang.MaMH and MaHD = @MaHD;", conn);
                cmd3.Parameters.AddWithValue("@MaHD", ma);
                SqlDataReader dr2 = cmd3.ExecuteReader();
                if (dr2.Read())
                {
                    ReportParameter ttt = new ReportParameter("TTT", dr2[0].ToString());
                    reportViewer1.LocalReport.SetParameters(ttt);

                }
                dr2.Close();

                ReportParameter mahd = new ReportParameter("MaHD",ma);
                reportViewer1.LocalReport.SetParameters(mahd);

                // Hiển thị báo cáo
                reportViewer1.RefreshReport();
            }
        }
    }
}
