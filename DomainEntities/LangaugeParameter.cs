using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class LangaugeParameter
    {
        public int SN { get; set; }
        public int? ID { get; set; }

        [DisplayName("Language")]       
        [Required(ErrorMessage = "Required")]
        public int LanguageID { get; set; }

        [DisplayName("Page")]
        [Required(ErrorMessage = "Required")]
        public string Page { get; set; }


        [DisplayName("Key")]
        [Required(ErrorMessage = "Required")]
        public string Key { get; set; }

        [DisplayName("Translated Word")]
        [Required(ErrorMessage = "Required")]
        public string TranslatedWord { get; set; }

        [DisplayName("Original Word in English")]
        [Required(ErrorMessage = "Required")]
        public string OriginalWordInEnglish { get; set; }

        public string Language { get; set; }
        public string AddedBy { get; set; }
        public string AddedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }

  
}
