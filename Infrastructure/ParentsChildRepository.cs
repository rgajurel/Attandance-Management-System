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
    public class ParentsChildRepository : IParentsChildRepository
    {
        public List<ParentsChild> GetAllStudents(string email)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();

                    param.Add("@email", email);

                    List<ParentsChild> studentsList = SqlMapper.Query<ParentsChild>(connection, "[dbo].[parentsChild]", param, commandType: CommandType.StoredProcedure).ToList();

                    return studentsList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
