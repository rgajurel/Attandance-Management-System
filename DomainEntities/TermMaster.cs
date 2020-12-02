using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
  public  class TermMaster
    {
        public int? ID  { get; set; }
        public int SN { get; set; }


        [DisplayName("Name")]
        [Required(ErrorMessage = " This field is Required")]
        public string TermName { get; set; }

        public bool IsFinalTerm { get; set; }

        public string IsFinalTermOrNot
        {
            get { if (IsFinalTerm == true) { return "Yes"; } else { return "No"; } }
        }

        [DisplayName("Term Percentage")]
        [Required(ErrorMessage = "Required")]
         [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only Numbers are Allowed")]
        [Range(0.0, 100)]
        public int TermPercentage { get; set; }
        public string AddedBy { get; set; }

        public DateTime AddedOn { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime UPdatedOn { get; set; }

        public Decimal TotalObtained { get; set; }

        public string Grade { get; set; }

       // [Required(ErrorMessage = "RequiredValue")]
        public List<TermMaster> TermMasterList { get; set; }

    }
}
