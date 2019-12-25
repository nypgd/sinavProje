using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
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
            
            secenekGizle();
            QUIZADET=quizIDvarmi();

        }

        int QUIZADET;
        int i = 1;
        int sorusayaci = 1;
        public bool durum = false;
        int sinavSuredeger;
        int SecenekSayac = 0;
        int QuizID;
        int quizIdAdet;
        double bildigiSorulacakSoruAdet=0, bilemedigisorulacakSoruAdet=0, geriyekalanSorulacak=0;
        int bildigiAdet, bilemedigiAdet;
        int dogruMuAdet = 0, yanlisMiadet = 0;
        int basari;
        double bildigiYuzde, bilemedigiYuzde, katsayi;
        ArrayList soruADi = new ArrayList();
        ArrayList DogruSecenek = new ArrayList();
        ArrayList soruID = new ArrayList();        
        ArrayList dersID = new ArrayList();
        ArrayList konuID = new ArrayList();
        ArrayList SecenekA = new ArrayList();
        ArrayList SecenekB = new ArrayList();
        ArrayList SecenekC = new ArrayList();
        ArrayList SecenekD = new ArrayList();
        ArrayList ogrisaretSecenek = new ArrayList();
        ArrayList dogruMu = new ArrayList();
        ArrayList quizID = new ArrayList();
        ArrayList sorular = new ArrayList();
        int sorulacakSoruSayisi = 20;
        int toplamSoruAdet = 0;


        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-RE244GE;Initial Catalog=sinav;Integrated Security=True");


        public void RasgeleSoruSecme(int sorulacakAdet)
        {
            
            baglanti.Open();
            SqlCommand komut = new SqlCommand("DECLARE @iNum INT SET @iNum = '" + sorulacakAdet + "' select top (@iNum) * from soru order by NEWID()", baglanti);
            SqlDataReader dr1 = komut.ExecuteReader();
          
            while (dr1.Read())
            {

                dersID.Add(dr1[1]);
                DogruSecenek.Add(dr1[2]);                                
                konuID.Add(dr1[7]);
                soruADi.Add(dr1[8]);
                soruID.Add(dr1[0]);
                SecenekA.Add(dr1[3]);
                SecenekB.Add(dr1[4]);
                SecenekC.Add(dr1[5]);
                SecenekD.Add(dr1[6]);
            }
            baglanti.Close();

        }


        public void SoruGetirmeBildigi(double bildigiAdet)
        {
                    
            baglanti.Open();            
            SqlCommand komut = new SqlCommand("DECLARE @iNum INT SET @iNum = '"+ bildigiAdet + "' select top (@iNum) * from soru,ogrenci where soru.soruId=ogrenci.soruId and ogrenci.dogruMu=1", baglanti);
            SqlDataReader dr1 = komut.ExecuteReader();
            
            while (dr1.Read())
            {
                
                dersID.Add(dr1["dersId"]);
                DogruSecenek.Add(dr1["dogruSecenekAdi"]);
                konuID.Add(dr1["konuId"]);
                soruADi.Add(dr1["soruAdi"]);
                soruID.Add(dr1["soruId"]);
                SecenekA.Add(dr1["secenekA"]);
                SecenekB.Add(dr1["secenekB"]);
                SecenekC.Add(dr1["secenekC"]);
                SecenekD.Add(dr1["secenekD"]);


            }
            baglanti.Close();
            
        }


        public void SoruGetirmeBilemedigi(double bilemedigiAdet)
        {
            
            baglanti.Open();
            SqlCommand komut = new SqlCommand("DECLARE @iNum INT SET @iNum = '" + bilemedigiAdet + "' select top (@iNum) * from soru,ogrenci where soru.soruId=ogrenci.soruId and ogrenci.quizId=1 and ogrenci.dogruMu=0", baglanti);
            SqlDataReader dr1 = komut.ExecuteReader();
            
            while (dr1.Read())
            {
                dersID.Add(dr1["dersId"]);
                DogruSecenek.Add(dr1["dogruSecenekAdi"]);
                konuID.Add(dr1["konuId"]);
                soruADi.Add(dr1["soruAdi"]);
                soruID.Add(dr1["soruId"]);
                SecenekA.Add(dr1["secenekA"]);
                SecenekB.Add(dr1["secenekB"]);
                SecenekC.Add(dr1["secenekC"]);
                SecenekD.Add(dr1["secenekD"]);

            }
            baglanti.Close();
            
        }

        public void GeriKalanSoruGetirme(int adet,double gelecekAdet)
        {

            baglanti.Open();
            SqlCommand komut2 = new SqlCommand();
            komut2.Connection = baglanti;
            komut2.CommandText = "delete from soruIdTutanTablo";
            komut2.ExecuteNonQuery();
            komut2.Dispose();
            baglanti.Close();


            for (int j = 0; j < adet;j++)
            {
                baglanti.Open();
                SqlCommand komut1 = new SqlCommand();
                komut1.Connection = baglanti;
                komut1.CommandText = "insert into soruIdTutanTablo(id) values('" + soruID[j] + "')";
                komut1.ExecuteNonQuery();
                komut1.Dispose();
                baglanti.Close();
            }



            baglanti.Open();
            SqlCommand komut = new SqlCommand("DECLARE @iNum INT SET @iNum = '" + gelecekAdet + "' select top (@iNum) dersId,dogruSecenekAdi,konuId,soruAdi,soruId,secenekA,secenekB,secenekC,secenekD from soru where soruId NOT IN(select id from soruIdTutanTablo) order by NEWID()", baglanti);
            SqlDataReader dr1 = komut.ExecuteReader();
            while (dr1.Read())
            {

                dersID.Add(dr1[0]);
                DogruSecenek.Add(dr1[1]);
                konuID.Add(dr1[2]);
                soruADi.Add(dr1[3]);
                soruID.Add(dr1[4]);
                SecenekA.Add(dr1[5]);
                SecenekB.Add(dr1[6]);
                SecenekC.Add(dr1[7]);
                SecenekD.Add(dr1[8]);

            }
            baglanti.Close();
        }



        

        public void Arraybosalt()
        {
            soruADi.Clear();
            DogruSecenek.Clear();
            soruID.Clear();
            dersID.Clear();
            SecenekA.Clear();
            SecenekB.Clear();
            SecenekC.Clear();
            SecenekD.Clear();
            ogrisaretSecenek.Clear();
            dogruMu.Clear();
            sorular.Clear();
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


        public int quizIDgetir()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from quiz where quizAdi='" + sinavadi.Text + "'", baglanti);
            SqlDataReader dr1 = komut.ExecuteReader();
            while (dr1.Read())
            {

                QuizID = int.Parse(dr1[0].ToString());

            }
            baglanti.Close();
            return QuizID;
        }
        
        public int quizIDvarmi()
        {
           
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select count(*) from quiz", baglanti);
            SqlDataReader dr1 = komut.ExecuteReader();
            while (dr1.Read())
            {

                quizIdAdet = int.Parse(dr1[0].ToString());

            }
            baglanti.Close();
            return quizIdAdet;
        }


        public int ToplamSoruAdet()
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand("select count(*) from soru", baglanti);
            SqlDataReader dr1 = komut.ExecuteReader();
            while (dr1.Read())
            {

                toplamSoruAdet = int.Parse(dr1[0].ToString());

            }
            baglanti.Close();
            return toplamSoruAdet;
        }


        public void secenekGizle()
        {
            radioButtonA.Visible = false;
            radioButtonB.Visible = false;
            radioButtonC.Visible = false;
            radioButtonD.Visible = false;
            A.Visible = false;
            B.Visible = false;
            C.Visible = false;
            D.Visible = false;
            soruDB.Visible = false;
            label6.Visible = false;
            sayac.Visible = false;

        }


        public void secenekGoster()
        {
            radioButtonA.Visible = true;
            radioButtonB.Visible = true;
            radioButtonC.Visible = true;
            radioButtonD.Visible = true;
            A.Visible = true;
            B.Visible = true;
            C.Visible = true;
            D.Visible = true;
            soruDB.Visible = true;
            label6.Visible = true;
            sayac.Visible = true;

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

        public void radioButonTemizle()
        {
            radioButtonA.Checked = false;
            radioButtonB.Checked = false;
            radioButtonC.Checked = false;
            radioButtonD.Checked = false;
        }


        
        public void ogrsoruEkle(int DERSID,int KONUID,int SORUID,string OGRSECENEK,byte DOGRUMU,int QUIZID)
        {
           
            baglanti.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = baglanti;
            cmd.CommandText = "INSERT INTO ogrenci(dersId,konuId,soruId,secenek,dogruMu,quizId) VALUES(@param1,@param2,@param3,@param4,@param5,@param6)";
            cmd.Parameters.AddWithValue("@param1", DERSID);
            cmd.Parameters.AddWithValue("@param2", KONUID);
            cmd.Parameters.AddWithValue("@param3", SORUID);
            cmd.Parameters.AddWithValue("@param4", OGRSECENEK);
            cmd.Parameters.AddWithValue("@param5", DOGRUMU);
            cmd.Parameters.AddWithValue("@param6", QUIZID);
            cmd.ExecuteNonQuery();
            baglanti.Close();

        }
        
        public int quizBildigiSoruAdet(int ID)
        {
            
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select count(dogruMu) from ogrenci,quiz where quiz.quizId=ogrenci.quizId and quiz.quizID='"+ID+"' and dogruMu=1", baglanti);
            SqlDataReader dr1 = komut.ExecuteReader();
            while (dr1.Read())
            {

                bildigiAdet = int.Parse(dr1[0].ToString());

            }          
            baglanti.Close();
            return bildigiAdet;
        }

        public int quizBilemedigiSoruAdet(int ID)
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand("select count(dogruMu) from ogrenci,quiz where quiz.quizId=ogrenci.quizId and quiz.quizID='" + ID + "' and dogruMu=0", baglanti);
            SqlDataReader dr1 = komut.ExecuteReader();
            while (dr1.Read())
            {

                bilemedigiAdet = int.Parse(dr1[0].ToString());

            }
            baglanti.Close();
            return bilemedigiAdet;
        }
               
        private void ogrenciSinav_Load(object sender, EventArgs e)
        {

            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                         (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);//Giriş formunu ortaladı
            label9.Text = sorulacakSoruSayisi.ToString();

            if (QUIZADET == 0)
            {
                RasgeleSoruSecme(sorulacakSoruSayisi);
            }else
            {
                SoruGetir();
  
            }
       


        }       
        public void SoruGetir()
        {
            if (QUIZADET != 0)
            {

                for (int k = 1; k <= QUIZADET; k++)
                {

                    bildigiSorulacakSoruAdet += quizBildigiSoruAdet(k);//bildigi sorulardan  %20 oranında veritabanından çekilecek
                    bilemedigisorulacakSoruAdet += quizBilemedigiSoruAdet(k);  //bilemediği sorulardan veritabanından çekilecek               

                }

                //MessageBox.Show(bildigiSorulacakSoruAdet.ToString());
                //MessageBox.Show(bilemedigisorulacakSoruAdet.ToString());
                katsayi = (sorulacakSoruSayisi) / (bildigiSorulacakSoruAdet + bilemedigisorulacakSoruAdet);


                bildigiSorulacakSoruAdet *= katsayi;
                bilemedigisorulacakSoruAdet *= katsayi;


                bildigiSorulacakSoruAdet = (bildigiSorulacakSoruAdet *20) / 100;

                bilemedigisorulacakSoruAdet = (bilemedigisorulacakSoruAdet * 80) / 100;


                bildigiSorulacakSoruAdet = Math.Round(bildigiSorulacakSoruAdet, 0);
                bilemedigisorulacakSoruAdet = Math.Round(bilemedigisorulacakSoruAdet, 0);

                geriyekalanSorulacak = sorulacakSoruSayisi  - (bildigiSorulacakSoruAdet + bilemedigisorulacakSoruAdet);//kalan sorulardan rasgele çekilecek
                //MessageBox.Show(bildigiSorulacakSoruAdet.ToString());
                //MessageBox.Show(bilemedigisorulacakSoruAdet.ToString());
                //MessageBox.Show(geriyekalanSorulacak.ToString());

                SoruGetirmeBildigi(bildigiSorulacakSoruAdet);
                SoruGetirmeBilemedigi(bilemedigisorulacakSoruAdet);
                GeriKalanSoruGetirme(soruID.Count, geriyekalanSorulacak);



            }


        }




        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


       
        ArrayList parca = new ArrayList();
        private void button4_Click(object sender, EventArgs e)
        {
            if (sinavadi.Text != "")
            {
                sinavAdikaydet();
                secenekGoster();
                timer1.Start();
                soruDB.Text = soruADi[0].ToString();
                A.Text = SecenekA[0].ToString();
                B.Text = SecenekB[0].ToString();
                C.Text = SecenekC[0].ToString();
                D.Text = SecenekD[0].ToString();
                sayac.Text = sorusayaci.ToString();



                button4.Enabled = false;
            }
            else
            {
                MessageBox.Show("Lütfen sınav adını giriniz");
            }
        }



        private void button8_Click(object sender, EventArgs e)
        {
            //yazılacak


            if (radioButtonA.Checked == true || radioButtonB.Checked == true || radioButtonC.Checked == true || radioButtonD.Checked == true)
            {

                if (radioButtonA.Checked == true)
                {

                    ogrisaretSecenek.Add("A");
                    if (DogruSecenek[SecenekSayac].ToString() == "A")
                    {
                        dogruMu.Add(1);

                    }
                    else
                    {
                        dogruMu.Add(0);

                    }
                    SecenekSayac++;

                }
                else if (radioButtonB.Checked == true)
                {

                    ogrisaretSecenek.Add("B");
                    if (DogruSecenek[SecenekSayac].ToString() == "B")
                    {
                        dogruMu.Add(1);

                    }
                    else
                    {
                        dogruMu.Add(0);

                    }
                    SecenekSayac++;


                }
                else if (radioButtonC.Checked == true)
                {
                    ogrisaretSecenek.Add("C");
                    if (DogruSecenek[SecenekSayac].ToString() == "C")
                    {
                        dogruMu.Add(1);

                    }
                    else
                    {
                        dogruMu.Add(0);

                    }
                    SecenekSayac++;

                }
                else if (radioButtonD.Checked == true)
                {
                    ogrisaretSecenek.Add("D");
                    if (DogruSecenek[SecenekSayac].ToString() == "D")
                    {
                        dogruMu.Add(1);

                    }
                    else
                    {
                        dogruMu.Add(0);

                    }
                    SecenekSayac++;

                }



                if (i != sorulacakSoruSayisi)
                {

                    sorusayaci++;
                    sayac.Text = sorusayaci.ToString();
                    soruDB.Text = soruADi[i].ToString();
                    A.Text = SecenekA[i].ToString();
                    B.Text = SecenekB[i].ToString();
                    C.Text = SecenekC[i].ToString();
                    D.Text = SecenekD[i].ToString();                   
                    i++;
                }
                else
                {

                    MessageBox.Show("Sınav Bitti");                   
                    button4.Enabled = false;
                    sinavadi.Enabled = false;
                    button8.Enabled = false;     
                    secenekGizle();
                    for (int i = 0; i < dogruMu.Count; i++)//doğru yanlış sayısı bulan döngü
                    {
                        if (dogruMu[i].ToString() == "1")
                        {
                            dogruMuAdet++;
                        }else
                        {
                            yanlisMiadet++;
                        }
                    }

                    sonucGoster();
                    dogruSayisi.Text = dogruMuAdet.ToString();
                    yanlisSayisi.Text = yanlisMiadet.ToString();
                    puan.Text = (dogruMuAdet * 5).ToString();
                    basari = ((dogruMuAdet * 100) / sorulacakSoruSayisi);
                    basariYuzde.Text = basari.ToString();

                    int ID = quizIDgetir();
                    basariEkle(basari);
                    for (int j = 0; j < sorulacakSoruSayisi; j++)
                    {
                        ogrsoruEkle(int.Parse(dersID[j].ToString()), int.Parse(konuID[j].ToString()), int.Parse(soruID[j].ToString()), ogrisaretSecenek[j].ToString(), byte.Parse(dogruMu[j].ToString()), ID);
                    }

                }

                radioButonTemizle();

            }
            else
            {
                MessageBox.Show("Lütfen bir seçeneği işaretleyiniz");
            }

        }



        public void sonucGoster()
        {
            label10.Visible = true;
            label11.Visible = true;
            label13.Visible = true;
            label14.Visible = true;
            label16.Visible = true;
            dogruSayisi.Visible = true;
            yanlisSayisi.Visible = true;
            puan.Visible = true;
            basariYuzde.Visible = true;
        }

        
        private void timer1_Tick(object sender, EventArgs e)
        {
            sinavSuredeger = int.Parse(zaman.Text);
            sinavSuredeger--;
            zaman.Text = sinavSuredeger.ToString();

            if (sinavSuredeger == 0)
            {
                timer1.Stop();
                MessageBox.Show("Süre Bitti");

            }



        }

    }
}
