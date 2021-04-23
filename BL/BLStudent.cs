using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyHocSinhTHPT_ADO.BL
{
    class BLStudent
    {
        BLAccount blAccount;
        BLResult blResult;
        public BLStudent()
        {
            blAccount = new BLAccount();
            blResult = new BLResult();
        }

        public bool AddStudent(string lName, string fName, DateTime birthday, bool isFemale, string homeTown, string idClass)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            string idStudent = CreateId();
            string pass = birthday.ToString("ddMMyyyy");

            blAccount.AddAccount(idStudent, pass);

            HocSinh item = new HocSinh();
            item.MaHS = idStudent;
            item.Ho = lName;
            item.Ten = fName;
            item.NgaySinh = birthday;
            item.Nu = isFemale;
            item.QueQuan = homeTown;
            item.MaHanhKiem = "CHUA";
            item.MaLop = idClass;
            item.TenTK = idStudent;
            qlhs.HocSinhs.Add(item);
            qlhs.SaveChanges();

            blResult.AddResult(idStudent);
            return true;
        }

        public bool UpdateStudent(string idStudent, string pass, string lName, string fName, DateTime birthday, bool isFemale, string homeTown, string idClass)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = (from item in qlhs.HocSinhs
                         where item.MaHS == idStudent
                         select item).SingleOrDefault();
            if (query != null)
            {
                query.Ho = lName;
                query.Ten = fName;
                query.NgaySinh = birthday;
                query.Nu = isFemale;
                query.QueQuan = homeTown;
                query.MaLop = idClass;
                query.TaiKhoan.Pass = pass;
                qlhs.SaveChanges();
            }
            return true;
        }

        public bool DeleteStudent(string idStudent)
        {
            blResult.DeleteResult(idStudent);

            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            HocSinh item = new HocSinh();
            item.MaHS = idStudent;
            qlhs.HocSinhs.Attach(item);
            qlhs.HocSinhs.Remove(item);
            qlhs.SaveChanges();

            blAccount.DeleteAccount(idStudent);
            return true;
        }

        public string CreateId()
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = (from item in qlhs.HocSinhs
                         orderby item.MaHS descending
                         select item);
            string idLast="";
            foreach (var item in query)
            {
                idLast = item.MaHS;
                break;
            }
            int id = int.Parse(idLast.Substring(2));
            string numId = (++id).ToString();
            string idNew = "HS";
            for (int i = 0; i < 3 - numId.Count(); i++)
            {
                idNew += '0';
            }
            idNew += numId;
            return idNew;
        }
        //
        public DataRow GetStudentById(string id)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = (from item in qlhs.HocSinhs
                         where item.MaHS == id
                         select item);
            DataTable dt = new DataTable();
            dt.Columns.Add("MaHS");
            dt.Columns.Add("Ho");
            dt.Columns.Add("Ten");
            dt.Columns.Add("NgaySinh", typeof(DateTime));
            dt.Columns.Add("Nu", typeof(bool));
            dt.Columns.Add("QueQuan");
            dt.Columns.Add("MaLop");
            dt.Columns.Add("MaHanhKiem");
            dt.Columns.Add("TenTK");
            foreach (var item in query)
            {
                dt.Rows.Add(item.MaHS, item.Ho, item.Ten, item.NgaySinh, item.Nu, item.QueQuan, item.MaLop, item.MaHanhKiem, item.TenTK);
            }
            return dt.Rows[0];
        }
        //
        public DataTable GetListStudent()
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = (from item in qlhs.HocSinhs
                         select item);
            DataTable dt = new DataTable();
            dt.Columns.Add("MaHS");
            dt.Columns.Add("Ho");
            dt.Columns.Add("Ten");
            dt.Columns.Add("NgaySinh", typeof(DateTime));
            dt.Columns.Add("Nu", typeof(bool));
            dt.Columns.Add("QueQuan");
            dt.Columns.Add("MaLop");
            dt.Columns.Add("HanhKiem");
            dt.Columns.Add("TenTK");
            dt.Columns.Add("Pass");
            foreach (var item in query)
            {
                dt.Rows.Add(item.MaHS, item.Ho, item.Ten, item.NgaySinh, item.Nu, item.QueQuan, item.MaLop, item.HanhKiem.TenHanhKiem, item.TenTK, item.TaiKhoan.Pass);
            }
            return dt;
        }
        //
        public DataTable GetListStudentByIdClass(string idClass)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = (from item in qlhs.HocSinhs
                         where item.MaLop == idClass
                         orderby item.Ten
                         select item);
            DataTable dt = new DataTable();
            dt.Columns.Add("STT");
            dt.Columns.Add("MaHS");
            dt.Columns.Add("Ho");
            dt.Columns.Add("Ten");
            dt.Columns.Add("MaHanhKiem");
            int i = 1;
            foreach (var item in query)
            {
                dt.Rows.Add(i++, item.MaHS, item.Ho, item.Ten, item.MaHanhKiem);
            }
            return dt;
        }

        public bool UpdateConductStudents(DataTable dt)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            foreach (DataRow row in dt.Rows)
            {
                string idStudent = row["MaHS"].ToString();
                string idConduct = row["MaHanhKiem"].ToString();
                var query = (from item in qlhs.HocSinhs
                             where item.MaHS == idStudent
                             select item).SingleOrDefault();
                if (query != null)
                {
                    query.MaHanhKiem = idConduct;
                    qlhs.SaveChanges();
                }
            }
            return true;
        }

        public DataTable SearchStudentByIdOrName(string idName)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = (from item in qlhs.HocSinhs
                         where item.MaHS.Contains(idName) || item.Ten.Contains(idName)
                         select item);
            DataTable dt = new DataTable();
            dt.Columns.Add("MaHS");
            dt.Columns.Add("Ho");
            dt.Columns.Add("Ten");
            dt.Columns.Add("NgaySinh", typeof(DateTime));
            dt.Columns.Add("Nu", typeof(bool));
            dt.Columns.Add("QueQuan");
            dt.Columns.Add("MaLop");
            dt.Columns.Add("MaHanhKiem");
            dt.Columns.Add("TenTK");
            dt.Columns.Add("Pass");
            foreach (var item in query)
            {
                dt.Rows.Add(item.MaHS, item.Ho, item.Ten, item.NgaySinh, item.Nu, item.QueQuan, item.MaLop, item.MaHanhKiem, item.TenTK, item.TaiKhoan.Pass);
            }
            return dt;
        }

    }
}
