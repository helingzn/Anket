using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Entity.models
{
    [Table("tbl_Sorular")]
    public class Soru
    {
        [Key]
        public int SoruID { get; set; }
        [Required] //soru cümlesi zorunlu
        public string SoruCümlesi { get; set; }
    }
    [Table("tbl_Cevaplar")]
    public class Cevap
    {
        [Key]
        public int CevapID { get; set; }
        [ForeignKey("CevabıVerenKisi")]
        public int KişiID { get; set; }
        [ForeignKey("Soru")]
        public int SoruId { get; set; }
        [Required]
        public Yanıt Yanıt { get; set; }
        public virtual Kişi CevabıVerenKisi { get; set; }
        public virtual Soru Soru { get; set; } //navigation Property
        

    }
    [Table("tbl_Kişiler")]
    public class Kişi
    {
        [Key]
        public int KişiID { get; set; }
        [Required]
        public string AdSoyad { get; set; }
    }

    public enum Yanıt
    { Hayır,Evet}
}
