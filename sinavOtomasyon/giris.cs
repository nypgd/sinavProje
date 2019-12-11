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
    public partial class giris : Form
    {
        public giris()
        {
            InitializeComponent();
        }
        string rolu;
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-RE244GE;Initial Catalog=sinav;Integrated Security=True");
        private void giris_Load(object sender, EventArgs e)
        {
            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                         (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);//Giriş formunu ortaladı
            rol.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //Kullanıcı girişi
                if (kullaniciAdi.Text == "" || sifre.Text == "" || rol.Text == "Rolünüzü Seçiniz")
                {
                    MessageBox.Show("Lütfen değerleri eksiksiz olarak giriniz");
                }
                else
                {                    
                    if (rol.Text == "Öğretmen")
                    {
                        rolu = "ogretmen".ToString();
                    }
                    else if (rol.Text == "Öğrenci")
                    {
                        rolu = "ogrenci";
                    }
                    else if (rol.Text == "Müdür")
                    {
                        rolu = "mudur";
                    }
                    baglanti.Open();
                    string sqlsorgu = "Select * from giris where kulAdi='" + kullaniciAdi.Text.Trim() + "' and sifre='" + sifre.Text.Trim() + "' and rol='" + rolu + "'";
                    SqlDataAdapter adaptor = new SqlDataAdapter(sqlsorgu, baglanti);
                    DataTable dtbl = new DataTable();
                    adaptor.Fill(dtbl);
                    baglanti.Close();

                    if (dtbl.Rows.Count > 0 && rol.Text == "Öğretmen")
                    {
                        ogretmen ogretmen = new ogretmen();
                        ogretmen.Show();
                        this.Hide();
                    }
                    else if (dtbl.Rows.Count > 0 && rol.Text == "Öğrenci")
                    {
                        ogrenciSinav ogrenci = new ogrenciSinav();
                        ogrenci.Show();
                        this.Hide();
                        
                    }
                    else if (dtbl.Rows.Count > 0 && rol.Text == "Müdür")
                    {
                        mudur mdr = new mudur();
                        mdr.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Hatalı kullanıcı adı veya şifre girdiniz");
                    }

                }

            }
            catch
            {
                MessageBox.Show("bağlatı hatası");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
