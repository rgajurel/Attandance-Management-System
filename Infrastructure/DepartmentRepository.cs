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
    public class DepartmentRepository : IDepartmentRepository
    {
        public bool AddUpdateDepartment(Department department)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", department.ID);
                    parameters.Add("@DepartmentName", department.DepartmentName);
                    parameters.Add("@OrganisationID", department.OrganisationID);
                    parameters.Add("@AddedBy", department.AddedBy);
                    parameters.Add("@UpdatedBy", department.UpdatedBy);
                    var savechanges = connection.Execute("[dbo].[AddUpdateDepartment]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteDepartment(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteDepartment]", parameters, commandType: CommandType.StoredProcedure);
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

        public Department EditDepartment(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    Department Department = SqlMapper.Query<Department>(connection, "[dbo].[EditDepartment]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return Department;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Department> GetAllDepartment()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@IsAdmin", new LoginUser().IsAdmin);
                    param.Add("@IsSuperAdmin", new LoginUser().IsSuperAdmin);
                    param.Add("@ID", new LoginUser().LoggedInuserID);
                    List<Department> departmentList = SqlMapper.Query<Department>(connection, "[dbo].[GetAllDepartment]",param, commandType: CommandType.StoredProcedure).ToList();

                    return departmentList;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
