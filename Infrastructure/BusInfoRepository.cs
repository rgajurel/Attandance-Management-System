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
    public class BusInfoRepository : IBusInfoRepository
    {
        public bool AddUpdateBusInfo(BusInfo busInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", busInfo.ID);
                    parameters.Add("@BusNo", busInfo.BusNo);
                    parameters.Add("@DriverPhoneNo", busInfo.DriverPhoneNo);
                    parameters.Add("@DriverName", busInfo.DriverName);
                    parameters.Add("@SupporterName", busInfo.SupporterName);
                    parameters.Add("@SupporterPhoneNo", busInfo.SupporterPhoneNo);
                    parameters.Add("@AddedBy", busInfo.AddedBy);
                    parameters.Add("@UpdatedBy", busInfo.UpdatedBy);
                    var savechanges = connection.Execute("[dbo].[AddUpdateBusInfo]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteBusInfo(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                   connection.Execute("[dbo].[DeleteBusInfo]", parameters, commandType: CommandType.StoredProcedure);
                    var savechanges= parameters.Get<Boolean>("@DeleteSuccess");
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

        public BusInfo EditBusInfo(int id)
        {

            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    BusInfo businfoedit = SqlMapper.Query<BusInfo>(connection, "[dbo].[EditBusInfo]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return businfoedit;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<BusInfo> GetAllBusInfo()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<BusInfo> busInfoList = SqlMapper.Query<BusInfo>(connection, "[dbo].[GetAllBusInfo]", commandType: CommandType.StoredProcedure).ToList();

                    return busInfoList;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
