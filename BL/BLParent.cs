using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyHocSinhTHPT_ADO.BL
{
    class BLParent
    {
        public BLParent()
        {
        }

        public bool AddParent(string idStudent, string name, bool isFather, DateTime birthday, string address, string phone)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            PhuHuynh item = new PhuHuynh();
            item.MaHS = idStudent;
            item.HoTen = name;
            item.Ba = isFather;
            item.NgaySinh = birthday;
            item.DiaChi = address;
            item.SoDT = phone;
            qlhs.PhuHuynhs.Add(item);
            qlhs.SaveChanges();
            return true;
        }

        public bool UpdateParent(string idStudent, string name, bool isFather, DateTime birthday, string address, string phone)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var queryCheck = from item in qlhs.PhuHuynhs
                        where item.MaHS == idStudent && item.Ba == isFather
                        select item;
            if (queryCheck.Count() != 0)  // Đã tồn tại thông tin phụ huynh
            {
                var query = (from item in qlhs.PhuHuynhs
                             where item.MaHS == idStudent && item.Ba == isFather
                             select item).SingleOrDefault();
                if (query != null)
                {
                    query.HoTen = name;
                    query.NgaySinh = birthday;
                    query.DiaChi = address;
                    query.SoDT = phone;
                    qlhs.SaveChanges();
                }
                return true;
            }
            else
                return AddParent(idStudent, name, isFather, birthday, address, phone);
        }

        public bool DeleteParent(string idStudent)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            PhuHuynh item = new PhuHuynh();
            item.MaHS = idStudent;
            qlhs.PhuHuynhs.Attach(item);
            qlhs.PhuHuynhs.Remove(item);
            qlhs.SaveChanges();
            return true;
        }

        public DataTable GetParentByIdStudent(string idStudent)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = from item in qlhs.PhuHuynhs
                        where item.MaHS == idStudent
                        select item;
            DataTable dt = new DataTable();
            dt.Columns.Add("MaHS");
            dt.Columns.Add("HoTen");
            dt.Columns.Add("Ba", typeof(bool));
            dt.Columns.Add("NgaySinh", typeof(DateTime));
            dt.Columns.Add("DiaChi");
            dt.Columns.Add("SoDT");
            foreach (var item in query)
            {
                dt.Rows.Add(item.MaHS, item.HoTen, item.Ba, item.NgaySinh, item.DiaChi, item.SoDT);
            }
            return dt;
        }
    }
}
