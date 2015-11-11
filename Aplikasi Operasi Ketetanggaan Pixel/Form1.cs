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
        Bitmap gambar_awal, gambar_akhir, gambar_tmp, gambar_hasil_sementara;
        Image<Bgr, Byte> gambar_awal_e, gambar_akhir_e, gambar_hasil_sementara_e;
        int mode, filter_standar, filter_advanced, panjang_kernel, skala_pembesaran;
        int[,] kernel;

        public Form1()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;

            radioButton9.Checked = true;
            mode = 1;
            filter_standar = -1;
            filter_advanced = -1;
            panjang_kernel = (int)numericUpDown1.Value;

            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            radioButton3.Enabled = false;
            radioButton4.Enabled = false;
            radioButton5.Enabled = false;
            radioButton6.Enabled = false;
            radioButton7.Enabled = false;
            radioButton8.Enabled = false;

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

            skala_pembesaran = 1000;
            textBox1.Text = "1000";
            trackBar3.Value = 1000;
        }

        private void button2_Click(object sender, EventArgs e)
        {         
            OpenFileDialog pilih_gambar = new OpenFileDialog();
            pilih_gambar.Filter = "File gambar (*.BMP; *.JPG; *.PNG)|*.BMP; *.JPG; *.PNG";
            if (pilih_gambar.ShowDialog() == DialogResult.OK)
            {
                gambar_tmp = new Bitmap(new Bitmap(pilih_gambar.FileName));

                //gambar_awal_e = new Image<Bgr, byte>(gambar_tmp.Width + (panjang_kernel - 1), gambar_tmp.Height + (panjang_kernel - 1));

                gambar_awal = new Bitmap(gambar_tmp.Width + (panjang_kernel - 1), gambar_tmp.Height + (panjang_kernel - 1));
                gambar_akhir = new Bitmap(new Bitmap(pilih_gambar.FileName));

                int r, g, b;

                for (int i = 0; i < gambar_awal.Width; i++)
                {
                    for (int j = 0; j < gambar_awal.Height; j++)
                    {
                        if(i < ((panjang_kernel - 1) / 2))//pemberian nilai 0
                        {
                            gambar_awal.SetPixel(i, j, Color.FromArgb(0, 0, 0));

                        }
                        else if(j < ((panjang_kernel - 1) / 2)) //pemberian nilai 0
                        {
                            gambar_awal.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                        }
                        else if(j >= (gambar_awal.Height - ((panjang_kernel - 1) / 2)))
                        {
                            gambar_awal.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                        }
                        else if (i >= (gambar_awal.Width - ((panjang_kernel - 1) / 2)))
                        {
                            gambar_awal.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                        }
                        else
                        {
                            r = gambar_tmp.GetPixel(i - ((panjang_kernel - 1) / 2), j - ((panjang_kernel - 1) / 2)).R;
                            g = gambar_tmp.GetPixel(i - ((panjang_kernel - 1) / 2), j - ((panjang_kernel - 1) / 2)).G;
                            b = gambar_tmp.GetPixel(i - ((panjang_kernel - 1) / 2), j - ((panjang_kernel - 1) / 2)).B;

                            gambar_awal.SetPixel(i, j, Color.FromArgb(r, g, b));
                        }
                    }
                }
                gambar_awal_e = new Image<Bgr, byte>(gambar_awal);

                pictureBox1.Size = new Size(gambar_tmp.Width, gambar_tmp.Height);
                pictureBox2.Size = new Size(gambar_akhir.Width, gambar_akhir.Height);

                pictureBox1.Image = gambar_tmp;
                pictureBox2.Image = gambar_akhir;

                trackBar1.Value = 1;
                trackBar2.Value = 1;

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
                kernelLowPassAwal();
                radioButton7.Enabled = true;
                radioButton8.Enabled = true;

                radioButton7.Checked = true;
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
                kernelHighPassAwal();
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton6.Checked==true)
            {
                filter_advanced = 3;
                kernelHighBoostAwal();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(mode==1)
            {
                if(filter_standar==1)
                {
                    filter_batas_primitif();
                    if(filter_advanced==1)
                    {
                        if (radioButton7.Checked == true)
                        {
                            low_pass_filter_primitif();
                        }
                        else if (radioButton8.Checked == true)
                        {
                            filter_median_primitif();
                        }
                    }
                    else if(filter_advanced==2)
                    {
                        high_pass_filter_primitif();
                    }
                    else if(filter_advanced==3)
                    {
                        high_boost_filter_primitif();
                    }
                }
                else if(filter_standar==2)
                {
                    filter_pererataan_primitif();
                    if (filter_advanced == 1)
                    {
                        if (radioButton7.Checked == true)
                        {
                            low_pass_filter_primitif();
                        }
                        else if (radioButton8.Checked == true)
                        {
                            filter_median_primitif();
                        }
                    }
                    else if (filter_advanced == 2)
                    {
                        high_pass_filter_primitif();
                    }
                    else if (filter_advanced == 3)
                    {
                        high_boost_filter_primitif();
                    }
                }
                else if(filter_standar==3)
                {
                    filter_median_primitif();
                    if (filter_advanced == 1)
                    {
                        if (radioButton7.Checked == true)
                        {
                            low_pass_filter_primitif();
                        }
                        else if (radioButton8.Checked == true)
                        {
                            filter_median_primitif();
                        }
                    }
                    else if (filter_advanced == 2)
                    {
                        high_pass_filter_primitif();
                    }
                    else if (filter_advanced == 3)
                    {
                        high_boost_filter_primitif();
                    }
                }
                else if(filter_advanced==1)
                {
                    if(radioButton7.Checked==true)
                    {
                        low_pass_filter_primitif();
                    }
                    else if(radioButton8.Checked==true)
                    {
                        filter_median_primitif();
                    }
                }
                else if(filter_advanced==2)
                {
                    high_pass_filter_primitif();
                }
                else if (filter_advanced==3)
                {
                    high_boost_filter_primitif();
                }
                
            }
            else if(mode==2)
            {
                if (filter_standar == 1)
                {
                    filter_batas_emgu();

                    if (filter_advanced == 1)
                    {
                        if (radioButton7.Checked == true)
                        {
                            low_pass_filter_emgu();
                        }
                        else if (radioButton8.Checked == true)
                        {
                            filter_median_emgu();
                        }
                    }
                    else if (filter_advanced == 2)
                    {
                        high_pass_filter_emgu();
                    }
                    else if (filter_advanced == 3)
                    {
                        high_boost_filter_emgu();
                    }
                }
                else if (filter_standar == 2)
                {
                    filter_pererataan_emgu();
                    if (filter_advanced == 1)
                    {
                        if (radioButton7.Checked == true)
                        {
                            low_pass_filter_emgu();
                        }
                        else if (radioButton8.Checked == true)
                        {
                            filter_median_emgu();
                        }
                    }
                    else if (filter_advanced == 2)
                    {
                        high_pass_filter_emgu();
                    }
                    else if (filter_advanced == 3)
                    {
                        high_boost_filter_emgu();
                    }
                }
                else if (filter_standar == 3)
                {
                    filter_median_emgu();
                    if (filter_advanced == 1)
                    {
                        if (radioButton7.Checked == true)
                        {
                            low_pass_filter_emgu();
                        }
                        else if (radioButton8.Checked == true)
                        {
                            filter_median_emgu();
                        }
                    }
                    else if (filter_advanced == 2)
                    {
                        high_pass_filter_emgu();
                    }
                    else if (filter_advanced == 3)
                    {
                        high_boost_filter_emgu();
                    }
                }
                else if (filter_advanced == 1)
                {
                    if (radioButton7.Checked == true)
                    {
                        low_pass_filter_emgu();
                    }
                    else if (radioButton8.Checked == true)
                    {
                        filter_median_emgu();
                    }
                }
                else if (filter_advanced == 2)
                {
                    high_pass_filter_emgu();
                }
                else if (filter_advanced == 3)
                {
                    high_boost_filter_emgu();
                }
            }
        }

        private void filter_batas_primitif()
        {
            //kodene...
            gambar_akhir = new Bitmap(gambar_tmp.Width, gambar_tmp.Height);
            gambar_hasil_sementara = new Bitmap(gambar_awal.Width, gambar_awal.Height);

            int[] nilai_mak = new int[3];
            int[] nilai_min = new int[3];
            int r, g, b, nilai_batas;

            nilai_batas = Convert.ToInt16(numericUpDown1.Value);
            nilai_batas = (nilai_batas - 1) / 2;
            for (int i = nilai_batas; i < gambar_awal.Width - nilai_batas; i++)
            {
                for (int j = nilai_batas; j < gambar_awal.Height - nilai_batas; j++)
                {
                    nilai_mak[0] = cari_nilai_mak(i, j, "red");
                    nilai_mak[1] = cari_nilai_mak(i, j, "green");
                    nilai_mak[2] = cari_nilai_mak(i, j, "blue");

                    nilai_min[0] = cari_nilai_min(i, j, "red");
                    nilai_min[1] = cari_nilai_min(i, j, "green");
                    nilai_min[2] = cari_nilai_min(i, j, "blue");

                    if ((gambar_awal.GetPixel(i, j).R) < nilai_min[0])
                        r = nilai_min[0];
                    else if ((gambar_awal.GetPixel(i, j).R) > nilai_mak[0])
                        r = nilai_mak[0];
                    else
                        r = gambar_awal.GetPixel(i, j).R;

                    if ((gambar_awal.GetPixel(i, j).G) < nilai_min[1])
                        g = nilai_min[1];
                    else if ((gambar_awal.GetPixel(i, j).G) > nilai_mak[1])
                        g = nilai_mak[1];
                    else
                        g = gambar_awal.GetPixel(i, j).G;

                    if ((gambar_awal.GetPixel(i, j).B) < nilai_min[2])
                        b = nilai_min[2];
                    else if ((gambar_awal.GetPixel(i, j).B) > nilai_mak[2])
                        b = nilai_mak[2];
                    else
                        b = gambar_awal.GetPixel(i, j).B;

                    gambar_akhir.SetPixel(i - nilai_batas, j - nilai_batas, Color.FromArgb(r, g, b));
                    if(filter_advanced != -1)
                    {
                        gambar_hasil_sementara.SetPixel(i, j, Color.FromArgb(r, g, b));
                    }
                }
            }
            pictureBox2.Image = (Bitmap)gambar_akhir;
        }

        private int cari_nilai_mak(int i, int j, string RGB)
        {
            int hasil=-1;
            if(RGB=="red")
            {
                try
                {
                    hasil = gambar_awal.GetPixel(i, j+1).R;

                    if (hasil < gambar_awal.GetPixel(i - 1, j + 1).R)
                        hasil = gambar_awal.GetPixel(i - 1, j + 1).R;

                    if (hasil < gambar_awal.GetPixel(i - 1, j).R)
                        hasil = gambar_awal.GetPixel(i - 1, j).R;

                    if (hasil < gambar_awal.GetPixel(i - 1, j - 1).R)
                        hasil = gambar_awal.GetPixel(i - 1, j - 1).R;

                    if (hasil < gambar_awal.GetPixel(i, j - 1).R)
                        hasil = gambar_awal.GetPixel(i, j - 1).R;

                    if (hasil < gambar_awal.GetPixel(i + 1, j - 1).R)
                        hasil = gambar_awal.GetPixel(i + 1, j - 1).R;

                    if (hasil < gambar_awal.GetPixel(i + 1, j).R)
                        hasil = gambar_awal.GetPixel(i + 1, j).R;

                    if (hasil < gambar_awal.GetPixel(i + 1, j + 1).R)
                        hasil = gambar_awal.GetPixel(i + 1, j + 1).R;
                }
                catch
                {
                    hasil = -1;
                }
            }
            else if(RGB=="green")
            {
                try
                {
                    hasil = gambar_awal.GetPixel(i, j + 1).G;

                    if (hasil < gambar_awal.GetPixel(i - 1, j + 1).G)
                        hasil = gambar_awal.GetPixel(i - 1, j + 1).G;

                    if (hasil < gambar_awal.GetPixel(i - 1, j).G)
                        hasil = gambar_awal.GetPixel(i - 1, j).G;

                    if (hasil < gambar_awal.GetPixel(i - 1, j - 1).G)
                        hasil = gambar_awal.GetPixel(i - 1, j - 1).G;

                    if (hasil < gambar_awal.GetPixel(i, j - 1).G)
                        hasil = gambar_awal.GetPixel(i, j - 1).G;

                    if (hasil < gambar_awal.GetPixel(i + 1, j - 1).G)
                        hasil = gambar_awal.GetPixel(i + 1, j - 1).G;

                    if (hasil < gambar_awal.GetPixel(i + 1, j).G)
                        hasil = gambar_awal.GetPixel(i + 1, j).G;

                    if (hasil < gambar_awal.GetPixel(i + 1, j + 1).G)
                        hasil = gambar_awal.GetPixel(i + 1, j + 1).G;
                }
                catch
                {
                    hasil = -1;
                }
            }
            else if(RGB=="blue")
            {
                try
                {
                    hasil = gambar_awal.GetPixel(i, j + 1).B;

                    if (hasil < gambar_awal.GetPixel(i - 1, j + 1).B)
                        hasil = gambar_awal.GetPixel(i - 1, j + 1).B;

                    if (hasil < gambar_awal.GetPixel(i - 1, j).B)
                        hasil = gambar_awal.GetPixel(i - 1, j).B;

                    if (hasil < gambar_awal.GetPixel(i - 1, j - 1).B)
                        hasil = gambar_awal.GetPixel(i - 1, j - 1).B;

                    if (hasil < gambar_awal.GetPixel(i, j - 1).B)
                        hasil = gambar_awal.GetPixel(i, j - 1).B;

                    if (hasil < gambar_awal.GetPixel(i + 1, j - 1).B)
                        hasil = gambar_awal.GetPixel(i + 1, j - 1).B;

                    if (hasil < gambar_awal.GetPixel(i + 1, j).B)
                        hasil = gambar_awal.GetPixel(i + 1, j).B;

                    if (hasil < gambar_awal.GetPixel(i + 1, j + 1).B)
                        hasil = gambar_awal.GetPixel(i + 1, j + 1).B;
                }
                catch
                {
                    hasil = -1;
                }
            }
            return hasil;
        }

        private int cari_nilai_min(int i, int j, string RGB)
        {
            int hasil = -1;
            if (RGB == "red")
            {
                try
                {
                    hasil = gambar_awal.GetPixel(i, j + 1).R;

                    if (hasil > gambar_awal.GetPixel(i - 1, j + 1).R)
                        hasil = gambar_awal.GetPixel(i - 1, j + 1).R;

                    if (hasil > gambar_awal.GetPixel(i - 1, j).R)
                        hasil = gambar_awal.GetPixel(i - 1, j).R;

                    if (hasil > gambar_awal.GetPixel(i - 1, j - 1).R)
                        hasil = gambar_awal.GetPixel(i - 1, j - 1).R;

                    if (hasil > gambar_awal.GetPixel(i, j - 1).R)
                        hasil = gambar_awal.GetPixel(i, j - 1).R;

                    if (hasil > gambar_awal.GetPixel(i + 1, j - 1).R)
                        hasil = gambar_awal.GetPixel(i + 1, j - 1).R;

                    if (hasil > gambar_awal.GetPixel(i + 1, j).R)
                        hasil = gambar_awal.GetPixel(i + 1, j).R;

                    if (hasil > gambar_awal.GetPixel(i + 1, j + 1).R)
                        hasil = gambar_awal.GetPixel(i + 1, j + 1).R;
                }
                catch
                {
                    hasil = -1;
                }
            }
            else if (RGB == "green")
            {
                try
                {
                    hasil = gambar_awal.GetPixel(i, j + 1).G;

                    if (hasil > gambar_awal.GetPixel(i - 1, j + 1).G)
                        hasil = gambar_awal.GetPixel(i - 1, j + 1).G;

                    if (hasil > gambar_awal.GetPixel(i - 1, j).G)
                        hasil = gambar_awal.GetPixel(i - 1, j).G;

                    if (hasil > gambar_awal.GetPixel(i - 1, j - 1).G)
                        hasil = gambar_awal.GetPixel(i - 1, j - 1).G;

                    if (hasil > gambar_awal.GetPixel(i, j - 1).G)
                        hasil = gambar_awal.GetPixel(i, j - 1).G;

                    if (hasil > gambar_awal.GetPixel(i + 1, j - 1).G)
                        hasil = gambar_awal.GetPixel(i + 1, j - 1).G;

                    if (hasil > gambar_awal.GetPixel(i + 1, j).G)
                        hasil = gambar_awal.GetPixel(i + 1, j).G;

                    if (hasil > gambar_awal.GetPixel(i + 1, j + 1).G)
                        hasil = gambar_awal.GetPixel(i + 1, j + 1).G;
                }
                catch
                {
                    hasil = -1;
                }
            }
            else if (RGB == "blue")
            {
                try
                {
                    hasil = gambar_awal.GetPixel(i, j + 1).B;

                    if (hasil > gambar_awal.GetPixel(i - 1, j + 1).B)
                        hasil = gambar_awal.GetPixel(i - 1, j + 1).B;

                    if (hasil > gambar_awal.GetPixel(i - 1, j).B)
                        hasil = gambar_awal.GetPixel(i - 1, j).B;

                    if (hasil > gambar_awal.GetPixel(i - 1, j - 1).B)
                        hasil = gambar_awal.GetPixel(i - 1, j - 1).B;

                    if (hasil > gambar_awal.GetPixel(i, j - 1).B)
                        hasil = gambar_awal.GetPixel(i, j - 1).B;

                    if (hasil > gambar_awal.GetPixel(i + 1, j - 1).B)
                        hasil = gambar_awal.GetPixel(i + 1, j - 1).B;

                    if (hasil > gambar_awal.GetPixel(i + 1, j).B)
                        hasil = gambar_awal.GetPixel(i + 1, j).B;

                    if (hasil > gambar_awal.GetPixel(i + 1, j + 1).B)
                        hasil = gambar_awal.GetPixel(i + 1, j + 1).B;
                }
                catch
                {
                    hasil = -1;
                }
            }
            return hasil;
        }

        private void filter_pererataan_primitif()
        {
            //kodene...
            gambar_akhir = new Bitmap(gambar_tmp.Width, gambar_tmp.Height);
            gambar_hasil_sementara = new Bitmap(gambar_awal.Width, gambar_awal.Height);

            int[] nilai_total = new int[3];
            int r, g, b,nilai_batas;
            double tmp;

            nilai_batas = Convert.ToInt16(numericUpDown1.Value);
            nilai_batas = (nilai_batas - 1) / 2;

            for (int i = nilai_batas; i < gambar_awal.Width - nilai_batas; i++)
            {
                for (int j = nilai_batas; j < gambar_awal.Height - nilai_batas; j++)
                {
                    nilai_total[0] = 0;
                    nilai_total[0] += gambar_awal.GetPixel(i, j + 1).R;
                    nilai_total[0] += gambar_awal.GetPixel(i - 1, j + 1).R;
                    nilai_total[0] += gambar_awal.GetPixel(i - 1, j).R;
                    nilai_total[0] += gambar_awal.GetPixel(i - 1, j - 1).R;
                    nilai_total[0] += gambar_awal.GetPixel(i, j - 1).R;
                    nilai_total[0] += gambar_awal.GetPixel(i + 1, j - 1).R;
                    nilai_total[0] += gambar_awal.GetPixel(i + 1, j).R;
                    nilai_total[0] += gambar_awal.GetPixel(i + 1, j + 1).R;

                    nilai_total[1] = 0;
                    nilai_total[1] += gambar_awal.GetPixel(i, j + 1).G;
                    nilai_total[1] += gambar_awal.GetPixel(i - 1, j + 1).G;
                    nilai_total[1] += gambar_awal.GetPixel(i - 1, j).G;
                    nilai_total[1] += gambar_awal.GetPixel(i - 1, j - 1).G;
                    nilai_total[1] += gambar_awal.GetPixel(i, j - 1).G;
                    nilai_total[1] += gambar_awal.GetPixel(i + 1, j - 1).G;
                    nilai_total[1] += gambar_awal.GetPixel(i + 1, j).G;
                    nilai_total[1] += gambar_awal.GetPixel(i + 1, j + 1).G;

                    nilai_total[2] = 0;
                    nilai_total[2] += gambar_awal.GetPixel(i, j + 1).B;
                    nilai_total[2] += gambar_awal.GetPixel(i - 1, j + 1).B;
                    nilai_total[2] += gambar_awal.GetPixel(i - 1, j).B;
                    nilai_total[2] += gambar_awal.GetPixel(i - 1, j - 1).B;
                    nilai_total[2] += gambar_awal.GetPixel(i, j - 1).B;
                    nilai_total[2] += gambar_awal.GetPixel(i + 1, j - 1).B;
                    nilai_total[2] += gambar_awal.GetPixel(i + 1, j).B;
                    nilai_total[2] += gambar_awal.GetPixel(i + 1, j + 1).B;

                    tmp = Math.Round(nilai_total[0] / 9F);
                    r = Convert.ToInt16(tmp);

                    tmp = Math.Round(nilai_total[1] / 9F);
                    g = Convert.ToInt16(tmp);

                    tmp = Math.Round(nilai_total[2] / 9F);
                    b = Convert.ToInt16(tmp);

                    //gambar_akhir.SetPixel(i - nilai_batas, j - nilai_batas, Color.FromArgb(r, g, b));
                    gambar_akhir.SetPixel(i - nilai_batas, j - nilai_batas, Color.FromArgb(r, g, b));
                    if (filter_advanced != -1)
                    {
                        gambar_hasil_sementara.SetPixel(i, j, Color.FromArgb(r, g, b));
                    }
                }
            }
            pictureBox2.Image = gambar_akhir;
        }

        private void filter_median_primitif()
        {
            //kodene...
            gambar_akhir = new Bitmap(gambar_tmp.Width, gambar_tmp.Height);
            gambar_hasil_sementara = new Bitmap(gambar_awal.Width, gambar_awal.Height);

            int r, g, b,nilai_batas;

            nilai_batas = Convert.ToInt16(numericUpDown1.Value);
            nilai_batas = (nilai_batas - 1) / 2;

            for (int i = nilai_batas; i < gambar_awal.Width - nilai_batas; i++)
            {
                for (int j = nilai_batas; j < gambar_awal.Height - nilai_batas; j++)
                {
                    r = cari_median(i, j, "red");
                    g = cari_median(i, j, "green");
                    b = cari_median(i, j, "blue");

                    gambar_akhir.SetPixel(i - nilai_batas, j - nilai_batas, Color.FromArgb(r, g, b));
                    if (filter_advanced != -1)
                    {
                        gambar_hasil_sementara.SetPixel(i, j, Color.FromArgb(r, g, b));
                    }
                }
            }
            pictureBox2.Image = (Bitmap)gambar_akhir;
        }

        private int cari_median(int i, int j, String RGB)
        {
            int[] data = new int[9];
            int tmp, hasil;
            if(RGB=="red")
            {
                data[0] = gambar_awal.GetPixel(i, j + 1).R;
                data[1] = gambar_awal.GetPixel(i - 1, j + 1).R;
                data[2] = gambar_awal.GetPixel(i - 1, j).R;
                data[3] = gambar_awal.GetPixel(i - 1, j - 1).R;

                data[4] = gambar_awal.GetPixel(i, j - 1).R;
                
                data[5] = gambar_awal.GetPixel(i + 1, j - 1).R;
                data[6] = gambar_awal.GetPixel(i + 1, j).R;
                data[7] = gambar_awal.GetPixel(i + 1, j + 1).R;
                data[8] = gambar_awal.GetPixel(i, j).R;               
            }
            else if (RGB == "green")
            {
                data[0] = gambar_awal.GetPixel(i, j + 1).G;
                data[1] = gambar_awal.GetPixel(i - 1, j + 1).G;
                data[2] = gambar_awal.GetPixel(i - 1, j).G;
                data[3] = gambar_awal.GetPixel(i - 1, j - 1).G;

                data[4] = gambar_awal.GetPixel(i, j - 1).G;

                data[5] = gambar_awal.GetPixel(i + 1, j - 1).G;
                data[6] = gambar_awal.GetPixel(i + 1, j).G;
                data[7] = gambar_awal.GetPixel(i + 1, j + 1).G;
                data[8] = gambar_awal.GetPixel(i, j).G;              
            }
            else if (RGB == "blue")
            {
                data[0] = gambar_awal.GetPixel(i, j + 1).B;
                data[1] = gambar_awal.GetPixel(i - 1, j + 1).B;
                data[2] = gambar_awal.GetPixel(i - 1, j).B;
                data[3] = gambar_awal.GetPixel(i - 1, j - 1).B;

                data[4] = gambar_awal.GetPixel(i, j - 1).B;

                data[5] = gambar_awal.GetPixel(i + 1, j - 1).B;
                data[6] = gambar_awal.GetPixel(i + 1, j).B;
                data[7] = gambar_awal.GetPixel(i + 1, j + 1).B;
                data[8] = gambar_awal.GetPixel(i, j).B;           
            }

            for (int a = 0; a < 8; a++)
            {
                for (int b = 0; b < 8 - a; b++)
                {
                    if (data[b] > data[b + 1])
                    {
                        tmp = data[b];
                        data[b] = data[b + 1];
                        data[b + 1] = tmp;
                    }
                }
            }
            hasil = data[4];
            return hasil;
        }

        private void filter_batas_emgu()
        {
            //kodene...
            gambar_akhir_e = new Image<Bgr, byte>(gambar_tmp.Width, gambar_tmp.Height);
            Byte[, ,] GetPixel_e = gambar_awal_e.Data; //Mengambil warna dari gambar awal
            Byte[, ,] SetPixel_e = gambar_akhir_e.Data; //Mengeset warna ke gambar akhir

            gambar_hasil_sementara_e = new Image<Bgr, byte>(gambar_awal.Width, gambar_awal.Height);

            int[] nilai_mak = new int[3];
            int[] nilai_min = new int[3];
            int r, g, b, nilai_batas;

            nilai_batas = Convert.ToInt16(numericUpDown1.Value);
            nilai_batas = (nilai_batas - 1) / 2;
            for (int i = nilai_batas; i < gambar_awal.Width - nilai_batas; i++)
            {
                for (int j = nilai_batas; j < gambar_awal.Height - nilai_batas; j++)
                {
                    nilai_mak[0] = cari_nilai_mak(i, j, "red");
                    nilai_mak[1] = cari_nilai_mak(i, j, "green");
                    nilai_mak[2] = cari_nilai_mak(i, j, "blue");

                    nilai_min[0] = cari_nilai_min(i, j, "red");
                    nilai_min[1] = cari_nilai_min(i, j, "green");
                    nilai_min[2] = cari_nilai_min(i, j, "blue");

                    if (GetPixel_e[j ,i , 2] < nilai_min[0])
                        r = nilai_min[0];
                    else if (GetPixel_e[j, i, 2] > nilai_mak[0])
                        r = nilai_mak[0];
                    else
                        r = GetPixel_e[j, i, 2];

                    if (GetPixel_e[j, i, 1] < nilai_min[1])
                        g = nilai_min[1];
                    else if (GetPixel_e[j, i, 1] > nilai_mak[1])
                        g = nilai_mak[1];
                    else
                        g = GetPixel_e[j, i, 1];

                    if (GetPixel_e[j, i, 0] < nilai_min[2])
                        b = nilai_min[2];
                    else if (GetPixel_e[j, i, 0] > nilai_mak[2])
                        b = nilai_mak[2];
                    else
                        b = GetPixel_e[j, i, 0];

                    //SETPIXEL
                    SetPixel_e[j - nilai_batas, i - nilai_batas, 0] = (byte)b;
                    SetPixel_e[j - nilai_batas, i - nilai_batas, 1] = (byte)g;
                    SetPixel_e[j - nilai_batas, i - nilai_batas, 2] = (byte)r;

                    if (filter_advanced != -1)
                    {
                        gambar_hasil_sementara_e.Data[j, i, 0] = (byte)b;
                        gambar_hasil_sementara_e.Data[j, i, 1] = (byte)g;
                        gambar_hasil_sementara_e.Data[j, i, 2] = (byte)r;
                    }
                }
            }
            pictureBox2.Image = gambar_akhir_e.ToBitmap();
        }

        private void filter_pererataan_emgu()
        {
            //kodene...
            gambar_akhir_e = new Image<Bgr, byte>(gambar_tmp.Width, gambar_tmp.Height);
            gambar_hasil_sementara_e = new Image<Bgr, byte>(gambar_awal.Width, gambar_awal.Height);

            Byte[, ,] GetPixel_e = gambar_awal_e.Data; //Mengambil warna dari gambar awal
            Byte[, ,] SetPixel_e = gambar_akhir_e.Data; //Mengeset warna ke gambar akhir

            int[] nilai_total = new int[3];
            int r, g, b, nilai_batas;
            double tmp;

            nilai_batas = Convert.ToInt16(numericUpDown1.Value);
            nilai_batas = (nilai_batas - 1) / 2;

            for (int i = nilai_batas; i < gambar_awal_e.Width - nilai_batas; i++)
            {
                for (int j = nilai_batas; j < gambar_awal_e.Height - nilai_batas; j++)
                {
                    nilai_total[0] = 0; //Blue
                    nilai_total[0] += GetPixel_e[j, i, 0];
                    nilai_total[0] += GetPixel_e[j + 1, i - 1, 0];
                    nilai_total[0] += GetPixel_e[j, i - 1, 0];
                    nilai_total[0] += GetPixel_e[j - 1, i - 1, 0];
                    nilai_total[0] += GetPixel_e[j - 1, i, 0];
                    nilai_total[0] += GetPixel_e[j - 1, i + 1, 0];
                    nilai_total[0] += GetPixel_e[j, i + 1, 0];
                    nilai_total[0] += GetPixel_e[j + 1, i + 1, 0];


                    nilai_total[1] = 0; //Green
                    nilai_total[1] += GetPixel_e[j, i, 1];
                    nilai_total[1] += GetPixel_e[j + 1, i - 1, 1];
                    nilai_total[1] += GetPixel_e[j, i - 1, 1];
                    nilai_total[1] += GetPixel_e[j - 1, i - 1, 1];
                    nilai_total[1] += GetPixel_e[j - 1, i, 1];
                    nilai_total[1] += GetPixel_e[j - 1, i + 1, 1];
                    nilai_total[1] += GetPixel_e[j, i + 1, 1];
                    nilai_total[1] += GetPixel_e[j + 1, i + 1, 1];

                    nilai_total[2] = 0; //Red
                    nilai_total[2] += GetPixel_e[j, i, 2];
                    nilai_total[2] += GetPixel_e[j + 1, i - 1, 2];
                    nilai_total[2] += GetPixel_e[j, i - 1, 2];
                    nilai_total[2] += GetPixel_e[j - 1, i - 1, 2];
                    nilai_total[2] += GetPixel_e[j - 1, i, 2];
                    nilai_total[2] += GetPixel_e[j - 1, i + 1, 2];
                    nilai_total[2] += GetPixel_e[j, i + 1, 2];
                    nilai_total[2] += GetPixel_e[j + 1, i + 1, 2];


                    tmp = Math.Round(nilai_total[0] / 9F);
                    b = Convert.ToInt16(tmp);

                    tmp = Math.Round(nilai_total[1] / 9F);
                    g = Convert.ToInt16(tmp);

                    tmp = Math.Round(nilai_total[2] / 9F);
                    r = Convert.ToInt16(tmp);

                    //SETPIXEL
                    SetPixel_e[j - nilai_batas, i - nilai_batas, 0] = (byte)b;
                    SetPixel_e[j - nilai_batas, i - nilai_batas, 1] = (byte)g;
                    SetPixel_e[j - nilai_batas, i - nilai_batas, 2] = (byte)r;

                    if (filter_advanced != -1)
                    {
                        gambar_hasil_sementara_e.Data[j, i, 0] = (byte)b;
                        gambar_hasil_sementara_e.Data[j, i, 1] = (byte)g;
                        gambar_hasil_sementara_e.Data[j, i, 2] = (byte)r;
                    }
                }
            }
            pictureBox2.Image = gambar_akhir_e.ToBitmap();
        }

        private void filter_median_emgu()
        {
            //kodene...
            gambar_akhir_e = new Image<Bgr, byte>(gambar_tmp.Width, gambar_tmp.Height);
            gambar_hasil_sementara_e = new Image<Bgr, byte>(gambar_awal.Width, gambar_awal.Height);

            Byte[, ,] GetPixel_e = gambar_awal_e.Data; //Mengambil warna dari gambar awal
            Byte[, ,] SetPixel_e = gambar_akhir_e.Data; //Mengeset warna ke gambar akhir

            int r, g, b, nilai_batas;

            nilai_batas = Convert.ToInt16(numericUpDown1.Value);
            nilai_batas = (nilai_batas - 1) / 2;

            for (int i = nilai_batas; i < gambar_awal.Width - nilai_batas; i++)
            {
                for (int j = nilai_batas; j < gambar_awal.Height - nilai_batas; j++)
                {
                    r = cari_median(i, j, "red");
                    g = cari_median(i, j, "green");
                    b = cari_median(i, j, "blue");

                    //SETPIXEL
                    SetPixel_e[j - nilai_batas, i - nilai_batas, 0] = (byte)b;
                    SetPixel_e[j - nilai_batas, i - nilai_batas, 1] = (byte)g;
                    SetPixel_e[j - nilai_batas, i - nilai_batas, 2] = (byte)r;

                    if (filter_advanced != -1)
                    {
                        gambar_hasil_sementara_e.Data[j, i, 0] = (byte)b;
                        gambar_hasil_sementara_e.Data[j, i, 1] = (byte)g;
                        gambar_hasil_sementara_e.Data[j, i, 2] = (byte)r;
                    }
                }
            }
            pictureBox2.Image = gambar_akhir_e.ToBitmap();
        }

        private void low_pass_filter_primitif()
        {
            int sum_matrik = 0, nilai_batas; //jumlahkan semua kernel
            nilai_batas = Convert.ToInt16(numericUpDown1.Value);
            nilai_batas = (nilai_batas - 1) / 2;
            for(int i = 0; i < panjang_kernel; i++)
            {
                for(int j = 0; j < panjang_kernel; j++)
                {
                    sum_matrik += kernel[i, j];
                }
            }

            if (sum_matrik != 0)
            {
                if(filter_standar != -1)
                {
                    gambar_akhir = new Bitmap(gambar_tmp.Width, gambar_tmp.Height);
                    Color warna;
                    int R, G, B, totalR, totalG, totalB;
                    for (int i = ((panjang_kernel - 1) / 2); i < gambar_awal.Width - ((panjang_kernel - 1) / 2); i++)
                    {
                        for (int j = ((panjang_kernel - 1) / 2); j < gambar_awal.Height - ((panjang_kernel - 1) / 2); j++)
                        {
                            totalR = 0;
                            totalG = 0;
                            totalB = 0;
                            for (int x = 0 - nilai_batas, k = 0; x < panjang_kernel - nilai_batas; x++, k++)
                            {
                                for (int y = 0 - nilai_batas, l = 0; y < panjang_kernel - nilai_batas; y++, l++)
                                {
                                    warna = gambar_hasil_sementara.GetPixel(i + x, j + y);
                                    R = warna.R;
                                    G = warna.G;
                                    B = warna.B;

                                    totalR += (kernel[k, l] * R);
                                    totalG += (kernel[k, l] * G);
                                    totalB += (kernel[k, l] * B);
                                }
                            }

                            totalR /= sum_matrik;
                            totalG /= sum_matrik;
                            totalB /= sum_matrik;

                            /*if (totalR > 255)
                                totalR = 255;
                            else if (totalR < 0)
                                totalR = 0;
                            if (totalG > 255)
                                totalG = 255;
                            else if (totalG < 0)
                                totalG = 0;
                            if (totalB > 255)
                                totalB = 255;
                            else if (totalB < 0)
                                totalB = 0;*/

                            gambar_akhir.SetPixel(i - ((panjang_kernel - 1) / 2), j - ((panjang_kernel - 1) / 2), Color.FromArgb(totalR, totalG, totalB));
                        }
                    }
                    pictureBox2.Image = gambar_akhir;
                }
                else
                {
                    gambar_akhir = new Bitmap(gambar_tmp.Width, gambar_tmp.Height);
                    Color warna;
                    int R, G, B, totalR, totalG, totalB;
                    for (int i = ((panjang_kernel - 1) / 2); i < gambar_awal.Width - ((panjang_kernel - 1) / 2); i++)
                    {
                        for (int j = ((panjang_kernel - 1) / 2); j < gambar_awal.Height - ((panjang_kernel - 1) / 2); j++)
                        {
                            totalR = 0;
                            totalG = 0;
                            totalB = 0;
                            for (int x = 0 - nilai_batas, k = 0; x < panjang_kernel - nilai_batas; x++, k++)
                            {
                                for (int y = 0 - nilai_batas, l = 0; y < panjang_kernel - nilai_batas; y++, l++)
                                {
                                    warna = gambar_awal.GetPixel(i + x, j + y);
                                    R = warna.R;
                                    G = warna.G;
                                    B = warna.B;

                                    totalR += (kernel[k, l] * R);
                                    totalG += (kernel[k, l] * G);
                                    totalB += (kernel[k, l] * B);
                                }
                            }

                            totalR /= sum_matrik;
                            totalG /= sum_matrik;
                            totalB /= sum_matrik;

                            /*if (totalR > 255)
                                totalR = 255;
                            else if (totalR < 0)
                                totalR = 0;
                            if (totalG > 255)
                                totalG = 255;
                            else if (totalG < 0)
                                totalG = 0;
                            if (totalB > 255)
                                totalB = 255;
                            else if (totalB < 0)
                                totalB = 0;*/

                            gambar_akhir.SetPixel(i - ((panjang_kernel - 1) / 2), j - ((panjang_kernel - 1) / 2), Color.FromArgb(totalR, totalG, totalB));
                        }
                    }
                    pictureBox2.Image = gambar_akhir;
                }
            }
            else
            {
                MessageBox.Show("Silahkan input kernel sesuai dengan aturan Low Pass Filter!");
            }
        }

        private void high_pass_filter_primitif()
        {
            if(filter_standar != -1)
            {
                int nilai_batas;
                nilai_batas = Convert.ToInt16(numericUpDown1.Value);
                nilai_batas = (nilai_batas - 1) / 2;

                gambar_akhir = new Bitmap(gambar_tmp.Width, gambar_tmp.Height);
                Color warna;
                int R, G, B, totalR, totalG, totalB;
                for (int i = ((panjang_kernel - 1) / 2); i < gambar_awal.Width - ((panjang_kernel - 1) / 2); i++)
                {
                    for (int j = ((panjang_kernel - 1) / 2); j < gambar_awal.Height - ((panjang_kernel - 1) / 2); j++)
                    {
                        totalR = 0;
                        totalG = 0;
                        totalB = 0;
                        for (int x = 0 - nilai_batas, k = 0; x < panjang_kernel - nilai_batas; x++, k++)
                        {
                            for (int y = 0 - nilai_batas, l = 0; y < panjang_kernel - nilai_batas; y++, l++)
                            {
                                warna = gambar_hasil_sementara.GetPixel(i + x, j + y);
                                R = warna.R;
                                G = warna.G;
                                B = warna.B;

                                totalR += (kernel[k, l] * R);
                                totalG += (kernel[k, l] * G);
                                totalB += (kernel[k, l] * B);
                            }
                        }

                        if (totalR > 255)
                            totalR = 255;
                        else if (totalR < 0)
                            totalR = 0;
                        if (totalG > 255)
                            totalG = 255;
                        else if (totalG < 0)
                            totalG = 0;
                        if (totalB > 255)
                            totalB = 255;
                        else if (totalB < 0)
                            totalB = 0;

                        gambar_akhir.SetPixel(i - nilai_batas, j - nilai_batas, Color.FromArgb(totalR, totalG, totalB));
                    }
                }
                pictureBox2.Image = gambar_akhir;
            }
            else
            {
                int nilai_batas;
                nilai_batas = Convert.ToInt16(numericUpDown1.Value);
                nilai_batas = (nilai_batas - 1) / 2;

                gambar_akhir = new Bitmap(gambar_tmp.Width, gambar_tmp.Height);
                Color warna;
                int R, G, B, totalR, totalG, totalB;
                for (int i = ((panjang_kernel - 1) / 2); i < gambar_awal.Width - ((panjang_kernel - 1) / 2); i++)
                {
                    for (int j = ((panjang_kernel - 1) / 2); j < gambar_awal.Height - ((panjang_kernel - 1) / 2); j++)
                    {
                        totalR = 0;
                        totalG = 0;
                        totalB = 0;
                        for (int x = 0 - nilai_batas, k = 0; x < panjang_kernel - nilai_batas; x++, k++)
                        {
                            for (int y = 0 - nilai_batas, l = 0; y < panjang_kernel - nilai_batas; y++, l++)
                            {
                                warna = gambar_awal.GetPixel(i + x, j + y);
                                R = warna.R;
                                G = warna.G;
                                B = warna.B;

                                totalR += (kernel[k, l] * R);
                                totalG += (kernel[k, l] * G);
                                totalB += (kernel[k, l] * B);
                            }
                        }

                        if (totalR > 255)
                            totalR = 255;
                        else if (totalR < 0)
                            totalR = 0;
                        if (totalG > 255)
                            totalG = 255;
                        else if (totalG < 0)
                            totalG = 0;
                        if (totalB > 255)
                            totalB = 255;
                        else if (totalB < 0)
                            totalB = 0;

                        gambar_akhir.SetPixel(i - nilai_batas, j - nilai_batas, Color.FromArgb(totalR, totalG, totalB));
                    }
                }
                pictureBox2.Image = gambar_akhir;
            }
        }

        private void high_boost_filter_primitif()
        {
            int nilai_batas;
            nilai_batas = Convert.ToInt16(numericUpDown1.Value);
            nilai_batas = (nilai_batas - 1) / 2;
            bool status = true;

            for (int i = 0; i < panjang_kernel; i++)
            {
                for (int j = 0; j < panjang_kernel; j++)
                {
                    if ((i != ((panjang_kernel - 1) / 2)) && (j != ((panjang_kernel - 1) / 2)))
                    {
                        if(kernel[i,j]!=-1)
                        {
                            status = false;
                        }
                    }                    
                }
            }

            if (((kernel[((panjang_kernel - 1) / 2), ((panjang_kernel - 1) / 2)]) > ((panjang_kernel*panjang_kernel)-1)) && (status==true))
            {
                if(filter_standar != -1)
                {
                    gambar_akhir = new Bitmap(gambar_tmp.Width, gambar_tmp.Height);
                    Color warna;
                    int R, G, B, totalR, totalG, totalB;
                    for (int i = ((panjang_kernel - 1) / 2); i < gambar_awal.Width - ((panjang_kernel - 1) / 2); i++)
                    {
                        for (int j = ((panjang_kernel - 1) / 2); j < gambar_awal.Height - ((panjang_kernel - 1) / 2); j++)
                        {
                            totalR = 0;
                            totalG = 0;
                            totalB = 0;
                            for (int x = 0 - nilai_batas, k = 0; x < panjang_kernel - nilai_batas; x++, k++)
                            {
                                for (int y = 0 - nilai_batas, l = 0; y < panjang_kernel - nilai_batas; y++, l++)
                                {
                                    warna = gambar_hasil_sementara.GetPixel(i + x, j + y);
                                    R = warna.R;
                                    G = warna.G;
                                    B = warna.B;

                                    totalR += (kernel[k, l] * R);
                                    totalG += (kernel[k, l] * G);
                                    totalB += (kernel[k, l] * B);
                                }
                            }

                            if (totalR > 255)
                                totalR = 255;
                            else if (totalR < 0)
                                totalR = 0;
                            if (totalG > 255)
                                totalG = 255;
                            else if (totalG < 0)
                                totalG = 0;
                            if (totalB > 255)
                                totalB = 255;
                            else if (totalB < 0)
                                totalB = 0;

                            gambar_akhir.SetPixel(i - nilai_batas, j - nilai_batas, Color.FromArgb(totalR, totalG, totalB));
                        }
                    }
                    pictureBox2.Image = gambar_akhir;
                }
                else
                {
                    gambar_akhir = new Bitmap(gambar_tmp.Width, gambar_tmp.Height);
                    Color warna;
                    int R, G, B, totalR, totalG, totalB;
                    for (int i = ((panjang_kernel - 1) / 2); i < gambar_awal.Width - ((panjang_kernel - 1) / 2); i++)
                    {
                        for (int j = ((panjang_kernel - 1) / 2); j < gambar_awal.Height - ((panjang_kernel - 1) / 2); j++)
                        {
                            totalR = 0;
                            totalG = 0;
                            totalB = 0;
                            for (int x = 0 - nilai_batas, k = 0; x < panjang_kernel - nilai_batas; x++, k++)
                            {
                                for (int y = 0 - nilai_batas, l = 0; y < panjang_kernel - nilai_batas; y++, l++)
                                {
                                    warna = gambar_awal.GetPixel(i + x, j + y);
                                    R = warna.R;
                                    G = warna.G;
                                    B = warna.B;

                                    totalR += (kernel[k, l] * R);
                                    totalG += (kernel[k, l] * G);
                                    totalB += (kernel[k, l] * B);
                                }
                            }

                            if (totalR > 255)
                                totalR = 255;
                            else if (totalR < 0)
                                totalR = 0;
                            if (totalG > 255)
                                totalG = 255;
                            else if (totalG < 0)
                                totalG = 0;
                            if (totalB > 255)
                                totalB = 255;
                            else if (totalB < 0)
                                totalB = 0;

                            gambar_akhir.SetPixel(i - nilai_batas, j - nilai_batas, Color.FromArgb(totalR, totalG, totalB));
                        }
                    }
                    pictureBox2.Image = gambar_akhir;
                }
            }
            else
            {
                MessageBox.Show("Silahkan input kernel sesuai dengan aturan Hight bost Filter!");
            }         
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                /*dataGridView1.Rows.Clear();
                dataGridView1.Refresh();

                int panjang;
                //double tmp;

                panjang_kernel = (int)numericUpDown1.Value;
                panjang = dataGridView1.Width / panjang_kernel;
                dataGridView1.ColumnCount = panjang_kernel;

                for (int i = 0; i < panjang_kernel; i++)
                {
                    var baris = new DataGridViewRow();
                    DataGridViewColumn kolom = dataGridView1.Columns[i];
                    kolom.Width = panjang;
                    dataGridView1.Rows.Add(baris);
                }
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToResizeRows = false;
                dataGridView1.AllowUserToResizeColumns = false;*/
                if(filter_advanced==1)
                {
                    kernelLowPassAwal();
                }
                else if(filter_advanced==2)
                {
                    kernelHighPassAwal();
                }
                else if(filter_advanced==3)
                {
                    kernelHighBoostAwal();
                }
            }
            catch
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                kernel = new int[panjang_kernel, panjang_kernel];

                for(int i=0;i<panjang_kernel;i++)
                {
                    for (int j = 0; j < panjang_kernel; j++)
                    {
                        kernel[i, j] = Convert.ToInt16(dataGridView1.Rows[i].Cells[j].Value);
                    }
                }
                MessageBox.Show("Data BERHASIL berhasil dimasukkan !");
            }
            catch
            {
                MessageBox.Show("Data TIDAK berhasil dimasukkan !");
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                pictureBox1.Width = gambar_tmp.Width;
                pictureBox1.Height = gambar_tmp.Height;

                pictureBox1.Width += (Convert.ToInt16(trackBar1.Value) * pictureBox1.Width) / skala_pembesaran;
                pictureBox1.Height += (Convert.ToInt16(trackBar1.Value) * pictureBox1.Height) / skala_pembesaran;
            }
            catch
            {

            }          
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            try
            {
                pictureBox2.Width = gambar_akhir.Width;
                pictureBox2.Height = gambar_akhir.Height;

                pictureBox2.Width += (Convert.ToInt16(trackBar2.Value) * pictureBox2.Width) / skala_pembesaran;
                pictureBox2.Height += (Convert.ToInt16(trackBar2.Value) * pictureBox2.Height) / skala_pembesaran;
            }
            catch
            {

            }           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("* Rasio pembesaran gambar merupakan rasio\n   pembesaran width dan height pada gambar.\n\n* Semakin tinggi rasio maka pembesaran\n   gambar ketika di scroll menjadi melambat.\n\n* Begitu juga sebaliknya\n\n* Skala pembesaran dari 1 sampai 10.000",
                                  "Rasio Pembesaran Gambar", MessageBoxButtons.OK,
                                  MessageBoxIcon.Information,
                                  0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Dibuat Oleh :\n\n1. I Wayan Ariantha Sentanu\t[1308605009]\n2. I Gede Pramarta Sedana\t[1308605027]\n3. I Putu Agus Suarya Wibawa\t[1308605034]\n4. Daniel Kurniawan\t\t[1308605039]",
                                  "Tentang Kami", MessageBoxButtons.OK,
                                  MessageBoxIcon.Information,
                                  0);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            skala_pembesaran = trackBar3.Value;
            MessageBox.Show("Skala pembesaran disimpan !",
                                  "Berhasil", MessageBoxButtons.OK,
                                  MessageBoxIcon.Information,
                                  0);
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = Convert.ToString(trackBar3.Value);
        }        

        private void kernelLowPassAwal()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            int panjang;
            //panjang_kernel = 3;
            panjang_kernel = (int)numericUpDown1.Value;
            panjang = dataGridView1.Width / panjang_kernel;
            dataGridView1.ColumnCount = panjang_kernel;
            for (int i = 0; i < panjang_kernel; i++)
            {
                var baris = new DataGridViewRow();
                DataGridViewColumn kolom = dataGridView1.Columns[i];
                kolom.Width = panjang;
                dataGridView1.Rows.Add();
            }

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;

            kernel = new int[panjang_kernel, panjang_kernel];
            for (int i = 0; i < panjang_kernel; i++)
            {
                for (int j = 0; j < panjang_kernel; j++)
                {
                    kernel[i,j] = Convert.ToInt16(dataGridView1[i, j].Value = 1);
                }
            }
        }

        private void kernelHighPassAwal()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            int panjang;
            //panjang_kernel = 3;
            panjang_kernel = (int)numericUpDown1.Value;
            panjang = dataGridView1.Width / panjang_kernel;
            dataGridView1.ColumnCount = panjang_kernel;
            for (int i = 0; i < panjang_kernel; i++)
            {
                var baris = new DataGridViewRow();
                DataGridViewColumn kolom = dataGridView1.Columns[i];
                kolom.Width = panjang;
                dataGridView1.Rows.Add();
            }

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;

            kernel = new int[panjang_kernel, panjang_kernel];
            for (int i = 0; i < panjang_kernel; i++)
            {
                for (int j = 0; j < panjang_kernel; j++)
                {
                    if (i == ((panjang_kernel - 1) / 2) && j == ((panjang_kernel - 1) / 2))
                    {
                        kernel[i, j] = Convert.ToInt16(dataGridView1[i, j].Value = 8);
                    }
                    else
                    {
                        kernel[i, j] = Convert.ToInt16(dataGridView1[i, j].Value = -1);
                    }
                }
            }
        }

        private void kernelHighBoostAwal()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            int panjang;
            //panjang_kernel = 3;
            panjang_kernel = (int)numericUpDown1.Value;
            panjang = dataGridView1.Width / panjang_kernel;
            dataGridView1.ColumnCount = panjang_kernel;
            for (int i = 0; i < panjang_kernel; i++)
            {
                var baris = new DataGridViewRow();
                DataGridViewColumn kolom = dataGridView1.Columns[i];
                kolom.Width = panjang;
                dataGridView1.Rows.Add();
            }

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;

            kernel = new int[panjang_kernel, panjang_kernel];
            for (int i = 0; i < panjang_kernel; i++)
            {
                for (int j = 0; j < panjang_kernel; j++)
                {
                    if (i == ((panjang_kernel - 1) / 2) && j == ((panjang_kernel - 1) / 2))
                    {
                        kernel[i, j] = Convert.ToInt16(dataGridView1[i, j].Value = ((panjang_kernel * panjang_kernel) - 1) +1);
                    }
                    else
                    {
                        kernel[i, j] = Convert.ToInt16(dataGridView1[i, j].Value = -1);
                    }
                }
            }
        }

        private void low_pass_filter_emgu()
        {
            int sum_matrik = 0, nilai_batas; //jumlahkan semua kernel
            nilai_batas = Convert.ToInt16(numericUpDown1.Value);
            nilai_batas = (nilai_batas - 1) / 2;
            for (int i = 0; i < panjang_kernel; i++)
            {
                for (int j = 0; j < panjang_kernel; j++)
                {
                    sum_matrik += kernel[i, j];
                }
            }

            if (sum_matrik != 0)
            {
                if (filter_standar != -1)
                {
                    gambar_akhir_e = new Image<Bgr, byte>(gambar_tmp.Width, gambar_tmp.Height);
                    int R, G, B, totalR, totalG, totalB;
                    for (int i = ((panjang_kernel - 1) / 2); i < gambar_awal.Width - ((panjang_kernel - 1) / 2); i++)
                    {
                        for (int j = ((panjang_kernel - 1) / 2); j < gambar_awal.Height - ((panjang_kernel - 1) / 2); j++)
                        {
                            totalR = 0;
                            totalG = 0;
                            totalB = 0;
                            for (int x = 0 - nilai_batas, k = 0; x < panjang_kernel - nilai_batas; x++, k++)
                            {
                                for (int y = 0 - nilai_batas, l = 0; y < panjang_kernel - nilai_batas; y++, l++)
                                {
                                    B = gambar_hasil_sementara_e.Data[j + y, i + x, 0];
                                    G = gambar_hasil_sementara_e.Data[j + y, i + x, 1];
                                    R = gambar_hasil_sementara_e.Data[j + y, i + x, 2];

                                    totalR += (kernel[k, l] * R);
                                    totalG += (kernel[k, l] * G);
                                    totalB += (kernel[k, l] * B);
                                }
                            }

                            totalR /= sum_matrik;
                            totalG /= sum_matrik;
                            totalB /= sum_matrik;

                            /*if (totalR > 255)
                                totalR = 255;
                            else if (totalR < 0)
                                totalR = 0;
                            if (totalG > 255)
                                totalG = 255;
                            else if (totalG < 0)
                                totalG = 0;
                            if (totalB > 255)
                                totalB = 255;
                            else if (totalB < 0)
                                totalB = 0;*/

                            gambar_akhir_e.Data[j - nilai_batas, i - nilai_batas, 0] = (byte)totalB;
                            gambar_akhir_e.Data[j - nilai_batas, i - nilai_batas, 1] = (byte)totalG;
                            gambar_akhir_e.Data[j - nilai_batas, i - nilai_batas, 2] = (byte)totalR;
                        }
                    }
                    pictureBox2.Image = gambar_akhir_e.ToBitmap();
                }
                else
                {
                    gambar_akhir_e = new Image<Bgr, byte>(gambar_tmp.Width, gambar_tmp.Height);
                    int R, G, B, totalR, totalG, totalB;
                    for (int i = ((panjang_kernel - 1) / 2); i < gambar_awal.Width - ((panjang_kernel - 1) / 2); i++)
                    {
                        for (int j = ((panjang_kernel - 1) / 2); j < gambar_awal.Height - ((panjang_kernel - 1) / 2); j++)
                        {
                            totalR = 0;
                            totalG = 0;
                            totalB = 0;
                            for (int x = 0 - nilai_batas, k = 0; x < panjang_kernel - nilai_batas; x++, k++)
                            {
                                for (int y = 0 - nilai_batas, l = 0; y < panjang_kernel - nilai_batas; y++, l++)
                                {
                                    B = gambar_awal_e.Data[j + y, i + x, 0];
                                    G = gambar_awal_e.Data[j + y, i + x, 1];
                                    R = gambar_awal_e.Data[j + y, i + x, 2];

                                    totalR += (kernel[k, l] * R);
                                    totalG += (kernel[k, l] * G);
                                    totalB += (kernel[k, l] * B);
                                }
                            }

                            totalR /= sum_matrik;
                            totalG /= sum_matrik;
                            totalB /= sum_matrik;

                            /*if (totalR > 255)
                                totalR = 255;
                            else if (totalR < 0)
                                totalR = 0;
                            if (totalG > 255)
                                totalG = 255;
                            else if (totalG < 0)
                                totalG = 0;
                            if (totalB > 255)
                                totalB = 255;
                            else if (totalB < 0)
                                totalB = 0;*/

                            gambar_akhir_e.Data[j - nilai_batas, i - nilai_batas, 0] = (byte)totalB;
                            gambar_akhir_e.Data[j - nilai_batas, i - nilai_batas, 1] = (byte)totalG;
                            gambar_akhir_e.Data[j - nilai_batas, i - nilai_batas, 2] = (byte)totalR;
                        }
                    }
                    pictureBox2.Image = gambar_akhir_e.ToBitmap();
                }
            }
            else
            {
                MessageBox.Show("Silahkan input kernel sesuai dengan aturan Low Pass Filter!");
            }
        }

        private void high_pass_filter_emgu()
        {
            if (filter_standar != -1)
            {
                int nilai_batas;
                nilai_batas = Convert.ToInt16(numericUpDown1.Value);
                nilai_batas = (nilai_batas - 1) / 2;

                gambar_akhir_e = new Image<Bgr, byte>(gambar_tmp.Width, gambar_tmp.Height);

                Color warna;
                int R, G, B, totalR, totalG, totalB;
                for (int i = ((panjang_kernel - 1) / 2); i < gambar_awal.Width - ((panjang_kernel - 1) / 2); i++)
                {
                    for (int j = ((panjang_kernel - 1) / 2); j < gambar_awal.Height - ((panjang_kernel - 1) / 2); j++)
                    {
                        totalR = 0;
                        totalG = 0;
                        totalB = 0;
                        for (int x = 0 - nilai_batas, k = 0; x < panjang_kernel - nilai_batas; x++, k++)
                        {
                            for (int y = 0 - nilai_batas, l = 0; y < panjang_kernel - nilai_batas; y++, l++)
                            {
                                B = gambar_hasil_sementara_e.Data[j + y, i + x, 0];
                                G = gambar_hasil_sementara_e.Data[j + y, i + x, 1];
                                R = gambar_hasil_sementara_e.Data[j + y, i + x, 2];

                                totalR += (kernel[k, l] * R);
                                totalG += (kernel[k, l] * G);
                                totalB += (kernel[k, l] * B);
                            }
                        }

                        if (totalR > 255)
                            totalR = 255;
                        else if (totalR < 0)
                            totalR = 0;
                        if (totalG > 255)
                            totalG = 255;
                        else if (totalG < 0)
                            totalG = 0;
                        if (totalB > 255)
                            totalB = 255;
                        else if (totalB < 0)
                            totalB = 0;

                        gambar_akhir_e.Data[j - nilai_batas, i - nilai_batas, 0] = (byte)totalB;
                        gambar_akhir_e.Data[j - nilai_batas, i - nilai_batas, 1] = (byte)totalG;
                        gambar_akhir_e.Data[j - nilai_batas, i - nilai_batas, 2] = (byte)totalR;
                    }
                }
                pictureBox2.Image = gambar_akhir_e.ToBitmap();
            }
            else
            {
                int nilai_batas;
                nilai_batas = Convert.ToInt16(numericUpDown1.Value);
                nilai_batas = (nilai_batas - 1) / 2;

                gambar_akhir_e = new Image<Bgr, byte>(gambar_tmp.Width, gambar_tmp.Height);

                Color warna;
                int R, G, B, totalR, totalG, totalB;
                for (int i = ((panjang_kernel - 1) / 2); i < gambar_awal.Width - ((panjang_kernel - 1) / 2); i++)
                {
                    for (int j = ((panjang_kernel - 1) / 2); j < gambar_awal.Height - ((panjang_kernel - 1) / 2); j++)
                    {
                        totalR = 0;
                        totalG = 0;
                        totalB = 0;
                        for (int x = 0 - nilai_batas, k = 0; x < panjang_kernel - nilai_batas; x++, k++)
                        {
                            for (int y = 0 - nilai_batas, l = 0; y < panjang_kernel - nilai_batas; y++, l++)
                            {
                                B = gambar_awal_e.Data[j + y, i + x, 0];
                                G = gambar_awal_e.Data[j + y, i + x, 1];
                                R = gambar_awal_e.Data[j + y, i + x, 2];

                                totalR += (kernel[k, l] * R);
                                totalG += (kernel[k, l] * G);
                                totalB += (kernel[k, l] * B);
                            }
                        }

                        if (totalR > 255)
                            totalR = 255;
                        else if (totalR < 0)
                            totalR = 0;
                        if (totalG > 255)
                            totalG = 255;
                        else if (totalG < 0)
                            totalG = 0;
                        if (totalB > 255)
                            totalB = 255;
                        else if (totalB < 0)
                            totalB = 0;

                        gambar_akhir_e.Data[j - nilai_batas, i - nilai_batas, 0] = (byte)totalB;
                        gambar_akhir_e.Data[j - nilai_batas, i - nilai_batas, 1] = (byte)totalG;
                        gambar_akhir_e.Data[j - nilai_batas, i - nilai_batas, 2] = (byte)totalR;
                    }
                }
                pictureBox2.Image = gambar_akhir_e.ToBitmap();
            }
        }

        private void high_boost_filter_emgu()
        {
            int nilai_batas;
            nilai_batas = Convert.ToInt16(numericUpDown1.Value);
            nilai_batas = (nilai_batas - 1) / 2;
            bool status = true;

            for (int i = 0; i < panjang_kernel; i++)
            {
                for (int j = 0; j < panjang_kernel; j++)
                {
                    if ((i != ((panjang_kernel - 1) / 2)) && (j != ((panjang_kernel - 1) / 2)))
                    {
                        if (kernel[i, j] != -1)
                        {
                            status = false;
                        }
                    }
                }
            }

            if (((kernel[((panjang_kernel - 1) / 2), ((panjang_kernel - 1) / 2)]) > ((panjang_kernel * panjang_kernel) - 1)) && (status == true))
            {
                if (filter_standar != -1)
                {
                    gambar_akhir_e = new Image<Bgr, byte>(gambar_tmp.Width, gambar_tmp.Height);

                    Color warna;
                    int R, G, B, totalR, totalG, totalB;
                    for (int i = ((panjang_kernel - 1) / 2); i < gambar_awal.Width - ((panjang_kernel - 1) / 2); i++)
                    {
                        for (int j = ((panjang_kernel - 1) / 2); j < gambar_awal.Height - ((panjang_kernel - 1) / 2); j++)
                        {
                            totalR = 0;
                            totalG = 0;
                            totalB = 0;
                            for (int x = 0 - nilai_batas, k = 0; x < panjang_kernel - nilai_batas; x++, k++)
                            {
                                for (int y = 0 - nilai_batas, l = 0; y < panjang_kernel - nilai_batas; y++, l++)
                                {
                                    B = gambar_hasil_sementara_e.Data[j + y, i + x, 0];
                                    G = gambar_hasil_sementara_e.Data[j + y, i + x, 1];
                                    R = gambar_hasil_sementara_e.Data[j + y, i + x, 2];

                                    totalR += (kernel[k, l] * R);
                                    totalG += (kernel[k, l] * G);
                                    totalB += (kernel[k, l] * B);
                                }
                            }

                            if (totalR > 255)
                                totalR = 255;
                            else if (totalR < 0)
                                totalR = 0;
                            if (totalG > 255)
                                totalG = 255;
                            else if (totalG < 0)
                                totalG = 0;
                            if (totalB > 255)
                                totalB = 255;
                            else if (totalB < 0)
                                totalB = 0;

                            gambar_akhir_e.Data[j - nilai_batas, i - nilai_batas, 0] = (byte)totalB;
                            gambar_akhir_e.Data[j - nilai_batas, i - nilai_batas, 1] = (byte)totalG;
                            gambar_akhir_e.Data[j - nilai_batas, i - nilai_batas, 2] = (byte)totalR;
                        }
                    }
                    pictureBox2.Image = gambar_akhir_e.ToBitmap();
                }
                else
                {
                    gambar_akhir_e = new Image<Bgr, byte>(gambar_tmp.Width, gambar_tmp.Height);

                    Color warna;
                    int R, G, B, totalR, totalG, totalB;
                    for (int i = ((panjang_kernel - 1) / 2); i < gambar_awal.Width - ((panjang_kernel - 1) / 2); i++)
                    {
                        for (int j = ((panjang_kernel - 1) / 2); j < gambar_awal.Height - ((panjang_kernel - 1) / 2); j++)
                        {
                            totalR = 0;
                            totalG = 0;
                            totalB = 0;
                            for (int x = 0 - nilai_batas, k = 0; x < panjang_kernel - nilai_batas; x++, k++)
                            {
                                for (int y = 0 - nilai_batas, l = 0; y < panjang_kernel - nilai_batas; y++, l++)
                                {
                                    B = gambar_awal_e.Data[j + y, i + x, 0];
                                    G = gambar_awal_e.Data[j + y, i + x, 1];
                                    R = gambar_awal_e.Data[j + y, i + x, 2];

                                    totalR += (kernel[k, l] * R);
                                    totalG += (kernel[k, l] * G);
                                    totalB += (kernel[k, l] * B);
                                }
                            }

                            if (totalR > 255)
                                totalR = 255;
                            else if (totalR < 0)
                                totalR = 0;
                            if (totalG > 255)
                                totalG = 255;
                            else if (totalG < 0)
                                totalG = 0;
                            if (totalB > 255)
                                totalB = 255;
                            else if (totalB < 0)
                                totalB = 0;

                            gambar_akhir_e.Data[j - nilai_batas, i - nilai_batas, 0] = (byte)totalB;
                            gambar_akhir_e.Data[j - nilai_batas, i - nilai_batas, 1] = (byte)totalG;
                            gambar_akhir_e.Data[j - nilai_batas, i - nilai_batas, 2] = (byte)totalR;
                        }
                    }
                    pictureBox2.Image = gambar_akhir_e.ToBitmap();
                }
            }
            else
            {
                MessageBox.Show("Silahkan input kernel sesuai dengan aturan Hight bost Filter!");
            }  
        }
   
    }
}
