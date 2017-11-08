using DAL;
using Domain_Entity.models;
using Domain_Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnketV2
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        AnketContext db = new AnketContext();
        private void button2_Click(object sender, EventArgs e)
        {
            Soru soru = new Soru();
            soru.SoruCümlesi = textBox2.Text;
            db.Sorular.Add(soru);
            db.SaveChanges();
            SoruYenile();

        }

        public void Form1_Load(object sender, EventArgs e)
        {
            SoruYenile();
            CevaplarıYenile();
        }
        public void SoruYenile()
        {
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = db.Sorular.ToList();
            flowLayoutPanel1.Controls.Clear();
            foreach (Soru item in db.Sorular)
            {
                Label lbl = new Label();
                lbl.AutoSize = true;
                lbl.Text = item.SoruCümlesi;
                flowLayoutPanel1.Controls.Add(lbl);
                //flowLayoutPanel1.SetFlowBreak(lbl, true);


                RadioButton r1 = new RadioButton();
                r1.Name = "Soru_" + item.SoruID;
                r1.Text = "Evet";


                RadioButton r2 = new RadioButton();
                r2.Name = "Soru_" + item.SoruID;
                r2.Text = "Hayır";

                FlowLayoutPanel p = new FlowLayoutPanel();
                p.Controls.Add(r1);
                p.Controls.Add(r2);
                flowLayoutPanel1.Controls.Add(p);
                flowLayoutPanel1.SetFlowBreak(p, true);
                //ComboBox c1 = new ComboBox();
                //c1.Items.Add("Evet");
                //c1.Items.Add("Hayır");
                //flowLayoutPanel1.Controls.Add(c1);
                //flowLayoutPanel1.SetFlowBreak(c1, true);

            }
        }

        public void CevaplarıYenile()
        {
            dataGridView1.DataSource = null;
            //dataGridView1.DataSource = db.Cevaplar.ToList();
            dataGridView1.DataSource = db.Cevaplar.Select(x => new CevapViewModel()
            {
                AdSoyad = x.CevabıVerenKisi.AdSoyad,
                Soru = x.Soru.SoruCümlesi,
                Cevap = x.Yanıt.ToString()
            }).ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {   //combobox
            //foreach(Control item in flowLayoutPanel1.Controls)
            //{
            //    if(item is ComboBox)
            //    { Soru_15 -> 15

            //string soruID=item.Rame.Replace("Soru_","")
            //       }
            //}


            //radiobtn
            foreach (Control pnl in flowLayoutPanel1.Controls)
            {
                if (pnl is FlowLayoutPanel)
                {
                    foreach (RadioButton item in ((FlowLayoutPanel)pnl).Controls)
                    {

                        RadioButton r = item;
                        if (r.Checked)
                        {
                            string soruID = item.Name.Replace("Soru_", "");
                            int SID = Convert.ToInt32(soruID);
                            Cevap c = new Cevap();
                            c.SoruId = SID;
                            c.Yanıt = r.Text == "Evet" ? Yanıt.Evet : Yanıt.Hayır;

                            Kişi k = db.Kişiler.Where(x => x.AdSoyad == textBox1.Text).FirstOrDefault();
                            if (k != null)
                                c.KişiID = k.KişiID;
                            else
                            {
                                k = new Kişi();
                                k.AdSoyad = textBox1.Text;
                                db.Kişiler.Add(k);
                                db.SaveChanges();
                                c.KişiID = k.KişiID;
                            }
                            db.Cevaplar.Add(c);
                            db.SaveChanges();
                        }
                    }
                }
            }

            CevaplarıYenile();
        }

        private void btn_sil_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 0)
                MessageBox.Show("Soru seçiniz");
            else
            {
                foreach (DataGridViewRow item in dataGridView2.SelectedRows)
                {
                    int soruID = (int)item.Cells[0].Value;
                    Soru silinecek = db.Sorular.Find(soruID);
                    db.Sorular.Remove(silinecek);
                }
                db.SaveChanges();
                SoruYenile();
            }

        }

        private void btn_düzenle_Click(object sender, EventArgs e)
        {

            if (dataGridView2.SelectedRows.Count == 0)
                MessageBox.Show("soru seçiniz");
            else
            {
                SoruDüzenle ds = new SoruDüzenle();
                int sID = (int)dataGridView2.SelectedRows[0].Cells[0].Value;
                Soru düzenlenecek = db.Sorular.Find(sID);
                ds.GelenSoru = düzenlenecek;
                ds.Show();

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
                MessageBox.Show("Cevap seçiniz");
            else
            {
                List<Cevap> silinecekler = new List<Cevap>();
                foreach(DataGridViewRow item in dataGridView1.SelectedRows)
                {
                    var silinecek = db.Cevaplar.ToList()[item.Index];
                    silinecekler.Add(silinecek);
                }
                db.Cevaplar.RemoveRange(silinecekler);
                db.SaveChanges();
            }
            SoruYenile();
            CevaplarıYenile();
        }
    }
}
