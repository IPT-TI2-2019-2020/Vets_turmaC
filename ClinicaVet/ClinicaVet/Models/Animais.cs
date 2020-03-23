using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaVet.Models {
   
   /// <summary>
   /// Classe representa a tabela dos 'Animais' na base de dados
   /// </summary>
   public class Animais {

      /// <summary>
      /// PK da tabela
      /// </summary>
      public int ID { set; get; }

      /// <summary>
      /// Nome do animal
      /// </summary>
      public string  Nome { get; set; }

      /// <summary>
      /// Espécie do animal
      /// </summary>
      public string Especie { get; set; }

      /// <summary>
      /// Peso do animal
      /// </summary>
      public float Peso { get; set; }

      /// <summary>
      /// Raça do animal
      /// </summary>
      public string Raca { get; set; }

      /// <summary>
      /// Nome do ficheiro com a fotografia do animal
      /// </summary>
      public string Foto { get; set; }


   }
}
