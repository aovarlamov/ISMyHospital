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
    public partial class addSoob : Form
    {
        public addSoob(string id)
        {
            InitializeComponent();
            label3.Text = id;
            DB db = new DB();
            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }
            db.openConnection(dbc);

            DataTable tableDolj = new DataTable();
            MySqlDataAdapter adapterDolj = new MySqlDataAdapter();
            MySqlCommand commandDolj = new MySqlCommand("SELECT * FROM `Personal`WHERE id = @ul", dbc);
            commandDolj.Parameters.Add("@uL", MySqlDbType.VarChar).Value = id;
            adapterDolj.SelectCommand = commandDolj;
            adapterDolj.Fill(tableDolj);

            MySqlCommand check1;
            DataTable table1 = new DataTable();
            MySqlDataAdapter adapter1 = new MySqlDataAdapter();
            check1 = null;
            if (tableDolj.Rows[0][5].ToString() == "1")
            {
                check1 = new MySqlCommand("SELECT * FROM `Personal` WHERE `iddolg` = 2 OR `iddolg` = 3", dbc);
            }
            else if (tableDolj.Rows[0][5].ToString() == "2")
            {
                check1 = new MySqlCommand("SELECT * FROM `Personal` WHERE `iddolg` = 1 OR `iddolg` = 3", dbc);
            }
            else if (tableDolj.Rows[0][5].ToString() == "2")
            {
                check1 = new MySqlCommand("SELECT * FROM `Personal` WHERE `iddolg` = 1 OR `iddolg` = 2", dbc);
            }
            adapter1.SelectCommand = check1;
            adapter1.Fill(table1);
            comboBox1.DataSource = table1;
            comboBox1.DisplayMember = "fio";
            db.closeConnection(dbc);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "" || textBox1.Text == "" || comboBox1.Text == "")
            {
                MessageBox.Show("Заполнены не все поля");
                return;
            }
            DB db = new DB();
            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }

            DataTable tableVrach = new DataTable();
            MySqlDataAdapter adapterVrach = new MySqlDataAdapter();
            MySqlCommand commandVrach = new MySqlCommand("SELECT * FROM `Personal`WHERE fio = @ul", dbc);
            commandVrach.Parameters.Add("@uL", MySqlDbType.VarChar).Value = comboBox1.Text;
            adapterVrach.SelectCommand = commandVrach;
            adapterVrach.Fill(tableVrach);

            MySqlCommand command = new MySqlCommand("INSERT INTO `Soobshen` (idpers1, idpers2, tema, soobshenie) VALUES (@idpers1, @idpers2, @tema, @soobshenie)", dbc);
            command.Parameters.Add("@idpers1", MySqlDbType.VarChar).Value = Convert.ToInt32(label3.Text);
            command.Parameters.Add("@idpers2", MySqlDbType.VarChar).Value = Convert.ToInt32(tableVrach.Rows[0][0]);
            command.Parameters.Add("@tema", MySqlDbType.VarChar).Value = textBox5.Text;
            command.Parameters.Add("@soobshenie", MySqlDbType.VarChar).Value = textBox1.Text;


            db.openConnection(dbc);

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Сообщение успешно отправлено!");
                this.Close();
            }
            else
                MessageBox.Show("Отмена добавление!");

            db.closeConnection(dbc);
        }
    }
}
