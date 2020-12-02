using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
    public interface IFeeCollectionRepository
    {
        #region Admin
        List<FeeCollectionReport> FeeCollectionBill(string BillNo);
        List<FeeDueReport> FeeDueBill(string BillNo);
        List<FeeCollection> GetAllFeeList(FeeCollection students);
        List<FeeCollection> GetAllStudentsList(FeeCollection students);
        List<FeeCollection> GetAllMonthList(String studentId);
        decimal CalculatePreviousDue (string StudentId, string SessionId, string FacultyId, string ClassId, string Section);
        string AddFeeCollection(List<FeeCollection> collections, string stuId, string session, string faculty, string classs, string section, string previousDue, string totalDiscount,string totalFee, string grandTotal, string balance, string totalPaid);
        string AddDueBill(List<FeeCollection> collections, string stuId, string session, string faculty, string classs, string section, string previousDue, string totalDiscount, string totalFee, string grandTotal);

        #endregion
    }
}
