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
    public partial class InThongKe : Form
    {
        SqlConnection conn = new Ketnoi().getConn();
        DateTime begin;
        DateTime end;
        public InThongKe()
        {
            InitializeComponent();
        }
        public void LoadData(DateTime a, DateTime b)
        {
            begin = a;
            end = b;
        }

        
        private void InThongKe_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection = conn)
            {
                connection.Open();
                String cc = "Select HoaDonMatHang.MaMH,MatHang.TenMH,Sum(SoLuong) as SoLuongDaBan,MatHang.GiaNhap,Sum(SoLuong)*MatHang.GiaNhap as TongVon " +
                    ",MatHang.GiaBan,Sum(SoLuong)*MatHang.GiaBan as DoanhThu,(Sum(SoLuong)*(MatHang.GiaBan-MatHang.GiaNhap)) as LoiNhuan " +
                    "from HoaDonMatHang Inner join MatHang on MatHang.MaMH = HoaDonMatHang.MaMH " +
                    "where HoaDonMatHang.MaHD in (Select MaHD from HoaDon where HoaDon.NgayMua between @BatDau and @KetThuc)" +
                    "group by HoaDonMatHang.MaMH,MatHang.TenMH,MatHang.GiaBan,MatHang.GiaNhap " +
                    "order by Sum(SoLuong)*(MatHang.GiaBan-MatHang.GiaNhap) desc";
                // Truy xuất dữ liệu từ hai bảng
                SqlCommand cmd = new SqlCommand(cc, conn);
                cmd.Parameters.AddWithValue("@BatDau", begin);
                cmd.Parameters.AddWithValue("@KetThuc", end);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet1 ds = new DataSet1();
                adapter.Fill(ds, ds.Tables[1].TableName);
                

                // Thiết lập nguồn dữ liệu cho ReportViewer
                reportViewer1.LocalReport.ReportEmbeddedResource = "QuanLyCuaHang.rpThongKe.rdlc";

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[1]));


                SqlCommand cmd1 = new SqlCommand("select Sum(SoLuong) from HoaDonMatHang where HoaDonMatHang.MaHD in (Select MaHD from HoaDon where HoaDon.NgayMua between @BatDau and @KetThuc);", conn);
                cmd1.Parameters.AddWithValue("@BatDau", begin);
                cmd1.Parameters.AddWithValue("@KetThuc", end);
                SqlDataReader dr = cmd1.ExecuteReader();
                if (dr.Read())
                {
                    ReportParameter tongsoluong = new ReportParameter("TongSoLuong", dr[0].ToString());
                    reportViewer1.LocalReport.SetParameters(tongsoluong);

                }
                dr.Close();
                
                SqlCommand cmd2 = new SqlCommand("select Sum(SoLuong*MatHang.GiaNhap),Sum(SoLuong*MatHang.GiaBan),Sum(SoLuong*(MatHang.GiaBan-MatHang.GiaNhap)) from HoaDonMatHang,MatHang where HoaDonMatHang.MaMH = MatHang.MaMH and HoaDonMatHang.MaHD in (Select MaHD from HoaDon where HoaDon.NgayMua between @BatDau and @KetThuc)", conn);
                cmd2.Parameters.AddWithValue("@BatDau", begin);
                cmd2.Parameters.AddWithValue("@KetThuc", end);
                SqlDataReader dr1 = cmd2.ExecuteReader();
                if (dr1.Read())
                {
                    ReportParameter tongvon = new ReportParameter("TongVon", dr1[0].ToString());
                    reportViewer1.LocalReport.SetParameters(tongvon);
                    ReportParameter tongdoanhthu = new ReportParameter("TongDoanhThu", dr1[1].ToString());
                    reportViewer1.LocalReport.SetParameters(tongdoanhthu);
                    ReportParameter tongloinhuan = new ReportParameter("TongLoiNhuan", dr1[2].ToString());
                    reportViewer1.LocalReport.SetParameters(tongloinhuan);

                }
                dr1.Close();

                ReportParameter bd = new ReportParameter("NgayBatDau", begin.ToString("dd/MM/yyyy HH:mm:ss"));
                reportViewer1.LocalReport.SetParameters(bd); 
                ReportParameter kt = new ReportParameter("NgayKetThuc", end.ToString("dd/MM/yyyy HH:mm:ss"));
                reportViewer1.LocalReport.SetParameters(kt);

                // Hiển thị báo cáo
                reportViewer1.RefreshReport();
            }
        }
    }
}
