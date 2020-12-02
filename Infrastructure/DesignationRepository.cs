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
  public  class DesignationRepository : IDesignationRepository
    {
        public bool AddUpdateDesignation(Designations designation)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", designation.ID);
                    parameters.Add("@Designation", designation.Designation);
                    parameters.Add("@OrganisationID", designation.OrganisationID);
                    parameters.Add("@AddedBy", designation.AddedBy);
                    parameters.Add("@UpdatedBy", designation.UpdatedBy);
                    var savechanges = connection.Execute("[dbo].[AddUpdateDesignation]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteDesignaiton(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteDesignation]", parameters, commandType: CommandType.StoredProcedure);
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

        public Designations EditDesignation(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    Designations facultyDesignation = SqlMapper.Query<Designations>(connection, "[dbo].[EditDesignation]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return facultyDesignation;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Designations> GetAllDesignation()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@IsAdmin", new LoginUser().IsAdmin);
                    param.Add("@IsSuperAdmin", new LoginUser().IsSuperAdmin);
                    param.Add("@ID", new LoginUser().LoggedInuserID);
                    List<Designations> designationList = SqlMapper.Query<Designations>(connection, "[dbo].[GetAllDesignation]",param, commandType: CommandType.StoredProcedure).ToList();

                    return designationList;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
