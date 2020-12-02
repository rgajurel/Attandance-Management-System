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
    public class GradeMasterRepository : IGradeMasterRepository
    {
        public bool AddUpdateGradeMaster(GradeMaster grademaster)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", grademaster.ID);
                    parameters.Add("@Grade", grademaster.Grade);
                    parameters.Add("@GradePoint", grademaster.GradePoint);
                    parameters.Add("@MarksFrom", grademaster.MarksFrom);
                    parameters.Add("@MarksTo", grademaster.MarksTo);
                    parameters.Add("@AddedBy", grademaster.AddedBy);
                    parameters.Add("@UpdatedBy", grademaster.UpdatedBy);
                    var savechanges = connection.Execute("[dbo].[AddUpdateGradeMaster]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteGradeMaster(string grade)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@grade", grade);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteGradeMaster]", parameters, commandType: CommandType.StoredProcedure);
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

        public GradeMaster EditGrademaster(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    GradeMaster grademasterEdit = SqlMapper.Query<GradeMaster>(connection, "[dbo].[EditGradeMaster]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return grademasterEdit;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<GradeMaster> GetAllGradeMaster()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<GradeMaster> GrademasterList = SqlMapper.Query<GradeMaster>(connection, "[dbo].[GetAllGradeMaster]", commandType: CommandType.StoredProcedure).ToList();

                    return GrademasterList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<SubSubject> GetAllSubSubject()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();                  
                        List<SubSubject> SubSubjectList = SqlMapper.Query<SubSubject>(connection, "[dbo].[GetAllSubSubject]", commandType: CommandType.StoredProcedure).ToList();

                    return SubSubjectList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
