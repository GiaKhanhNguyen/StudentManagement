using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyHocSinhTHPT_ADO.BL
{
    class BLResult
    {
        BLSubject blSubject;
        BLSemester blSemester;
        public BLResult()
        {
            blSemester = new BLSemester();
            blSubject = new BLSubject();
        }

        public bool AddResult(string idStudent)
        {
            DataTable dtListSubject = blSubject.GetListSubject();
            DataTable dtListSemester = blSemester.GetListSemester();
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            foreach (DataRow rowSemester in dtListSemester.Rows)
            {
                string idSemester = rowSemester["MaHK"].ToString().Trim();
                foreach (DataRow rowSubject in dtListSubject.Rows)
                {
                    string idSubject = rowSubject["MaMH"].ToString().Trim();
                    DiemSo item = new DiemSo();
                    item.MaHS = idStudent;
                    item.MaMH = idStudent;
                    item.MaHK = idSemester;
                    qlhs.DiemSoes.Add(item);
                    qlhs.SaveChanges();
                }
            }
            return true;
        }

        public bool UpdatePointStudents(DataTable dt, string idSubject, string idSemester)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            foreach (DataRow row in dt.Rows)
            {
                string idStudent = row["MaHS"].ToString();
                var query = (from item in qlhs.DiemSoes
                             where item.MaHS == idStudent && item.MaMH == idSubject && item.MaHK == idSemester
                             select item).SingleOrDefault();
                if (query != null)
                {
                    float t;
                    if (float.TryParse(row["DiemMieng"].ToString(),out t))
                        query.DiemMieng = t;
                    if (float.TryParse(row["Diem15PhutLan1"].ToString(), out t))
                        query.Diem15PhutLan1 = t;
                    if (float.TryParse(row["Diem15PhutLan2"].ToString(), out t))
                        query.Diem15PhutLan2 = t;
                    if (float.TryParse(row["Diem1TietLan1"].ToString(), out t))
                        query.Diem1TietLan1 = t;
                    if (float.TryParse(row["Diem1TietLan2"].ToString(), out t))
                        query.Diem1TietLan2 = t;
                    if (float.TryParse(row["DiemThi"].ToString(), out t))
                        query.DiemThi = t;
                    qlhs.SaveChanges();
                }
            }
            return true;
        }

        public bool DeleteResult(string idStudent)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            DiemSo item = new DiemSo();
            item.MaHS = idStudent;
            qlhs.DiemSoes.Attach(item);
            qlhs.DiemSoes.Remove(item);
            qlhs.SaveChanges();
            return true;
        }

        public DataTable GetResultTableSemester(string idStudent, string idSemester)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = from item in qlhs.DiemSoes
                        where item.MaHS == idStudent && item.MaHK == idSemester
                        select item;
            DataTable dt = new DataTable();
            dt.Columns.Add("TenMH");
            dt.Columns.Add("DiemMieng", typeof(float));
            dt.Columns.Add("Diem15PhutLan1", typeof(float));
            dt.Columns.Add("Diem15PhutLan2", typeof(float));
            dt.Columns.Add("Diem1TietLan1", typeof(float));
            dt.Columns.Add("Diem1TietLan2", typeof(float));
            dt.Columns.Add("DiemThi", typeof(float));
            foreach (var item in query)
            {
                dt.Rows.Add(item.MonHoc.TenMH, item.DiemMieng, item.Diem15PhutLan1, item.Diem15PhutLan2, item.Diem1TietLan1, item.Diem1TietLan2, item.DiemThi);
            }
            CalculateSubjectsGPA(dt);
            return dt;
        }

        public DataTable GetPointTableSubjectByClass(string idClass, string idSubject, string idSemester)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = from item in qlhs.DiemSoes
                        where item.MaMH == idSubject && item.MaHK == idSemester && item.HocSinh.MaLop == idClass
                        select item;
            DataTable dt = new DataTable();
            dt.Columns.Add("Ho");
            dt.Columns.Add("Ten");
            dt.Columns.Add("DiemMieng", typeof(float));
            dt.Columns.Add("Diem15PhutLan1", typeof(float));
            dt.Columns.Add("Diem15PhutLan2", typeof(float));
            dt.Columns.Add("Diem1TietLan1", typeof(float));
            dt.Columns.Add("Diem1TietLan2", typeof(float));
            dt.Columns.Add("DiemThi", typeof(float));
            foreach (var item in query)
            {
                dt.Rows.Add(item.HocSinh.Ho, item.HocSinh.Ten, item.DiemMieng, item.Diem15PhutLan1, item.Diem15PhutLan2, item.Diem1TietLan1, item.Diem1TietLan2, item.DiemThi);
            }
            return dt;
        }

        // Tính điểm trung bình các môn học
        private void CalculateSubjectsGPA(DataTable dt)
        {
            dt.Columns.Add("DiemTrungBinh");
            foreach (DataRow row in dt.Rows)
            {
                if (row.IsNull("DiemMieng") || row.IsNull("Diem15PhutLan1") || row.IsNull("Diem15PhutLan2")
                    || row.IsNull("Diem1TietLan1") || row.IsNull("Diem1TietLan2") || row.IsNull("DiemThi"))
                    continue;
                float gpaSubject = (float.Parse(row["DiemMieng"].ToString()) + float.Parse(row["Diem15PhutLan1"].ToString()) + float.Parse(row["Diem15PhutLan2"].ToString())
                    + float.Parse(row["Diem1TietLan1"].ToString()) * 2 + float.Parse(row["Diem1TietLan2"].ToString()) * 2 + float.Parse(row["DiemThi"].ToString()) * 3) / 10;
                row["DiemTrungBinh"] = gpaSubject;
            }
        }

        /// <summary>
        /// Trả về điểm trung bình tổng, nếu chưa tính được trả về -1
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public float CalculateSemester(string idStudent, string idSemester)
        {
            QuanLyHocSinhTHPTEntities qlhs = new QuanLyHocSinhTHPTEntities();
            var query = from item in qlhs.DiemSoes
                        where item.MaHS == idStudent && item.MaHK == idSemester
                        select item;
            DataTable dt = new DataTable();
            dt.Columns.Add("TenMH");
            dt.Columns.Add("HeSo", typeof(int));
            dt.Columns.Add("DiemMieng", typeof(float));
            dt.Columns.Add("Diem15PhutLan1", typeof(float));
            dt.Columns.Add("Diem15PhutLan2", typeof(float));
            dt.Columns.Add("Diem1TietLan1", typeof(float));
            dt.Columns.Add("Diem1TietLan2", typeof(float));
            dt.Columns.Add("DiemThi", typeof(float));
            foreach (var item in query)
            {
                dt.Rows.Add(item.MonHoc.TenMH, item.MonHoc.HeSo, item.DiemMieng, item.Diem15PhutLan1, item.Diem15PhutLan2, item.Diem1TietLan1, item.Diem1TietLan2, item.DiemThi);
            }
            CalculateSubjectsGPA(dt);
            float total = 0;
            int k = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (row.IsNull("DiemTrungBinh"))
                    return -1;
                total += float.Parse(row["DiemTrungBinh"].ToString()) * (int)row["HeSo"];
                k += (int)row["HeSo"];
            }
            return total / k;
        }

        public string Classification(float gpa, string conduct)
        {
            if (conduct == "Chưa xét")
                return "Chưa có";
            if (gpa >= 8 && conduct == "Tốt")
                return "Giỏi";
            if (gpa >= 6.5 && (conduct == "Khá" || conduct == "Tốt"))
                return "Khá";
            if (gpa >= 5 && (conduct == "Trung bình" || conduct == "Khá" || conduct == "Tốt"))
                return "Trung Bình";
            if (gpa >= 3.5)
                return "Yếu";
            if (gpa >= 0)
                return "Kém";
            return "Chưa có";
        }

    }
}
