using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        private DataGridView dataGridView1;
        private Button btnLoadCsv;
        private Button btnSubmit;
        private TextBox txtName;
        private TextBox txtEmail;
        private TextBox txtAge;
        private Label lblName;
        private Label lblEmail;
        private Label lblAge;

        private static string connectionString = "server=localhost;port=3307;database=school;user=root;password=;";

        public Form1()
        {
            this.Text = "Diákok kezelése";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            dataGridView1 = new DataGridView();
            dataGridView1.Location = new Point(20, 20);
            dataGridView1.Size = new Size(500, 400);
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            this.Controls.Add(dataGridView1);

            lblName = new Label();
            lblName.Text = "Név";
            lblName.Location = new Point(550, 40);
            this.Controls.Add(lblName);

            txtName = new TextBox();
            txtName.Location = new Point(550, 60);
            txtName.Width = 200;
            this.Controls.Add(txtName);

            lblEmail = new Label();
            lblEmail.Text = "Email";
            lblEmail.Location = new Point(550, 100);
            this.Controls.Add(lblEmail);

            txtEmail = new TextBox();
            txtEmail.Location = new Point(550, 120);
            txtEmail.Width = 200;
            this.Controls.Add(txtEmail);

            lblAge = new Label();
            lblAge.Text = "Életkor";
            lblAge.Location = new Point(550, 160);
            this.Controls.Add(lblAge);

            txtAge = new TextBox();
            txtAge.Location = new Point(550, 180);
            txtAge.Width = 200;
            this.Controls.Add(txtAge);

            btnSubmit = new Button();
            btnSubmit.Text = "Hozzáadás";
            btnSubmit.Location = new Point(550, 220);
            btnSubmit.Width = 200;
            btnSubmit.Click += BtnSubmit_Click;
            this.Controls.Add(btnSubmit);

            btnLoadCsv = new Button();
            btnLoadCsv.Text = "CSV betöltése";
            btnLoadCsv.Location = new Point(550, 270);
            btnLoadCsv.Width = 200;
            btnLoadCsv.Click += BtnLoadCsv_Click;
            this.Controls.Add(btnLoadCsv);

            LoadGrid();
        }

        private void LoadGrid()
        {
            dataGridView1.DataSource = GetStudents();
        }

        private DataTable GetStudents()
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var adapter = new MySqlDataAdapter("SELECT * FROM Students", conn);
                var table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        private void Studentsbe(List<Student> students)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                foreach (var s in students)
                {
                    string query = "INSERT INTO Students (Name, Email, Age) VALUES (@Name, @Email, @Age)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", s.Name);
                        cmd.Parameters.AddWithValue("@Email", s.Email);
                        cmd.Parameters.AddWithValue("@Age", s.Age);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void BtnLoadCsv_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV files (*.csv)|*.csv";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                List<Student> students = CsvHelperService.ReadStudents(ofd.FileName);
                Studentsbe(students);
                LoadGrid();
            }
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtAge.Text, out int age))
                return;

            Student student = new Student
            {
                Id = new Random().Next(1000, 9999),
                Name = txtName.Text,
                Email = txtEmail.Text,
                Age = age
            };

            Studentsbe(new List<Student> { student });
            LoadGrid();

            txtName.Clear();
            txtEmail.Clear();
            txtAge.Clear();
        }
    }
}

