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
    public class HostelIndoRepository : IHostelInfoRepository
    {
        public bool AddUpdateHostelInfo(HostelInfo hostelInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", hostelInfo.ID);
                    parameters.Add("@HostelName", hostelInfo.HostelName);
                    parameters.Add("@Address", hostelInfo.Address);
                    parameters.Add("@ContactNo", hostelInfo.ContactNo);
                    parameters.Add("@PersonIncharge", hostelInfo.PersonIncharge);
                    parameters.Add("@InchargePhoneNo", hostelInfo.InchargePhoneNo);                   
                    parameters.Add("@AddedBy", hostelInfo.AddedBy);
                    parameters.Add("@UpdatedBy", hostelInfo.UpdatedBy);
                    var savechanges = connection.Execute("[dbo].[AddUpdateHostelInfo]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteHostelInfo(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteHostelInfo]", parameters, commandType: CommandType.StoredProcedure);
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

        public HostelInfo EditHostelInfo(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    HostelInfo hosetlInfoedit = SqlMapper.Query<HostelInfo>(connection, "[dbo].[EditHostelInfo]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return hosetlInfoedit;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<HostelInfo> GetAllHostelInfo()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<HostelInfo> hostelInfoList = SqlMapper.Query<HostelInfo>(connection, "[dbo].[GetAllHostelInfo]", commandType: CommandType.StoredProcedure).ToList();

                    return hostelInfoList;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
