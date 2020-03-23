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

      public string Nome { get; set; }

      public string NumCedulaProf { get; set; }

      public string Foto { get; set; }

      /// <summary>
      ///  lista de 'consultas' a que o Veterinário está associado
      /// </summary>
      public ICollection<Consultas> Consultas { get; set; }

   }
}
