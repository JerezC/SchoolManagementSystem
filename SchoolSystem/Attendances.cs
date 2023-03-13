using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolSystem
{
    public partial class Attendances : Form
    {
        public Attendances()
        {
            InitializeComponent();
            DisplayAttendance();
            FillStudId();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void FillStudId()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("select StId from StudentTbl", Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("StId", typeof(int));
            dt.Load(rdr);
            StIdCb.ValueMember = "StId";
            StIdCb.DataSource = dt;

            Con.Close();
        }

        private void GetStudName()
        {
            Con.Open();
            SqlCommand cmd =  new SqlCommand("select * from StudentTbl where StId= @SId", Con);
            cmd.Parameters.AddWithValue("@SId", StIdCb.SelectedValue.ToString());
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach(DataRow dr in dt.Rows)
            {
                StNameTb.Text = dr["StName"].ToString();
            }

            Con.Close();
        }

        SqlConnection Con = new SqlConnection(""); // Add Connection String To SQL Server here



        private void DisplayAttendance()
        {
            Con.Open();

            string Query = "select * from AttendanceTbl";

            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);

            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            AttendanceDGV.DataSource = ds.Tables[0];

            Con.Close();
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MainMenu Obj = new MainMenu();
            Obj.Show();
            this.Hide();
        }

        private void Reset()
        {
            StNameTb.Text = "";
            StIdCb.SelectedIndex = -1;
            AttStatusCb.SelectedIndex = -1;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (StNameTb.Text == "" || AttStatusCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();


                    SqlCommand cmd = new SqlCommand("insert into AttendanceTbl(AttStId, AttStName, AttDate, AttStatus) values (@StId, @StName, @AttDate, @Status)", Con);

                    cmd.Parameters.AddWithValue("@StId", StIdCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@StName", StNameTb.Text);
                    cmd.Parameters.AddWithValue("@AttDate", AttDatePicker.Value.Date);
                    cmd.Parameters.AddWithValue("@Status", AttStatusCb.SelectedItem.ToString());

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Attendance Taken");

                    //Cerramos la conexion a la Db
                    Con.Close();
                    DisplayAttendance();
                    Reset();

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }

        private void StIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetStudName();
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            Reset();
        }

        int Key = 0;
        private void AttendanceDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            StIdCb.SelectedValue = AttendanceDGV.SelectedRows[0].Cells[1].Value.ToString();
            StNameTb.Text = AttendanceDGV.SelectedRows[0].Cells[2].Value.ToString();
            AttDatePicker.Text = AttendanceDGV.SelectedRows[0].Cells[3].Value.ToString();
            AttStatusCb.SelectedItem = AttendanceDGV.SelectedRows[0].Cells[4].Value.ToString();

            if (StNameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(AttendanceDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (StNameTb.Text == "" || AttStatusCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();


                    

                    SqlCommand cmd = new SqlCommand("update AttendanceTbl set AttStId= @StId, AttStName= @StName, AttDate= @AttDate, AttStatus= @Status where AttNum= @ANum", Con);

                    cmd.Parameters.AddWithValue("@StId", StIdCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@StName", StNameTb.Text);
                    cmd.Parameters.AddWithValue("@AttDate", AttDatePicker.Value.Date);
                    cmd.Parameters.AddWithValue("@Status", AttStatusCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@ANum", Key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Attendance Updated");

                    Con.Close();
                    DisplayAttendance();
                    Reset();

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }
    }
}
