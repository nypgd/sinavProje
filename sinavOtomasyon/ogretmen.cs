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
    public partial class ogretmen : Form
    {
        public ogretmen()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-RE244GE;Initial Catalog=sinav;Integrated Security=True");

        public void comboDersListeleme()
        {
            comboBox1.Items.Clear();
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("select dersAdi from ders", baglanti);
                SqlDataReader dr = komut.ExecuteReader();

                while (dr.Read())
                {
                    comboBox1.Items.Add(dr[0]);
                }
                comboBox1.SelectedIndex = 0;
                baglanti.Close();
            }

        }

        public void comboKonuListeleme(string dersAdi)
        {
            comboBox2.Items.Clear();
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("select konuAdi from konu,ders where dersAdi='" + dersAdi + "' and ders.dersId=konu.dersId ", baglanti);
                SqlDataReader dr = komut.ExecuteReader();

                while (dr.Read())
                {
                    comboBox2.Items.Add(dr[0]);
                }
                                
                if (comboBox2.Items.Count > 0)
                {
                    comboBox2.SelectedItem = comboBox2.Items[0];
                }
                else
                {
                    comboBox2.Text = "";
                }
                baglanti.Close();
            }
        }
        bool radioButtonDurum = false;
        string dogrusecenek;
        private void button3_Click(object sender, EventArgs e)
        {
            //radio butonlardan yalnızca birinin seçilmesi kontrol edilmiştir
            if(radioButton1.Checked == true)
            {
                radioButtonDurum = true;
                dogrusecenek = "A";
            }else if (radioButton2.Checked == true)
            {
                radioButtonDurum = true;
                dogrusecenek = "B";
            }
            else if (radioButton3.Checked == true)
            {
                radioButtonDurum = true;
                dogrusecenek = "C";
            }
            else if (radioButton4.Checked == true)
            {
                radioButtonDurum = true;
                dogrusecenek = "D";
            }



            if (comboBox1.Text == "" || comboBox2.Text == "" || textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || radioButtonDurum==false)
            {
                MessageBox.Show("Lütfen bilgileri eksiksiz doldurun");
            }
            else 
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "insert into soru(dersId,dogruSecenekAdi,secenekA,secenekB,secenekC,secenekD,konuId,soruAdi) values((select dersId from ders where dersAdi='" + comboBox1.Text + "'),'"+ dogrusecenek + "','"+textBox2.Text+"','"+textBox3.Text+"','"+textBox4.Text+"','"+textBox5.Text+ "',(select konuId from konu where konuAdi = '" + comboBox2.Text + "'),'"+textBox1.Text+"')";
                komut.ExecuteNonQuery();
                komut.Dispose();
                MessageBox.Show("kayıt eklendi.");
                baglanti.Close();



            }
        }

        private void ogretmen_Load(object sender, EventArgs e)
        {
            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                       (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);//Giriş formunu ortaladı
            comboDersListeleme();
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;



        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            string secilen_Ders= comboBox1.SelectedItem.ToString();
            comboKonuListeleme(secilen_Ders);
        }
        string resimYolu;
        private void button2_Click(object sender, EventArgs e)
        {
            ogrenciSinavAnaliz rapor = new ogrenciSinavAnaliz();
            rapor.Show();
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Font = new Font(textBox1.Font, textBox1.Font.Style ^ FontStyle.Bold);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Font = new Font(textBox1.Font, textBox1.Font.Style ^ FontStyle.Italic);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.Font = new Font(textBox1.Font, textBox1.Font.Style ^ FontStyle.Underline);
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            //siyah beyaz mavi kırmızı sarı yeşil mor pembe kahve
            switch (comboBox5.Text)//Combodan seçilen renge göre metin kutusu  o renge boyanıyor
            {
                case "Siyah":
                    {
                        textBox1.ForeColor = Color.Black;
                        break;
                    }
                case "Beyaz":
                    {
                        textBox1.ForeColor = Color.White;
                        break;
                    }

                case "Mavi":
                    {
                        textBox1.ForeColor = Color.Blue;
                        break;
                    }
                case "Kırmızı":
                    {
                        textBox1.ForeColor = Color.Red;
                        break;
                    }

                case "Sarı":
                    {
                        textBox1.ForeColor = Color.Yellow;
                        break;
                    }

                case "Yeşil":
                    {
                        textBox1.ForeColor = Color.Green;
                        break;
                    }
                case "Mor":
                    {
                        textBox1.ForeColor = Color.Purple;
                        break;
                    }

                case "Pembe":
                    {
                        textBox1.ForeColor = Color.Pink;
                        break;
                    }

                case "Kahverengi":
                    {
                        textBox1.ForeColor = Color.Brown;
                        break;
                    }

            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
    


        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string yaziTipi = comboBox3.Text;            
            textBox1.Font = new Font(yaziTipi, 16);
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OgretmensoruSilmeGuncelleme sil = new OgretmensoruSilmeGuncelleme();
            sil.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SinavSil sil = new SinavSil();
            sil.Show();

        }
    }
}
