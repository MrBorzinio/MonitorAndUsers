using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TestMonitorAndUsers
{
    public partial class log_in : Form
    { 
        DataBase dataBase=new DataBase();
    
        public log_in()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void log_in_Load(object sender, EventArgs E)
        {
            //tbx_password.PasswordChar = '●';
            tbx_password.UseSystemPasswordChar = true;
            PictureBox_Hide.Visible = false;
            tbx_login.MaxLength = 50;
            tbx_password.MaxLength = 50;
        }

        private void PictureBox_Hide_Click(object sender, EventArgs e)// клик по перечеркнутому (включаем защиту)
        {
            tbx_password.UseSystemPasswordChar = true;
            PictureBox_Hide.Visible = false;
            PictureBox_Show.Visible = true;
        }
        private void PictureBox_Show_Click(object sender, EventArgs e)// клик по глазу (отображаем пароль открытым)
        
        {
            tbx_password.UseSystemPasswordChar = false;
            PictureBox_Hide.Visible = true;
            PictureBox_Show.Visible = false;
        }

        private void btn_Enter_Click(object sender, EventArgs e)
        {
            var loginUser = tbx_password.Text;
            var passUser = tbx_password.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string querystring = $"select id_user, login_user,password_user from tUsers where login_user='{loginUser}' and password_user='{passUser}'";

            SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count == 1)
            {
                MessageBox.Show("Вы успешно вошли!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form1 frm1 = new Form1();
                this.Hide();
                frm1.ShowDialog();
                //this.Show();

            }
            else
                MessageBox.Show("Такого аккаунта не существует!", "Аккаунта не существует!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            sign_up frm_sign = new sign_up();
            frm_sign.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tbx_login.Text = "";
            tbx_password.Text = "";
        }

      
    }
}
