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
    public class JobTypeRepository : IJobTypeRepository
    {
        public bool AddUpdateJobType(JobType jobType)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", jobType.ID);
                    parameters.Add("@JobTypeName", jobType.JobTypeName);
                    parameters.Add("@AddedBy", new LoginUser().UserName);
                    parameters.Add("@UpdatedBy", new LoginUser().UserName);
                    var savechanges = connection.Execute("[dbo].[AddUpdateJobType]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteJobType(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteJobType]", parameters, commandType: CommandType.StoredProcedure);
                    var savechanges = parameters.Get<Boolean>("@DeleteSuccess");
                    if (savechanges)
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
                throw ex;
            }
        }

        public JobType EditJobType(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    JobType jobType = SqlMapper.Query<JobType>(connection, "[dbo].[EditJobType]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return jobType;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<JobType> GetAllJobType()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<JobType> jobTypeList = SqlMapper.Query<JobType>(connection, "[dbo].[GetAllJobType]", commandType: CommandType.StoredProcedure).ToList();

                    return jobTypeList;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
