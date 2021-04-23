using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyHocSinhTHPT_ADO.BL
{
    class BLAccount
    {
        
        public BLAccount()
        {
        }
        //
        public bool AddAccount(string username, string pass)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            TaiKhoan item = new TaiKhoan();
            item.TenTK = username;
            item.Pass = pass;
            qlhs.TaiKhoans.Add(item);
            qlhs.SaveChanges();
            return true;
        }
        //
        public bool UpdateAccount(string username, string pass)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = (from item in qlhs.TaiKhoans
                         where item.TenTK == username
                         select item).SingleOrDefault();
            if (query != null)
            {
                query.Pass = pass;
                qlhs.SaveChanges();
            }
            return true;
        }
        //
        public bool DeleteAccount(string username)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            TaiKhoan item = new TaiKhoan();
            item.TenTK = username;
            qlhs.TaiKhoans.Attach(item);
            qlhs.TaiKhoans.Remove(item);
            qlhs.SaveChanges();
            return true;
        }
        //
        public bool CheckAccount(string username, string password, TypeAccount typeAcc, ref string errorLogin)
        {
            if (IsExist(username,typeAcc))
            {
                QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
                var query = from item in qlhs.TaiKhoans
                            where item.TenTK == username && item.Pass == password
                            select item;
                if (query.Count() != 0)
                    return true;
                else
                    errorLogin = "Sai mật khẩu!";
            }
            else
            {
                errorLogin = "Tài khoản không tồn tại!";
            }
            return false;
        }
        //
        private bool IsExist(string username, TypeAccount typeAcc)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            if (typeAcc == TypeAccount.Student)
            {
                var query = from item in qlhs.HocSinhs
                        where item.TenTK == username
                        select item;
                if (query.Count() != 0)
                    return true;
            }
            else if (typeAcc == TypeAccount.Teacher)
            {
                var query = from item in qlhs.GiaoViens
                            where item.TenTK == username
                            select item;
                if (query.Count() != 0)
                    return true;
            }
            else if (!username.Contains("HS") && !username.Contains("GV"))
            {
                var query = from item in qlhs.TaiKhoans
                            where item.TenTK == username
                            select item;
                if (query.Count() != 0)
                    return true;
            }
            return false;
        }
    }
}
