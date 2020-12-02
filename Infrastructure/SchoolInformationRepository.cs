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
    public class SchoolInformationRepository : ISchoolInformationRepository
    {
        public bool AddUpdateSchoolInformation(SchoolInformation schoolInformation)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", schoolInformation.ID);
                    parameters.Add("@Name", schoolInformation.Name);
                    parameters.Add("@Address", schoolInformation.Address);
                    parameters.Add("@Email", schoolInformation.Email);
                    parameters.Add("@Phone", schoolInformation.Phone);
                    parameters.Add("@Mobile", schoolInformation.Mobile);
                    parameters.Add("@Fax", schoolInformation.Fax);
                    parameters.Add("@ContactPerson", schoolInformation.ContactPerson);
                    parameters.Add("@RegistrationNo", schoolInformation.RegistrationNo);
                    parameters.Add("@EstablishedYear", schoolInformation.EstablishedYear);
                    parameters.Add("@SchooTypeID", schoolInformation.SchoolTypeID);
                    parameters.Add("@Image", schoolInformation.Image);
                    parameters.Add("@IsMainBranch", schoolInformation.IsMainBranch);
                    parameters.Add("@AddedBy", schoolInformation.AddedBy);
                   
                    parameters.Add("@UpdatedBy", schoolInformation.UpdatedBy);                   

                    var savechanges = connection.Execute("[dbo].[AddUpdateSchoolInformation]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteSchoolInformation(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                   connection.Execute("[dbo].[DeleteSchoolInformation]", parameters, commandType: CommandType.StoredProcedure);
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

        public SchoolInformation EditSchoolInformation(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    SchoolInformation editschoolinformation = SqlMapper.Query<SchoolInformation>(connection, "[dbo].[EditSchoolInformation]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return editschoolinformation;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<SchoolInformation> GetAllSchoolInformation()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<SchoolInformation> schoolInformationList = SqlMapper.Query<SchoolInformation>(connection, "[dbo].[GetAllSchoolInformation]", commandType: CommandType.StoredProcedure).ToList();

                    return schoolInformationList;
                }
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
