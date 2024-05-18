using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace kutuphane_otomasyou.Models.table.kitaplar
{
    [Table("KitapTuru")]
    public class KitapTuru
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [StringLength(50), Required]
        public string tur_adi { get; set; }
    }
}