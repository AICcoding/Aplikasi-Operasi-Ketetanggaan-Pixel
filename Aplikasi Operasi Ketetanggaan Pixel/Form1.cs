using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.Util;
using Emgu.CV.Structure;

namespace Aplikasi_Operasi_Ketetanggaan_Pixel
{
    public partial class Form1 : Form
    {
        Bitmap gambar_awal, gambar_akhir;
        Image<Bgr, Byte> gambar_awal_e, gambar_akhir_e;
        int mode, filter_standar, filter_advanced;

        public Form1()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;

            radioButton9.Checked = true;
            mode = 1;
            filter_standar = -1;
            filter_advanced = -1;

            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            radioButton3.Enabled = false;
            radioButton4.Enabled = false;
            radioButton5.Enabled = false;
            radioButton6.Enabled = false;
            radioButton7.Enabled = false;
            radioButton8.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog pilih_gambar = new OpenFileDialog();
            pilih_gambar.Filter = "File gambar (*.BMP; *.JPG; *.PNG)|*.BMP; *.JPG; *.PNG";
            if (pilih_gambar.ShowDialog() == DialogResult.OK)
            {
                gambar_awal_e = new Image<Bgr, byte>(pilih_gambar.FileName);
                gambar_akhir_e = new Image<Bgr, byte>(pilih_gambar.FileName);

                gambar_awal = new Bitmap(new Bitmap(pilih_gambar.FileName)); //gambar patokan pengolahan
                gambar_akhir = new Bitmap(new Bitmap(pilih_gambar.FileName)); //gambar sebelum diedit

                pictureBox1.Image = gambar_awal;
            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton9.Checked==true)
            {
                mode = 1;
            }
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton10.Checked==true)
            {
                mode = 2;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked==true)
            {
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
                radioButton3.Enabled = true;
            }
            else if (checkBox1.Checked == false)
            {
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                radioButton3.Enabled = false;
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;

                filter_standar = -1;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                radioButton4.Enabled = true;
                //radioButton7.Enabled = true;
                //radioButton8.Enabled = true;
                radioButton5.Enabled = true;
                radioButton6.Enabled = true;
                
            }
            else if (checkBox2.Checked == false)
            {
                radioButton4.Enabled = false;
                radioButton7.Enabled = false;
                radioButton8.Enabled = false;
                radioButton5.Enabled = false;
                radioButton6.Enabled = false;

                radioButton4.Checked = false;
                radioButton7.Checked = false;
                radioButton8.Checked = false;
                radioButton5.Checked = false;
                radioButton6.Checked = false;

                filter_advanced = -1;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked==true)
            {
                filter_standar = 1;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                filter_standar = 2;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                filter_standar = 3;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true)
            {
                filter_advanced = 1;
                radioButton7.Enabled = true;
                radioButton8.Enabled = true;
            }
            else if (radioButton4.Checked == false)
            {
                radioButton7.Enabled = false;
                radioButton8.Enabled = false;
                radioButton7.Checked = false;
                radioButton8.Checked = false;
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked == true)
            {
                filter_advanced = 2;
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton6.Checked==true)
            {
                filter_advanced = 3;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //mode, filter_standar, filter_advanced;
            if(mode==1)
            {
                if(filter_standar==1)
                {
                    filter_batas_primitif();
                }
                else if(filter_standar==2)
                {
                    filter_pererataan_primitif();
                }
                else if(filter_standar==3)
                {
                    filter_median_primitif();
                }
            }
            else if(mode==2)
            {
                if (filter_standar == 1)
                {
                    filter_batas_emgu();
                }
                else if (filter_standar == 2)
                {
                    filter_pererataan_emgu();
                }
                else if (filter_standar == 3)
                {
                    filter_median_emgu();
                }
            }
        }

        private void filter_batas_primitif()
        {
            //kodene...
        }

        private void filter_pererataan_primitif()
        {
            //kodene...
        }

        private void filter_median_primitif()
        {
            //kodene...
        }

        private void filter_batas_emgu()
        {
            //kodene...
        }

        private void filter_pererataan_emgu()
        {
            //kodene...
        }

        private void filter_median_emgu()
        {
            //kodene...
        }        
    }
}
