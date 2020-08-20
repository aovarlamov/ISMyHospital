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
    public partial class glavrachForm : Form
    {
        public glavrachForm(string id)
        {
            InitializeComponent();
            label4.Text = id;
            ft_update();
            ft_update2();
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
            MySqlCommand commandUpdateTableZapis = new MySqlCommand("SELECT * FROM Zapis as A LEFT JOIN Spec as B ON(A.idspec = B.id) LEFT JOIN Vrach as C ON(A.idvrach = C.id) LEFT JOIN Statuszapis as D ON(A.idstatus = D.id) LEFT JOIN Personal as E ON(C.idpers = E.id) LEFT JOIN Personal as F ON(A.idpers = F.id)", dbc);
            commandUpdateTableZapis.ExecuteNonQuery();
            reader = commandUpdateTableZapis.ExecuteReader();

            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                var now = DateTime.Today;

                data.Add(new String[8]);
                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[9].ToString();
                data[data.Count - 1][2] = reader[22].ToString();
                data[data.Count - 1][3] = reader[28].ToString();
                data[data.Count - 1][4] = reader[4].ToString();
                data[data.Count - 1][5] = reader[5].ToString();
                data[data.Count - 1][6] = reader[6].ToString() + " ч.";
                data[data.Count - 1][7] = reader[19].ToString();
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
            MySqlCommand commandUpdateTableZapis = new MySqlCommand("SELECT * FROM Soobshen as A LEFT JOIN Personal as B ON(A.idpers1 = B.id) LEFT JOIN Personal as C ON(A.idpers2 = C.id) WHERE A.idpers1 = @uL OR A.idpers2 = @uL", dbc);
            commandUpdateTableZapis.Parameters.Add("@uL", MySqlDbType.VarChar).Value = label4.Text;
            commandUpdateTableZapis.ExecuteNonQuery();
            reader = commandUpdateTableZapis.ExecuteReader();

            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                var now = DateTime.Today;

                data.Add(new String[4]);
                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[7].ToString();
                data[data.Count - 1][2] = reader[13].ToString();
                data[data.Count - 1][3] = reader[3].ToString();
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

        private void button8_Click(object sender, EventArgs e)
        {
            ft_update2();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                DataTable table = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command1 = new MySqlCommand("SELECT * FROM `Zapis` WHERE `id` = @id", dbc);
                command1.Parameters.Add("@id", MySqlDbType.VarChar).Value = textBox1.Text;
                adapter.SelectCommand = command1;
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    showZapis showZapis = new showZapis(textBox1.Text);
                    showZapis.Show();
                }
                else
                {
                    MessageBox.Show("Поля с данным ID не найдено!");
                }
            }
            else
            {
                MessageBox.Show("Укажите ID для просмотра!");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ft_update3();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            addSoob addSoob = new addSoob(label4.Text);
            addSoob.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                DataTable table = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command1 = new MySqlCommand("SELECT * FROM `Soobshen` WHERE (`idpers1` = @idmy OR `idpers2` = @idmy) AND `id` = @id", dbc);
                command1.Parameters.Add("@id", MySqlDbType.VarChar).Value = textBox2.Text;
                command1.Parameters.Add("@idmy", MySqlDbType.VarChar).Value = label4.Text;
                adapter.SelectCommand = command1;
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    showSoob showSoob = new showSoob(textBox2.Text);
                    showSoob.Show();
                }
                else
                {
                    MessageBox.Show("Поля с данным ID не найдено!");
                }
            }
            else
            {
                MessageBox.Show("Укажите ID для просмотра!");
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 59) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
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
