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
        Bitmap gambar_awal, gambar_akhir, gambar_tmp;
        Image<Bgr, Byte> gambar_awal_e, gambar_akhir_e;
        int mode, filter_standar, filter_advanced, panjang_kernel;
        int[,] kernel;


        public Form1()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;

            radioButton9.Checked = true;
            mode = 1;
            filter_standar = -1;
            filter_advanced = -1;
            panjang_kernel = 0;

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

                gambar_tmp = new Bitmap(new Bitmap(pilih_gambar.FileName));
                gambar_awal = new Bitmap(gambar_tmp.Width + ((int)numericUpDown1.Value - 1), gambar_tmp.Height + ((int)numericUpDown1.Value - 1));
                gambar_akhir = new Bitmap(new Bitmap(pilih_gambar.FileName));

                int r, g, b;

                for (int i = 0; i < gambar_awal.Width; i++)
                {
                    for (int j = 0; j < gambar_awal.Height; j++)
                    {
                        if(i < (((int)numericUpDown1.Value - 1) / 2))//pemberian nilai 0
                        {
                            gambar_awal.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                        }
                        else if(j < (((int)numericUpDown1.Value - 1) / 2)) //pemberian nilai 0
                        {
                            gambar_awal.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                        }
                        else if(j >= (gambar_awal.Height - (((int)numericUpDown1.Value - 1) / 2)))
                        {
                            gambar_awal.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                        }
                        else if (i >= (gambar_awal.Width - (((int)numericUpDown1.Value - 1) / 2)))
                        {
                            gambar_awal.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                        }
                        else
                        {
                            r = gambar_tmp.GetPixel(i - (((int)numericUpDown1.Value - 1) / 2), j - (((int)numericUpDown1.Value - 1) / 2)).R;
                            g = gambar_tmp.GetPixel(i - (((int)numericUpDown1.Value - 1) / 2), j - (((int)numericUpDown1.Value - 1) / 2)).G;
                            b = gambar_tmp.GetPixel(i - (((int)numericUpDown1.Value - 1) / 2), j - (((int)numericUpDown1.Value - 1) / 2)).B;

                            gambar_awal.SetPixel(i, j, Color.FromArgb(r, g, b));
                        }
                    }
                }
                
                pictureBox1.Image = gambar_tmp;
                pictureBox2.Image = gambar_awal;
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

                    gambar_akhir.SetPixel(i - nilai_batas, j - nilai_batas, Color.FromArgb(r, g, b));
                }
            }
            pictureBox2.Image = (Bitmap)gambar_akhir;
        }

        private void filter_median_primitif()
        {
            //kodene...
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
        }

        private void filter_pererataan_emgu()
        {
            //kodene...
        }

        private void filter_median_emgu()
        {
            //kodene...
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();

                int panjang;
                //double tmp;

                panjang_kernel = (int)numericUpDown1.Value;
                panjang = dataGridView1.Width / (int)numericUpDown1.Value;
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
                dataGridView1.AllowUserToResizeColumns = false;
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
                        MessageBox.Show(Convert.ToString(kernel[i, j]));
                    }
                }
                MessageBox.Show("Data BERHASIL berhasil dimasukkan !");
            }
            catch
            {
                MessageBox.Show("Data TIDAK berhasil dimasukkan !");
            }
        }        
    }
}
