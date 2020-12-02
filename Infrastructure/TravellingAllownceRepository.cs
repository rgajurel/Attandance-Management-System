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
    public class TravellingAllownceRepository : ITravellingAllowanceRepository
    {
        public bool AddUpdateTravellingAllowance(TravellingAllowance leave)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", leave.ID);
                    parameters.Add("@OrganisationID", leave.OrganisationID);                  
                    parameters.Add("@EmployeeID", leave.EmployeeID);
                   parameters.Add("@DateFrom", leave.DateFrom);
                    parameters.Add("@DateTo", leave.DateTo);
                    parameters.Add("@NepaliDateFrom", leave.NepaliDateFrom);
                    parameters.Add("@NepaliDateTo", leave.NepaliDateTo);                  
                    parameters.Add("@Year", leave.Year);
                    parameters.Add("@Month", leave.Month);
                    parameters.Add("@Amount", leave.Amount);
                    parameters.Add("@Status", leave.Status);
                    parameters.Add("@Amount", leave.Description);
                    parameters.Add("@AddedBy", new LoginUser().UserName);
                    parameters.Add("@UpdatedBy", new LoginUser().UserName);
                    var savechanges = connection.Execute("[dbo].[AddUpdateTravellingAllownce]", parameters, commandType: CommandType.StoredProcedure);
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

        public List<TravellingAllowance> GetAllTravellingAllownace(TravellingAllowance search)
        {
            throw new NotImplementedException();
        }
    }
}
