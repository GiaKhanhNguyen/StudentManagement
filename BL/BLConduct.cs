using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyHocSinhTHPT_ADO.BL
{
    class BLConduct
    {
        public BLConduct()
        {
        }
        //
        public string GetNameConduct(string idConduct)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = (from item in qlhs.HanhKiems
                         where item.MaHanhKiem == idConduct
                         select item).SingleOrDefault();
            return query.TenHanhKiem;
        }
        //
        public DataTable GetListConduct()
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = from item in qlhs.HanhKiems
                        select item;
            DataTable dt = new DataTable();
            dt.Columns.Add("MaHanhKiem");
            dt.Columns.Add("TenHanhKiem");
            foreach (var item in query)
            {
                dt.Rows.Add(item.MaHanhKiem, item.TenHanhKiem);
            }
            return dt;
        }
    }
}
