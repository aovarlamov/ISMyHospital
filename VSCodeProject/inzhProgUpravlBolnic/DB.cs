using MySql.Data.MySqlClient;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace inzhProgUpravlBolnic
{
    class DB
    {
        MySqlConnection connection = new MySqlConnection("server=37.140.192.37; port=3306; username=u1059812_default; password=_z0UFXvq; database=u1059812_inzhproekt; charset=utf8");

        public void openConnection(MySqlConnection dbs)
        {
            if (dbs.State == System.Data.ConnectionState.Closed)
                dbs.Open();
        }
        public void closeConnection(MySqlConnection dbs)
        {
            if (dbs.State == System.Data.ConnectionState.Open)
                dbs.Close();
        }

        public MySqlConnection getConnection()
        {
            try
            {
                openConnection(connection);
                return connection;
            }
            catch
            {
                MessageBox.Show("Нет подключения к сети или проблемы на хостинге!");
                return null;
            }
        }
    }
}

