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

namespace sinavOtomasyon
{
    public partial class mudur : Form
    {
        public mudur()
        {
            InitializeComponent();
        }



        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-RE244GE;Initial Catalog=sinav;Integrated Security=True");
        public void konudatagridListeleme()
        {


            baglanti.Open();
            SqlCommand komut = new SqlCommand("select dersAdi as 'DERS ADI',konuAdi as 'KONU ADI' from ders,konu where ders.dersId=konu.dersId", baglanti);
            SqlDataAdapter adaptor = new SqlDataAdapter(komut);
            DataSet datasinav = new DataSet();
            adaptor.Fill(datasinav);
            dataGridView2.DataSource = datasinav.Tables[0];
            baglanti.Close();

        }

        public void dersdatagridListeleme()
        {


            baglanti.Open();
            SqlCommand komut = new SqlCommand("select dersAdi as 'DERS ADI' from ders", baglanti);
            SqlDataAdapter adaptor = new SqlDataAdapter(komut);
            DataSet datasinav = new DataSet();
            adaptor.Fill(datasinav);
            dataGridView1.DataSource = datasinav.Tables[0];
            baglanti.Close();


        }

        public void comboListeleme()
        {
            comboBox1.Items.Clear();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from ders", baglanti);
            SqlDataReader dr = komut.ExecuteReader();

            while (dr.Read())
            {
                comboBox1.Items.Add(dr[1]);
            }
            if (!string.IsNullOrEmpty(comboBox1.Text))
            {
                comboBox1.SelectedIndex = 0;
            }
            
            baglanti.Close();

        }

        private void mudur_Load(object sender, EventArgs e)
        {
            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                         (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);//Giriş formunu ortaladı

            dataGridView1.AllowUserToAddRows = false;//kendiliğinden eklenen satunu sildi
            dataGridView1.RowHeadersVisible = false;//satır başlığımı gizledik
            this.dataGridView1.AllowUserToResizeColumns = false;//sütunları tekrar boyutlandırmayı engelledik
            this.dataGridView1.AllowUserToResizeRows = false;//satırları tekrar boyutlandırmayı engelledik
            dataGridView1.ReadOnly = true;//hücrelere değer girilmesini engelledik.

            dataGridView2.AllowUserToAddRows = false;//kendiliğinden eklenen satunu sildi
            dataGridView2.RowHeadersVisible = false;//satır başlığımı gizledik
            this.dataGridView2.AllowUserToResizeColumns = false;//sütunları tekrar boyutlandırmayı engelledik
            this.dataGridView2.AllowUserToResizeRows = false;//satırları tekrar boyutlandırmayı engelledik
            dataGridView2.ReadOnly = true;//hücrelere değer girilmesini engelledik.
            dersdatagridListeleme();
            comboListeleme();
            konudatagridListeleme();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {

                if (textBox1.Text != "")
                {
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand();
                    komut.Connection = baglanti;
                    komut.CommandText = "INSERT INTO ders(dersAdi) VALUES(@param1)";
                    komut.Parameters.AddWithValue("@param1", textBox1.Text);
                    komut.ExecuteNonQuery();
                    komut.Dispose();
                    MessageBox.Show("kayıt eklendi.");
                    baglanti.Close();
                    dersdatagridListeleme();
                    comboListeleme();
                }
                else
                {
                    MessageBox.Show("Lütfen eklenecek dersi giriniz.");
                }



            }
            catch
            {
                MessageBox.Show("HATA");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            textBox1.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult uyari1;
            int secilen2 = dataGridView1.SelectedCells[0].RowIndex;
            if (secilen2 >= 0)
            {
                string dersadi = dataGridView1.CurrentRow.Cells["DERS ADI"].Value.ToString();
                uyari1 = MessageBox.Show(this, dersadi + " Dersinin Kaydını Silmek istiyor musunuz?", "SİLME UYARISI", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (uyari1 == DialogResult.Yes)
                {

                    baglanti.Open();
                    string sql = "DELETE FROM ders WHERE dersAdi=@numara";
                    SqlCommand komut = new SqlCommand(sql, baglanti);
                    komut.Parameters.AddWithValue("@numara", dersadi);
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    dersdatagridListeleme();
                    konudatagridListeleme();
                    comboListeleme();

                }
            }
            else
            {
                MessageBox.Show("Lütfen silinecek Kaydı seçiniz");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int secilen3 = dataGridView1.SelectedCells[0].RowIndex;
            if (secilen3 >= 0 && textBox1.Text != "")
            {
                int secilen = dataGridView1.SelectedCells[0].RowIndex;
                string secilidata = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
                baglanti.Open();
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "update ders set dersAdi=@ders where dersAdi='" + secilidata + "' ";
                komut.Parameters.AddWithValue("@ders", textBox1.Text);
                komut.ExecuteNonQuery();
                komut.Dispose();
                MessageBox.Show("kayıt güncellendi.");
                baglanti.Close();
                comboListeleme();
                dersdatagridListeleme();
                konudatagridListeleme();
            }
            else
            {
                MessageBox.Show("Lütfen güncellenecek Kaydı seçiniz");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView2.SelectedCells[1].RowIndex;
            textBox2.Text = dataGridView2.Rows[secilen].Cells[1].Value.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int secilen3 = dataGridView2.SelectedCells[0].RowIndex;
            if (secilen3 >= 0 && textBox2.Text != "")
            {
                int secilen = dataGridView2.SelectedCells[1].RowIndex;
                string secilidata = dataGridView2.Rows[secilen].Cells[1].Value.ToString();
                baglanti.Open();
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "update konu set konuAdi=@konu where konuAdi='" + secilidata + "' ";
                komut.Parameters.AddWithValue("@konu", textBox2.Text);
                komut.ExecuteNonQuery();
                komut.Dispose();
                MessageBox.Show("kayıt güncellendi.");
                baglanti.Close();
                comboListeleme();
                konudatagridListeleme();
            }
            else
            {
                MessageBox.Show("Lütfen güncellenecek Kaydı seçiniz");
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult uyari1;
            int secilen2 = dataGridView2.SelectedCells[0].RowIndex;
            if (secilen2 >= 0)
            {
                string konuadi = dataGridView2.CurrentRow.Cells["KONU ADI"].Value.ToString();
                uyari1 = MessageBox.Show(this, konuadi + " Dersinin Kaydını Silmek istiyor musunuz?", "SİLME UYARISI", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (uyari1 == DialogResult.Yes)
                {

                    baglanti.Open();
                    string sql = "DELETE FROM konu WHERE konuAdi=@konu";
                    SqlCommand komut = new SqlCommand(sql, baglanti);
                    komut.Parameters.AddWithValue("@konu", konuadi);
                    komut.ExecuteNonQuery();                    
                    baglanti.Close();
                    konudatagridListeleme();
                    comboListeleme();

                }
            }
            else
            {
                MessageBox.Show("Lütfen silinecek Kaydı seçiniz");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (comboBox1.SelectedIndex >= 0 && textBox2.Text!="")
                {
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand();
                    komut.Connection = baglanti;                 
                    komut.CommandText = "INSERT INTO konu(konuAdi,dersId) VALUES('"+textBox2.Text+"',(select dersId from ders where dersAdi='" + comboBox1.SelectedItem + "'))";                                  
                    komut.ExecuteNonQuery();
                    komut.Dispose();
                    MessageBox.Show("kayıt eklendi.");
                    baglanti.Close();
                    konudatagridListeleme();
                    comboListeleme();

                }
                else
                {
                    MessageBox.Show("Lütfen seçilen derse ait konuyu yazınız.");
                }

            }
            catch
            {
                MessageBox.Show("HATA");
            }
        }
    }
}
