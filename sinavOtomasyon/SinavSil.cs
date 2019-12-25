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
    public partial class SinavSil : Form
    {
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-RE244GE;Initial Catalog=sinav;Integrated Security=True");
        public SinavSil()
        {
            InitializeComponent();
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


            if (comboBox1.Items.Count != 0)//veri tabanından hiçbir değer gelmiyorsa listeleme dedik
            {

                comboBox1.SelectedIndex = 0;
            }

        }


        public void QuizSil()
        {

            baglanti.Open();
            string sql = "truncate table ogrenci DELETE FROM quiz DBCC CHECKIDENT ('sinav.dbo.quiz',RESEED, 0)";
            SqlCommand komut = new SqlCommand(sql, baglanti);
            komut.Parameters.AddWithValue("@Param", comboBox1.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Sınav Silindi");
   
        }

        private void SinavSil_Load(object sender, EventArgs e)
        {
            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
            (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);//Giriş formunu ortaladı
            comboQuizListeleme();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                QuizSil();
                comboQuizListeleme();
            }else
            {
                MessageBox.Show("silinecek sınav yok");
            }

        }
    }
}
