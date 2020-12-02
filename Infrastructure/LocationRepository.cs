using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using Dapper;
using System.Data;

namespace Infrastructure
{ 
    //public class LocationRepository : ILocationInfoRepository
    //{
    //    public bool AddUpdateLocationInfo(Location location)
    //    {
    //        try
    //        {
    //            using (IDbConnection connection = DBManager.DbConnect())
    //            {
    //                DynamicParameters parameters = new DynamicParameters();
    //                parameters.Add("@ID", location.ID);
    //                parameters.Add("@LocationName", location.LocationName);
    //                parameters.Add("@Fee", location.Fee);
    //                parameters.Add("@AddedBy", location.AddedBy);
    //                parameters.Add("@UpdatedBy", location.UpdatedBy);
    //                var savechanges = connection.Execute("[dbo].[AddUpdateLocation]", parameters, commandType: CommandType.StoredProcedure);
    //                if (savechanges > 0)
    //                {
    //                    return true;
    //                }
    //                else
    //                {
    //                    return false;
    //                }

    //            }

    //        }
    //        catch (Exception ex)
    //        {
    //            return false;
    //        }
    //    }

    //    public bool DeleteLocationInfo(int id)
    //    {
    //        try
    //        {
    //            using (IDbConnection connection = DBManager.DbConnect())
    //            {
    //                DynamicParameters parameters = new DynamicParameters();
    //                parameters.Add("@ID", id);
    //                parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
    //                connection.Execute("[dbo].[DeleteLocation]", parameters, commandType: CommandType.StoredProcedure);
    //                var savechanges = parameters.Get<Boolean>("@DeleteSuccess");
    //                if (savechanges)
    //                {
    //                    return true;
    //                }
    //                else
    //                {
    //                    return false;
    //                }

    //            }

    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public Location EditLocationInfo(int id)
    //    {
    //        try
    //        {
    //            using (IDbConnection connection = DBManager.DbConnect())
    //            {
    //                DynamicParameters parameters = new DynamicParameters();
    //                parameters.Add("@ID", id);
    //                Location locationedit = SqlMapper.Query<Location>(connection, "[dbo].[EditLocation]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
    //                return locationedit;
    //            }
    //        }
    //        catch (Exception ex)
    //        {

    //            throw ex;
    //        }
    //    }

    //    public List<Location> GetAllLocationInfo()
    //    {
    //        try
    //        {
    //            using (IDbConnection connection = DBManager.DbConnect())
    //            {
    //                List<Location> locationList = SqlMapper.Query<Location>(connection, "[dbo].[GetAllLocation]", commandType: CommandType.StoredProcedure).ToList();

    //                return locationList;
    //            }
    //        }
    //        catch (Exception ex)
    //        {

    //            return null;
    //        }
    //    }
    //}
}
