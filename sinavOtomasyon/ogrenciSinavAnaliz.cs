using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace sinavOtomasyon
{
    public partial class ogrenciSinavAnaliz : Form
    {
        public ogrenciSinavAnaliz()
        {
            InitializeComponent();
        }
        ArrayList konuAdi = new ArrayList();
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-RE244GE;Initial Catalog=sinav;Integrated Security=True");
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


        public void comboQuizListeleme()
        {
            comboBox1.Items.Clear();
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("select quizAdi from quiz", baglanti);
                SqlDataReader dr = komut.ExecuteReader();

                while (dr.Read())
                {
                    comboBox1.Items.Add(dr[0]);
                }                
                baglanti.Close();
            }

            if (!string.IsNullOrEmpty(comboBox1.Text))//veri tabanından hiçbir değer gelmiyorsa listeleme dedik
            {
                comboBox1.SelectedIndex = 0;
            }

        }
        int basari;

        public void konuListeleme(string ders)
        {
            this.chart1.Series["Başarı"].Points.Clear();
            comboBox1.Items.Clear();            
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select distinct konu.konuAdi from ogrenci,konu where ogrenci.dersId=(select dersId from ders where dersAdi='"+ders+ "') and ogrenci.quizId=(select quizId from quiz where quizAdi='"+ comboBox1.Text + "') and konu.konuId=ogrenci.konuId", baglanti);                  
            SqlDataReader dr = komut.ExecuteReader();

            while (dr.Read())
            {
                konuAdi.Add(dr[0]);
            }

            baglanti.Close();
            
            for (int k = 0; k < konuAdi.Count; k++)
            {
                if (soruAdet(k) != 0)
                {
                    basari = dogrusoruAdet(k) * 100 / soruAdet(k);
                     this.chart1.Series["Başarı"].Points.AddXY("doğru="+dogrusoruAdet(k)+"\nsoruMiktarı="+soruAdet(k)+"\n"+konuAdi[k], basari);
                   
                }

                // MessageBox.Show(soruAdet(k).ToString()+","+ dogrusoruAdet(k));
                
            }
            konuAdi.Clear();//çok önemli konu array ini her seferinde boşlatmamız gerekiyor çünkü üzerine ekliyor


        }

        int kacSoruvar;
        public int soruAdet(int n)
        {


            baglanti.Open();
            SqlCommand komut = new SqlCommand("select count(*) from ogrenci,konu where ogrenci.dersId=(select dersId from ders where dersAdi='" + dataGridView1.Rows[secilen].Cells[0].Value.ToString() + "') and ogrenci.quizId=(select quizId from quiz where quizAdi='" + comboBox1.Text + "') and konu.konuId=ogrenci.konuId and konuAdi='" + konuAdi[n] + "'", baglanti);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                kacSoruvar = int.Parse(dr[0].ToString());
            }
            baglanti.Close();
            return kacSoruvar;

        }

        int kacdogruvar;
        public int dogrusoruAdet(int n)
        {


            baglanti.Open();
            SqlCommand komut = new SqlCommand("select count(*) from ogrenci,konu where ogrenci.dersId=(select dersId from ders where dersAdi='" + dataGridView1.Rows[secilen].Cells[0].Value.ToString() + "') and ogrenci.quizId=(select quizId from quiz where quizAdi='" + comboBox1.Text + "') and konu.konuId=ogrenci.konuId and konuAdi='" + konuAdi[n] + "' and dogruMu=1", baglanti);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                kacdogruvar = int.Parse(dr[0].ToString());
            }
            baglanti.Close();
            return kacdogruvar;

        }

        int basariYuzde;
        public int basariYuzdesi(string sinav)
        {


            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from quiz where quizAdi='"+sinav+"' ", baglanti);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                basariYuzde = int.Parse(dr[2].ToString());
            }
            baglanti.Close();
            return basariYuzde;

        }


        private void ogrenciSinavAnaliz_Load(object sender, EventArgs e)
        {
            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
           (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);//Giriş formunu ortaladı

            dataGridView1.AllowUserToAddRows = false;//kendiliğinden eklenen satunu sildi
            dataGridView1.RowHeadersVisible = false;//satır başlığımı gizledik
            this.dataGridView1.AllowUserToResizeColumns = false;//sütunları tekrar boyutlandırmayı engelledik
            this.dataGridView1.AllowUserToResizeRows = false;//satırları tekrar boyutlandırmayı engelledik
            dataGridView1.ReadOnly = true;//hücrelere değer girilmesini engelledik.

            comboQuizListeleme();
            dersdatagridListeleme();


          

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {          

            this.chart1.Titles.Clear();           
            this.chart1.Titles.Add(dataGridView1.Rows[secilen].Cells[0].Value.ToString());

            if (this.chart1.Series["Başarı"].Points.Count() != 0)
            {
                this.chart1.Series["Başarı"].Points.Clear();
            }

            konuListeleme(dataGridView1.Rows[secilen].Cells[0].Value.ToString());
            comboQuizListeleme();




        }
        int secilen;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            secilen = dataGridView1.SelectedCells[0].RowIndex;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.chart1.Titles.Clear();
            this.chart1.Titles.Add("BÜTÜN SINAVLARIN BAŞARI DURUMU");

            if (this.chart1.Series["Başarı"].Points.Count() != 0)
            {
                this.chart1.Series["Başarı"].Points.Clear();
            }

            //basariYuzdesi(int.Parse(comboBox1.Items[j].ToString()))
            for (int j = 0; j < comboBox1.Items.Count; j++)
            {                
                this.chart1.Series["Başarı"].Points.AddXY(comboBox1.Items[j].ToString(), basariYuzdesi(comboBox1.Items[j].ToString()));
            }
       
            


        }
    }
}
