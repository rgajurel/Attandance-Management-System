using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
    public interface IEntranceRepository
    {
        //IEnumerable<QuizNotification> GetAllNotification(int Identifier);
        //IEnumerable<QuizCourse> GetAllCourseForQuiz();
        EntranceEntity AddUpdateEntrance(EntranceEntity objInfo);
        IEnumerable<EntranceEntity> GetAllEntranceListing(SearchEntranceParam objInfo);
        EntranceEntity GetEntranceByID(int EntranceID);
        int DeleteEntranceByID(int EntranceID);
        bool BatchUpdateEntrance(string JsonObject);
        string GetBatchUploadStatus(string JsonObject);
    }
}
