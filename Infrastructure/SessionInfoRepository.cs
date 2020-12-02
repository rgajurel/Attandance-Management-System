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
    public class SessionInfoRepository : ISessionInfoRepository
    {
        public bool AddUpdateSessionInfo(SessionInfo sessionInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", sessionInfo.ID);
                    parameters.Add("@Session", sessionInfo.Session);
                    parameters.Add("@IsActive", sessionInfo.IsActive);
                    parameters.Add("@AddedBy", sessionInfo.AddedBy);
                    parameters.Add("@UpdatedBy", sessionInfo.UpdatedBy);
                    var savechanges = connection.Execute("[dbo].[AddUpdateSessionInfo]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteSessionInfo(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                   parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                     connection.Execute("[dbo].[DeleteSessionInfo]", parameters, commandType: CommandType.StoredProcedure);
                    var savechanges = parameters.Get<Boolean>("@DeleteSuccess");
                    if(savechanges)
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

        public SessionInfo EditSessionInfo(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    SessionInfo sessionedit = SqlMapper.Query<SessionInfo>(connection, "[dbo].[EditSessionInfo]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return sessionedit;
                }
            }
            catch (Exception ex)
            {

                throw;
            } 
        }

        public List<SessionInfo> GetAllSessionInfo()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<SessionInfo> sessionInfoList = SqlMapper.Query<SessionInfo>(connection, "[dbo].[GetAllSessionInfo]", commandType: CommandType.StoredProcedure).ToList();

                    return sessionInfoList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
    }

