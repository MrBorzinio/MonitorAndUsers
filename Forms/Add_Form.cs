using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace TestMonitorAndUsers
{
    public partial class Add_Form : Form
    {
        DataBase dataBase = new DataBase();

        public Add_Form()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            dataBase.Openconnection();

            var designation = textBox_Designation.Text;
            var nameBid = textBox_Name.Text;
            var taskTyp = textBox_TaskType.Text;
            int priority;
            
            if (int.TryParse(textBox_Priority.Text, out priority))
            {
                var addQuery= $"insert into tBids(nf_Designation, nf_Name,nf_TaskType,nf_Priority ) values('{designation}', '{nameBid}', '{taskTyp}', '{priority}')";
                var command = new SqlCommand(addQuery, dataBase.GetConnection());

                command.ExecuteNonQuery();
                MessageBox.Show("Запись успешно создана!", "Успех!",MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();

            }
            else
            {
                MessageBox.Show("Приоритет должен иметь числовой формат!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            dataBase.Closeconnection();

        }
    }
}
