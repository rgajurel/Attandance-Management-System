using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class EntranceAnswerEntity
    {
        public int QuestionID { get; set; }
        public string EntranceOption { get; set; }
        public bool IsCorrectAnswer { get; set; }
        public int AnswerPoolID { get; set; }
    }
}
