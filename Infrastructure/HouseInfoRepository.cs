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
    public class HouseInfoRepository : IHouseInfoRepository
    {
        public bool AddUpdateHouseInfo(HouseInfo housename)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", housename.ID);
                    parameters.Add("@houseName", housename.HouseName);
                    parameters.Add("@AddedBy", housename.AddedBy);
                    parameters.Add("@UpdatedBy", housename.UpdatedBy);
                    var savechanges = connection.Execute("[dbo].[AddUpdateHouseInfo]", parameters, commandType: CommandType.StoredProcedure);
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

      

        public bool DeleteHouseInfo(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    var savechanges = connection.Execute("[dbo].[DeleteHouseInfo]", parameters, commandType: CommandType.StoredProcedure);
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

        public HouseInfo EditHouseInfo(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    HouseInfo houseEdit = SqlMapper.Query<HouseInfo>(connection, "[dbo].[EditHouseInfo]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return houseEdit;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<HouseInfo> GetAllHouseInfo()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {

                    List<HouseInfo> houseList = SqlMapper.Query<HouseInfo>(connection, "[dbo].[GetAllHouseInfo]", commandType: CommandType.StoredProcedure).ToList();
                    return houseList;
                }
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
