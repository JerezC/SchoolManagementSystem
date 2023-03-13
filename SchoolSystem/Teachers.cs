using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolSystem
{
    public partial class Teachers : Form
    {
        public Teachers()
        {
            InitializeComponent();
            DisplayTeachers();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        SqlConnection Con = new SqlConnection("");// Add Connection String To SQL Server here



        private void DisplayTeachers()
        {
            Con.Open();

            string Query = "select * from TeacherTbl";

            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);

            
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            TeachersDGV.DataSource = ds.Tables[0];

            Con.Close();
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {

            if (TNameTb.Text == "" || TPhoneTb.Text == "" || TAddTb.Text == "" || TGenCB.SelectedIndex == -1 || TSubCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();


                    SqlCommand cmd = new SqlCommand("insert into TeacherTbl(TName, TGen, TPhone, TSub, TAdd, TDOB) values (@TName, @TGen, @TPhone, @TSub, @TAdd, @TDOB)", Con);

                    cmd.Parameters.AddWithValue("@TName", TNameTb.Text);
                    cmd.Parameters.AddWithValue("@TGen", TGenCB.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@TPhone", TPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@TSub", TSubCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@TAdd", TAddTb.Text);
                    cmd.Parameters.AddWithValue("@TDOB", TDOB.Value.Date);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Teacher Added");

                    Con.Close();
                    DisplayTeachers();
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
            TNameTb.Text = "";
            TPhoneTb.Text = "";
            TAddTb.Text = "";
            TSubCb.SelectedIndex = 0;
            TGenCB.SelectedIndex = 0;
        }

        int Key = 0;

        private void TeachersDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            TNameTb.Text = TeachersDGV.SelectedRows[0].Cells[1].Value.ToString();
            TGenCB.SelectedItem = TeachersDGV.SelectedRows[0].Cells[2].Value.ToString();
            TPhoneTb.Text = TeachersDGV.SelectedRows[0].Cells[3].Value.ToString();
            TSubCb.SelectedItem = TeachersDGV.SelectedRows[0].Cells[4].Value.ToString();
            TAddTb.Text = TeachersDGV.SelectedRows[0].Cells[5].Value.ToString();
            TDOB.Text = TeachersDGV.SelectedRows[0].Cells[6].Value.ToString();


            if (TNameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(TeachersDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Select a teacher");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from TeacherTbl where TId= @TKey", Con);
                    cmd.Parameters.AddWithValue("@TKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Teacher Deleted");
                    Con.Close();
                    DisplayTeachers();
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
            if (TNameTb.Text == "" || TPhoneTb.Text == "" || TAddTb.Text == "" || TGenCB.SelectedIndex == -1 || TSubCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();


                    SqlCommand cmd = new SqlCommand("update TeacherTbl set TName= @TName, TGen= @TGen, TPhone= @TPhone, TSub= @TSub, TAdd= @TAdd, TDOB= @TDOB where TId= @TeachId", Con);

                    cmd.Parameters.AddWithValue("@TName", TNameTb.Text);
                    cmd.Parameters.AddWithValue("@TGen", TGenCB.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@TPhone", TPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@TSub", TSubCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@TAdd", TAddTb.Text);
                    cmd.Parameters.AddWithValue("@TDOB", TDOB.Value.Date);
                    cmd.Parameters.AddWithValue("@TeachId", Key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Teacher Updated");

                    Con.Close();
                    DisplayTeachers();
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
            MainMenu Obj = new MainMenu();
            Obj.Show();
            this.Hide();
        }

        private void TNameTb_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
