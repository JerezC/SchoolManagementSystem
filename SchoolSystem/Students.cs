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
using System.Runtime.Remoting.Contexts;

namespace SchoolSystem
{
    public partial class Students : Form
    {
        public Students()
        {
            InitializeComponent();
            DisplayStudent();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Students_Load(object sender, EventArgs e)
        {
            this.studentTblTableAdapter.Fill(this.schoolDbDataSet.StudentTbl);

        }
        SqlConnection Con = new SqlConnection("");// Add Connection String To SQL Server here


        private void DisplayStudent()
        {
            Con.Open();

            string Query = "Select * from StudentTbl";

            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);

            
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            StudentsDGV.DataSource = ds.Tables[0];

            Con.Close();
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (StNameTb.Text == "" || FeesTb.Text == "" || AddressTb.Text == "" || StGenCb.SelectedIndex == -1 || ClassCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();


                    SqlCommand cmd = new SqlCommand("insert into StudentTbl(StName, StGen, StDOB, StClass, StFees, StAddress) values (@SName, @SGen, @SDob, @SClass, @SFees, @SAddress)", Con);

                    cmd.Parameters.AddWithValue("@SName", StNameTb.Text);
                    cmd.Parameters.AddWithValue("@SGen", StGenCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SDob", DOBPicker.Value.Date);
                    cmd.Parameters.AddWithValue("@SClass", ClassCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SFees", FeesTb.Text);
                    cmd.Parameters.AddWithValue("@SAddress", AddressTb.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student Added");

                    Con.Close();
                    DisplayStudent();
                    Reset();

                }
                catch (Exception Ex) 
                {
                    MessageBox.Show(Ex.Message);
                }
                
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Reset()
        {
            Key = 0;
            StNameTb.Text = "";
            FeesTb.Text = "";
            AddressTb.Text = "";
            ClassCb.SelectedIndex = 0;
            StGenCb.SelectedIndex = 0;
        }

        int Key = 0;
        private void StudentsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            StNameTb.Text = StudentsDGV.SelectedRows[0].Cells[1].Value.ToString();
            StGenCb.SelectedItem = StudentsDGV.SelectedRows[0].Cells[2].Value.ToString();
            DOBPicker.Text = StudentsDGV.SelectedRows[0].Cells[3].Value.ToString();
            ClassCb.SelectedItem = StudentsDGV.SelectedRows[0].Cells[4].Value.ToString();
            FeesTb.Text = StudentsDGV.SelectedRows[0].Cells[5].Value.ToString();
            AddressTb.Text = StudentsDGV.SelectedRows[0].Cells[6].Value.ToString();

            if (StNameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(StudentsDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if(Key == 0)
            {
                MessageBox.Show("Select a student");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from studentTbl where StId= @StKey", Con);
                    cmd.Parameters.AddWithValue("@StKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student deleted");
                    Con.Close();
                    DisplayStudent();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (StNameTb.Text == "" || FeesTb.Text == "" || AddressTb.Text == "" || StGenCb.SelectedIndex == -1 || ClassCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();


                    SqlCommand cmd = new SqlCommand("update StudentTbl set StName=@Name, StGen=@SGen, StDOB=@SDob, StClass= @SClass, StFees= @SFees, StAddress= @SAddress where StId=@StudentId", Con);

                    cmd.Parameters.AddWithValue("@SName", StNameTb.Text);
                    cmd.Parameters.AddWithValue("@SGen", StGenCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SDob", DOBPicker.Value.Date);
                    cmd.Parameters.AddWithValue("@SClass", ClassCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SFees", FeesTb.Text);
                    cmd.Parameters.AddWithValue("@SAddress", AddressTb.Text);
                    cmd.Parameters.AddWithValue("@StudentId", Key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student Updated");

                    Con.Close();
                    DisplayStudent();
                    Reset();

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            MainMenu Obj =  new MainMenu();
            Obj.Show();
            this.Hide();
        }
    }
}
