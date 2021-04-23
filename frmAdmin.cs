using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PhanMemQuanLyHocSinhTHPT_ADO.BL;

namespace PhanMemQuanLyHocSinhTHPT_ADO
{
    public partial class frmAdmin : Form
    {
        BLStudent blStudent;
        BLTeacher blTeacher;
        BLClass blClass;
        BLParent blParent;
        BLSubject blSubject;
        BLGradeLevel blGradeLevel;
        string username;
        bool isAddingStudent, isAddingTeacher;
        public frmAdmin(string username)
        {
            InitializeComponent();
            blStudent = new BLStudent();
            blTeacher = new BLTeacher();
            blClass = new BLClass();
            blParent = new BLParent();
            blSubject = new BLSubject();
            blGradeLevel = new BLGradeLevel();
            this.username = username;
        }

        private void frmAdmin_Load(object sender, EventArgs e)
        {
            this.Text = "Admin";
            LoadTabStudent();
            LoadTabTeacher();
            LoadTabClass();
        }

        #region Xử lý tab Student
        private void LoadTabStudent()
        {
            pnlInfoStudent.Enabled = false;
            txtPassStudent.Enabled = true;
            pnlModifyStudent.Enabled = true;
            pnlConfirmStudent.Enabled = false;
            pnlSearchStudent.Enabled = true;
            LoadDgvStudent();
            cmbClassStudent.DataSource = blClass.GetListClass();
            cmbClassStudent.DisplayMember = "TenLop";
            cmbClassStudent.ValueMember = "MaLop";
            txtSearchTeacher.ResetText();
            isAddingStudent = false;
        }

        private void LoadDgvStudent()
        {
            DataTable dt = blStudent.GetListStudent();
            dgvStudent.DataSource = dt;
            dgvStudent.CurrentCell = dgvStudent.Rows[0].Cells[0];
            dgvStudent_CellClick(null, null);
        }

        private void dgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = dgvStudent.CurrentCell.RowIndex;
            if (r == -1)
                return;
            lblUsernameStudent.Text = dgvStudent.Rows[r].Cells["TenTK"].Value.ToString().Trim();
            lblIdStudent.Text = dgvStudent.Rows[r].Cells["MaHS"].Value.ToString().Trim();
            txtPassStudent.Text = dgvStudent.Rows[r].Cells["Pass"].Value.ToString();
            txtLastNameStudent.Text = dgvStudent.Rows[r].Cells["Ho"].Value.ToString();
            txtFirstNameStudent.Text = dgvStudent.Rows[r].Cells["Ten"].Value.ToString();
            dtpBirthdayStudent.Value = (DateTime)dgvStudent.Rows[r].Cells["NgaySinh"].Value;
            chkFemaleStudent.Checked = (bool)dgvStudent.Rows[r].Cells["Nu"].Value;
            cmbClassStudent.SelectedValue = dgvStudent.Rows[r].Cells["MaLop"].Value.ToString().Trim();
            txtHometownStudent.Text = dgvStudent.Rows[r].Cells["QueQuan"].Value.ToString();

            DataTable dt = blParent.GetParentByIdStudent(lblIdStudent.Text);
            txtNameFather.Text = txtAddressFather.Text = txtPhoneFather.Text = txtNameMother.Text = txtAddressMother.Text = txtPhoneMother.Text = "";
            dtpBirthdayFather.Value = dtpBirthdayMother.Value = new DateTime(2000, 1, 1);
            foreach (DataRow row in dt.Rows)
            {
                if ((bool)row["Ba"])
                {
                    txtNameFather.Text = row["HoTen"].ToString();
                    dtpBirthdayFather.Value = ((DateTime)row["NgaySinh"]);
                    txtAddressFather.Text = row["DiaChi"].ToString();
                    txtPhoneFather.Text = row["SoDT"].ToString();
                }
                else
                {
                    txtNameMother.Text = row["HoTen"].ToString();
                    dtpBirthdayMother.Value = ((DateTime)row["NgaySinh"]);
                    txtAddressMother.Text = row["DiaChi"].ToString();
                    txtPhoneMother.Text = row["SoDT"].ToString();
                }
            }
        }

        private void btnRefreshStudent_Click(object sender, EventArgs e)
        {
            LoadTabStudent();
        }

        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            isAddingStudent = true;
            lblUsernameStudent.Text = lblIdStudent.Text = blStudent.CreateId();
            txtPassStudent.Text = txtLastNameStudent.Text = txtFirstNameStudent.Text = txtHometownStudent.Text = "";
            chkFemaleStudent.Checked = false;
            dtpBirthdayStudent.Value = new DateTime(2000, 1, 1);
            txtNameFather.Text = txtAddressFather.Text = txtPhoneFather.Text = txtNameMother.Text = txtAddressMother.Text = txtPhoneMother.Text = "";
            dtpBirthdayFather.Value = dtpBirthdayMother.Value = new DateTime(2000, 1, 1);
            pnlInfoStudent.Enabled = true;
            txtPassStudent.Enabled = false;
            pnlModifyStudent.Enabled = false;
            pnlConfirmStudent.Enabled = true;
            pnlSearchStudent.Enabled = false;
            txtLastNameStudent.Focus();
        }

        private void btnModStudent_Click(object sender, EventArgs e)
        {
            pnlInfoStudent.Enabled = true;
            pnlModifyStudent.Enabled = false;
            pnlConfirmStudent.Enabled = true;
            pnlSearchStudent.Enabled = false;
        }

        private void btnDelStudent_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn xóa không?", "Trả lời",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                blParent.DeleteParent(lblIdStudent.Text);
                if (blStudent.DeleteStudent(lblIdStudent.Text))
                    MessageBox.Show("Xóa thành công");
                else
                    MessageBox.Show("Xóa thất bại");
            }
            else
                return;
            LoadTabStudent();
        }

        private void btnSaveStudent_Click(object sender, EventArgs e)
        {
            if (isAddingStudent)
                if (blStudent.AddStudent(txtLastNameStudent.Text, txtFirstNameStudent.Text, dtpBirthdayStudent.Value, chkFemaleStudent.Checked, txtHometownStudent.Text, cmbClassStudent.SelectedValue.ToString()))
                {
                    if (txtNameFather.Text != "")
                        blParent.AddParent(lblIdStudent.Text, txtNameFather.Text, true, dtpBirthdayFather.Value, txtAddressFather.Text, txtPhoneFather.Text);
                    if (txtNameMother.Text != "")
                        blParent.AddParent(lblIdStudent.Text, txtNameMother.Text, false, dtpBirthdayMother.Value, txtAddressMother.Text, txtPhoneMother.Text);
                    MessageBox.Show("Thêm thành công");
                }
                else
                    MessageBox.Show("Thêm thất bại");
            else
            {
                if (blStudent.UpdateStudent(lblIdStudent.Text, txtPassStudent.Text, txtLastNameStudent.Text, txtFirstNameStudent.Text, dtpBirthdayStudent.Value, chkFemaleStudent.Checked, txtHometownStudent.Text, cmbClassStudent.SelectedValue.ToString()))
                {
                    blParent.UpdateParent(lblIdStudent.Text, txtNameFather.Text, true, dtpBirthdayFather.Value, txtAddressFather.Text, txtPhoneFather.Text);
                    blParent.UpdateParent(lblIdStudent.Text, txtNameMother.Text, false, dtpBirthdayMother.Value, txtAddressMother.Text, txtPhoneMother.Text);
                    MessageBox.Show("Cập nhật thành công");
                }
                else
                    MessageBox.Show("Cập nhật thất bại");
            }
            LoadTabStudent();
        }

        private void btnCancelStudent_Click(object sender, EventArgs e)
        {
            LoadTabStudent();
        }

        private void btnSearchStudent_Click(object sender, EventArgs e)
        {
            DataTable dt = blStudent.SearchStudentByIdOrName(txtSearchStudent.Text);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy");
                return;
            }
            dgvStudent.DataSource = dt;
            dgvStudent.CurrentCell = dgvStudent.Rows[0].Cells[0];
            dgvStudent_CellClick(null, null);
        }
        #endregion

        #region Xử lý tab teacher
        private void LoadTabTeacher()
        {
            pnlInfoTeacher.Enabled = false;
            txtPassTeacher.Enabled = true;
            cmbSubjectTeacher.Enabled = true;
            pnlModifyTeacher.Enabled = true;
            pnlConfirmTeacher.Enabled = false;
            pnlSearchTeacher.Enabled = true;
            LoadDgvTeacher();
            cmbSubjectTeacher.DataSource = blSubject.GetListSubject();
            cmbSubjectTeacher.DisplayMember = "TenMH";
            cmbSubjectTeacher.ValueMember = "MaMH";
            txtSearchTeacher.ResetText();
            isAddingTeacher = false;
        }

        private void dgvTeacher_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = dgvTeacher.CurrentCell.RowIndex;
            if (r == -1)
                return;
            lblUsernameTeacher.Text = dgvTeacher.Rows[r].Cells["TenTK"].Value.ToString().Trim();
            lblIdTeacher.Text = dgvTeacher.Rows[r].Cells["MaGV"].Value.ToString().Trim();
            txtPassTeacher.Text = dgvTeacher.Rows[r].Cells["Pass"].Value.ToString();
            txtLastNameTeacher.Text = dgvTeacher.Rows[r].Cells["Ho"].Value.ToString();
            txtFirstNameTeacher.Text = dgvTeacher.Rows[r].Cells["Ten"].Value.ToString();
            dtpBirthdayTeacher.Value = (DateTime)dgvTeacher.Rows[r].Cells["NgaySinh"].Value;
            chkFemaleTeacher.Checked = (bool)dgvTeacher.Rows[r].Cells["Nu"].Value;
            cmbSubjectTeacher.SelectedValue = dgvTeacher.Rows[r].Cells["MaMH"].Value.ToString().Trim();
            txtAddressTeacher.Text = dgvTeacher.Rows[r].Cells["DiaChi"].Value.ToString();
            txtPhoneTeacher.Text = dgvTeacher.Rows[r].Cells["SoDT"].Value.ToString();
        }

        private void LoadDgvTeacher()
        {
            DataTable dt = blTeacher.GetListTeacher();
            dgvTeacher.DataSource = dt;
            dgvTeacher.CurrentCell = dgvTeacher.Rows[0].Cells[0];
            dgvTeacher_CellClick(null, null);
        }

        private void btnRefreshTeacher_Click(object sender, EventArgs e)
        {
            LoadTabTeacher();
        }

        private void btnAddTeacher_Click(object sender, EventArgs e)
        {
            isAddingTeacher = true;
            lblUsernameTeacher.Text = lblIdTeacher.Text = blTeacher.CreateId();
            txtPassTeacher.Text = txtLastNameTeacher.Text = txtFirstNameTeacher.Text = txtAddressTeacher.Text = txtPhoneTeacher.Text =  "";
            chkFemaleTeacher.Checked = false;
            dtpBirthdayTeacher.Value = new DateTime(2000, 1, 1);
            pnlInfoTeacher.Enabled = true;
            txtPassTeacher.Enabled = false;
            pnlModifyTeacher.Enabled = false;
            pnlConfirmTeacher.Enabled = true;
            pnlSearchTeacher.Enabled = false;
            txtLastNameTeacher.Focus();
        }

        private void btnModTeacher_Click(object sender, EventArgs e)
        {
            pnlInfoTeacher.Enabled = true;
            pnlModifyTeacher.Enabled = false;
            pnlConfirmTeacher.Enabled = true;
            pnlSearchTeacher.Enabled = false;
            cmbSubjectTeacher.Enabled = false;
        }

        private void btnDelTeacher_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn xóa không?", "Trả lời",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                if (blTeacher.DeleteTeacher(lblIdTeacher.Text))
                    MessageBox.Show("Xóa thành công");
                else
                    MessageBox.Show("Xóa thất bại");
            }
            else
                return;
            LoadTabTeacher();
        }

        private void btnSaveTeacher_Click(object sender, EventArgs e)
        {
            if (isAddingTeacher)
                if (blTeacher.AddTeacher(txtLastNameTeacher.Text, txtFirstNameTeacher.Text, dtpBirthdayTeacher.Value, chkFemaleTeacher.Checked, txtAddressTeacher.Text, txtPhoneTeacher.Text, cmbSubjectTeacher.SelectedValue.ToString()))
                    MessageBox.Show("Thêm thành công");
                else
                    MessageBox.Show("Thêm thất bại");
            else
            {
                if (blTeacher.UpdateTeacher(lblIdTeacher.Text, txtPassTeacher.Text, txtLastNameTeacher.Text, txtFirstNameTeacher.Text, dtpBirthdayTeacher.Value, chkFemaleTeacher.Checked, txtAddressTeacher.Text, txtPhoneTeacher.Text))
                    MessageBox.Show("Cập nhật thành công");
                else
                    MessageBox.Show("Cập nhật thất bại");
            }
            LoadTabTeacher();
        }

        private void btnSearchTeacher_Click(object sender, EventArgs e)
        {
            DataTable dt = blTeacher.SearchTeacherByIdOrName(txtSearchTeacher.Text);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy");
                return;
            }
            dgvTeacher.DataSource = dt;
            dgvTeacher.CurrentCell = dgvStudent.Rows[0].Cells[0];
            dgvTeacher_CellClick(null, null);
        }

        private void btnCancelTeacher_Click(object sender, EventArgs e)
        {
            LoadTabTeacher();
        }
        #endregion

        #region Xử lý tab Class
        private void LoadTabClass()
        {
            //this.rpvClass.RefreshReport();
            LoadCmbGradeLevel();
            pnlModifyClass.Enabled = true;
            pnlConfirmClass.Enabled = false;
            cmbManageClass.Enabled = false;
            cmbTeacherClass.Enabled = false;
        }

        private void LoadCmbGradeLevel()
        {
            DataTable dt = blGradeLevel.GetListGradeLevel();
            cmbGradeLevel.ValueMember = "MaKhoi";
            cmbGradeLevel.DisplayMember = "TenKhoi";
            cmbGradeLevel.DataSource = dt;
            cmbGradeLevel_SelectedIndexChanged(null, null);
        }

        private void LoadCmbClass()
        {
            DataTable dt = blClass.GetListClassByIdGradeLevel(cmbGradeLevel.SelectedValue.ToString());
            cmbClass.ValueMember = "MaLop";
            cmbClass.DisplayMember = "TenLop";
            cmbClass.DataSource = dt;
            cmbClass_SelectedIndexChanged(null, null);
        }

        private void cmbGradeLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCmbClass();
        }

        private void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dtsQuanLyHocSinhTHPT.TongHop' table. You can move, or remove it, as needed.
            this.TongHopTableAdapter.Fill(this.dtsQuanLyHocSinhTHPT.TongHop, cmbClass.SelectedValue.ToString());
            this.rpvClass.RefreshReport();
            LoadGpbInfoClass();
        }

        private void cmbSubjectClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCmbTeacherClass();
        }

        private void LoadGpbInfoClass()
        {
            lblSizeClass.Text = blStudent.GetListStudentByIdClass(cmbClass.SelectedValue.ToString()).Rows.Count.ToString(); ;

            cmbManageClass.ValueMember = "MaGV";
            cmbManageClass.DisplayMember = "HoTen";
            cmbManageClass.DataSource = blTeacher.GetListTeacherNotManage(cmbClass.SelectedValue.ToString());
            cmbManageClass.SelectedValue = ((DataTable)cmbClass.DataSource).Rows[cmbClass.SelectedIndex]["MaGVCN"];

            cmbSubjectClass.ValueMember = "MaMH";
            cmbSubjectClass.DisplayMember = "TenMH";
            cmbSubjectClass.DataSource = blSubject.GetListSubject();
            cmbSubjectClass_SelectedIndexChanged(null, null);
        }

        private void btnChangeTeacher_Click(object sender, EventArgs e)
        {
            pnlModifyClass.Enabled = false;
            pnlConfirmClass.Enabled = true;
            cmbManageClass.Enabled = true;
            cmbTeacherClass.Enabled = true;
        }

        private void btnRefreshClass_Click(object sender, EventArgs e)
        {
            LoadTabClass();
        }

        private void btnSaveClass_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn cập nhật không?", "Trả lời",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                if (blTeacher.UpdateTeacherByIdClassAndIdSubject(cmbTeacherClass.SelectedValue.ToString(), cmbClass.SelectedValue.ToString(),cmbSubjectClass.SelectedValue.ToString()))
                    MessageBox.Show("Cập nhật thành công");
                else
                    MessageBox.Show("Cập nhật thất bại");
            }
            else
                return;
            LoadTabClass();
        }

        private void btnCancelClass_Click(object sender, EventArgs e)
        {
            LoadTabClass();
        }

        private void LoadCmbTeacherClass()
        {
            cmbTeacherClass.ValueMember = "MaGV";
            cmbTeacherClass.DisplayMember = "HoTen";
            cmbTeacherClass.DataSource = blTeacher.GetListTeacherByIdSubject(cmbSubjectClass.SelectedValue.ToString());
            cmbTeacherClass.SelectedValue = blTeacher.GetIdTeacherByIdSubjectAndIdClass(cmbSubjectClass.SelectedValue.ToString(),cmbClass.SelectedValue.ToString());
        }
        #endregion

        private void frmAdmin_FormClosing(object sender, FormClosingEventArgs e)
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

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
