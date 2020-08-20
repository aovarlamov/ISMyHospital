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
    public partial class showSoob : Form
    {
        public showSoob(string id)
        {
            InitializeComponent();
            label3.Text = id;
            DB db = new DB();
            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command1 = new MySqlCommand("SELECT * FROM Soobshen as A LEFT JOIN Personal as B ON(A.idpers1 = B.id) LEFT JOIN Personal as C ON(A.idpers2 = C.id) WHERE A.id = @id", dbc);
            command1.Parameters.Add("@id", MySqlDbType.VarChar).Value = label3.Text;
            adapter.SelectCommand = command1;
            adapter.Fill(table);
            textBox5.Text = table.Rows[0][3].ToString();
            textBox1.Text = table.Rows[0][4].ToString();
            label8.Text = table.Rows[0][7].ToString();
            label7.Text = table.Rows[0][13].ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
