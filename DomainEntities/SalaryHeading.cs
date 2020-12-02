using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
  public class SalaryHeading
    {
        public int? ID { get; set; }

        public int SN { get; set; }

        [DisplayName("Head Name")]
        [Required(ErrorMessage ="Head Name is Required")]
        public string HeadName { get; set; }
        public bool IsAdd { get; set; }
        public string IsAd { get { if (IsAdd == true) { return "Yes"; } else { return "No"; } } }

        public string IsBasic { get { if (IsBasicSalary == true) { return "Yes"; } else { return "No"; } } }
        public bool IsSaving { get; set; }

        public bool IsTaxSaving { get; set; }
        public string IsSave { get { if (IsSaving == true) { return "Yes"; } else { return "No"; } } }

        [DisplayName("Sort Order")]
        [Required(ErrorMessage = "Sort Order is Required")]
        public int SortOrder { get; set; }
        public bool IsTax { get; set; }
        public bool IsCompanyContribution { get; set; }
        public bool IsBasicSalary { get; set; }

        public string IsTx { get { if (IsTax == true) { return "Yes"; } else { return "No"; } } }

        public bool IsSalaryCalculatePoint { get; set; }

        public string IsSalaryCalculate { get { if (IsSalaryCalculatePoint == true) { return "Yes"; } else { return "No"; } } }

        public string AddedBy { get; set; }

        public DateTime AddedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }
    }



}
