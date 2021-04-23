using PhanMemQuanLyHocSinhTHPT_ADO.BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhanMemQuanLyHocSinhTHPT_ADO
{
    public partial class frmStudent : Form
    {
        string username;
        BLStudent blStudent;
        BLParent blParent;
        BLSemester blSemester;
        BLResult blResult;
        BLConduct blConduct;
        public frmStudent(string username)
        {
            InitializeComponent();
            this.Text = username;
            this.username = username;
            blStudent = new BLStudent();
            blParent = new BLParent();
            blSemester = new BLSemester();
            blResult = new BLResult();
            blConduct = new BLConduct();
        }

        private void frmStudent_Load(object sender, EventArgs e)
        {
            LoadInfoStudent();
            LoadInfoParent();
            LoadCmbSemester();
            cmbSemester.SelectedValue = "HK2";
        }

        // Load Thông tin học sinh
        private void LoadInfoStudent()
        {
            DataRow row = blStudent.GetStudentById(username);
            lblId.Text = row["MaHS"].ToString().Trim();
            lblClass.Text = row["MaLop"].ToString().Trim();
            lblName.Text = row["Ho"].ToString() + " " + row["Ten"].ToString();
            lblBirthday.Text = ((DateTime)row["NgaySinh"]).ToString("dd/MM/yyyy");
            lblGender.Text = (bool)row["Nu"] ? "Nữ" : "Nam";
            lblHometown.Text = row["QueQuan"].ToString();
            lblTitle.Text = lblName.Text + " - " + lblId.Text;
            lblConduct.Text = blConduct.GetNameConduct(row["MaHanhKiem"].ToString().Trim());
        }

        // Load thông tin phụ huynh
        private void LoadInfoParent()
        {
            DataTable dt = blParent.GetParentByIdStudent(username);
            foreach(DataRow row in dt.Rows)
            {
                if ((bool)row["Ba"])
                {
                    lblNameFather.Text = row["HoTen"].ToString();
                    lblBirthdayFather.Text = ((DateTime)row["NgaySinh"]).ToString("dd/MM/yyyy");
                    lblAddressFather.Text = row["DiaChi"].ToString();
                    lblPhoneFather.Text = row["SoDT"].ToString();
                }
                else
                {
                    lblNameMother.Text = row["HoTen"].ToString();
                    lblBirthdayMother.Text = ((DateTime)row["NgaySinh"]).ToString("dd/MM/yyyy");
                    lblAddressMother.Text = row["DiaChi"].ToString();
                    lblPhoneMother.Text = row["SoDT"].ToString();
                }
            }
        }

        // Load combobox Học kỳ
        private void LoadCmbSemester()
        {
            DataTable dt = blSemester.GetListSemester();
            cmbSemester.ValueMember = "MaHK";
            cmbSemester.DisplayMember = "TenHK";
            cmbSemester.DataSource = dt;
        }

        // Load DataGridView bảng điểm
        private void LoadDgvResultTable()
        {
            DataTable dt = blResult.GetResultTableSemester(username, cmbSemester.SelectedValue.ToString());
            dt.Columns["TenMH"].ColumnName = "Môn học";
            dt.Columns["DiemMieng"].ColumnName = "Điểm miệng";
            dt.Columns["Diem15PhutLan1"].ColumnName = "Điểm 15 phút lần 1";
            dt.Columns["Diem15PhutLan2"].ColumnName = "Điểm 15 phút lần 2";
            dt.Columns["Diem1TietLan1"].ColumnName = "Điểm 1 tiết lần 1";
            dt.Columns["Diem1TietLan2"].ColumnName = "Điểm 1 tiết lần 2";
            dt.Columns["DiemThi"].ColumnName = "Điểm thi";
            dt.Columns["DiemTrungBinh"].ColumnName = "Điểm trung bình";
            float gpa = blResult.CalculateSemester(username, cmbSemester.SelectedValue.ToString());
            if (gpa < 0)
                lblGPA.Text = "Chưa có";
            else
                lblGPA.Text = gpa.ToString();
            lblClassification.Text = blResult.Classification(gpa, lblConduct.Text);
            dgvResultTable.DataSource = dt;
        }

        private void cmbSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvResultTable();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmStudent_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn đăng xuất", "Trả lời",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
                e.Cancel = true;
        }

        private void btnChangePass_Click(object sender, EventArgs e)
        {
            frmChangePassword f = new frmChangePassword(username);
            f.ShowDialog();
        }
    }
}