using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyHocSinhTHPT_ADO.BL
{
    class BLGradeLevel
    {
        public BLGradeLevel()
        {
        }

        public DataTable GetListGradeLevel()
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = from item in qlhs.Khois
                        select item;
            DataTable dt = new DataTable();
            dt.Columns.Add("MaKhoi");
            dt.Columns.Add("TenKhoi");
            foreach (var item in query)
            {
                dt.Rows.Add(item.MaKhoi, item.TenKhoi);
            }
            return dt;
        }
    }
}
