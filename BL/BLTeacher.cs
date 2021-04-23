using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyHocSinhTHPT_ADO.BL
{
    class BLTeacher
    {
        BLAccount blAccount;
        public BLTeacher()
        {
            blAccount = new BLAccount();
        }

        public bool AddTeacher(string lName, string fName, DateTime birthday, bool isFemale, string address, string phone, string idSubject)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            string idTeacher = CreateId();
            string pass = birthday.ToString("ddMMyyyy");
            blAccount.AddAccount(idTeacher, pass);

            GiaoVien item = new GiaoVien();
            item.MaGV = idTeacher;
            item.Ho = lName;
            item.Ten = fName;
            item.NgaySinh = birthday;
            item.Nu = isFemale;
            item.DiaChi = address;
            item.SoDT = phone;
            item.MaMH = idSubject;
            item.TenTK = idTeacher;
            qlhs.GiaoViens.Add(item);
            qlhs.SaveChanges();
            return true;
        }

        public bool UpdateTeacher(string idTeacher, string pass, string lName, string fName, DateTime birthday, bool isFemale, string address, string phone)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = (from item in qlhs.GiaoViens
                         where item.MaGV == idTeacher
                         select item).SingleOrDefault();
            if (query != null)
            {
                query.Ho = lName;
                query.Ten = fName;
                query.NgaySinh = birthday;
                query.Nu = isFemale;
                query.DiaChi = address;
                query.SoDT = phone;
                query.TaiKhoan.Pass = pass;
                qlhs.SaveChanges();
            }
            return true;
        }

        public bool DeleteTeacher(string idTeacher)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            GiaoVien item = new GiaoVien();
            item.MaGV = idTeacher;
            qlhs.GiaoViens.Attach(item);
            qlhs.GiaoViens.Remove(item);
            qlhs.SaveChanges();

            blAccount.DeleteAccount(idTeacher);
            return true;
        }

        public DataRow GetTeacherById(string id)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = (from item in qlhs.GiaoViens
                         where item.MaGV == id
                         select item);
            DataTable dt = new DataTable();
            dt.Columns.Add("MaGV");
            dt.Columns.Add("Ho");
            dt.Columns.Add("Ten");
            dt.Columns.Add("NgaySinh", typeof(DateTime));
            dt.Columns.Add("Nu", typeof(bool));
            dt.Columns.Add("DiaChi");
            dt.Columns.Add("SoDT");
            dt.Columns.Add("MaMH");
            dt.Columns.Add("TenTK");
            foreach (var item in query)
            {
                dt.Rows.Add(item.MaGV, item.Ho, item.Ten, item.NgaySinh, item.Nu, item.DiaChi, item.SoDT, item.MaMH, item.TenTK);
            }
            return dt.Rows[0];
        }

        public string CreateId()
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = (from item in qlhs.GiaoViens
                         orderby item.MaGV descending
                         select item);
            string idLast = "";
            foreach (var item in query)
            {
                idLast = item.MaGV;
                break;
            }
            int id = int.Parse(idLast.Substring(2));
            string numId = (++id).ToString();
            string idNew = "GV";
            for (int i = 0; i < 3 - numId.Count(); i++)
            {
                idNew += '0';
            }
            idNew += numId;
            return idNew;
        }

        public DataTable GetListTeacher()
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = (from item in qlhs.GiaoViens
                         select item);
            DataTable dt = new DataTable();
            dt.Columns.Add("MaGV");
            dt.Columns.Add("Ho");
            dt.Columns.Add("Ten");
            dt.Columns.Add("NgaySinh", typeof(DateTime));
            dt.Columns.Add("Nu", typeof(bool));
            dt.Columns.Add("DiaChi");
            dt.Columns.Add("SoDT");
            dt.Columns.Add("MaMH");
            dt.Columns.Add("TenTK");
            dt.Columns.Add("Pass");
            foreach (var item in query)
            {
                dt.Rows.Add(item.MaGV, item.Ho, item.Ten, item.NgaySinh, item.Nu, item.DiaChi, item.SoDT, item.MaMH, item.TenTK, item.TaiKhoan.Pass);
            }
            return dt;
        }
        public DataTable GetListTeacherNotManage(string idClass)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = (from item in qlhs.GiaoViens
                         select item);
            DataTable dt = new DataTable();
            dt.Columns.Add("MaGV");
            dt.Columns.Add("HoTen");
            foreach (var item in query)
            {
                dt.Rows.Add(item.MaGV, item.Ho + " " + item.Ten);
            }
            return dt;
        }

        public DataTable GetListTeacherByIdSubject(string idSubject)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = (from item in qlhs.GiaoViens
                         where item.MaMH == idSubject
                         select item);
            DataTable dt = new DataTable();
            dt.Columns.Add("MaGV");
            dt.Columns.Add("HoTen");
            foreach (var item in query)
            {
                dt.Rows.Add(item.MaGV, item.Ho + " " + item.Ten);
            }
            return dt;
        }

        public DataTable SearchTeacherByIdOrName(string idName)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = (from item in qlhs.GiaoViens
                         where item.MaGV.Contains(idName) || item.Ten.Contains(idName)
                         select item);
            DataTable dt = new DataTable();
            dt.Columns.Add("MaGV");
            dt.Columns.Add("Ho");
            dt.Columns.Add("Ten");
            dt.Columns.Add("NgaySinh", typeof(DateTime));
            dt.Columns.Add("Nu", typeof(bool));
            dt.Columns.Add("DiaChi");
            dt.Columns.Add("SoDT");
            dt.Columns.Add("MaMH");
            dt.Columns.Add("TenTK");
            dt.Columns.Add("Pass");
            foreach (var item in query)
            {
                dt.Rows.Add(item.MaGV, item.Ho, item.Ten, item.NgaySinh, item.Nu, item.DiaChi, item.SoDT, item.MaMH, item.TenTK, item.TaiKhoan.Pass);
            }
            return dt;
        }

        public string GetIdTeacherByIdSubjectAndIdClass(string idSubject, string idClass)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = (from item in qlhs.PhanCongs
                         where item.MaMH == idSubject && item.MaLop == idClass
                         select item).SingleOrDefault();
            return query.MaGV;
        }

        public bool UpdateTeacherByIdClassAndIdSubject(string idTeacher, string idClass, string idSubject)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = (from item in qlhs.PhanCongs
                         where item.MaLop == idClass && item.MaMH == idSubject
                         select item).SingleOrDefault();
            if (query != null)
            {
                query.MaGV = idTeacher;
                qlhs.SaveChanges();
            }
            return true;
        }
    }
}
