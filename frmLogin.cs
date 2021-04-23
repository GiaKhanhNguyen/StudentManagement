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
    enum TypeAccount { Student, Teacher, Admin }

    public partial class frmLogin : Form
    {
        BLAccount blAccount;
        public frmLogin()
        {
            InitializeComponent();
            blAccount = new BLAccount();
        }

        
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            TypeAccount typeAcc;
            string errorLogin = "";
            Form f;

            typeAcc = (rdoStudent.Checked) ? TypeAccount.Student : (rdoTeacher.Checked) ? TypeAccount.Teacher : TypeAccount.Admin;

            if (blAccount.CheckAccount(username,password,typeAcc,ref errorLogin))
            {
                if (rdoStudent.Checked)
                {
                    f = new frmStudent(username);
                }
                else if (rdoTeacher.Checked)
                {
                    f = new frmTeacher(username);
                }
                else
                {
                    f = new frmAdmin(username);
                }

                this.Hide();
                f.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show(errorLogin, "Thông báo", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát chương trình", "Trả lời",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
                e.Cancel = true;
        }
    }
}
