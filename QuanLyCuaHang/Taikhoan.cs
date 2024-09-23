using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHang
{
    internal class Taikhoan
    {
        private string tk;
        private string mk;

        public Taikhoan()
        {

        }   
        public Taikhoan(string tk,string mk)
        {
            this.tk = tk;
            this.mk = mk;
        }

        public bool Login(string username,string password)
        {
            if (username == this.tk || password == this.mk)
            {
                return true;
            }
            else return false;
        }
           
    }
}
