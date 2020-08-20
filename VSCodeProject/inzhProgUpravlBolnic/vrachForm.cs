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
    public partial class vrachForm : Form
    {
        public vrachForm(string id)
        {
            InitializeComponent();
            label4.Text = id;
            ft_update();
            //Проверка статуса
            if (label19.Text != "Подтвержден")
            {
                tabControl1.TabPages.Remove(tabPage2);
                tabControl1.TabPages.Remove(tabPage3);
                label21.Text = "Ваш статус не пподтвержден! Вы не можете использовать функции учетной записи врача!";
            }
            else
            {
                label21.Visible = false;
                ft_update2();
                ft_update3();
            }
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
            MySqlCommand commandUpdDan = new MySqlCommand("SELECT * FROM Vrach as A LEFT JOIN Personal as B ON(A.idpers = B.id) LEFT JOIN Spec as C ON(A.idspec = C.id) LEFT JOIN PodverjVrach as D ON(A.idpodver = D.id) WHERE A.idpers = @id", dbc);
            commandUpdDan.Parameters.Add("@id", MySqlDbType.VarChar).Value = Convert.ToInt32(label4.Text);
            adapterUpdDan.SelectCommand = commandUpdDan;
            adapterUpdDan.Fill(tableUpdDan);
            label7.Text = tableUpdDan.Rows[0][9].ToString();
            label8.Text = tableUpdDan.Rows[0][10].ToString();
            label3.Text = tableUpdDan.Rows[0][14].ToString();
            label13.Text = tableUpdDan.Rows[0][6].ToString();
            label15.Text = tableUpdDan.Rows[0][4].ToString();
            label17.Text = tableUpdDan.Rows[0][5].ToString();
            label19.Text = tableUpdDan.Rows[0][17].ToString();
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
            MySqlCommand commandUpdateTableZapis = new MySqlCommand("SELECT * FROM Zapis as A LEFT JOIN Spec as B ON(A.idspec = B.id) LEFT JOIN Vrach as C ON(A.idvrach = C.id) LEFT JOIN Statuszapis as D ON(A.idstatus = D.id) LEFT JOIN Personal as E ON(C.idpers = E.id) LEFT JOIN Personal as F ON(A.idpers = F.id) WHERE E.id = @uL", dbc);
            commandUpdateTableZapis.Parameters.Add("@uL", MySqlDbType.VarChar).Value = label4.Text;
            commandUpdateTableZapis.ExecuteNonQuery();
            reader = commandUpdateTableZapis.ExecuteReader();

            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                var now = DateTime.Today;

                data.Add(new String[6]);
                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[28].ToString();
                data[data.Count - 1][2] = reader[4].ToString();
                data[data.Count - 1][3] = reader[5].ToString();
                data[data.Count - 1][4] = reader[6].ToString() + " ч.";
                data[data.Count - 1][5] = reader[19].ToString();
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
                MySqlCommand command1 = new MySqlCommand("SELECT * FROM `Zapis` as A LEFT JOIN Vrach as B ON(A.idvrach = B.id) WHERE A.id = @id AND B.idpers = @idmy", dbc);
                command1.Parameters.Add("@id", MySqlDbType.VarChar).Value = textBox1.Text;
                command1.Parameters.Add("@idmy", MySqlDbType.VarChar).Value = label4.Text;
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
                MySqlCommand command1 = new MySqlCommand("SELECT * FROM `Zapis` as A LEFT JOIN Vrach as B ON(A.idvrach = B.id) WHERE A.id = @id AND B.idpers = @idmy AND `idstatus` = @idstatus", dbc);
                command1.Parameters.Add("@id", MySqlDbType.VarChar).Value = textBox1.Text;
                command1.Parameters.Add("@idmy", MySqlDbType.VarChar).Value = label4.Text;
                command1.Parameters.Add("@idstatus", MySqlDbType.VarChar).Value = 1;
                adapter.SelectCommand = command1;
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    MySqlCommand command2 = new MySqlCommand("UPDATE `Zapis` SET `idstatus`= @idstatus WHERE `id` = @id", dbc);
                    command2.Parameters.Add("@id", MySqlDbType.VarChar).Value = textBox1.Text;
                    command2.Parameters.Add("@idstatus", MySqlDbType.VarChar).Value = 2;
                    command2.ExecuteNonQuery();
                    MessageBox.Show("Вы отменили запись с ID - " + textBox1.Text + "!");
                    ft_update2();
                }
                else
                {
                    MessageBox.Show("Поля с данным ID не найдено или имеет статус, который нельзя отменить!");
                }
                db.closeConnection(dbc);
            }
            else
            {
                MessageBox.Show("Укажите ID записи для отмены!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
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
                MySqlCommand command1 = new MySqlCommand("SELECT * FROM `Zapis` as A LEFT JOIN Vrach as B ON(A.idvrach = B.id) WHERE A.id = @id AND B.idpers = @idmy AND `idstatus` = @idstatus", dbc);
                command1.Parameters.Add("@id", MySqlDbType.VarChar).Value = textBox1.Text;
                command1.Parameters.Add("@idmy", MySqlDbType.VarChar).Value = label4.Text;
                command1.Parameters.Add("@idstatus", MySqlDbType.VarChar).Value = 1;
                adapter.SelectCommand = command1;
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    MySqlCommand command2 = new MySqlCommand("UPDATE `Zapis` SET `idstatus`= @idstatus WHERE `id` = @id", dbc);
                    command2.Parameters.Add("@id", MySqlDbType.VarChar).Value = textBox1.Text;
                    command2.Parameters.Add("@idstatus", MySqlDbType.VarChar).Value = 3;
                    command2.ExecuteNonQuery();
                    MessageBox.Show("Вы подтвердили запись с ID - " + textBox1.Text + "!");
                    ft_update2();
                }
                else
                {
                    MessageBox.Show("Поля с данным ID не найдено или имеет статус, который нельзя подтвердить!");
                }
                db.closeConnection(dbc);
            }
            else
            {
                MessageBox.Show("Укажите ID записи для подтверждения!");
            }
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
                MySqlCommand command1 = new MySqlCommand("SELECT * FROM `Zapis` as A LEFT JOIN Vrach as B ON(A.idvrach = B.id) WHERE A.id = @id AND B.idpers = @idmy AND `idstatus` = @idstatus", dbc);
                command1.Parameters.Add("@id", MySqlDbType.VarChar).Value = textBox1.Text;
                command1.Parameters.Add("@idmy", MySqlDbType.VarChar).Value = label4.Text;
                command1.Parameters.Add("@idstatus", MySqlDbType.VarChar).Value = 3;
                adapter.SelectCommand = command1;
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    MySqlCommand command2 = new MySqlCommand("UPDATE `Zapis` SET `idstatus`= @idstatus WHERE `id` = @id", dbc);
                    command2.Parameters.Add("@id", MySqlDbType.VarChar).Value = textBox1.Text;
                    command2.Parameters.Add("@idstatus", MySqlDbType.VarChar).Value = 4;
                    command2.ExecuteNonQuery();
                    MessageBox.Show("Прием с ID - " + textBox1.Text + "состоялся!");
                    ft_update2();
                }
                else
                {
                    MessageBox.Show("Поля с данным ID не найдено или имеет статус, который нельзя отметить как состоявщийся!");
                }
                db.closeConnection(dbc);
            }
            else
            {
                MessageBox.Show("Укажите ID записи!");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ft_update2();
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

        private void button9_Click(object sender, EventArgs e)
        {
            ft_update3();
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
