using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public int emp_id;
        public MySqlConnection Connect()
        {
            string Con = "server=127.0.0.1; user=root; database=tables;password=";
            MySqlConnection con = new MySqlConnection(Con);
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return con;
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid())
                {
                    MySqlConnection con = Connect();
                    MySqlCommand cmd = new MySqlCommand("insert into employee(id,emp_name,email_id) values(@id, @emp_name, @email_id)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", tbEmp_id.Text);
                    cmd.Parameters.AddWithValue("@emp_name", tbEmp_name.Text);
                    cmd.Parameters.AddWithValue("@email_id", tbEmail.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Inserted Successfully", "Insert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GetStudentsRecord();
                    Reset();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Id must be Unique.");
            }
            
        }

        private void Reset()
        {
            emp_id = 0;
            tbEmp_id.ResetText();
            tbEmp_name.ResetText();
            tbEmail.ResetText();
            tbEmp_id.Focus();
        }

        private void GetStudentsRecord()
        {

            dgvEmployeeList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            MySqlConnection con = Connect();
            String sql = "Select * from employee";
            MySqlCommand cmd = new MySqlCommand(sql, con);
            DataTable dTable = new DataTable();
            
            MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
            MyAdapter.SelectCommand = cmd;
            MyAdapter.Fill(dTable);
            dgvEmployeeList.DataSource = dTable;
            con.Close();
        }

        private bool IsValid()
        {
            if (tbEmp_name.Text == string.Empty)
            {
                MessageBox.Show("Employee information is Required ", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (emp_id > 0)
            {
                MySqlConnection con = Connect();
                MySqlCommand cmd = new MySqlCommand("Update employee set emp_name=@emp_name,email_id=@email_id where id = @id", con);
                cmd.Parameters.AddWithValue("@id", tbEmp_id.Text);
                cmd.Parameters.AddWithValue("@emp_name", tbEmp_name.Text);
                cmd.Parameters.AddWithValue("@email_id", tbEmail.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Updated Successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //GetStudentsRecord();
                Reset();
                con.Close();
            }
            else
            {
                MessageBox.Show("Please Select an Employee to update his information", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (emp_id > 0)
            {
                MySqlConnection con = Connect();
                MySqlCommand cmd = new MySqlCommand("Delete from employee where id = @id", con);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@id", this.emp_id);

                var abc = cmd.ExecuteNonQuery();

                MessageBox.Show("Deleted Successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //GetStudentsRecord();
                Reset();
                con.Close();
            }
            else
            {
                MessageBox.Show("Please Select an Employee to Delete his information", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            GetStudentsRecord();
        }

        private void dgvEmployeeList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            emp_id = Convert.ToInt32(dgvEmployeeList.SelectedRows[0].Cells[0].Value);
            tbEmp_id.Text = dgvEmployeeList.SelectedRows[0].Cells[0].Value.ToString();
            tbEmp_name.Text = dgvEmployeeList.SelectedRows[0].Cells[1].Value.ToString();
            tbEmail.Text = dgvEmployeeList.SelectedRows[0].Cells[2].Value.ToString();
        }
    }
}
