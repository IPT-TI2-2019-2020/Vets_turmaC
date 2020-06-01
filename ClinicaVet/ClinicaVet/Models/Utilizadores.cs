using ClinicaVet.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaVet.Models {
   public class Utilizadores {

      [Key]
      public int IdUtilizadores { get; set; }

      [Required(ErrorMessage = "O Nome é de preenchimento obrigatório")]
      [StringLength(40, ErrorMessage = "O {0} só pode ter, no máximo, {1} carateres.")]
      [RegularExpression("[A-ZÁÍÓÚÉÂ][a-zãõáéíóúàèìòùäëïöüçâêîôû]+" +
         "(( | e |-|'| d'| de | d[ao](s)? )[A-ZÁÍÓÚÉÂ][a-zãõáéíóúàèìòùäëïöüçâêîôû]+){1,3}",
            ErrorMessage = "Só são aceites letras. Cada palavra deve começar por uma Maiúscula, separadas por um espaço em branco.")]
      public string Nome { get; set; }

      public string Morada { get; set; }

      public string CodPostal { get; set; }

      public string Telemovel { get; set; }

      public string Email { get; set; }

      public string Fotografia { get; set; }



      // ..... outros a atributos relevantes





      /// <summary>
      /// relacionamento com a Autenticação
      /// atributo para referenciar o Utilizador que se autentica 
      /// É uma FK, apesar de não ser definida de forma expressa
      /// </summary>
      public string UserID { get; set; }


   }
}
