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

    enum RowState
    {
        Existed,
        New,
        Modified,
        ModifiedNew,
        Deleted
    }
    public partial class Form1 : Form
    {
        DataBase dataBase=new DataBase();
        int selectedRow;

        public Form1()
        {
            InitializeComponent();
        }

        private void CreatColumns()
        {
            dataGridView1.Columns.Add("nf_id", "id");
            dataGridView1.Columns.Add("nf_Designation", "Заявка");
            dataGridView1.Columns.Add("nf_Name", "Наименование");
            dataGridView1.Columns.Add("nf_TaskType", "Тип");
            dataGridView1.Columns.Add("nf_Priority", "Приоритет");
            dataGridView1.Columns.Add("IsNew", String.Empty);
        }

        private void ClearFields()
        {
            textBox_id.Text = "";
            textBox_Designation.Text = "";
            textBox_Name.Text = "";
            textBox_TaskType.Text = "";
            textBox_Priority.Text = "";
        }

        private void ReadSingleRow (DataGridView dgw, IDataReader record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), record.GetInt32(4), RowState.ModifiedNew);
        }
        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string queryString = $"select * from ordrsip.dbo.tBids";
            SqlCommand command = new SqlCommand(queryString, dataBase.GetConnection());
            dataBase.Openconnection();
            SqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
              ReadSingleRow(dgw, reader);
            }
            reader.Close();
        }

        private void Form1_Load(object sender,EventArgs e)
        {
            CreatColumns();
            RefreshDataGrid(dataGridView1);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];
                textBox_id.Text = row.Cells[0].Value.ToString();
                textBox_Designation.Text = row.Cells[1].Value.ToString();
                textBox_Name.Text = row.Cells[2].Value.ToString();
                textBox_TaskType.Text = row.Cells[3].Value.ToString();
                textBox_Priority.Text = row.Cells[4].Value.ToString();
            }
            
        }

        private void Search(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string searchString = $"select * from ordrsip.dbo.tBids where concat (nf_Id, nf_Designation, nf_Name, nf_TaskType, nf_Priority) like '%" + textBox_search.Text + "%'";
            SqlCommand com = new SqlCommand(searchString, dataBase.GetConnection());
            dataBase.Openconnection();
            SqlDataReader read = com.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRow(dgw, read);
            }
            read.Close();
        }
        private void textBox_search_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }
        

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
            ClearFields();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Add_Form addfrm = new Add_Form();
            addfrm.Show();
        }

        private void deleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows[index].Visible = false;
            if (dataGridView1.Rows[index].Cells[0].Value.ToString()==string.Empty)
            {
                dataGridView1.Rows[index].Cells[5].Value = RowState.Deleted;
                return ;
            }
            dataGridView1.Rows[index].Cells[5].Value = RowState.Deleted;
        }

        private new void Update()
        {
            dataBase.Openconnection();

            for(int index=0; index<dataGridView1.Rows.Count;index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[5].Value;
                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var deleteQuery = $"delete from ordrsip.dbo.tBids where nf_Id={id}";

                    var command = new SqlCommand(deleteQuery, dataBase.GetConnection());
                    command.ExecuteNonQuery();
                }

                if (rowState == RowState.Modified)
                {
                    var id = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var designation = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var name = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    var taskType = dataGridView1.Rows[index].Cells[3].Value.ToString();
                    int priority= Convert.ToInt32(dataGridView1.Rows[index].Cells[4].Value.ToString());


                    var changeQuery = $"update tBids set nf_Designation='{designation}', nf_Name='{name}', nf_TaskType='{taskType}', nf_Priority='{priority}' where nf_Id='{id}'";

                    var command = new SqlCommand(changeQuery, dataBase.GetConnection());
                    command.ExecuteNonQuery();
                }
            }
            dataBase.Closeconnection();
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            deleteRow();
            ClearFields();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Update();
        }

        private void Change()
        {
            var selectedRowIndex= dataGridView1.CurrentCell.RowIndex;

            DataGridViewRow row = dataGridView1.Rows[selectedRow];
            var id= textBox_id.Text ;
            var designation=textBox_Designation.Text ;
            var name=textBox_Name.Text ;
            var taskType=textBox_TaskType.Text ;
            int priority; //= textBox_Priority.Text;

            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                if(int.TryParse(textBox_Priority.Text, out priority))
                {
                    dataGridView1.Rows[selectedRowIndex].SetValues(id, designation, name, taskType, priority);
                    dataGridView1.Rows[selectedRowIndex].Cells[5].Value = RowState.Modified;
                }
                else
                {
                    MessageBox.Show("Приоритет должен иметь числовой формат!");
                }
            }

        }
        private void btnChange_Click(object sender, EventArgs e)
        {
            Change();
            ClearFields();
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
    }

}
