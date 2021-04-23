using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyHocSinhTHPT_ADO.BL
{
    class BLClass
    {
        public BLClass()
        {
        }
        //
        public DataTable GetListClass()
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = from item in qlhs.Lops
                        select item;
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLop");
            dt.Columns.Add("TenLop");
            dt.Columns.Add("MaGVCN");
            dt.Columns.Add("MaKhoi");
            foreach (var item in query)
            {
                dt.Rows.Add(item.MaLop, item.TenLop, item.MaGVCN, item.MaKhoi);
            }
            return dt;
        }
        //
        public DataTable GetListClassByIdTeacher(string idTeacher)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = from item in qlhs.PhanCongs
                        where item.MaGV == idTeacher
                        select item;
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLop");
            dt.Columns.Add("TenLop");
            foreach (var item in query)
            {
                dt.Rows.Add(item.MaLop, item.Lop.TenLop);
            }
            return dt;
        }
        //
        public DataTable GetListClassByIdGradeLevel(string idGradeLevel)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = from item in qlhs.Lops
                        where item.MaKhoi == idGradeLevel
                        select item;
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLop");
            dt.Columns.Add("TenLop");
            dt.Columns.Add("MaGVCN");
            dt.Columns.Add("MaKhoi");
            foreach (var item in query)
            {
                dt.Rows.Add(item.MaLop, item.TenLop, item.MaGVCN, item.MaKhoi);
            }
            return dt;
        }
        //
        public DataTable GetManageClassByIdTeacher(string idTeacher)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = from item in qlhs.Lops
                        where item.MaGVCN == idTeacher
                        select item;
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLop");
            dt.Columns.Add("TenLop");
            foreach (var item in query)
            {
                dt.Rows.Add(item.MaLop, item.TenLop);
            }
            return dt;
        }
        //
        public string GetNameClass(string idClass)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = (from item in qlhs.Lops
                         where item.MaLop == idClass
                         select item).SingleOrDefault();
            return query.TenLop;
        }
    }
}
