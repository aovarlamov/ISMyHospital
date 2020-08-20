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
    public partial class regForm : Form
    {
        public regForm()
        {
            InitializeComponent();
        }

        private void buttomLogin_Click(object sender, EventArgs e)
        {
            if (loginField.Text == "" || passField.Text == "" || passField2.Text == "")
            {
                MessageBox.Show("Заполнены не все поля");
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
                MessageBox.Show("Пароль должен содержать:\n\n*От 5 до 25 символов;\n*Пароль не должен содержать логин!");
                return;
            }
            DB db = new DB();
            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }
            //Создаем новую запись о логие и пароле
            MySqlCommand commandRegLog = new MySqlCommand("INSERT INTO `Users` (login, pass) VALUES (@login, @pass)", dbc);
            commandRegLog.Parameters.Add("@login", MySqlDbType.VarChar).Value = loginField.Text;
            commandRegLog.Parameters.Add("@pass", MySqlDbType.VarChar).Value = passField.Text;
            db.openConnection(dbc);
            commandRegLog.ExecuteNonQuery();
            //Получаем данные о ID логина и паролья
            DataTable tableRegIdLog = new DataTable();
            MySqlDataAdapter adapterRegIdLog = new MySqlDataAdapter();
            MySqlCommand commandRegIdLog = new MySqlCommand("SELECT * FROM `Users` WHERE `login` = @uL", dbc);
            commandRegIdLog.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginField.Text;
            adapterRegIdLog.SelectCommand = commandRegIdLog;
            adapterRegIdLog.Fill(tableRegIdLog);
            //Создаем персональные данные для логина и пароля
            MySqlCommand commandRegPers = new MySqlCommand("INSERT INTO `Personal` (idlog, fio, phone, npolis, iddolg) VALUES (@idlog, @fio, @phone, @npolis, @iddolg)", dbc);
            commandRegPers.Parameters.Add("@idlog", MySqlDbType.VarChar).Value = Convert.ToInt32(tableRegIdLog.Rows[0][0]);
            commandRegPers.Parameters.Add("@fio", MySqlDbType.VarChar).Value = "";
            commandRegPers.Parameters.Add("@phone", MySqlDbType.VarChar).Value = "";
            commandRegPers.Parameters.Add("@npolis", MySqlDbType.VarChar).Value = "";
            commandRegPers.Parameters.Add("@iddolg", MySqlDbType.VarChar).Value = 1;
            commandRegPers.ExecuteNonQuery();
            //Получаем данные о ID персональных данных
            DataTable tableRegIdPers = new DataTable();
            MySqlDataAdapter adapterRegIdPers = new MySqlDataAdapter();
            MySqlCommand commandRegIdPers = new MySqlCommand("SELECT * FROM Personal AS A LEFT JOIN Users AS C ON (A.idlog = C.id) WHERE C.login = @uL", dbc);
            commandRegIdPers.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginField.Text;
            adapterRegIdPers.SelectCommand = commandRegIdPers;
            adapterRegIdPers.Fill(tableRegIdPers);
            //Создаем запись о сборе данных
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

            MessageBox.Show("Вы успешно зарегестрированы!");

            db.closeConnection(dbc);
            this.Close();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }

        //Проверка на индентичный логин
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

        //Проверка пароля
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
    }
}
