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
    public class EditMarksEntryRepository : IEditMarksEntryRepository
    {
        public List<MarksEntry> GetAllMarksEntryEdit(MarksEntry marksentry)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();

                    param.Add("@SessionID", marksentry.SessionID);
                    param.Add("@ClassID", marksentry.ClassID);
                    param.Add("@Section", marksentry.Section);
                    param.Add("@SubjectID", marksentry.SubjectID);
                    param.Add("@TermID", marksentry.TermID);

                    List<MarksEntry> studentsList = SqlMapper.Query<MarksEntry>(connection, "[dbo].[GetAllMarksEntryUpdate]", param, commandType: CommandType.StoredProcedure).ToList();

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
