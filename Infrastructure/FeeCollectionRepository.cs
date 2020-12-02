using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using System.Data;
using Dapper;
using System.Transactions;

namespace Infrastructure
{
    public class FeeCollectionRepository : IFeeCollectionRepository
    {

        public List<FeeCollectionReport> FeeCollectionBill(string BillNo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@ID", BillNo);
                    List<FeeCollectionReport> studentBill = SqlMapper.Query<FeeCollectionReport>(connection, "[dbo].[feePaymentBill]", param, commandType: CommandType.StoredProcedure).ToList();
                    return studentBill;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<FeeDueReport> FeeDueBill(string BillNo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@ID", BillNo);
                    List<FeeDueReport> studentDueBill = SqlMapper.Query<FeeDueReport>(connection, "[dbo].[feeDueBill]", param, commandType: CommandType.StoredProcedure).ToList();
                    return studentDueBill;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<FeeCollection> GetAllStudentsList(FeeCollection studentsList)
        {

            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@offset", studentsList.offset);
                    param.Add("@PageSize", studentsList.pageSize);
                    param.Add("@session", studentsList.Session);
                    param.Add("@faculty", studentsList.Faculty);
                    param.Add("@class", studentsList.Class);
                    param.Add("@section", studentsList.Section==null ? "":studentsList.Section);
                    param.Add("@studentName", studentsList.StudentName == null ? "" : studentsList.StudentName);
                    List<FeeCollection> students = SqlMapper.Query<FeeCollection>(connection, "[dbo].[GetAllStudents]", param, commandType: CommandType.StoredProcedure).ToList();
                    return students;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<FeeCollection> GetAllMonthList(string studentId)
        {

            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@studentId", studentId);
                    List<FeeCollection> months = SqlMapper.Query<FeeCollection>(connection, "[dbo].[GetFeeCollecionMonth]", param, commandType: CommandType.StoredProcedure).ToList();
                    return months;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<FeeCollection> GetAllFeeList(FeeCollection feeList)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    
                    param.Add("@studentId", feeList.StudentId);
                    param.Add("@session", feeList.SessionID);
                    param.Add("@faculty", feeList.FacultyID);
                    param.Add("@class", feeList.ClassID);
                    param.Add("@section", feeList.Section);
                    param.Add("@month", feeList.Month);
                    List<FeeCollection> students = SqlMapper.Query<FeeCollection>(connection, "[dbo].[getFeeCollectionFeeList]", param, commandType: CommandType.StoredProcedure).ToList();
                    return students;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public decimal CalculatePreviousDue(string StudentId, string SessionId, string FacultyId, string ClassId, string Section)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@studentId", StudentId);
                    param.Add("@session", SessionId);
                    param.Add("@faculty", FacultyId);
                    param.Add("@class", ClassId);
                    param.Add("@section", Section);
                    param.Add("@previousDue", dbType: DbType.Decimal, direction: ParameterDirection.Output);

                    connection.Execute("[dbo].[GetPreviousDue]", param, commandType: CommandType.StoredProcedure);
                    var previousDue = param.Get<dynamic>("@previousDue");
                    if(previousDue == null)
                    {
                        return 0;
                    }
                    else
                    {
                        return Convert.ToDecimal(previousDue);
                    }        
                    
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public string AddFeeCollection(List<FeeCollection> collections, string stuId, string session, string faculty, string classs, string section, string previousDue, string totalDiscount,string totalFee, string grandTotal, string balance, string totalPaid)
        {
            string message = "";
            using (IDbConnection connection = DBManager.DbConnect())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MaxId", 0, dbType: DbType.Int16, direction: ParameterDirection.Output);
                connection.Execute("[dbo].[GetMaxFeeCollectionId]", parameters, commandType: CommandType.StoredProcedure);
                int billNo = parameters.Get<dynamic>("@MaxId");


                parameters = new DynamicParameters();
                parameters.Add("@date", DateTime.Now.ToShortDateString());
                parameters.Add("@nepalimonth", 1, dbType: DbType.Int16, direction: ParameterDirection.Output);
                connection.Execute("[dbo].[getNepaliMonth]", parameters, commandType: CommandType.StoredProcedure);
                int nepaliMonth = parameters.Get<dynamic>("@nepalimonth");

                using (var scope = new TransactionScope())
                {
                    try
                    {
                        int count = 0;
                        parameters = new DynamicParameters();
                        parameters.Add("@ID", billNo);
                        parameters.Add("@studentId", stuId);
                        parameters.Add("@session", session);
                        parameters.Add("@Faculty", faculty);
                        parameters.Add("@class", classs);
                        parameters.Add("@section", section);
                        parameters.Add("@TotalFee", totalFee);
                        parameters.Add("@TotalDiscount", totalDiscount);
                        parameters.Add("@previousDue", previousDue);
                        parameters.Add("@GrandTotal", grandTotal);
                        parameters.Add("@TotalPaid", totalPaid);
                        parameters.Add("@PaymentDue", (Convert.ToDecimal(grandTotal)-Convert.ToDecimal(totalPaid)));
                        parameters.Add("@PaymentDate", DateTime.Now.ToShortDateString());
                        parameters.Add("@AddedBy", "");
                        connection.Execute("[dbo].[AddFeeCollection]", parameters, commandType: CommandType.StoredProcedure);

                        for (int i = 0; i < collections.Count; i++)
                        {
                            if (collections[i].IsAdmin == true)
                            {
                                parameters = new DynamicParameters();
                                parameters.Add("@FCId", billNo);
                                parameters.Add("@TypeId", collections[i].TypeId);
                                parameters.Add("@FeeType", collections[i].Type);
                                parameters.Add("@Month", collections[i].MonthId);
                                parameters.Add("@Fee", collections[i].Fee);
                                parameters.Add("@Discount", collections[i].Discount);
                                parameters.Add("@AddedBy", "");                               

                                count += connection.Execute("[dbo].[AddFeeCollectionDetails]", parameters, commandType: CommandType.StoredProcedure);

                            }
                        }
                        if(count==0 && Convert.ToDecimal(previousDue)>0 && Convert.ToDecimal(totalPaid) > 0)
                        {
                            parameters = new DynamicParameters();
                            parameters.Add("@FCId", billNo);
                            parameters.Add("@TypeId", "0");
                            parameters.Add("@FeeType", "Previous Due");
                            parameters.Add("@Month", nepaliMonth);
                            parameters.Add("@Fee", previousDue);
                            parameters.Add("@Discount", "0");
                            parameters.Add("@AddedBy", "");
                            count += connection.Execute("[dbo].[AddFeeCollectionDetails]", parameters, commandType: CommandType.StoredProcedure);
                        }
                        scope.Complete();
                        message = billNo.ToString();
                    }
                    catch (Exception ex)
                    {
                        message = "Failure";
                        throw ex;
                    }
                }
            }
            return message;
        }

        public string AddDueBill(List<FeeCollection> collections, string stuId, string session, string faculty, string classs, string section, string previousDue, string totalDiscount, string totalFee, string grandTotal)
        {
            string message = "";
            using (IDbConnection connection = DBManager.DbConnect())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MaxId", 0, dbType: DbType.Int16, direction: ParameterDirection.Output);
                connection.Execute("[dbo].[GetMaxFeeDueBillId]", parameters, commandType: CommandType.StoredProcedure);
                int billNo = parameters.Get<dynamic>("@MaxId");
                parameters = new DynamicParameters();
                parameters.Add("@date", DateTime.Now.ToShortDateString());
                parameters.Add("@nepalimonth", 1, dbType: DbType.Int16, direction: ParameterDirection.Output);
                connection.Execute("[dbo].[getNepaliMonth]", parameters, commandType: CommandType.StoredProcedure);
                int nepaliMonth = parameters.Get<dynamic>("@nepalimonth");

                using (var scope = new TransactionScope())
                {
                    try
                    {
                        int count = 0;
                        parameters = new DynamicParameters();
                        parameters.Add("@ID", billNo);
                        parameters.Add("@studentId", stuId);
                        parameters.Add("@session", session);
                        parameters.Add("@Faculty", faculty);
                        parameters.Add("@class", classs);
                        parameters.Add("@section", section);
                        parameters.Add("@TotalFee", totalFee);
                        parameters.Add("@TotalDiscount", totalDiscount);
                        parameters.Add("@previousDue", previousDue);
                        parameters.Add("@GrandTotal", grandTotal);
                        parameters.Add("@billingDate", DateTime.Now.ToShortDateString());
                        parameters.Add("@AddedBy", "");
                        connection.Execute("[dbo].[AddFeeDueBill]", parameters, commandType: CommandType.StoredProcedure);

                        for (int i = 0; i < collections.Count; i++)
                        {
                            if (collections[i].IsAdmin == true)
                            {
                                parameters = new DynamicParameters();
                                parameters.Add("@FCDId", billNo);
                                parameters.Add("@TypeId", collections[i].TypeId);
                                parameters.Add("@FeeType", collections[i].Type);
                                parameters.Add("@Month", collections[i].MonthId);
                                parameters.Add("@Fee", collections[i].Fee);
                                parameters.Add("@Discount", collections[i].Discount);
                                parameters.Add("@AddedBy", "");

                                count += connection.Execute("[dbo].[AddFeeDueBillDetails]", parameters, commandType: CommandType.StoredProcedure);

                            }
                        }
                        if (count == 0 && Convert.ToDecimal(previousDue) > 0)
                        {
                            parameters = new DynamicParameters();
                            parameters.Add("@FCDId", billNo);
                            parameters.Add("@TypeId", "0");
                            parameters.Add("@FeeType", "Previous Due");
                            parameters.Add("@Month", nepaliMonth);
                            parameters.Add("@Fee", previousDue);
                            parameters.Add("@Discount", "0");
                            parameters.Add("@AddedBy", "");
                            count += connection.Execute("[dbo].[AddFeeDueBillDetails]", parameters, commandType: CommandType.StoredProcedure);
                        }
                        scope.Complete();
                        message = billNo.ToString();
                    }
                    catch (Exception ex)
                    {
                        message = "Failure";
                        throw ex;
                    }
                }
            }
            return message;
        }
    }
}
