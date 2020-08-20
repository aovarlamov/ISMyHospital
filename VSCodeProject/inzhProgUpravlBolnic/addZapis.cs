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
    public partial class addZapis : Form
    {
        public addZapis(string id)
        {
            InitializeComponent();
            label4.Text = id;
            updateSpec();
            updateVrach();
            updateTime();
        }

        void updateSpec()
        {
            DB db = new DB();
            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }
            db.openConnection(dbc);
            MySqlCommand check1;
            DataTable table1 = new DataTable();
            MySqlDataAdapter adapter1 = new MySqlDataAdapter();
            check1 = new MySqlCommand("SELECT * FROM `Spec`", dbc);
            adapter1.SelectCommand = check1;
            adapter1.Fill(table1);
            comboBox2.DataSource = table1;
            comboBox2.DisplayMember = "name";
            db.closeConnection(dbc);
        }

        void updateVrach()
        {
            DB db = new DB();
            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }
            db.openConnection(dbc);
            MySqlCommand check1;
            DataTable table1 = new DataTable();
            MySqlDataAdapter adapter1 = new MySqlDataAdapter();
            check1 = new MySqlCommand("SELECT * FROM `Vrach` as A LEFT JOIN  Spec as B ON(A.idspec = B.id) LEFT JOIN Personal as C ON(A.idpers = C.id) WHERE B.name = @name", dbc);
            check1.Parameters.Add("@name", MySqlDbType.VarChar).Value = comboBox2.Text;
            adapter1.SelectCommand = check1;
            adapter1.Fill(table1);
            comboBox1.DataSource = table1;
            comboBox1.DisplayMember = "fio";
            db.closeConnection(dbc);
        }

        void updateTime()
        {
            DB db = new DB();
            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }
            db.openConnection(dbc);
            MySqlCommand check1;
            DataTable table1 = new DataTable();
            MySqlDataAdapter adapter1 = new MySqlDataAdapter();
            check1 = new MySqlCommand("SELECT * FROM `Vrach` as A LEFT JOIN  Spec as B ON(A.idspec = B.id) LEFT JOIN Personal as C ON(A.idpers = C.id) WHERE C.fio = @name", dbc);
            check1.Parameters.Add("@name", MySqlDbType.VarChar).Value = comboBox1.Text;
            adapter1.SelectCommand = check1;
            adapter1.Fill(table1);
            if (table1.Rows.Count > 0)
            {
                label9.Text = table1.Rows[0][6].ToString();
                int i = Convert.ToInt32(table1.Rows[0][4]);
                while (i <= Convert.ToInt32(table1.Rows[0][5]))
                {
                    comboBox3.Items.Add(i);
                    i++;
                }
            }
            db.closeConnection(dbc);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void buttomLogin_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }
            if (!string.IsNullOrEmpty(comboBox2.Text) && !string.IsNullOrEmpty(comboBox1.Text)
                 && !string.IsNullOrEmpty(comboBox3.Text) && !string.IsNullOrWhiteSpace(passField.Text))
            {
                DateTime thisDate = DateTime.Today;
                int com = thisDate.CompareTo(dateTimePicker1.Value);
                if (com != 1)
                {
                    db.openConnection(dbc);
                    DataTable tableSpec = new DataTable();
                    MySqlDataAdapter adapterSpec = new MySqlDataAdapter();
                    MySqlCommand commandSpec = new MySqlCommand("SELECT * FROM `Spec` WHERE `name` = @uL", dbc);
                    commandSpec.Parameters.Add("@uL", MySqlDbType.VarChar).Value = comboBox2.Text;
                    adapterSpec.SelectCommand = commandSpec;
                    adapterSpec.Fill(tableSpec);

                    DataTable tableVrach = new DataTable();
                    MySqlDataAdapter adapterVrach = new MySqlDataAdapter();
                    MySqlCommand commandVrach = new MySqlCommand("SELECT * FROM `Vrach` as A LEFT JOIN Personal as B ON(A.idpers = B.id) WHERE B.fio = @ul", dbc);
                    commandVrach.Parameters.Add("@uL", MySqlDbType.VarChar).Value = comboBox1.Text;
                    adapterVrach.SelectCommand = commandVrach;
                    adapterVrach.Fill(tableVrach);

                    MySqlCommand command = new MySqlCommand("INSERT INTO `Zapis` (idspec, idvrach, idpers, prichina, date, time, idstatus) VALUES (@idspec, @idvrach, @idpers, @prichina, @date, @time, @idstatus)", dbc);
                    command.Parameters.Add("@idspec", MySqlDbType.VarChar).Value = Convert.ToInt32(tableSpec.Rows[0][0]);
                    command.Parameters.Add("@idvrach", MySqlDbType.VarChar).Value = Convert.ToInt32(tableVrach.Rows[0][0]);
                    command.Parameters.Add("@idpers", MySqlDbType.VarChar).Value = Convert.ToInt32(label4.Text);
                    command.Parameters.Add("@prichina", MySqlDbType.VarChar).Value = passField.Text;
                    command.Parameters.Add("@date", MySqlDbType.VarChar).Value = dateTimePicker1.Value.ToString();
                    command.Parameters.Add("@time", MySqlDbType.VarChar).Value = comboBox3.Text;
                    command.Parameters.Add("@idstatus", MySqlDbType.VarChar).Value = 1;

                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Свдения успешно добавлены!");
                        this.Close();
                    }
                    else
                        MessageBox.Show("Отмена добавление!");
                    db.closeConnection(dbc);

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Нельзя записаться на дату, которая уже прошла!");
                }

            }
            else
            {
                MessageBox.Show("Заполните все поля!");
            }
        }

        //При изменение комбо бокса обновляются данные в следующих комбобоксах 
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateVrach();
            comboBox3.Items.Clear();
            label9.Text = "";
            updateTime();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            updateTime();
        }
    }
}
