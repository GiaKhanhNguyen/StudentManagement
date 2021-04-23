using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyHocSinhTHPT_ADO.BL
{
    class BLSemester
    {
        public BLSemester()
        {
        }

        public DataTable GetListSemester()
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = from item in qlhs.HocKies
                        select item;
            DataTable dt = new DataTable();
            dt.Columns.Add("MaHK");
            dt.Columns.Add("TenHK");
            dt.Columns.Add("HeSo", typeof(int));
            foreach (var item in query)
            {
                dt.Rows.Add(item.MaHK, item.TenHK, item.HeSo);
            }
            return dt;
        }
    }
}
