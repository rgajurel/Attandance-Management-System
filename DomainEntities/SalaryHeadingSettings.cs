using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
  public  class SalaryHeadingSettings
    {
        public int SN { get; set; }
        public int ID { get; set; }

        [DisplayName("JobType")]      
        [Required(ErrorMessage = "Required")]
        public int JobTypeID { get; set; }
        public string HeadName { get; set; }

        public bool IsAdd { get; set; }
        public bool IsSaving { get; set; }
        public bool IsTax { get; set; }
        public bool IsSalaryCalculatePoint { get; set; }
        public bool IsChecked { get; set; }
        public int SortOrder { get; set; }
        public string AddedBy { get; set; }
        public string UpdatedBy { get; set; }

        public DateTime AddedOn { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}
