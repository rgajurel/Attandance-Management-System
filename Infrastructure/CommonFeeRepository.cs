using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using System.Data;
using Dapper;

namespace Infrastructure
{
    public class CommonFeeRepository : ICommonFeeRepository
    {
        public string AddUpdateCommonFee(CommonFee fee)
        {
            try
            {
                var checkDublicate = checkCommonFee(fee);
                if (checkDublicate)
                {
                    return "Already Inserted";
                }

                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", fee.ID);
                    parameters.Add("@Session", fee.Session);
                    parameters.Add("@Faculty", fee.Faculty);
                    parameters.Add("@class", fee.Class);
                    parameters.Add("@section", fee.Section);
                    parameters.Add("@type", fee.Type);
                    parameters.Add("@month", fee.Month);
                    parameters.Add("@fee", fee.Fee);
                    parameters.Add("@AddedBy", fee.AddedBy);
                    parameters.Add("@UpdatedBy", fee.UpdatedBy);
                    var savechanges = connection.Execute("[dbo].[AddUpdateCommonFee]", parameters, commandType: CommandType.StoredProcedure);
                    if (savechanges > 0)
                    {
                        return "true";
                    }
                    else
                    {
                        return "false";
                    }
                }
            }
            catch (Exception ex)
            {
                return "false";
            }
        }

        public bool checkCommonFee(CommonFee fee)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", fee.ID);
                    parameters.Add("@Session", fee.Session);
                    parameters.Add("@Faculty", fee.Faculty);
                    parameters.Add("@class", fee.Class);
                    parameters.Add("@section", fee.Section);
                    parameters.Add("@type", fee.Type);
                    parameters.Add("@month", fee.Month);
                    parameters.Add("@fee", fee.Fee);
                    parameters.Add("@AddedBy", fee.AddedBy);
                    parameters.Add("@UpdatedBy", fee.UpdatedBy);
                    parameters.Add("@status", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                    connection.Execute("[dbo].[CheckCommonFee]", parameters, commandType: CommandType.StoredProcedure);
                    bool checkDublicate = parameters.Get<Boolean>("@status");
                    return checkDublicate;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool DeleteCommonFee(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    var savechanges = connection.Execute("[dbo].[DeleteCommonFee]", parameters, commandType: CommandType.StoredProcedure);
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

        public CommonFee EditCommonFee(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    CommonFee cmFee = SqlMapper.Query<CommonFee>(connection, "[dbo].[EditCommonFee]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return cmFee;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<CommonFee> GetAllCommonFee()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    List<CommonFee> feeList = SqlMapper.Query<CommonFee>(connection, "[dbo].[GetAllCommonFee]", param, commandType: CommandType.StoredProcedure).ToList();
                    return feeList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public Class GetSectionBasedOnClass(string id,string faculty)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@ID", id);
                    param.Add("@Faculty", faculty);
                    Class sections = SqlMapper.Query<Class>(connection, "[dbo].[GetSectionBasedOnClassFaculty]", param, commandType: CommandType.StoredProcedure).FirstOrDefault(); ;

                    return sections;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<CommonFee> GetClassBasedOnFaculty(string faculty)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Faculty", faculty);
                    List<CommonFee> classes = SqlMapper.Query<CommonFee>(connection, "[dbo].[GetClassBasedOnFaculty]", param, commandType: CommandType.StoredProcedure).ToList();

                    return classes;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        
    }
}
