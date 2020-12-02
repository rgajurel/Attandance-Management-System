using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using DomainInterface;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using System.Transactions;

namespace Infrastructure
{
    public class CommonFeeDiscountRepository : ICommonFeeDiscountRepository
    {

        public string AddUpdateCommonFeeDiscount(List<CommonFeeDiscount> discounts, string facultyID, string sessionId, string classId, string section, string type, string month)
        {
            string message = "";
            using (IDbConnection connection = DBManager.DbConnect())
            {
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        int count = 0;
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@Session", sessionId);
                        parameters.Add("@Faculty", facultyID);
                        parameters.Add("@class", classId);
                        parameters.Add("@section", section);
                        parameters.Add("@type", type);
                        parameters.Add("@month", month);
                        parameters.Add("@AddedBy", "");
                        parameters.Add("@UpdatedBy", "");
                        for (int i = 0; i < discounts.Count; i++)
                        {
                            parameters.Add("@ID", discounts[i].ID);
                            parameters.Add("@StudentId", discounts[i].StudentId);
                            parameters.Add("@fee", discounts[i].Fee);
                            parameters.Add("@discount", discounts[i].Discount);

                            count += connection.Execute("[dbo].[AddUpdateRemoveDiscount]", parameters, commandType: CommandType.StoredProcedure);

                        }
                        scope.Complete();
                        message = "Success";
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


        public List<CommonFeeDiscount> GetAllCommonFeeDiscount(CommonFeeDiscount commonFeeDiscount)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();

                    param.Add("@session", commonFeeDiscount.Session);
                    param.Add("@faculty", commonFeeDiscount.Faculty);
                    param.Add("@class", commonFeeDiscount.Class);
                    param.Add("@section", commonFeeDiscount.Section);
                    param.Add("@type", commonFeeDiscount.Type);
                    param.Add("@month", commonFeeDiscount.Month);

                    List<CommonFeeDiscount> discounts = SqlMapper.Query<CommonFeeDiscount>(connection, "[dbo].[GetAllDiscount]", param, commandType: CommandType.StoredProcedure).ToList();

                    return discounts;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<CommonFeeDiscount> GetClassBasedOnFaculty(string faculty)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Faculty", faculty);
                    List<CommonFeeDiscount> classes = SqlMapper.Query<CommonFeeDiscount>(connection, "[dbo].[GetClassBasedOnFaculty]", param, commandType: CommandType.StoredProcedure).ToList();

                    return classes;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<CommonFeeDiscount> GetFeeTypeBasedOnSection(string FacultyId, string SessionId, string ClassId, string SectionId)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@faculty", FacultyId);
                    param.Add("@session", SessionId);
                    param.Add("@class", ClassId);
                    param.Add("@section", SectionId);
                    List<CommonFeeDiscount> FeeTypes = SqlMapper.Query<CommonFeeDiscount>(connection, "[dbo].[getSelectedCommonFee]", param, commandType: CommandType.StoredProcedure).ToList();
                    return FeeTypes;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<CommonFeeDiscount> GetMonthBasedOnFeeType(string facultyID, string sessionId, string classId, string section, string type)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@faculty", facultyID);
                    param.Add("@session", sessionId);
                    param.Add("@class", classId);
                    param.Add("@section", section);
                    param.Add("@type", type);
                    List<CommonFeeDiscount> Months = SqlMapper.Query<CommonFeeDiscount>(connection, "[dbo].[getFeeTypeMonth]", param, commandType: CommandType.StoredProcedure).ToList();
                    return Months;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public Class GetSectionBasedOnClass(string Class, string Faculty)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@ID", Class);
                    param.Add("@Faculty", Faculty);
                    Class sections = SqlMapper.Query<Class>(connection, "[dbo].[GetSectionBasedOnClassFaculty]", param, commandType: CommandType.StoredProcedure).FirstOrDefault(); ;

                    return sections;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
