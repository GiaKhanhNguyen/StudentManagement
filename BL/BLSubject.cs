using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyHocSinhTHPT_ADO.BL
{
    class BLSubject
    {
        public BLSubject()
        {
        }

        public string GetNameSubject(string idSubject)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = (from item in qlhs.MonHocs
                         where item.MaMH == idSubject
                         select item).SingleOrDefault();
            return query.TenMH;
        }

        public DataTable GetListSubject()
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = from item in qlhs.MonHocs
                        select item;
            DataTable dt = new DataTable();
            dt.Columns.Add("MaMH");
            dt.Columns.Add("TenMH");
            dt.Columns.Add("HeSo",typeof(int));
            foreach (var item in query)
            {
                dt.Rows.Add(item.MaMH, item.TenMH, item.HeSo);
            }
            return dt;
        }

    }
}
