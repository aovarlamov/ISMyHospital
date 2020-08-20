using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace inzhProgUpravlBolnic
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            //Устанавливаем соединение с нашей БД
            DB db = new DB();
            DataTable tableCheckLogPass = new DataTable();
            MySqlDataAdapter adapterCheckLogPass = new MySqlDataAdapter();
            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }
            db.openConnection(dbc);
            //Проверяем наличие в БД соответствующего логина и пароля
            MySqlCommand commandCheckLogPass = new MySqlCommand("SELECT * FROM `Users` WHERE `login` = @uL AND `pass` = @uP", dbc);
            commandCheckLogPass.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginField.Text;
            commandCheckLogPass.Parameters.Add("@uP", MySqlDbType.VarChar).Value = passField.Text;
            adapterCheckLogPass.SelectCommand = commandCheckLogPass;
            adapterCheckLogPass.Fill(tableCheckLogPass);
            commandCheckLogPass.ExecuteNonQuery();

            if (tableCheckLogPass.Rows.Count > 0)
            {
                //Находим информацию о должности
                DataTable tableCheckDolj = new DataTable();
                MySqlDataAdapter adapterCheckDolj = new MySqlDataAdapter();
                MySqlCommand commandCheckDolj = new MySqlCommand("SELECT * FROM `Personal` WHERE `idlog` = @uL", dbc);
                commandCheckDolj.Parameters.Add("@uL", MySqlDbType.VarChar).Value = Convert.ToInt32(tableCheckLogPass.Rows[0][0]);
                adapterCheckDolj.SelectCommand = commandCheckDolj;
                adapterCheckDolj.Fill(tableCheckDolj);
                commandCheckDolj.ExecuteNonQuery();

                //Находим информацию о согласие на обработку данных о компьютере
                DataTable tableCheckSogl = new DataTable();
                MySqlDataAdapter adapterCheckSogl = new MySqlDataAdapter();
                MySqlCommand commandCheckSogl = new MySqlCommand("SELECT * FROM `Sbordann` WHERE `idpers` = @uL", dbc);
                commandCheckSogl.Parameters.Add("@uL", MySqlDbType.VarChar).Value = Convert.ToInt32(tableCheckDolj.Rows[0][0]);
                adapterCheckSogl.SelectCommand = commandCheckSogl;
                adapterCheckSogl.Fill(tableCheckSogl);
                commandCheckSogl.ExecuteNonQuery();
                string videoproc = null;
                string proc = null;
                string banklabel = null;
                string disk = null;
                string mac = null;
                //Если пользователь только зарегистрировался и это его первый вход
                if (tableCheckSogl.Rows[0][2].ToString() == "1")
                {
                    var result = new System.Windows.Forms.DialogResult();
                    result = MessageBox.Show("Готовы ли Вы предоставлять отчет о вашей ОС каждый месяц?\n\nЭто требуется для улучшения нашей программы", "Внимание!",
                    MessageBoxButtons.YesNo);

                    MySqlCommand commandUpdSbor = new MySqlCommand("UPDATE `Sbordann` SET `idsogl`=@idsogl, `lastupdate`=@lastupdate, `dan1`=@dan1, `dan2`=@dan2, `dan3`=@dan3, `dan4`=@dan4, `dan5`=@dan5 WHERE `idpers`=@id", dbc);
                    commandUpdSbor.Parameters.AddWithValue("id", tableCheckDolj.Rows[0][0]);
                    DateTime thisDay = DateTime.Today;
                    if (result == DialogResult.Yes)
                    {
                        //Записываем все необходимые нам данные о его компьютере
                        ManagementObjectSearcher searcher11 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");
                        ManagementObjectSearcher searcher12 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PhysicalMemory");
                        ManagementObjectSearcher searcher13 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive");
                        ManagementObjectSearcher searcher8 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapterConfiguration");

                        foreach (ManagementObject queryObj in searcher11.Get())
                        {
                            videoproc = string.Format("VideoProcessor: {0}", queryObj["VideoProcessor"]);
                        }
                        foreach (ManagementObject queryObj in searcher12.Get())
                        {
                            banklabel = string.Format("BankLabel: {0} ; Capacity: {1} Gb; Speed: {2} ", queryObj["BankLabel"], Math.Round(System.Convert.ToDouble(queryObj["Capacity"]) / 1024 / 1024 / 1024, 2), queryObj["Speed"]);
                        }
                        foreach (ManagementObject queryObj in searcher8.Get())
                        {
                            proc = string.Format("ProcName: {0}", queryObj["Name"]);
                        }
                        foreach (ManagementObject queryObj in searcher13.Get())
                        {
                            disk = string.Format("DiskDriver Model: {0}; Size: {1} Gb", queryObj["Model"], Math.Round(System.Convert.ToDouble(queryObj["Size"]) / 1024 / 1024 / 1024, 2));
                        }
                        foreach (ManagementObject queryObj in searcher.Get())
                        {
                            mac = string.Format("MACAddress: {0}", queryObj["MACAddress"]);
                        }
                        commandUpdSbor.Parameters.AddWithValue("dan1", proc);
                        commandUpdSbor.Parameters.AddWithValue("dan2", videoproc);
                        commandUpdSbor.Parameters.AddWithValue("dan3", disk);
                        commandUpdSbor.Parameters.AddWithValue("dan4", banklabel);
                        commandUpdSbor.Parameters.AddWithValue("dan5", mac);
                        commandUpdSbor.Parameters.AddWithValue("idsogl", 2);
                        commandUpdSbor.Parameters.AddWithValue("lastupdate", thisDay.ToString("d"));

                    }
                    else
                    {
                        commandUpdSbor.Parameters.AddWithValue("idsogl", 3);
                        commandUpdSbor.Parameters.AddWithValue("lastupdate", thisDay.ToString("d"));
                    }

                    commandUpdSbor.ExecuteNonQuery();
                }
                //Если пользователь дал добро на обработку его данных, смотри последнее обновление данных
                else if (tableCheckSogl.Rows[0][2].ToString() == "2")
                {
                    string dateInput = tableCheckSogl.Rows[0][3].ToString();
                    DateTime parsedDate = DateTime.Parse(dateInput);
                    DateTime thisDay = DateTime.Today;
                    int checkMonth = parsedDate.Month;
                    int thisMonth = thisDay.Month;
                    if (checkMonth != thisMonth)
                    {
                        MySqlCommand commandUpdSbor = new MySqlCommand("UPDATE `Sbordann` SET `idsogl`=@idsogl, `lastupdate`=@lastupdate, `dan1`=@dan1, `dan2`=@dan2, `dan3`=@dan3, `dan4`=@dan4, `dan5`=@dan5 WHERE `idpers`=@id", dbc);
                        commandUpdSbor.Parameters.AddWithValue("id", tableCheckDolj.Rows[0][0]);
                        ManagementObjectSearcher searcher11 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");
                        ManagementObjectSearcher searcher12 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PhysicalMemory");
                        ManagementObjectSearcher searcher13 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive");
                        ManagementObjectSearcher searcher8 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapterConfiguration");

                        foreach (ManagementObject queryObj in searcher11.Get())
                        {
                            videoproc = string.Format("VideoProcessor: {0}", queryObj["VideoProcessor"]);
                        }
                        foreach (ManagementObject queryObj in searcher12.Get())
                        {
                            banklabel = string.Format("BankLabel: {0} ; Capacity: {1} Gb; Speed: {2} ", queryObj["BankLabel"], Math.Round(System.Convert.ToDouble(queryObj["Capacity"]) / 1024 / 1024 / 1024, 2), queryObj["Speed"]);
                        }
                        foreach (ManagementObject queryObj in searcher8.Get())
                        {
                            proc = string.Format("ProcName: {0}", queryObj["Name"]);
                        }
                        foreach (ManagementObject queryObj in searcher13.Get())
                        {
                            disk = string.Format("DiskDriver Model: {0}; Size: {1} Gb", queryObj["Model"], Math.Round(System.Convert.ToDouble(queryObj["Size"]) / 1024 / 1024 / 1024, 2));
                        }
                        foreach (ManagementObject queryObj in searcher.Get())
                        {
                            mac = string.Format("MACAddress: {0}", queryObj["MACAddress"]);
                        }
                        commandUpdSbor.Parameters.AddWithValue("dan1", proc);
                        commandUpdSbor.Parameters.AddWithValue("dan2", videoproc);
                        commandUpdSbor.Parameters.AddWithValue("dan3", disk);
                        commandUpdSbor.Parameters.AddWithValue("dan4", banklabel);
                        commandUpdSbor.Parameters.AddWithValue("dan5", mac);
                        commandUpdSbor.Parameters.AddWithValue("idsogl", 2);
                        commandUpdSbor.Parameters.AddWithValue("lastupdate", thisDay.ToString("d"));
                        commandUpdSbor.ExecuteNonQuery();
                    }
                }
                //Проверяем должность пользователя
                if (tableCheckDolj.Rows[0][2].ToString() != "")
                {
                    if (tableCheckDolj.Rows[0][5].ToString() == "1")
                    {
                        this.Hide();
                        pacForm pacForm = new pacForm(tableCheckDolj.Rows[0][0].ToString());
                        pacForm.Show();
                    }
                    else if (tableCheckDolj.Rows[0][5].ToString() == "2")
                    {
                        this.Hide();
                        vrachForm vrachForm = new vrachForm(tableCheckDolj.Rows[0][0].ToString());
                        vrachForm.Show();
                    }
                    else if (tableCheckDolj.Rows[0][5].ToString() == "3")
                    {
                        this.Hide();
                        glavrachForm glavrachForm = new glavrachForm(tableCheckDolj.Rows[0][0].ToString());
                        glavrachForm.Show();
                    }
                    else if (tableCheckDolj.Rows[0][5].ToString() == "4")
                    {
                        this.Hide();
                        adminForm adminForm = new adminForm(tableCheckDolj.Rows[0][0].ToString());
                        adminForm.Show();
                    }
                    else if (tableCheckDolj.Rows[0][5].ToString() == "5")
                    {
                        this.Hide();
                        operForm operForm = new operForm(tableCheckDolj.Rows[0][0].ToString());
                        operForm.Show();
                    }
                }
                else
                {
                    this.Hide();
                    dopInfo dopInfo = new dopInfo(tableCheckDolj.Rows[0][0].ToString());
                    dopInfo.Show();
                }

            }
            else
                MessageBox.Show("Неверные данные");
            db.closeConnection(dbc);
        }

        private void regLabel_Click(object sender, EventArgs e)
        {
            this.Hide();
            regForm regForm = new regForm();
            regForm.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Hide();
            regVrach regVrach = new regVrach();
            regVrach.Show();
        }
    }
}
