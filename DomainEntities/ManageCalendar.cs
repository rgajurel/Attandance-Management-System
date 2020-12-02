using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class ManageCalendar
    {
        private int maxday=32;
        public int MaxDay
        {
            get
            {
                return this.maxday;
            }
            set
            {
                this.maxday = value;
            }
        }
        public int SN { get; set; }
        public int? ID { get; set; }

        [DisplayName("Days")]
        [Required(ErrorMessage = "This Field is Required")]
        public int YearID { get; set; }

        [DisplayName("Month")]
        [Required(ErrorMessage = "This Field is Required")]
        public int MonthID { get; set; }

        [DisplayName("Days")]
        [Required(ErrorMessage = "This Field is Required")]
        [LessThanOrEqualTo("MaxDay", ErrorMessage = "Must be Less than MaxDays")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Only Numbers are Allowed")]
        public int Days { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
