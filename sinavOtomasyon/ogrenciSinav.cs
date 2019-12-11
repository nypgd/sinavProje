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

namespace sinavOtomasyon
{
    public partial class ogrenciSinav : Form
    {
        public ogrenciSinav()
        {
            InitializeComponent();
        }

        int i = 0;
        int sorusayaci = 1;
        int satirSayisi, satirSayisiOgrenci;
        string bol;
        int guncellemeSayac=0;
        public bool durum = false;
        string dogruSecenek,ogrenciSecenek;
        string dogruAdet,yanlisAdet;
        ArrayList adet = new ArrayList();
        ArrayList dogruSecenekDizi = new ArrayList();

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-RE244GE;Initial Catalog=sinav;Integrated Security=True");


        public void degerleriDiziyeatma()
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from soru", baglanti);
            SqlDataReader dr1 = komut.ExecuteReader();
            // MessageBox.Show(dr1.Read().ToString());
            while (dr1.Read())
            {
                adet.Add(dr1[0]);

            }


            baglanti.Close();

        }



        public void SatirSayisiOgrenme()
        {

            baglanti.Open();
            SqlCommand satir = new SqlCommand("select count(*) from soru", baglanti);
            SqlDataReader dr2 = satir.ExecuteReader();
            while (dr2.Read())
            {
                satirSayisi = Convert.ToInt32(dr2[0]);
            }
            baglanti.Close();
        }

        public int SatirSayisiOgrenmeOgrenci()
        {

            baglanti.Open();
            SqlCommand satir = new SqlCommand("select count(*) from ogrenci", baglanti);
            SqlDataReader dr2 = satir.ExecuteReader();
            while (dr2.Read())
            {
                satirSayisiOgrenci = Convert.ToInt32(dr2[0]);
            }
            baglanti.Close();
            return satirSayisiOgrenci;
        }



        public string dogruAdetMiktari()
        {
            baglanti.Open();
            SqlCommand miktar = new SqlCommand("select count(*) from ogrenci where dogruMu=1 and quizId=(select quizId from quiz where quizAdi='"+sinavadi.Text+"' ) ", baglanti);
            SqlDataReader dr3 = miktar.ExecuteReader();
            while (dr3.Read())
            {
                dogruAdet = dr3[0].ToString();
            }
            baglanti.Close();
            return dogruAdet;
        }


        public string yanlisAdetMiktari()
        {
            baglanti.Open();
            SqlCommand miktar = new SqlCommand("select count(*) from ogrenci where dogruMu=0 and quizId=(select quizId from quiz where quizAdi='" + sinavadi.Text + "' )", baglanti);
            SqlDataReader dr3 = miktar.ExecuteReader();
            while (dr3.Read())
            {
                yanlisAdet = dr3[0].ToString();
            }
            baglanti.Close();
            return yanlisAdet;

        }


        public void dogruMUBildi()
        {

            baglanti.Open();
            SqlCommand satir = new SqlCommand("select * from ogrenci", baglanti);
            SqlDataReader dr3 = satir.ExecuteReader();
            while (dr3.Read())
            {
                ogrenciSecenek = dr3[4].ToString();
            }
            baglanti.Close();
        }

        public void dogruMuKayit()
        {

            baglanti.Open();
            SqlCommand komut1 = new SqlCommand();
            komut1.Connection = baglanti;
            komut1.CommandText = "update ogrenci set dogruMu=1 where soruId=(select soruId from soru where soruAdi = '" + label1.Text + "') and quizId=(select quizId from quiz where quizAdi='"+sinavadi.Text+"')";
            komut1.ExecuteNonQuery();
            komut1.Dispose();
            baglanti.Close();
        }

        public void yanlisMiKayit()
        {

            baglanti.Open();
            SqlCommand komut1 = new SqlCommand();
            komut1.Connection = baglanti; 
            komut1.CommandText = "update ogrenci set dogruMu=0 where soruId=(select soruId from soru where soruAdi = '" + label1.Text + "') and quizId=(select quizId from quiz where quizAdi='" + sinavadi.Text + "')";
            komut1.ExecuteNonQuery();
            komut1.Dispose();
            baglanti.Close();
        }


        int k = 0;
        public void soruListele()
        {
            
            if (i < 0)
            {
                i = 0;
            } 


            if (i <= adet.Count && i>=0)
            {



                if (radioButton1.Checked == true)
                {                   
                    baglanti.Open();
                    SqlCommand komut1 = new SqlCommand();                   
                    komut1.Connection = baglanti;
                    komut1.CommandText = "insert into ogrenci(secenek,dersId,konuId,soruId,quizId) values('A',(select dersId from soru where soruAdi='" + label1.Text + "'),(select konuId from soru where soruAdi='" + label1.Text + "'),(select soruId from soru where soruAdi='" + label1.Text + "'),(select quizId from quiz where quizAdi='"+sinavadi.Text+"'))";
                    komut1.ExecuteNonQuery();
                    komut1.Dispose();
                    baglanti.Close();

                    dogruMUBildi();
                    
                    if (dogruSecenek == ogrenciSecenek)
                    {
                        //MessageBox.Show("Doğru bildi");
                        dogruMuKayit();
                    }
                    else
                    {
                        // MessageBox.Show("Yanlış bildi");
                        yanlisMiKayit();
                    }

                    

                }
                else if (radioButton2.Checked == true)
                {
                    baglanti.Open();
                    SqlCommand komut1 = new SqlCommand();
                    komut1.Connection = baglanti;
                    komut1.CommandText = "insert into ogrenci(secenek,dersId,konuId,soruId,quizId) values('B',(select dersId from soru where soruAdi='" + label1.Text + "'),(select konuId from soru where soruAdi='" + label1.Text + "'),(select soruId from soru where soruAdi='" + label1.Text + "'),(select quizId from quiz where quizAdi='" + sinavadi.Text + "'))";
                    komut1.ExecuteNonQuery();
                    komut1.Dispose();
                    baglanti.Close();


                    dogruMUBildi();

                    if (dogruSecenek == ogrenciSecenek)
                    {
                        dogruMuKayit();
                       // MessageBox.Show("Doğru bildi");
                    }
                    else
                    {
                        yanlisMiKayit();
                       // MessageBox.Show("Yanlış bildi");
                    }

                }
                else if (radioButton3.Checked == true)
                {
                    baglanti.Open();
                    SqlCommand komut1 = new SqlCommand();
                    komut1.Connection = baglanti;
                    komut1.CommandText = "insert into ogrenci(secenek,dersId,konuId,soruId,quizId) values('C',(select dersId from soru where soruAdi='" + label1.Text + "'),(select konuId from soru where soruAdi='" + label1.Text + "'),(select soruId from soru where soruAdi='" + label1.Text + "'),(select quizId from quiz where quizAdi='" + sinavadi.Text + "'))";
                    komut1.ExecuteNonQuery();
                    komut1.Dispose();
                    baglanti.Close();

                    dogruMUBildi();

                    if (dogruSecenek == ogrenciSecenek)
                    {
                        dogruMuKayit();
                        //MessageBox.Show("Doğru bildi");
                    }
                    else
                    {
                        yanlisMiKayit();
                       // MessageBox.Show("Yanlış bildi");
                    }

                }
                else if (radioButton4.Checked == true)
                {
                    baglanti.Open();
                    SqlCommand komut1 = new SqlCommand();
                    komut1.Connection = baglanti;                    
                    komut1.CommandText = "insert into ogrenci(secenek,dersId,konuId,soruId,quizId) values('D',(select dersId from soru where soruAdi='" + label1.Text + "'),(select konuId from soru where soruAdi='" + label1.Text + "'),(select soruId from soru where soruAdi='" + label1.Text + "'),(select quizId from quiz where quizAdi='" + sinavadi.Text + "'))";
                    komut1.ExecuteNonQuery();
                    komut1.Dispose();
                    baglanti.Close();

                    dogruMUBildi();

                    if (dogruSecenek == ogrenciSecenek)
                    {
                        dogruMuKayit();
                        //MessageBox.Show("Doğru bildi");
                    }
                    else
                    {
                        yanlisMiKayit();
                        // MessageBox.Show("Yanlış bildi");
                    }

                }




                if (i < adet.Count)
                {
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("select * from soru where soruId='" + adet[i] + "'", baglanti);                                      
                    SqlDataReader dr1 = komut.ExecuteReader();
                    //MessageBox.Show(adet[i].ToString()); 
                    while (dr1.Read())
                    {

                        label1.Text = dr1["soruAdi"].ToString();
                        label2.Text = dr1["secenekA"].ToString();
                        label3.Text = dr1["secenekB"].ToString();
                        label4.Text = dr1["secenekC"].ToString();
                        label5.Text = dr1["secenekD"].ToString();
                        //label7.Text = dr1["soruId"].ToString();
                        label7.Text = (sorusayaci).ToString();
                        label9.Text = satirSayisi.ToString();
                        dogruSecenek = dr1[2].ToString();
                       // dogruSecenekDizi.Add(dogruSecenek);


                    }
                    baglanti.Close();

                }else
                {
                    deger = 0;
                    zaman.Text = deger.ToString();
                    timer1.Stop();
                    MessageBox.Show("sınav Bitti");                    
                    ekranDondurma();


                }


            }


        }


        public void sinavAdikaydet()
        {          
            baglanti.Open();
            SqlCommand komut1 = new SqlCommand();
            komut1.Connection = baglanti;
            komut1.CommandText = "insert into quiz(quizAdi,tarih) values('"+sinavadi.Text+ "',CONVERT(DATE, GETDATE(), 104))";
            komut1.ExecuteNonQuery();
            komut1.Dispose();
            baglanti.Close();

        }


        public void basariEkle(int basari)
        {
            baglanti.Open();
            SqlCommand komut1 = new SqlCommand();
            komut1.Connection = baglanti;
            komut1.CommandText = "update quiz set basari='"+basari+"' where  quizAdi='"+sinavadi.Text+"'";
            komut1.ExecuteNonQuery();
            komut1.Dispose();
            baglanti.Close();
        }

     


        int BasariYuzde;
        public void ekranDondurma()
        {

            label6.Visible = false;
            label7.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label10.Visible = true;

            label13.Visible = true;

            label11.Visible = true;
            yanlisSayisi.Visible = true;

            label14.Visible = true;
            puan.Visible = true;

            label16.Visible = true;
            basariYuzde.Visible = true;

            dogruSayisi.Visible = true;
            panel1.Enabled = true;


            radioButton1.Visible = false;
            radioButton2.Visible = false;
            radioButton3.Visible = false;
            radioButton4.Visible = false;

            button8.Visible = false;

            if (SatirSayisiOgrenmeOgrenci() != 0)
            {
                dogruSayisi.Text = dogruAdetMiktari();
                yanlisSayisi.Text = yanlisAdetMiktari();
                puan.Text = (int.Parse(dogruAdetMiktari()) * 2).ToString();
                BasariYuzde = (int.Parse(dogruAdetMiktari()) * 100 / (int.Parse(dogruAdetMiktari()) + int.Parse(yanlisAdetMiktari())));
                basariYuzde.Text = BasariYuzde.ToString();
                basariEkle(BasariYuzde);
            }
            else
            {
                dogruSayisi.Text = "0";
                yanlisSayisi.Text = "0";
                puan.Text = "0";
                basariYuzde.Text = "0";

            }


        }
       
        public void ogrenci_Load(object sender, EventArgs e)
        {
                   
            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                         (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);//Giriş formunu ortaladı

            panel1.Enabled = false;
            degerleriDiziyeatma();
            SatirSayisiOgrenme();
            label10.Visible = false;
            dogruSayisi.Visible = false;

            label11.Visible = false;
            yanlisSayisi.Visible = false;

            label14.Visible = false;
            puan.Visible = false;

            label16.Visible = false;
            basariYuzde.Visible = false;

            label13.Visible = false;



        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void radioButtonFalse()
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked==true || radioButton2.Checked==true || radioButton3.Checked==true || radioButton4.Checked == true)
            {
                i += 1;
                sorusayaci++;
                soruListele();
                radioButtonFalse();
            }else
            {
                MessageBox.Show("Lütfen bir seçeneği işaretleyiniz");
            }


        }

        string tarih;
        public string tarihGetir()
        {

            baglanti.Open();
            SqlCommand satir = new SqlCommand("select * from quiz", baglanti);
            SqlDataReader dr3 = satir.ExecuteReader();
            while (dr3.Read())
            {
                tarih = dr3[3].ToString();
            }
            baglanti.Close();
            return tarih;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(tarihGetir());
            if (tarihGetir()!= null)
            {
                string[] tarih = tarihGetir().Split(' ');
                MessageBox.Show(tarih[0].ToString());
                MessageBox.Show(DateTime.Today.ToShortDateString());
                if (tarih[0].ToString() == DateTime.Today.ToShortDateString())
                {
                    MessageBox.Show("Sınava giriş yapabilirsiniz");
                    if (sinavadi.Text != "")
                    {
                        sinavAdikaydet();
                        timer1.Start();
                        soruListele();
                        panel1.Enabled = true;
                        button3.Enabled = false;
                        sinavadi.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("Lütfen sınav adını giriniz");
                    }

                }
                else
                {
                    MessageBox.Show("Süre henüz dolmadı");
                }
            }
            else
            {
                //MessageBox.Show("sınava kayıt oluyorsunuz");
                if (sinavadi.Text != "")
                {
                    sinavAdikaydet();
                    timer1.Start();
                    soruListele();
                    panel1.Enabled = true;
                    button3.Enabled = false;
                    sinavadi.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Lütfen sınav adını giriniz");
                }

            }


        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }
        int deger;
        private void timer1_Tick(object sender, EventArgs e)
        {
            deger = int.Parse(zaman.Text);            
            deger--;
            zaman.Text = deger.ToString();

            if (deger == 0)
            {
                timer1.Stop();
                MessageBox.Show("Süre Bitti");                             
                ekranDondurma();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
