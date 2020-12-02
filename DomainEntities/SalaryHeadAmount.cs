using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class SalaryHeadAmount
    {       
            public int ID { get; set; }
            public int SN { get; set; }

            [DisplayName("Organisation")]
            [Required(ErrorMessage = "Organisation is Required")]
            public int OrganisationID { get; set; }

            [DisplayName("Salary Heading")]
            [Required(ErrorMessage = "Salary Heading is Required")]
            public int SalaryHeadID { get; set; }

           public bool IsAdded { get; set; }
            public int EmployeeID { get; set; }    
            public decimal Amount { get; set; }     
            public string EmployeeName { get; set; }
           public string OrganisationName { get; set; }
          public string AddedBy { get; set; }
            public string AddedOn { get; set; }
            public string UpdatedBy { get; set; }
            public string UpdatedOn { get; set; }
        
    }
}
