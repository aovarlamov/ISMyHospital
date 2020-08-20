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
    public partial class operForm : Form
    {
        public operForm(string id)
        {
            InitializeComponent();
            label4.Text = id;
            ft_update();
            ft_update3();
            ft_update4();
        }
        void ft_update()
        {
            DB db = new DB();
            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }
            DataTable tableUpdDan = new DataTable();
            MySqlDataAdapter adapterUpdDan = new MySqlDataAdapter();
            MySqlCommand commandUpdDan = new MySqlCommand("SELECT * FROM `Personal` WHERE `id` = @id", dbc);
            commandUpdDan.Parameters.Add("@id", MySqlDbType.VarChar).Value = Convert.ToInt32(label4.Text);
            adapterUpdDan.SelectCommand = commandUpdDan;
            adapterUpdDan.Fill(tableUpdDan);
            label7.Text = tableUpdDan.Rows[0][2].ToString();
            label8.Text = tableUpdDan.Rows[0][3].ToString();
        }

        void ft_update3()
        {
            dataGridView1.Rows.Clear();
            DB db = new DB();

            MySqlDataReader reader = null;

            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }
            db.openConnection(dbc);
            MySqlCommand commandUpdateTableZapis = new MySqlCommand("SELECT * FROM Vrach as A LEFT JOIN Personal as B ON(A.idpers = B.id) LEFT JOIN Spec as C ON(A.idspec = C.id) LEFT JOIN PodverjVrach as D ON(A.idpodver = D.id) WHERE A.idpodver != @pod", dbc);
            commandUpdateTableZapis.Parameters.Add("@pod", MySqlDbType.VarChar).Value = "2";
            commandUpdateTableZapis.ExecuteNonQuery();
            reader = commandUpdateTableZapis.ExecuteReader();

            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                var now = DateTime.Today;

                data.Add(new String[7]);
                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[9].ToString();
                data[data.Count - 1][2] = reader[14].ToString();
                data[data.Count - 1][3] = reader[4].ToString();
                data[data.Count - 1][4] = reader[5].ToString();
                data[data.Count - 1][5] = reader[6].ToString();
                data[data.Count - 1][6] = reader[17].ToString();
            }
            db.closeConnection(dbc);

            foreach (string[] s in data)
            {
                dataGridView1.Rows.Add(s);
            }
        }
        void ft_update4()
        {
            dataGridView3.Rows.Clear();
            DB db = new DB();

            MySqlDataReader reader = null;

            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }
            db.openConnection(dbc);
            MySqlCommand commandUpdateTableZapis = new MySqlCommand("SELECT * FROM Vrach as A LEFT JOIN Personal as B ON(A.idpers = B.id) LEFT JOIN Spec as C ON(A.idspec = C.id) WHERE A.idpodver != @pod AND A.idpodver != @pod1", dbc);
            commandUpdateTableZapis.Parameters.Add("@pod", MySqlDbType.VarChar).Value = "1";
            commandUpdateTableZapis.Parameters.Add("@pod1", MySqlDbType.VarChar).Value = "3";
            commandUpdateTableZapis.ExecuteNonQuery();
            reader = commandUpdateTableZapis.ExecuteReader();

            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                var now = DateTime.Today;

                data.Add(new String[6]);
                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[9].ToString();
                data[data.Count - 1][2] = reader[14].ToString();
                data[data.Count - 1][3] = reader[4].ToString() + " ч.";
                data[data.Count - 1][4] = reader[5].ToString() + " ч.";
                data[data.Count - 1][5] = reader[6].ToString();
            }
            db.closeConnection(dbc);

            foreach (string[] s in data)
            {
                dataGridView3.Rows.Add(s);
            }
        }

        private void просмотретьДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm LoginForm = new LoginForm();
            LoginForm.Show();
        }

        private void редактироватьToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                db.openConnection(dbc);
                DataTable table = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command1 = new MySqlCommand("SELECT * FROM `Vrach` WHERE id = @id", dbc);
                command1.Parameters.Add("@id", MySqlDbType.VarChar).Value = textBox1.Text;
                adapter.SelectCommand = command1;
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    MySqlCommand command2 = new MySqlCommand("UPDATE `Vrach` SET `idpodver`= @idpodver WHERE `id` = @id", dbc);
                    command2.Parameters.Add("@id", MySqlDbType.VarChar).Value = textBox1.Text;
                    command2.Parameters.Add("@idpodver", MySqlDbType.VarChar).Value = 2;
                    command2.ExecuteNonQuery();
                    MessageBox.Show("Вы подтвердили врача с ID - " + textBox1.Text + "! Пожалуйста обновите таблицу!");
                }
                else
                {
                    MessageBox.Show("Поля с данным ID не найдено!");
                }
                db.closeConnection(dbc);
            }
            else
            {
                MessageBox.Show("Укажите ID врача для подтверждения статуса!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                db.openConnection(dbc);
                DataTable table = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command1 = new MySqlCommand("SELECT * FROM `Vrach` WHERE id = @id", dbc);
                command1.Parameters.Add("@id", MySqlDbType.VarChar).Value = textBox1.Text;
                adapter.SelectCommand = command1;
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    MySqlCommand command2 = new MySqlCommand("UPDATE `Vrach` SET `idpodver`= @idpodver WHERE `id` = @id", dbc);
                    command2.Parameters.Add("@id", MySqlDbType.VarChar).Value = textBox1.Text;
                    command2.Parameters.Add("@idpodver", MySqlDbType.VarChar).Value = 3;
                    command2.ExecuteNonQuery();
                    MessageBox.Show("Врача с ID - " + textBox1.Text + " получил отказ в доступе! Пожалуйста обновите таблицу!");
                }
                else
                {
                    MessageBox.Show("Поля с данным ID не найдено!");
                }
                db.closeConnection(dbc);
            }
            else
            {
                MessageBox.Show("Укажите ID врача для отказа!");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ft_update3();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 59) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void оПрограммеToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            oProge oProge = new oProge();
            oProge.Show();
        }
    }
}
