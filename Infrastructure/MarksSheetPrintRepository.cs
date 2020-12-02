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
    public class MarksSheetPrintRepository : IMarksSheetPrintRepository
    {
        TermMasterRepository termMaster = new TermMasterRepository();

        MarkSheetPrint IMarksSheetPrintRepository.GetStudentInfoForClient(string studentId, string session, string faculty, string section, string termId)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    MarkSheetPrint ms = new MarkSheetPrint();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@studentId", studentId);
                    param.Add("@session", session);
                    param.Add("@faculty", faculty);
                    param.Add("@section", section);
                    param.Add("@termId", termId);
                    ms = SqlMapper.Query<MarkSheetPrint>(connection, "[dbo].[getStudentForClientResult]", param, commandType: CommandType.StoredProcedure).FirstOrDefault<MarkSheetPrint>();
                    return ms;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public List<MarkShitStudentsPrint> GetAllMarkSheets(MarkSheetPrint marksheetprint)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@SessionID", marksheetprint.SessionID);
                    param.Add("@FacultyID", marksheetprint.FacultyID);
                    param.Add("@Section", marksheetprint.Section == null ? "" : marksheetprint.Section);
                    param.Add("@ClassID", marksheetprint.ClassID);
                    param.Add("@TermID", marksheetprint.TermID);
                    param.Add("@Student", marksheetprint.StudentName);
                    param.Add("@resultType", marksheetprint.ResultType);

                    bool isfinalterm = GetFinalTermOrNot(marksheetprint.TermID);
                    if (isfinalterm == false)
                    {
                        List<MarkShitStudentsPrint> marksSheetList = SqlMapper.Query<MarkShitStudentsPrint>(connection, "[dbo].[GetAllMarksSheetPrint]", param, commandType: CommandType.StoredProcedure).ToList();
                        var marksSheetList1 = SqlMapper.Query<ResulTypeMarkSheet>(connection, "[dbo].[GetAllStudentMarkSheetPrint]", param, commandType: CommandType.StoredProcedure).ToList();

                        if (marksSheetList != null && marksSheetList1 != null)
                        {
                            if (marksheetprint.ResultType == 2)
                            {
                                foreach (var marks in marksSheetList)
                                {
                                    marks.Date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, TimeZoneInfo.Local.Id, "Nepal Standard Time"); ;
                                    marks.AllResultData = marksSheetList1.Where(model => model.StudentID == marks.ID).ToList();
                                    marks.TotalFM = marksSheetList1.Where(model => model.StudentID == marks.ID).ToList().Sum(model => Convert.ToDecimal(model.FM));
                                    marks.TotalPM = marksSheetList1.Where(model => model.StudentID == marks.ID).ToList().Sum(model => Convert.ToDecimal(model.PM));
                                    marks.TotalObtained = marksSheetList1.Where(model => model.StudentID == marks.ID).ToList().Sum(model => Convert.ToDecimal(model.Obtained));
                                    marks.Percentage = (marks.TotalObtained / marks.TotalFM) * 100;

                                    var final = GetGradePointAndGrade(Math.Round(marks.Percentage));
                                    marks.FinalGrade = final.Grade;
                                    marks.GradePoint = Convert.ToString(marks.AllResultData.Where(model => model.GradePoint != "Absent"||model.GradePoint!="" ||model.GradePoint!=null).Sum(model => Convert.ToDecimal(model.GradePoint)) / marks.AllResultData.Count());
                                }
                            }
                            else
                            {
                                foreach (var mark in marksSheetList1)
                                {
                                    mark.HighestGradeObtained = GetGradePointAndGrade(Convert.ToDecimal(mark.HighestMarksObtained)).Grade;
                                }
                                foreach (var marks in marksSheetList)
                                {
                                    marks.Date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.Date, TimeZoneInfo.Local.Id, "Nepal Standard Time"); ;
                                    marks.AllResultData = marksSheetList1.Where(model => model.StudentID == marks.ID).ToList();
                                    marks.TotalFM = marksSheetList1.Where(model => model.StudentID == marks.ID).ToList().Sum(model => Convert.ToDecimal(model.FM));
                                    marks.TotalPM = marksSheetList1.Where(model => model.StudentID == marks.ID).ToList().Sum(model => Convert.ToDecimal(model.PM));
                                    marks.TotalObtained = marksSheetList1.Where(model => model.StudentID == marks.ID).ToList().Sum(model => Convert.ToDecimal(model.Obtained));
                                    marks.Percentage = (marks.TotalObtained / marks.TotalFM) * 100;

                                    var final = GetGradePointAndGrade(Math.Round(marks.Percentage));
                                    marks.FinalGrade = final.Grade;
                                    marks.GradePoint = Convert.ToString(marks.AllResultData.Where(model => model.GradePoint != "Absent"|| model.GradePoint!=""||model.GradePoint!=null).Sum(model => Convert.ToDecimal(model.GradePoint)) / marks.AllResultData.Count());
                                }
                            }


                            return marksSheetList;
                        }
                        else
                        {

                            return null;
                        }


                    }
                    else
                    {
                        List<MarkShitStudentsPrint> marksSheetList = SqlMapper.Query<MarkShitStudentsPrint>(connection, "[dbo].[GetAllMarksSheetPrint]", param, commandType: CommandType.StoredProcedure).ToList();
                        var marksSheetList1 = SqlMapper.Query<ResulTypeMarkSheet>(connection, "[dbo].[GetAllStudentMarkSheetPrintFinalTerm]", param, commandType: CommandType.StoredProcedure).ToList();
                        List<MarkShitStudentsPrint> mar = new List<MarkShitStudentsPrint>();

                        if (marksheetprint.ResultType == 2)
                        {
                            foreach (var marks in marksSheetList)
                            {
                                MarkShitStudentsPrint mm = new MarkShitStudentsPrint();
                                mm.IsFinal = isfinalterm;
                                mm.ActiveSession = marks.ActiveSession;
                                mm.StudentName = marks.StudentName;
                                mm.TermName = marks.TermName;
                                mm.Phone = marks.Phone;
                                mm.Class = marks.Class;
                                mm.Section = marks.Section;
                                mm.RollNo = marks.RollNo;
                                mm.SchoolName = marks.SchoolName;
                                mm.FatherName = marks.FatherName;
                                mm.MotherName = marks.MotherName;
                                mm.Logo = marks.Logo;
                                mm.TotalDays = marks.TotalDays;
                                mm.PresentDays = marks.PresentDays;
                                mm.Date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.Date, TimeZoneInfo.Local.Id, "Nepal Standard Time"); ;
                                mm.AllTermsForHeadings = termMaster.GetAllTermMaster().OrderBy(model => model.ID).ToList();
                                mm.AllResultData = new List<ResulTypeMarkSheet>();
                                var data = marksSheetList1.Where(model => model.StudentID == marks.ID).ToList().GroupBy(model => model.SubjectName);

                                foreach (var Subject in data)
                                {
                                    ResulTypeMarkSheet resultType = new ResulTypeMarkSheet();
                                    resultType.AllTerms = new List<TermMaster>();

                                    foreach (var term in Subject.OrderBy(model => model.TermID))
                                    {
                                        var terms = new TermMaster();
                                        terms.TotalObtained = Math.Round((term.TotalObtained / 100) * term.TermPercentage, 0);
                                        terms.ID = term.TermID;
                                        terms.IsFinalTerm = term.isFinalTerm;                              
                                        resultType.AllTerms.Add(terms);
                                    }
                                    resultType.TotalObtained = Math.Round(resultType.AllTerms.Sum(model => model.TotalObtained));
                                    resultType.SubjectName = Subject.Select(model => model.SubjectName).FirstOrDefault();
                                    resultType.FM = Subject.Where(model => model.isFinalTerm == true && model.SubjectName == resultType.SubjectName).Select(model => model.FM).FirstOrDefault();
                                    resultType.FinalTotal = marksSheetList1.Where(model => model.SubjectName == resultType.SubjectName).Select(model => model.FinalTotal).FirstOrDefault();
                                    resultType.GradePoint = GetGradePointAndGrade(resultType.TotalObtained).GradePoint;

                                    resultType.PM = Subject.Where(model => model.isFinalTerm == true && model.SubjectName == resultType.SubjectName).Select(model => model.PM).FirstOrDefault();
                                    resultType.Grade = GetGradePointAndGrade((resultType.TotalObtained / Convert.ToInt16(resultType.FM)) * 100).Grade;
                                    mm.AllResultData.Add(resultType);


                                }
                                mm.TotalFM = mm.AllResultData.Sum(model => Convert.ToInt16(model.FM));
                                mm.TotalPM = mm.AllResultData.Sum(model => Convert.ToInt16(model.PM));
                                mm.TotalObtained = mm.AllResultData.Sum(model => model.TotalObtained);
                                mm.Percentage = (mm.TotalObtained / mm.TotalFM) * 100;
                                mm.FinalGrade = GetGradePointAndGrade(Math.Round(mm.Percentage, 0)).Grade;
                                mm.GradePoint = Convert.ToString(Math.Round(mm.AllResultData.Where(model =>model.GradePoint != "Absent"||model.GradePoint!=""||model.GradePoint!=null).Sum(model => Convert.ToDecimal(model.GradePoint)), 2) / mm.AllResultData.Count());
                                mar.Add(mm);
                            }
                            //GetAllStudentMarkSheetPrintFinalTerm
                            return mar;
                        }
                        else
                        {
                            foreach (var marks in marksSheetList)
                            {
                                MarkShitStudentsPrint mm = new MarkShitStudentsPrint();
                                mm.IsFinal = isfinalterm;
                                mm.ActiveSession = marks.ActiveSession;
                                mm.StudentName = marks.StudentName;
                                mm.TermName = marks.TermName;
                                mm.Phone = marks.Phone;
                                mm.Class = marks.Class;
                                mm.Section = marks.Section;
                                mm.RollNo = marks.RollNo;
                                mm.SchoolName = marks.SchoolName;
                                mm.FatherName = marks.FatherName;
                                mm.MotherName = marks.MotherName;
                                mm.Logo = marks.Logo;
                                mm.TotalDays = marks.TotalDays;
                                mm.PresentDays = marks.PresentDays;
                                mm.Date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.Date, TimeZoneInfo.Local.Id, "Nepal Standard Time"); ;
                                mm.AllTermsForHeadings = termMaster.GetAllTermMaster().OrderBy(model => model.ID).ToList();
                                mm.AllResultData = new List<ResulTypeMarkSheet>();
                                var data = marksSheetList1.Where(model => model.StudentID == marks.ID).ToList().GroupBy(model => model.SubjectName);

                                foreach (var Subject in data)
                                {
                                    ResulTypeMarkSheet resultType = new ResulTypeMarkSheet();
                                    resultType.AllTerms = new List<TermMaster>();

                                    foreach (var term in Subject.OrderBy(model => model.TermID))
                                    {
                                        var terms = new TermMaster();
                                        terms.TotalObtained = Math.Round((term.TotalObtained / 100) * term.TermPercentage, 0);
                                        terms.Grade = GetGradePointAndGrade(term.TotalObtained).Grade;
                                        terms.ID = term.TermID;
                                        resultType.AllTerms.Add(terms);

                                    }
                                    resultType.TotalObtained = Math.Round(resultType.AllTerms.Sum(model => model.TotalObtained));
                                    resultType.SubjectName = Subject.Select(model => model.SubjectName).FirstOrDefault();
                                    resultType.FinalTotal = marksSheetList1.Where(model => model.SubjectName == resultType.SubjectName).Select(model => model.FinalTotal).FirstOrDefault();
                                    resultType.FM = Subject.Where(model => model.isFinalTerm == true && model.SubjectName == resultType.SubjectName).Select(model => model.FM).FirstOrDefault();
                                    resultType.Grade = GetGradePointAndGrade((resultType.TotalObtained/Convert.ToInt16(resultType.FM))*100).Grade;
                                    resultType.HighestGradeObtained = GetGradePointAndGrade(resultType.FinalTotal).Grade;
                                   
                                    resultType.PM = Subject.Where(model => model.isFinalTerm == true && model.SubjectName == resultType.SubjectName).Select(model => model.PM).FirstOrDefault();

                                    resultType.GradePoint = GetGradePointAndGrade(resultType.TotalObtained).GradePoint;
                                    mm.AllResultData.Add(resultType);


                                }
                                mm.TotalFM = mm.AllResultData.Sum(model => Convert.ToInt16(model.FM));
                                mm.TotalPM = mm.AllResultData.Sum(model => Convert.ToInt16(model.PM));
                                mm.TotalObtained = mm.AllResultData.Sum(model => model.TotalObtained);
                                mm.Percentage = (mm.TotalObtained / mm.TotalFM) * 100;
                                mm.FinalGrade = GetGradePointAndGrade(Math.Round(mm.Percentage, 0)).Grade;
                                mm.GradePoint = Convert.ToString(Math.Round(mm.AllResultData.Where(model => model.GradePoint != null|| model.GradePoint != "Absent"|| model.GradePoint!="").Sum(model => Convert.ToDecimal(model.GradePoint)), 2) / mm.AllResultData.Count());
                                mar.Add(mm);
                            }
                            //GetAllStudentMarkSheetPrintFinalTerm
                            return mar;
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GradeMaster GetGradePointAndGrade(decimal percentage)
        {
            GradeMasterRepository gradeMasterRepo = new GradeMasterRepository();
            GradeMaster gradeMaster = new GradeMaster();
            var allGrade = gradeMasterRepo.GetAllGradeMaster().OrderByDescending(model => model.ID).ToList();

            foreach (var grade in allGrade)
            {
                if ((Convert.ToDecimal(percentage) >= grade.MarksFrom) && (Convert.ToDecimal(percentage) <= grade.MarksTo))
                {
                    gradeMaster.Grade = grade.Grade;
                    gradeMaster.GradePoint = grade.GradePoint;
                    break;
                }

            }
            return gradeMaster;
            
        }

        public List<MarkSheetPrint> GetAllStudents(MarkSheetPrint marksheetprint)
        {
             
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();                   
                    param.Add("@FacultyID", marksheetprint.FacultyID);
                    param.Add("@Section", marksheetprint.Section == null ? "" : marksheetprint.Section);
                    param.Add("@ClassID", marksheetprint.ClassID);                   
                    param.Add("@SessionID", marksheetprint.SessionID);  
                         
                    
                   var  studentsList = SqlMapper.Query<MarkSheetPrint>(connection, "[dbo].[GetAllStudentsForMarksSheet]", param, commandType: CommandType.StoredProcedure).ToList();

                    return studentsList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool GetFinalTermOrNot(int TermID)
        {
            TermMasterRepository termmaster = new TermMasterRepository();
            var terms= termmaster.GetAllTermMaster().Where(model => model.ID == TermID).FirstOrDefault();
            return terms.IsFinalTerm;            


        }

       
    }
    }

