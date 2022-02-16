using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AgenciaCronosApi.Models
{
    [Table("tb_integrantes")]
    public class Integrantes
    {
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("excluido")]
        public bool Excluido { get; set; } = false;

        [Column("nome")]
        public string Nome { get; set; }

        [Column("cpf")]
        public string Cpf { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("celular")]
        public string Celular { get; set; }
               

        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; }
    }
}
