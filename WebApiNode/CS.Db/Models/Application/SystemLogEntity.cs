using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CS.Db.Models.Application
{
    [Table("SystemLog")]
    public class SystemLogEntity
    {
        [Key]
        public int ID { get; set; }
    
        
        public string UrlRequest { get; set; }


        public DateTime DateCreate { get; set; }
    }
}
