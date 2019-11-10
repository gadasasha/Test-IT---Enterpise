using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestAppIT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadDataAll();
        }

        string connectionString = @"Data Source=GADA-HOME\SQLEXPRESS;Initial Catalog=TestDB_001;Integrated Security=True";
        string queryAll = "SELECT * FROM [Shop]";

        private void LoadDataAll()
        {

            SqlConnection myConnection = new SqlConnection(connectionString);

            myConnection.Open();

            SqlCommand command = new SqlCommand(queryAll, myConnection);

            SqlDataReader reader = command.ExecuteReader();

            List<string[]> data = new List<string[]>();

            int index = reader.FieldCount;

            while (reader.Read())
            {
                data.Add(new string[index]);

                for (int i = 0; i < index; i++)
                {
                    data[data.Count-1][i] = reader[i].ToString();
                }

            }

            reader.Close();

            myConnection.Close();

            foreach (string[] s in data)
            {
                dataGridView1.Rows.Add(s);
            }

            Column1.HeaderText = "Id";
            Column1.Visible = false;

            Column2.HeaderText = "Дата";
            Column2.Visible = true;

            Column3.HeaderText = "Организация";
            Column3.Visible = true;

            Column4.HeaderText = "Город";
            Column4.Visible = true;

            Column5.HeaderText = "Страна";
            Column5.Visible = true;

            Column6.HeaderText = "Менеджер";
            Column6.Visible = true;

            Column7.HeaderText = "Количество";
            Column7.Visible = true;

            Column8.HeaderText = "Сумма";
            Column8.Visible = true;


        }

        private void LoadDataStructured()
        {

            SqlConnection myConnection = new SqlConnection(connectionString);

            myConnection.Open();

            string queryStructured = $"SELECT {getStringFilterForSelect()} SUM(Quantity), SUM(Summ) FROM [Shop] GROUP BY  {getStringFilterForGroupBy().Substring(0, getStringFilterForGroupBy().Length - 2)}";

            SqlCommand command = new SqlCommand(queryStructured, myConnection);

            SqlDataReader reader = command.ExecuteReader();

            List<string[]> data = new List<string[]>();

            int index = reader.FieldCount;

            while (reader.Read())
            {
                data.Add(new string[index]);

                for (int i = 0; i < index; i++)
                {
                    data[data.Count - 1][i] = reader[i].ToString();
                }

            }
            
            reader.Close();

            myConnection.Close();

            foreach (string[] s in data)
            {
                dataGridView1.Rows.Add(s);
            }


            List<DataGridViewTextBoxColumn> ColumnList = new List<DataGridViewTextBoxColumn>
            {
                Column1,
                Column2,
                Column3,
                Column4,
                Column5,
                Column6,
                Column7,
                Column8
            };

            for (int i = 0; i < ColumnList.Count; i++)
            {
                ColumnList[i].Visible = false;
            }
            for (int i = 0; i < index; i++)
            {
                ColumnList[i].Visible = true;
            }

            for (int i = 0; i < index-2; i++)
            {
                ColumnList[i].HeaderText = CheckBoxList[i].Text;

            }
            ColumnList[index-2].HeaderText = "Количество";
            ColumnList[index-1].HeaderText = "Сумма";

        }

        private static List<string> groupingList = new List<string>();

        private static void SetFilter(CheckBox checkBox1,
                                      CheckBox checkBox2, 
                                      CheckBox checkBox3,
                                      CheckBox checkBox4,
                                      CheckBox checkBox5)
        {
            groupingList.Clear();

            if (checkBox1.Checked )
            {
                groupingList.Add("Date, ");
            }
            if (checkBox2.Checked)
            {
                groupingList.Add("OrganizationName, ");
            }
            if (checkBox3.Checked)
            {
                groupingList.Add("City, ");
            }
            if (checkBox4.Checked)
            {
                groupingList.Add("Country, ");
            }
            if (checkBox5.Checked)
            {
                groupingList.Add("ManagerName, ");
            }
            if (!checkBox1.Checked && !checkBox2.Checked
                && !checkBox3.Checked && !checkBox4.Checked && !checkBox5.Checked)
            {
                groupingList.Add("  ");
            }
        }

        private static string getStringFilterForSelect()
        {
            string s = "";
            foreach (string g in groupingList)
            {
               s += g;
            }

            return s;
        }
        private static string getStringFilterForGroupBy()
        {
            string s = "";
            foreach (string g in groupingList)
            {
                s =  g + s;
            }

            return s;
        }

        private static List<CheckBox> CheckBoxList = new List<CheckBox>();
        private static void ChekBoxColector(CheckBox checkBox1,
                                              CheckBox checkBox2,
                                              CheckBox checkBox3,
                                              CheckBox checkBox4,
                                              CheckBox checkBox5)
        {
            CheckBoxList.Clear();

            if (checkBox1.Checked)
            {
                CheckBoxList.Add(checkBox1);
            }
            if (checkBox2.Checked)
            {
                CheckBoxList.Add(checkBox2);
            }
            if (checkBox3.Checked)
            {
                CheckBoxList.Add(checkBox3);
            }
            if (checkBox4.Checked)
            {
                CheckBoxList.Add(checkBox4);
            }
            if (checkBox5.Checked)
            {
                CheckBoxList.Add(checkBox5);
            }

        }



        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            SetFilter(checkBox1, checkBox2, checkBox3, checkBox4, checkBox5);

            ChekBoxColector(checkBox1, checkBox2, checkBox3, checkBox4, checkBox5);

            if (checkBox1.Checked || checkBox2.Checked
                || checkBox3.Checked || checkBox4.Checked || checkBox5.Checked)
            {
                LoadDataStructured();
            }
            else
            {
                LoadDataAll();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            LoadDataAll();

            ChekBoxColector(checkBox1, checkBox2, checkBox3, checkBox4, checkBox5);

            for (int i = 0; i < CheckBoxList.Count; i++)

            {

                CheckBoxList[i].CheckState = CheckState.Unchecked;

            }

        }




    }
}
