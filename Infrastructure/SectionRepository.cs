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
    public class SectionRepository : ISectionRepository
    {

        public int GetSectionCount(string section)
        {
            try {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@section", section);
                    parameters.Add("@Count", 0, dbType: DbType.Int16, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[GetSectionCount]", parameters, commandType: CommandType.StoredProcedure);
                    var sectioncount = parameters.Get<dynamic>("@Count");
                    return sectioncount;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
        public bool AddUpdateSection(Section section)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", section.ID);
                    parameters.Add("@Name", section.Name);
                    parameters.Add("@AddedBy", section.AddedBy);
                    parameters.Add("@UpdatedBy", section.UpdatedBy);
                    var savechanges = connection.Execute("[dbo].[AddUpdateSection]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteSection(string section)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Section", section);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteSection]", parameters, commandType: CommandType.StoredProcedure);
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

        public Section EditSection(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    Section sectionedit = SqlMapper.Query<Section>(connection, "[dbo].[EditSection]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return sectionedit;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<Section> GetAllSection()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<Section> sectionInfoList = SqlMapper.Query<Section>(connection, "[dbo].[GetAllSection]", commandType: CommandType.StoredProcedure).ToList();

                    return sectionInfoList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
