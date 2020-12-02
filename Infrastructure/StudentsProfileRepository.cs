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
    public class StudentsProfileRepository : IStudentsProfileRepository
    {
        public List<StudentsProfile> getStudentsInfo(string Id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();

                    param.Add("@studentId", Id);

                    List<StudentsProfile> studentsProfile = SqlMapper.Query<StudentsProfile>(connection, "[dbo].[studentsProfile]", param, commandType: CommandType.StoredProcedure).ToList();

                    return studentsProfile;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
