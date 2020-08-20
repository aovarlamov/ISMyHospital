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
    public partial class showZapis : Form
    {
        public showZapis(string id)
        {
            InitializeComponent();
            label4.Text = id;
            DB db = new DB();

            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlConnection dbc = db.getConnection();
            if (dbc == null)
            {
                return;
            }
            
            MySqlCommand commandUpdateTableZapis = new MySqlCommand("SELECT * FROM Zapis as A LEFT JOIN Spec as B ON(A.idspec = B.id) LEFT JOIN Vrach as C ON(A.idvrach = C.id) LEFT JOIN Statuszapis as D ON(A.idstatus = D.id) LEFT JOIN Personal as E ON(C.idpers = E.id) LEFT JOIN Personal as F ON(A.idpers = F.id) WHERE A.id = @uL", dbc);
            commandUpdateTableZapis.Parameters.Add("@uL", MySqlDbType.VarChar).Value = label4.Text;
            db.openConnection(dbc);
            adapter.SelectCommand = commandUpdateTableZapis;
            adapter.Fill(table);
            label7.Text = table.Rows[0][0].ToString();
            label8.Text = table.Rows[0][22].ToString();
            label9.Text = table.Rows[0][9].ToString();
            label10.Text = table.Rows[0][4].ToString();
            label6.Text = table.Rows[0][5].ToString();
            label5.Text = table.Rows[0][6].ToString() + " ч.";
            label14.Text = table.Rows[0][17].ToString();
            label17.Text = table.Rows[0][19].ToString();
            label16.Text = table.Rows[0][28].ToString();

            db.closeConnection(dbc);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
