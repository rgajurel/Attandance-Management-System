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
    public class MarksSheetLedgerRepository : IMarkSheetLedgerRepository
    {
        public List<MarksSheetLedger> GetAllMarksSheetLedger(MarksEntry marksentry)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();

                    param.Add("@SessionID", marksentry.SessionID);
                    param.Add("@ClassID", marksentry.ClassID);
                    param.Add("@Section", marksentry.Section);                  
                    param.Add("@TermID", marksentry.TermID);
                    param.Add("@FacultyID", marksentry.FacultyID);


                    List<MarksSheetLedger> marksSheetLedger = SqlMapper.Query<MarksSheetLedger>(connection, "[dbo].[GetMarkSheetLedgerBasedOnFilter]", param, commandType: CommandType.StoredProcedure).ToList();

                    return marksSheetLedger;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
