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
    public partial class frmTeacher : Form
    {
        string username;
        string idManageClass;
        string idSubject;
        BLTeacher blTeacher;
        BLSubject blSubject;
        BLClass blClass;
        BLStudent blStudent;
        BLConduct blConduct;
        BLResult blResult;
        BLSemester blSemester;

        public frmTeacher(string username)
        {
            InitializeComponent();
            blTeacher = new BLTeacher();
            blSubject = new BLSubject();
            blClass = new BLClass();
            blStudent = new BLStudent();
            blConduct = new BLConduct();
            blResult = new BLResult();
            blSemester = new BLSemester();
            this.Text = username;
            this.username = username;
            DataTable dtManageClass = blClass.GetManageClassByIdTeacher(username);
            this.idManageClass = "";
            if (dtManageClass.Rows.Count > 0)
                idManageClass = dtManageClass.Rows[0]["MaLop"].ToString().Trim();
            if (idManageClass == "")
                pnlModifyConduct.Enabled = false;
        }

        private void frmTeacher_Load(object sender, EventArgs e)
        {
            this.Text = username;
            LoadInfoTeacher();
            CreateColumnDgvListStudent();
            LoadDgvListStudent();
            LoadCmbClasses();
            LoadDgvPointTable();
        }

        private void LoadCmbClasses()
        {
            DataTable dtClasses = blClass.GetListClassByIdTeacher(username);
            cmbClasses.ValueMember = "MaLop";
            cmbClasses.DisplayMember = "TenLop";
            cmbClasses.DataSource = dtClasses;
            cmbClasses_SelectedIndexChanged(null, null);
        }

        private void LoadCmbSemester()
        {
            DataTable dtSemesters = blSemester.GetListSemester();
            cmbSemesters.ValueMember = "MaHK";
            cmbSemesters.DisplayMember = "TenHK";
            cmbSemesters.DataSource = dtSemesters;
            cmbSemesters_SelectedIndexChanged(null,null);
        }

        private void LoadDgvPointTable()
        {
            if (cmbClasses.SelectedValue == null)
            {
                pnlModifyPoint.Enabled = false;
                return;
            }
            string idClass = cmbClasses.SelectedValue.ToString().Trim();
            string idSemester = cmbSemesters.SelectedValue.ToString().Trim();
            DataTable dt = blResult.GetPointTableSubjectByClass(idClass, idSubject, idSemester);
            dgvPointTable.DataSource = dt;
        }

        // Load Thông tin giáo viên
        private void LoadInfoTeacher()
        {
            DataRow row = blTeacher.GetTeacherById(username);
            lblUsername.Text = username;
            lblId.Text = row["MaGV"].ToString().Trim();
            lblName.Text = row["Ho"].ToString() + " " + row["Ten"].ToString();
            lblBirthday.Text = ((DateTime)row["NgaySinh"]).ToString("dd/MM/yyyy");
            lblGender.Text = (bool)row["Nu"] ? "Nữ" : "Nam";
            lblAddress.Text = row["DiaChi"].ToString();
            lblPhone.Text = row["SoDT"].ToString();
            idSubject = row["MaMH"].ToString().Trim();
            lblSubject.Text = lblSubject2.Text = blSubject.GetNameSubject(idSubject);
            lblClasses.ResetText();
            DataTable dtClasses = blClass.GetListClassByIdTeacher(username);
            foreach (DataRow r in dtClasses.Rows)
                lblClasses.Text += r["TenLop"].ToString() + ", ";
            if (idManageClass != "")
                lblManageClass.Text = lblManageClass2.Text = blClass.GetNameClass(idManageClass);
            else
                lblManageClass.Text = lblManageClass2.Text = "Không có";
            lblTitle.Text = lblName.Text + " - " + lblId.Text;
        }

        private void LoadDgvListStudent()
        {
            DataTable dt = blStudent.GetListStudentByIdClass(idManageClass);
            dgvListStudent.DataSource = dt;
        }

        private void CreateColumnDgvListStudent()
        {
            // Tạo cột thứ tự
            DataGridViewTextBoxColumn columnStt = new DataGridViewTextBoxColumn();
            columnStt.Name = "STT";
            columnStt.HeaderText = "STT";
            columnStt.DataPropertyName = "STT";
            columnStt.SortMode = DataGridViewColumnSortMode.NotSortable;
            columnStt.ReadOnly = true;
            dgvListStudent.Columns.Add(columnStt);
            // Tạo cột họ
            DataGridViewTextBoxColumn columnHo = new DataGridViewTextBoxColumn();
            columnHo.Name = "Ho";
            columnHo.HeaderText = "Họ";
            columnHo.DataPropertyName = "Ho";
            columnHo.SortMode = DataGridViewColumnSortMode.NotSortable;
            columnHo.ReadOnly = true;
            dgvListStudent.Columns.Add(columnHo);
            // Tạo cột tên
            DataGridViewTextBoxColumn columnTen = new DataGridViewTextBoxColumn();
            columnTen.Name = "Ten";
            columnTen.HeaderText = "Tên";
            columnTen.DataPropertyName = "Ten";
            columnTen.SortMode = DataGridViewColumnSortMode.NotSortable;
            columnTen.ReadOnly = true;
            dgvListStudent.Columns.Add(columnTen);
            // Tạo cột tên
            DataGridViewTextBoxColumn columnMaHS = new DataGridViewTextBoxColumn();
            columnMaHS.Name = "MaHS";
            columnMaHS.HeaderText = "Mã số";
            columnMaHS.DataPropertyName = "MaHS";
            columnMaHS.SortMode = DataGridViewColumnSortMode.NotSortable;
            columnMaHS.ReadOnly = true;
            dgvListStudent.Columns.Add(columnMaHS);
            // Tạo cột hạnh kiểm
            DataGridViewComboBoxColumn columnHanhKiem = new DataGridViewComboBoxColumn();
            columnHanhKiem.Name = "HanhKiem";
            columnHanhKiem.HeaderText = "Hạnh kiểm";
            columnHanhKiem.DataSource = blConduct.GetListConduct();
            columnHanhKiem.ValueMember = "MaHanhKiem";
            columnHanhKiem.DisplayMember = "TenHanhKiem";
            columnHanhKiem.DataPropertyName = "MaHanhKiem";
            columnHanhKiem.ReadOnly = true;
            columnHanhKiem.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvListStudent.Columns.Add(columnHanhKiem);
        }

        private void btnModConduct_Click(object sender, EventArgs e)
        {
            pnlModifyConduct.Enabled = false;
            pnlConfirmConduct.Enabled = true;
            dgvListStudent.Columns["HanhKiem"].ReadOnly = false;
        }

        private void btnCancelConduct_Click(object sender, EventArgs e)
        {
            LoadDgvListStudent();
            pnlModifyConduct.Enabled = true;
            pnlConfirmConduct.Enabled = false;
            dgvListStudent.Columns["HanhKiem"].ReadOnly = true;
        }

        private void btnSaveConduct_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn lưu không?", "Trả lời",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                DataTable dt = (DataTable)dgvListStudent.DataSource;
                if (blStudent.UpdateConductStudents(dt))
                    MessageBox.Show("Cập nhật thành công");
                else
                    MessageBox.Show("Cập nhật thất bại");
                LoadDgvListStudent();
                pnlModifyConduct.Enabled = true;
                pnlConfirmConduct.Enabled = false;
                dgvListStudent.Columns["HanhKiem"].ReadOnly = true;
            }
        }

        private void cmbClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCmbSemester();
        }

        private void cmbSemesters_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvPointTable();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmTeacher_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn đăng xuất", "Trả lời",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
                e.Cancel = true;
        }

        private void btnModPoint_Click(object sender, EventArgs e)
        {
            pnlModifyPoint.Enabled = false;
            pnlConfirmPoint.Enabled = true;
        }

        private void btnCancelPoint_Click(object sender, EventArgs e)
        {
            LoadDgvPointTable();
            pnlModifyPoint.Enabled = true;
            pnlConfirmPoint.Enabled = false;
        }

        private void btnSavePoint_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn lưu không?", "Trả lời",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                DataTable dt = (DataTable)dgvPointTable.DataSource;
                if (blResult.UpdatePointStudents(dt,idSubject,cmbSemesters.SelectedValue.ToString()))
                    MessageBox.Show("Cập nhật thành công");
                else
                    MessageBox.Show("Cập nhật thất bại");
                LoadDgvPointTable();
                pnlModifyPoint.Enabled = true;
                pnlConfirmPoint.Enabled = false;
            }
        }

        private void btnChangePass_Click(object sender, EventArgs e)
        {
            frmChangePassword f = new frmChangePassword(username);
            f.ShowDialog();
        }
    }
}
