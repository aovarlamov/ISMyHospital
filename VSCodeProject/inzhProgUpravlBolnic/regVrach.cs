using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace inzhProgUpravlBolnic
{
    public partial class regVrach : Form
    {
        public regVrach()
        {
            InitializeComponent();
            DB db = new DB();
            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }
            //Заполнение комбо бокса
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

        private void buttomLogin_Click(object sender, EventArgs e)
        {
            if (loginField.Text == "" || passField.Text == "" || passField2.Text == "" || textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show("Заполнены не все поля");
                return;
            }
            if (Convert.ToInt32(textBox3.Text) > 22 || Convert.ToInt32(textBox3.Text) < 7 || Convert.ToInt32(textBox4.Text) > 22 || Convert.ToInt32(textBox3.Text) < 7
                || Convert.ToInt32(textBox3.Text) > Convert.ToInt32(textBox4.Text))
            {
                MessageBox.Show("Укажите корректное время приема! (Больница работает с 7 до 22)");
                return;
            }
            if (passField.Text != passField2.Text)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }
            if (!checkBox1.Checked)
            {
                MessageBox.Show("Вы дальнейшей регистрации Вы должны согласиться на обработку персноальных данных!");
                return;
            }
            if (!checkUser())
                return;
            if (!checkPass(passField.Text) || checkPass1(passField.Text, loginField.Text))
            {
                MessageBox.Show("Пароль должен содержать:\n\n*От 5 до 25 символов;\n*Пароль не должен содержать логин\n\n*Пароль должен состоять из латинских букв!");
                return;
            }
            DB db = new DB();
            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }
            //Регистрация как и обычного пользователя, но добавлятеся запись в таблицу врачей
            MySqlCommand commandRegLog = new MySqlCommand("INSERT INTO `Users` (login, pass) VALUES (@login, @pass)", dbc);
            commandRegLog.Parameters.Add("@login", MySqlDbType.VarChar).Value = loginField.Text;
            commandRegLog.Parameters.Add("@pass", MySqlDbType.VarChar).Value = passField.Text;
            db.openConnection(dbc);
            commandRegLog.ExecuteNonQuery();

            DataTable tableRegIdLog = new DataTable();
            MySqlDataAdapter adapterRegIdLog = new MySqlDataAdapter();
            MySqlCommand commandRegIdLog = new MySqlCommand("SELECT * FROM `Users` WHERE `login` = @uL", dbc);
            commandRegIdLog.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginField.Text;
            adapterRegIdLog.SelectCommand = commandRegIdLog;
            adapterRegIdLog.Fill(tableRegIdLog);

            MySqlCommand commandRegPers = new MySqlCommand("INSERT INTO `Personal` (idlog, fio, phone, npolis, iddolg) VALUES (@idlog, @fio, @phone, @npolis, @iddolg)", dbc);
            commandRegPers.Parameters.Add("@idlog", MySqlDbType.VarChar).Value = Convert.ToInt32(tableRegIdLog.Rows[0][0]);
            commandRegPers.Parameters.Add("@fio", MySqlDbType.VarChar).Value = textBox1.Text;
            commandRegPers.Parameters.Add("@phone", MySqlDbType.VarChar).Value = textBox2.Text;
            commandRegPers.Parameters.Add("@npolis", MySqlDbType.VarChar).Value = "0";
            commandRegPers.Parameters.Add("@iddolg", MySqlDbType.VarChar).Value = 2;
            commandRegPers.ExecuteNonQuery();

            DataTable tableRegIdPers = new DataTable();
            MySqlDataAdapter adapterRegIdPers = new MySqlDataAdapter();
            MySqlCommand commandRegIdPers = new MySqlCommand("SELECT * FROM Personal AS A LEFT JOIN Users AS C ON (A.idlog = C.id) WHERE C.login = @uL", dbc);
            commandRegIdPers.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginField.Text;
            adapterRegIdPers.SelectCommand = commandRegIdPers;
            adapterRegIdPers.Fill(tableRegIdPers);

            MySqlCommand commandCreatSborDan = new MySqlCommand("INSERT INTO `Sbordann` (idpers, idsogl, lastupdate, dan1, dan2, dan3, dan4, dan5) VALUES (@idpers, @idsogl, @lastupdate, @dan1, @dan2, @dan3, @dan4, @dan5)", dbc);
            commandCreatSborDan.Parameters.Add("@idpers", MySqlDbType.VarChar).Value = Convert.ToInt32(tableRegIdPers.Rows[0][0]);
            commandCreatSborDan.Parameters.Add("@idsogl", MySqlDbType.VarChar).Value = 1;
            commandCreatSborDan.Parameters.Add("@lastupdate", MySqlDbType.VarChar).Value = "";
            commandCreatSborDan.Parameters.Add("@dan1", MySqlDbType.VarChar).Value = "";
            commandCreatSborDan.Parameters.Add("@dan2", MySqlDbType.VarChar).Value = "";
            commandCreatSborDan.Parameters.Add("@dan3", MySqlDbType.VarChar).Value = "";
            commandCreatSborDan.Parameters.Add("@dan4", MySqlDbType.VarChar).Value = "";
            commandCreatSborDan.Parameters.Add("@dan5", MySqlDbType.VarChar).Value = "";
            commandCreatSborDan.ExecuteNonQuery();

            DataTable tableRegSpec = new DataTable();
            MySqlDataAdapter adapterRegSpec = new MySqlDataAdapter();
            MySqlCommand commandRegSpec = new MySqlCommand("SELECT * FROM `Spec` WHERE `name` = @uL", dbc);
            commandRegSpec.Parameters.Add("@uL", MySqlDbType.VarChar).Value = comboBox2.Text;
            adapterRegSpec.SelectCommand = commandRegSpec;
            adapterRegSpec.Fill(tableRegSpec);

            MySqlCommand commandCreatVrach = new MySqlCommand("INSERT INTO `Vrach` (idpers, idspec, idpodver, chasinach, chasikonec, kabin) VALUES (@idpers, @idspec, @idpodver, @chasinach, @chasikonec, @kabin)", dbc);
            commandCreatVrach.Parameters.Add("@idpers", MySqlDbType.VarChar).Value = Convert.ToInt32(tableRegIdPers.Rows[0][0]);
            commandCreatVrach.Parameters.Add("@idspec", MySqlDbType.VarChar).Value = Convert.ToInt32(tableRegSpec.Rows[0][0]); ;
            commandCreatVrach.Parameters.Add("@idpodver", MySqlDbType.VarChar).Value = 1;
            commandCreatVrach.Parameters.Add("@chasinach", MySqlDbType.VarChar).Value = textBox3.Text;
            commandCreatVrach.Parameters.Add("@chasikonec", MySqlDbType.VarChar).Value = textBox4.Text;
            commandCreatVrach.Parameters.Add("@kabin", MySqlDbType.VarChar).Value = textBox5.Text;
            commandCreatVrach.ExecuteNonQuery();

            MessageBox.Show("Ваш запрос успешно отправлен! Ждите пока оператор подтвердит вашу учетную запись!");

            db.closeConnection(dbc);
            this.Close();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }

        public Boolean checkUser()
        {
            DB db = new DB();

            DataTable tableCheck = new DataTable();

            MySqlDataAdapter adapterCheck = new MySqlDataAdapter();

            MySqlCommand commandCheck = new MySqlCommand("SELECT * FROM `Users` WHERE `login` = @uL", db.getConnection());
            commandCheck.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginField.Text;

            adapterCheck.SelectCommand = commandCheck;
            adapterCheck.Fill(tableCheck);

            if (tableCheck.Rows.Count > 0)
            {
                MessageBox.Show("Пользователь с таким логином уже существует!");
                return false;
            }
            else
                return true;
        }

        public bool checkPass(string plainText)
        {
            Regex regex = new Regex("[0-9a-zA-Z!@#$%&]{5,25}");
            Match match = regex.Match(plainText);
            return match.Success;
        }

        public bool checkPass1(string plainText, string login)
        {
            bool b = plainText.Contains(login);
            return b;
        }

        public int pass_test(string pass, string login)
        {
            bool b;
            b = pass.Contains(login);

            bool c;
            c = checkPass(pass);
            if (b == false && c == true)
                return 1;
            else
            {
                return 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 59) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 59) && e.KeyChar != 8)
                e.Handled = true;
        }
    }
}
