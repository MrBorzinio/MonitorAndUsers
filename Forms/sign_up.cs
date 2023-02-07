using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TestMonitorAndUsers
{

    public partial class sign_up : Form
    {
        DataBase dataBase = new DataBase();
        public sign_up()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void sign_up_Load(object sender, EventArgs E)
        {
            tbx_password.PasswordChar = '*';
            pictureBox3.Visible = false;
            tbx_login.MaxLength = 50;
            tbx_password.MaxLength = 50;
        }

        private void btn_Enter_Click(object sender, EventArgs e)
        {
            var loginUser = tbx_password.Text;
            var passUser = tbx_password.Text;

            string querystring = $"insert into tUsers(login_user, password_user) values('{loginUser}', '{passUser}')";
            SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());

            dataBase.Openconnection();

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Аккаунт успешно создан!", "Успех!");
                log_in frm_login = new log_in();
                this.Hide();
                frm_login.ShowDialog();
            }
            else
            {
                MessageBox.Show("Аккаунт не создан!");
            }
            dataBase.Closeconnection();   
        }

        private Boolean checkuser()
        {
            var loginUser = tbx_login.Text;
            var passUser = tbx_password.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string querystring = $"select id_user, login_user,password_user from tUsers where login_user='{loginUser}' and password_user='{passUser}'";

            SqlCommand command = new SqlCommand(querystring, dataBase.GetConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count >1)
            {
                MessageBox.Show("Пользователь ужк существует!");
                return true;
            }
            else
            {
                return false;
            }
                

        }
           
          
    }
}
