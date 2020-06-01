using ClinicaVet.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaVet.Models {
   public class Veterinarios {

      public Veterinarios() {
         // inicializar a lista de consultas efetuadas por um veterinário
         Consultas = new HashSet<Consultas>();
      }

      // System.ComponentModel.DataAnnotations Namespace
      // https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=netcore-3.1


      // [Key]
      public int ID { get; set; }

      [Required(ErrorMessage = "O Nome é de preenchimento obrigatório")]
      [StringLength(40, ErrorMessage = "O {0} só pode ter, no máximo, {1} carateres.")]
      [RegularExpression("[A-ZÁÍÓÚÉÂ][a-zãõáéíóúàèìòùäëïöüçâêîôû]+" +
         "(( | e |-|'| d'| de | d[ao](s)? )[A-ZÁÍÓÚÉÂ][a-zãõáéíóúàèìòùäëïöüçâêîôû]+){1,3}",
            ErrorMessage = "Só são aceites letras. Cada palavra deve começar por uma Maiúscula, separadas por um espaço em branco.")]
      public string Nome { get; set; }

      [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
      // vet-XXXXXX  ---> a palavra VET, um hífen, seguido de 6 digitos
      [RegularExpression("vet-[0-9]{6}", ErrorMessage = "Deve introduzir a palavra 'vet-' (em minúsculas), seguida de 6 dígitos.")]
      [StringLength(10, ErrorMessage = "O {0} só pode ter, no máximo, {1} carateres.")]
      [Display(Name = "Nº Cédula Profissional")]
      public string NumCedulaProf { get; set; }

      // [Required]
      public string Foto { get; set; }

      /// <summary>
      ///  lista de 'consultas' a que o Veterinário está associado
      /// </summary>
      public virtual ICollection<Consultas> Consultas { get; set; }

      // ******************************************************************


      /// <summary>
      /// Relaciona os dados do Veterinário com a pessoa que se autentica
      /// </summary>
      [ForeignKey(nameof(Utilizador))]
      public int? UtilizadorFK { get; set; }
      public Utilizadores Utilizador { get; set; }



   }
}
