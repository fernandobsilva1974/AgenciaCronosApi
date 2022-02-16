using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AgenciaCronosApi.Models
{
    [Table("tb_posts")]
    public class Posts
    {
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("excluido")]
        public bool Excluido { get; set; } = false;

        [Column("numero")]
        public int? Numero { get; set; }

        [Column("titulo")]
        public string Titulo { get; set; }

        [Column("descritivo")]
        public string Descritivo { get; set; }

     
        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; }
    }
}
