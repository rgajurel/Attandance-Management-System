using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class TaxMaster
    {
        public int SN { get; set; }
        public int? ID { get; set; }
        [DisplayName("Amount From")]
        [Required(ErrorMessage = "This Field is Required")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Only Numbers are Allowed")]
        public decimal AmountFrom { get; set; }

        [DisplayName("From To")]
        [Required(ErrorMessage = "This Field is Required")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Only Numbers are Allowed")]
        public decimal AmountTo { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Only Numbers are Allowed")]
        public decimal TaxPercentage { get; set; }
        public int SortOrder { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
