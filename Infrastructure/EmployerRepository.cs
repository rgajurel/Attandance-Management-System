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
    public class EmployerRepository : IEmployerRepository
    {
        #region Admin
        public List<Department> GetDepartmentBasedOnOrganisation(string id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@ID", id);
                    List<Department> departmentOrganisation = SqlMapper.Query<Department>(connection, "[dbo].[GetDepartmentbasedOnOrganisation]", param, commandType: CommandType.StoredProcedure).ToList(); ;
                    return departmentOrganisation;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Designations> GetDesignationBasedOnOrganisation(string id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@ID", id);
                    List<Designations> designationOrganisation = SqlMapper.Query<Designations>(connection, "[dbo].[GetDesignationbasedOnOrganisation]", param, commandType: CommandType.StoredProcedure).ToList(); ;
                    return designationOrganisation;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool AddUpdateEmployee(Employee employer)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", employer.ID);
                    parameters.Add("@Name", employer.Name);
                    parameters.Add("@Gender", employer.Gender);
                    parameters.Add("@EnglishJoioningDate", employer.EnglishJoioningDate);
                    parameters.Add("@NepaliJoioningDate", DateConversionHelper.GetEnglsihTimeToNepaliDateTime(employer.EnglishJoioningDate));
                    parameters.Add("@Qualifications", employer.Qualifications);
                    parameters.Add("@EnglishDateOfBirth", employer.EnglishDateOfBirth);
                    parameters.Add("@NepaliDateOfBirth", DateConversionHelper.GetEnglsihTimeToNepaliDateTime(employer.EnglishDateOfBirth));
                    parameters.Add("@Email", employer.Email);
                    parameters.Add("@PhoneNo", employer.PhoneNo);
                    parameters.Add("@MobileNo", employer.MobileNo);

                    parameters.Add("@CitizenshipNo", employer.CitizenshipNo);
                    parameters.Add("@Image", employer.Image);
                    parameters.Add("@UserID", employer.UserID);
                    parameters.Add("@OrganisationID", employer.OrganisationID);
                    parameters.Add("@DepartmentID", employer.DepartmentID);
                    parameters.Add("@DesignationID", employer.DesignationID);
                    parameters.Add("@JobTypeID", employer.JobTypeID);
                    parameters.Add("@EntryTime", employer.EntryTime);
                    parameters.Add("@ExitTime", employer.ExitTime);
                    parameters.Add("@Status", employer.Status);

                    parameters.Add("@FatherName", employer.FatherName);
                    parameters.Add("@PermanentAddress", employer.PermanentAddress);
                    parameters.Add("@TemporaryAddress", employer.TemporaryAddress);
                    parameters.Add("@EmpCode", employer.EmpCode);
                    parameters.Add("@CITNumber", employer.CITNumber);
                    parameters.Add("@PFNumber", employer.PFNumber);
                    parameters.Add("@BankAccountNo", employer.BankAccountNo);
                    parameters.Add("@PANNumber", employer.PANNumber);
                    parameters.Add("@AddedBy", employer.AddedBy);
                    parameters.Add("@UpdatedBy", employer.UpdatedBy);       
                                
                    var savechanges = connection.Execute("[dbo].[AddUpdateEmployee]", parameters, commandType: CommandType.StoredProcedure);
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
                throw ex;
            }
        }

        public List<EmployeeSearch> GetAllEmployee(EmployeeSearch search)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@offset", search.offset);
                    param.Add("@PageSize", search.pageSize);
                    param.Add("@Name", search.EmployerSearchName==null? "":search.EmployerSearchName);
                    param.Add("@OrganisationSearchID", search.OrganisationSearchID);
                    param.Add("@DepartmentSearchID", search.DepartmentSearchID);
                    param.Add("@DesignationSearchID", search.DesignationSearchID);
                    param.Add("@UserID", search.UserIDSearch);                    

                    List<EmployeeSearch> employeeList = SqlMapper.Query<EmployeeSearch>(connection, "[dbo].[GetAllEmployee]", param, commandType: CommandType.StoredProcedure).ToList();

                    return employeeList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Employee GetEmployeeByUserID(string userid)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@UserID", userid);
                   Employee employeeUserID = SqlMapper.Query<Employee>(connection, "[dbo].[GetUserID]", param, commandType: CommandType.StoredProcedure).FirstOrDefault(); ;
                    return employeeUserID;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

       public Employee EditEmployee(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    Employee employeeedit = SqlMapper.Query<Employee>(connection, "[dbo].[EditEmployeeInfo]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return employeeedit;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Employee EditEmployeeDeviceUserID(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    Employee employeeedit = SqlMapper.Query<Employee>(connection, "[dbo].[EditEmployeeInfoByDeviceID]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return employeeedit;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Employee DetailsEmployer(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    Employee employeeDetails = SqlMapper.Query<Employee>(connection, "[dbo].[DetailsEmployee]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return employeeDetails;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public UniqueNoGeneration GetUniqueDeivceID()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                 
                    UniqueNoGeneration registrationno = SqlMapper.Query<UniqueNoGeneration>(connection, "[dbo].[GetUniqueNumberGeneratorForEmployee]", param, commandType: CommandType.StoredProcedure).FirstOrDefault(); ;

                    return registrationno;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
