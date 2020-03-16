using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CS.Db.Models.Rest
{
    [Table("RestUser")]
    public class RestUserEntity
    {
        [Key]
        public int ID { get; set; }

      
        [MaxLength(128)]
        public string Name { get; set; }


        public string Description { get; set; }

        [MaxLength(128)]
        public string AuthKey { get; set; }



        public bool IsActive { get; set; }

        /// <summary>
        /// список разрешенных IP, с которых можно обращаться.
        /// если значение пустое разрешаем со всех
        /// </summary>
        public string WhiteListIp { get; set; }
    

    }
}
