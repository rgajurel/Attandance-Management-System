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
    public class PersonalFeeRepository : IPersonalFeeRepository
    {
        public string AddUpdatePersonalFee(List<PersonalFee> PersonalFees, string facultyID, string sessionId, string classId, string section, string type, string month)
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
                        for (int i = 0; i < PersonalFees.Count; i++)
                        {
                            parameters.Add("@ID", PersonalFees[i].ID);
                            parameters.Add("@StudentId", PersonalFees[i].StudentId);
                            parameters.Add("@RollNo",PersonalFees[i].RollNo);
                            parameters.Add("@fee", PersonalFees[i].Fee);
                            parameters.Add("@discount", PersonalFees[i].Discount);
                            count += connection.Execute("[dbo].[AddUpdatePersonalFee]", parameters, commandType: CommandType.StoredProcedure);

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

        public bool DeletePersonalFee(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    var savechanges = connection.Execute("[dbo].[DeletePersonalFee]", parameters, commandType: CommandType.StoredProcedure);
                    if (savechanges > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<PersonalFee> GetAllPersonalFee(PersonalFee personalFee)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();

                    param.Add("@session", personalFee.Session);
                    param.Add("@faculty", personalFee.Faculty);
                    param.Add("@class", personalFee.Class);
                    param.Add("@section", personalFee.Section);
                    param.Add("@type", personalFee.Type);
                    param.Add("@month", personalFee.Month);

                    List<PersonalFee> fees = SqlMapper.Query<PersonalFee>(connection, "[dbo].[GetAllPersonalFee]", param, commandType: CommandType.StoredProcedure).ToList();

                    return fees;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<PersonalFee> GetClassBasedOnFaculty(string faculty)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Faculty", faculty);
                    List<PersonalFee> classes = SqlMapper.Query<PersonalFee>(connection, "[dbo].[GetClassBasedOnFaculty]", param, commandType: CommandType.StoredProcedure).ToList();

                    return classes;
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
