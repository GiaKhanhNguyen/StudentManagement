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
    public partial class frmChangePassword : Form
    {
        string username;
        BLAccount blAccount;
        public frmChangePassword(string username)
        {
            InitializeComponent();
            blAccount = new BLAccount();
            this.username = username;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            // Hỏi xác nhận thay đổi mật khẩu
            DialogResult dr = MessageBox.Show("Bạn có muốn thay đổi mật khẩu?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                string err = "";
                if (blAccount.CheckAccount(username, txtOldPass.Text,TypeAccount.Admin, ref err)  && txtNewPass.Text == txtReNewPass.Text)
                {
                    blAccount.UpdateAccount(username, txtNewPass.Text);
                    // Thông báo mật khẩu đã thay đổi
                    MessageBox.Show("Mật khẩu của bạn đã được thay đổi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else if (err == "Sai mật khẩu!")
                    MessageBox.Show("Mật khẩu cũ không đúng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("Nhập lại mật khẩu mới không trùng khớp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
