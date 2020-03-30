using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaVet.Models {
   public class Veterinarios {

      public Veterinarios() {
         // inicializar a lista de consultas efetuadas por um veterinário
         Consultas = new HashSet<Consultas>();
      }

      [Key]
      public int ID { get; set; }

      [Required(ErrorMessage ="O Nome é de preenchimento obrigatório")]
      [StringLength(40,ErrorMessage ="O {0} só pode ter, no máximo, {1} carateres.")]
      public string Nome { get; set; }

      [Required]
      public string NumCedulaProf { get; set; }

      [Required]
      public string Foto { get; set; }

      /// <summary>
      ///  lista de 'consultas' a que o Veterinário está associado
      /// </summary>
      public ICollection<Consultas> Consultas { get; set; }

   }
}
