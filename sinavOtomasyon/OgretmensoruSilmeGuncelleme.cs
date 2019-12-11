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
    public partial class OgretmensoruSilmeGuncelleme : Form
    {
        public OgretmensoruSilmeGuncelleme()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-RE244GE;Initial Catalog=sinav;Integrated Security=True");

        public void sorudatagridListeleme()
        {
            dataGridView1.AllowUserToAddRows = false;//kendiliğinden eklenen satunu sildi
            dataGridView1.RowHeadersVisible = false;//satır başlığımı gizledik
            this.dataGridView1.AllowUserToResizeColumns = false;//sütunları tekrar boyutlandırmayı engelledik
            this.dataGridView1.AllowUserToResizeRows = false;//satırları tekrar boyutlandırmayı engelledik
            dataGridView1.ReadOnly = true;//hücrelere değer girilmesini engelledik.
            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("select soruAdi as 'SORU ADI' from soru", baglanti);
                SqlDataAdapter adaptor = new SqlDataAdapter(komut);
                DataSet datasinav = new DataSet();
                adaptor.Fill(datasinav);
                dataGridView1.DataSource = datasinav.Tables[0];
                baglanti.Close();
            }
            catch
            {
                MessageBox.Show("HATA");
            }
        }

        private void OgretmensoruSilme_Load(object sender, EventArgs e)
        {
            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
            (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);//Giriş formunu ortaladı
            sorudatagridListeleme();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {            
            int Secilendeger = dataGridView1.SelectedCells[0].RowIndex;
            textBox2.Text = dataGridView1.Rows[Secilendeger].Cells[0].Value.ToString();
        }

        private void button3_Click(object sender, EventArgs e)//silme
        {
            DialogResult uyari;
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            string soruADi= dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            // MessageBox.Show(secilen3.ToString());           
            if (secilen>=0 && textBox2.Text!="")
            {
                uyari = MessageBox.Show(this, " Soruyu silmek istiyor musunuz?", "SİLME UYARISI", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (uyari == DialogResult.Yes)
                {
                    baglanti.Open();                   
                    string sql = "DELETE FROM soru WHERE soruId=(select soruId from soru where soruAdi='" + soruADi + "')";
                    SqlCommand komut = new SqlCommand(sql, baglanti);                   
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Soru silindi");
                    baglanti.Close();
                    sorudatagridListeleme();
                }


                //MessageBox.Show(dataGridView1.Rows[secilen].Cells[0].Value.ToString());
            }
            else
            {
                MessageBox.Show("Lütfen silinecek Kaydı seçiniz");
            }



            



        }

        private void button1_Click(object sender, EventArgs e)//güncelleme
        {

            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            string soruADi = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            // MessageBox.Show(secilen3.ToString());           
            if (secilen >= 0 && textBox2.Text != "")
            {               
                baglanti.Open();
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "update soru set soruAdi=@soru where soruAdi='" + soruADi + "' ";
                komut.Parameters.AddWithValue("@soru", textBox2.Text);
                komut.ExecuteNonQuery();
                komut.Dispose();
                MessageBox.Show("kayıt güncellendi.");
                baglanti.Close();
                sorudatagridListeleme();

            }
            else
            {
                MessageBox.Show("Lütfen güncellenecek Kaydı seçiniz");
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
