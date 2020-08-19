using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MusicPlayerApp
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();
            txtFilePath.Text = dlg.FileName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFile(txtFilePath.Text);
            MessageBox.Show("Saved");
        }

        private void SaveFile(string filePath)
        {
            using (Stream stream = File.OpenRead(filePath))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                var fi = new FileInfo(filePath);
                string name = fi.Name;

                string query = "INSERT INTO Songs(Data,FileName)VALUES(@data,@name)";

                using (SqlConnection cn = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = name;
                    cmd.Parameters.Add("@data", SqlDbType.VarBinary).Value = buffer;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Database=MusicPlayer;Integrated Security=true;");
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (SqlConnection cn = GetConnection())
            {
                string query = "SELECT FileName FROM Songs";
                SqlDataAdapter adp = new SqlDataAdapter(query, cn);
                DataTable dt = new DataTable();
                adp.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dgvSongs.DataSource = dt;
                }
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            var selectedRow = dgvSongs.SelectedRows;
            foreach (var row in selectedRow)
            {
                int id = (int)((DataGridViewRow)row).Cells[0].Value;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = dgvSongs.DataSource;
            bs.Filter = "FileName like '%" + searchtextBox.Text + "%'";
            dgvSongs.DataSource = bs;
        }

        //private void OpenFile(int id)
        //{
        //    using (SqlConnection cn = GetConnection())
        //    {
        //        string query = "SELECT Data,FileName FROM Songs WHERE ID=@id";
        //        SqlCommand cmd = new SqlCommand(query cn);
        //        cn.Open();
        //        var reader = cmd.ExecuteReader();
        //        if(reader.Read())
        //        {
        //            var name = reader["FileName"].ToString();
        //            var data = (byte[])reader["data"];
        //            File.WriteAllBytes(data);
        //        }
        //    }
        //}
    }
}
