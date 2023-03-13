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
    public partial class Events : Form
    {
        public Events()
        {
            InitializeComponent();
            DisplayEvents();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        SqlConnection Con = new SqlConnection("");// Add Connection String To SQL Server here



        private void DisplayEvents()
        {
            Con.Open();

            string Query = "select * from EventsTbl";

            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);

            //
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            EventsDGV.DataSource = ds.Tables[0];

            Con.Close();
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            MainMenu Obj = new MainMenu(); 
            Obj.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Reset()
        {
            Key = 0;
            EDescTb.Text = "";
            EdurationTb.Text = "";
        }

        

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (EDescTb.Text == "" || EdurationTb.Text == "" )
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();


                    SqlCommand cmd = new SqlCommand("insert into EventsTbl(EDesc, EDate, EDuration) values (@EvDesc, @EvDate, @EvDuration)", Con);

                    cmd.Parameters.AddWithValue("@EvDesc", EDescTb.Text);
                    cmd.Parameters.AddWithValue("@EvDate", EDate.Value.Date);
                    cmd.Parameters.AddWithValue("@EvDuration", EdurationTb.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Event Added");

                    Con.Close();
                    DisplayEvents();
                    Reset();

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Select an Event");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from EventsTbl where EId= @EKey", Con);
                    cmd.Parameters.AddWithValue("@EKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Event Deleted");
                    Con.Close();
                    DisplayEvents();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        int Key = 0;
        private void EventsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            EDescTb.Text = EventsDGV.SelectedRows[0].Cells[1].Value.ToString();
            EDate.Text = EventsDGV.SelectedRows[0].Cells[2].Value.ToString();
            EdurationTb.Text = EventsDGV.SelectedRows[0].Cells[3].Value.ToString();


            if (EDescTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(EventsDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (EDescTb.Text == "" || EdurationTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();


                    SqlCommand cmd = new SqlCommand("update EventsTbl set EDesc= @EvDesc, EDate= @EvDate, EDuration= @EvDuration where EId= @EvId", Con);

                    cmd.Parameters.AddWithValue("@EvDesc", EDescTb.Text);
                    cmd.Parameters.AddWithValue("@EvDate", EDate.Value.Date);
                    cmd.Parameters.AddWithValue("@EvDuration", EdurationTb.Text);
                    cmd.Parameters.AddWithValue("@EvId", Key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Event Updated");

                    Con.Close();
                    DisplayEvents();
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
