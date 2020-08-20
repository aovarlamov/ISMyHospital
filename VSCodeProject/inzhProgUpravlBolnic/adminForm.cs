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
    public partial class adminForm : Form
    {
        public adminForm(string id)
        {
            InitializeComponent();
            label4.Text = id;
            ft_update();
            ft_update2();
            ft_update3();
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

        void ft_update2()
        {
            dataGridView2.Rows.Clear();
            DB db = new DB();

            MySqlDataReader reader = null;

            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }
            db.openConnection(dbc);
            MySqlCommand commandUpdateTableZapis = new MySqlCommand("SELECT * FROM Personal as A LEFT JOIN Users as B ON(A.idlog = B.id) LEFT JOIN Dolj as C ON(A.iddolg = C.id)", dbc);
            reader = commandUpdateTableZapis.ExecuteReader();

            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                var now = DateTime.Today;

                data.Add(new String[6]);
                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[7].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString();
                data[data.Count - 1][4] = reader[4].ToString();
                data[data.Count - 1][5] = reader[10].ToString();
            }
            db.closeConnection(dbc);

            foreach (string[] s in data)
            {
                dataGridView2.Rows.Add(s);
            }
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
            MySqlCommand commandUpdateTableZapis = new MySqlCommand("SELECT * FROM Sbordann as A LEFT JOIN Personal as B ON(A.idpers = B.id) LEFT JOIN Soglasie as C ON(A.idsogl = C.id) LEFT JOIN Users as D ON(B.idlog = D.id)", dbc);
            reader = commandUpdateTableZapis.ExecuteReader();

            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                var now = DateTime.Today;

                data.Add(new String[9]);
                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[18].ToString();
                data[data.Count - 1][2] = reader[16].ToString();
                data[data.Count - 1][3] = reader[3].ToString();
                data[data.Count - 1][4] = reader[4].ToString();
                data[data.Count - 1][5] = reader[5].ToString();
                data[data.Count - 1][6] = reader[6].ToString();
                data[data.Count - 1][7] = reader[7].ToString();
                data[data.Count - 1][8] = reader[8].ToString();
            }
            db.closeConnection(dbc);

            foreach (string[] s in data)
            {
                dataGridView1.Rows.Add(s);
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

        private void button8_Click(object sender, EventArgs e)
        {
            ft_update2();
            ft_update3();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }
            db.openConnection(dbc);
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                DataTable table = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command1 = new MySqlCommand("SELECT * FROM `Personal` WHERE `id` = @id", dbc);
                command1.Parameters.Add("@id", MySqlDbType.VarChar).Value = textBox1.Text;
                adapter.SelectCommand = command1;
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    var result = new System.Windows.Forms.DialogResult();
                    result = MessageBox.Show("Точно удалить пользователя?", "Внимание!",
                    MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {

                        MySqlCommand command = new MySqlCommand("DELETE FROM `Users` WHERE `id`=@id", dbc);
                        command.Parameters.AddWithValue("id", Convert.ToInt32(table.Rows[0][1]));
                        command.ExecuteNonQuery();
                        MessageBox.Show("Вы успешно удалили пользователя с ID - " + textBox1.Text + " !");
                    }
                }
                else
                {
                    MessageBox.Show("Поля с данным ID не существует!");
                }
            }
            else
            {
                MessageBox.Show("Укажите ID пользователя которого хотите удалить!");
            }
            db.closeConnection(dbc);
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
