using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AgenciaCronosApi.Models
{
    [Table("tb_servicos")]
    public class Servicos
    {
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("excluido")]
        public bool Excluido { get; set; } = false;

        [Column("cod_servico")]
        public string CodServico { get; set; }

        [Column("desc_servico")]
        public string DescServico { get; set; }
              
        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; }
    }
}
