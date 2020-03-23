using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaVet.Models {
   public class Consultas {

      [Key] // anotação que força este atributo a ser PK. Mas, não seria necessário, pq o atributo chama-se 'ID'
      public int ID { get; set; }
      
      public DateTime Data { get; set; }

      public string Observacoes { get; set; }

      // criar as FK
      [ForeignKey("Veterinario")]
      public int VeterinarioFK { get; set; }
      public Veterinarios Veterinario { get; set; }

      [ForeignKey(nameof(Animal))]
      public int AnimalFK { get; set; }
      public Animais Animal { get; set; }
   }
}
