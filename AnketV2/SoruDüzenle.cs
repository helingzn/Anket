using DAL;
using Domain_Entity.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnketV2
{
    public partial class SoruDüzenle : Form
    {
        public Soru GelenSoru { get; set; }
        public SoruDüzenle()
        {
            InitializeComponent();
        }

        private void SoruDüzenle_Load(object sender, EventArgs e)
        {
            textBox2.Text = GelenSoru.SoruCümlesi;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //EF bir kayıt değişiklik yapılabilmesi için context üzerinden geliyorsa mümkün
            AnketContext db = new AnketContext();
            var düzenlenecek = db.Sorular.Find(GelenSoru.SoruID);
            düzenlenecek.SoruCümlesi = textBox2.Text;
            db.Entry(düzenlenecek).State =EntityState.Modified;
            db.SaveChanges();
            Form1 f = (Form1)Application.OpenForms["Form1"];
            f.SoruYenile();
            f.CevaplarıYenile();
            
        }
    }
}
