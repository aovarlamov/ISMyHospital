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

namespace inzhProgUpravlBolnic
{
    public partial class dopInfo : Form
    {
        public dopInfo(string id)
        {
            InitializeComponent();
            label4.Text = id;
            DB db = new DB();
            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }
            db.openConnection(dbc);
            MySqlCommand checkDolj;
            DataTable tableDolj = new DataTable();
            MySqlDataAdapter adapterDolj = new MySqlDataAdapter();
            checkDolj = new MySqlCommand("SELECT * FROM `Personal` WHERE `id`=@id", dbc);
            checkDolj.Parameters.Add("@id", MySqlDbType.VarChar).Value = id;
            adapterDolj.SelectCommand = checkDolj;
            adapterDolj.Fill(tableDolj);
            if (tableDolj.Rows[0][5].ToString() != "1")
            {
                textBox3.Text = "Только для пациентов!";
                textBox3.ReadOnly = true;
            }
            db.closeConnection(dbc);
        }

        private void buttomLogin_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox2.Text)
                 && !string.IsNullOrWhiteSpace(textBox3.Text))
            {
                db.openConnection(dbc);

                MySqlCommand checkDolj;
                DataTable tableDolj = new DataTable();
                MySqlDataAdapter adapterDolj = new MySqlDataAdapter();
                checkDolj = new MySqlCommand("SELECT * FROM `Personal` WHERE `id`=@id", dbc);
                checkDolj.Parameters.Add("@id", MySqlDbType.VarChar).Value = label4.Text;
                adapterDolj.SelectCommand = checkDolj;
                adapterDolj.Fill(tableDolj);

                MySqlCommand commandUpdatePers = new MySqlCommand("UPDATE `Personal` SET `fio`=@fio, `phone`=@phone, `npolis`=@npolis WHERE `id`=@id", dbc);
                commandUpdatePers.Parameters.AddWithValue("fio", textBox1.Text);
                commandUpdatePers.Parameters.AddWithValue("phone", textBox2.Text);
                if (tableDolj.Rows[0][5].ToString() == "1")
                {
                    commandUpdatePers.Parameters.AddWithValue("npolis", textBox3.Text);
                }
                else
                {
                    commandUpdatePers.Parameters.AddWithValue("npolis", "0");
                }
                commandUpdatePers.Parameters.AddWithValue("id", label4.Text);
                commandUpdatePers.ExecuteNonQuery();
                MessageBox.Show("Вы успешно заполнили данные!");

                if (tableDolj.Rows[0][5].ToString() == "1")
                {
                    this.Hide();
                    pacForm pacForm = new pacForm(label4.Text);
                    pacForm.Show();
                }
                else if (tableDolj.Rows[0][5].ToString() == "2")
                {
                    this.Hide();
                    vrachForm vrachForm = new vrachForm(label4.Text);
                    vrachForm.Show();
                }
                else if (tableDolj.Rows[0][5].ToString() == "3")
                {
                    this.Hide();
                    glavrachForm glavrachForm = new glavrachForm(label4.Text);
                    glavrachForm.Show();
                }
                else if (tableDolj.Rows[0][5].ToString() == "4")
                {
                    this.Hide();
                    adminForm adminForm = new adminForm(label4.Text);
                    adminForm.Show();
                }
                else if (tableDolj.Rows[0][5].ToString() == "5")
                {
                    this.Hide();
                    operForm operForm = new operForm(label4.Text);
                    operForm.Show();
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }

    }
}
