using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using System.Transactions;

namespace Infrastructure
{
    public class SalaryCalculationRepository : ISalarCalculationRepository
    {
        public List<AttandanceDays> AttandanceInformation(string id, string year, string month)
        {
            using (IDbConnection connection = DBManager.DbConnect())
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@employeeid", id);
                param.Add("@year", year);
                param.Add("@month", month);
                List<AttandanceDays> employeeAttandanceList = SqlMapper.Query<AttandanceDays>(connection, "[dbo].[GetEmployeeAttandanceInformation]", param, commandType: CommandType.StoredProcedure).ToList();

                List<AttandanceDays> totalAttandance = new List<AttandanceDays>();
                var totalOfficial = employeeAttandanceList.Where(x => x.AttandanceName == "Offical Attandance").ToList();
                totalAttandance.Add(new AttandanceDays()
                {
                    AttandanceName = "Offical Attandance",
                    Days = totalOfficial.Count() > 0 ? totalOfficial.Sum(y => Convert.ToDecimal(y.Days)).ToString() : "0.00",
                });

                var totalManual = employeeAttandanceList.Where(x => x.AttandanceName == "Manual Attandance").ToList();
                totalAttandance.Add(new AttandanceDays()
                {
                    AttandanceName = "Manual Attandance",
                    Days = totalManual.Count() > 0 ? totalManual.Sum(y => Convert.ToDecimal(y.Days)).ToString() : "0.00",
                });

                var totalDaily = employeeAttandanceList.Where(x => x.AttandanceName == "Daily Attandance").ToList();
                totalAttandance.Add(new AttandanceDays()
                {
                    AttandanceName = "Daily Attandance",
                    Days = totalDaily.Count() > 0 ? totalDaily.Sum(y => Convert.ToDecimal(y.Days)).ToString() : "0.00",
                });

                var totalDevice = employeeAttandanceList.Where(x => x.AttandanceName == "Device Attandance").ToList();
                totalAttandance.Add(new AttandanceDays()
                {
                    AttandanceName = "Device Attandance",
                    Days = totalDevice.Count() > 0 ? totalDevice.Sum(y => Convert.ToDecimal(y.Days)).ToString() : "0.00",
                });

                totalAttandance[0].Total = totalAttandance.Sum(x => Convert.ToDecimal(x.Days)).ToString();
                return totalAttandance;

            }
        }

        public ListSalaryInfoAdd CalculateTax(decimal taxableamount,string employeeid)
        {
            ListSalaryInfoAdd salaryInfoList = new ListSalaryInfoAdd();
            using (IDbConnection connection = DBManager.DbConnect())
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@employeeid", employeeid);

                List<SalaryInfoDetail> salaryHeadingList = SqlMapper.Query<SalaryInfoDetail>(connection, "[dbo].[GetEmployeeSalaryInfo]", param, commandType: CommandType.StoredProcedure).OrderBy(x=>x.SortOrder).ToList();
                var taxSaving = salaryHeadingList.Where(x => x.IsTaxSaving == true).ToList();
                decimal totalTaxSaving = taxSaving.Sum(x => x.Amount);
                var amount = (taxableamount);
                int months = (int)GetTotalMonths.Months;
                var totaltaxableamount = ((amount) * months) + salaryHeadingList.FirstOrDefault(x => x.IsBasicSalary == true).Amount - totalTaxSaving;
                decimal remainingtaxableamount = 0;
                decimal tax1 = 0;
                decimal remainingtax = 0;
                salaryInfoList.TaxInfo = new List<SalaryInfo>();
                List<TaxMaster> taxMasterList = SqlMapper.Query<TaxMaster>(connection, "[dbo].[GetAllTax]", commandType: CommandType.StoredProcedure).OrderBy(x => x.SortOrder).ToList();
                var taxlist = salaryHeadingList.Where(x => x.IsTax == true).OrderBy(x => x.SortOrder).ToList();


                if (totaltaxableamount < (taxMasterList[0].AmountFrom - taxMasterList[0].AmountTo))
                {
                    tax1 = (totaltaxableamount * taxMasterList[0].TaxPercentage) / (12 * 100);
                    remainingtax = 0;

                }
                else
                {

                    for (int i = 0; i < taxMasterList.Count(); i++)
                    {
                        if (i == 0)
                        {
                            tax1 = (taxMasterList[0].AmountTo * taxMasterList[0].TaxPercentage) / (12 * 100);
                            remainingtaxableamount = totaltaxableamount - taxMasterList[0].AmountTo;
                        }
                        else
                        {
                            if (remainingtaxableamount > 0)
                            {
                                if (remainingtaxableamount >= (taxMasterList[i].AmountTo - taxMasterList[i].AmountFrom))
                                {
                                    remainingtax += ((taxMasterList[i].AmountTo - taxMasterList[i].AmountFrom) * taxMasterList[i].TaxPercentage) / (12 * 100);
                                    remainingtaxableamount = remainingtaxableamount - (taxMasterList[i].AmountTo - taxMasterList[i].AmountFrom);
                                }
                                else
                                {
                                    remainingtax += ((remainingtaxableamount) * taxMasterList[i].TaxPercentage) / (12 * 100);
                                    remainingtaxableamount = remainingtaxableamount - (taxMasterList[i].AmountTo - taxMasterList[i].AmountFrom);
                                }

                            }
                        }
                    }
                    {


                    }

                }

                salaryInfoList.TaxInfo.Add(new SalaryInfo()
                {
                    SalaryHeadingID = taxlist.Where(x => x.IsTax == true).ToList()[0].SalaryHeadingID.ToString(),
                    SalHeadingName = taxlist.Where(x => x.IsTax == true).ToList()[0].SalHeadingName.ToString(),
                    Amount = Math.Round(tax1, 2),
                    SortOrder = taxlist.ToList()[0].SortOrder
                });

                salaryInfoList.TaxInfo.Add(new SalaryInfo()
                {
                    SalaryHeadingID = taxlist.Where(x => x.IsTax == true).ToList()[1].SalaryHeadingID.ToString(),
                    SalHeadingName = taxlist.Where(x => x.IsTax == true).ToList()[1].SalHeadingName.ToString(),
                    Amount = Math.Round(remainingtax, 2),
                    SortOrder = taxlist.ToList()[1].SortOrder
                });


                salaryInfoList.SalInfoFinalTotal = new SalaryInfo()
                {
                    SalaryHeadingID = salaryHeadingList.Where(x => x.IsSalaryCalculatePoint == true).ToList()[1].SalaryHeadingID.ToString(),
                    SalHeadingName = salaryHeadingList.Where(x => x.IsSalaryCalculatePoint == true).ToList()[1].SalHeadingName.ToString(),
                    Amount = (amount - salaryInfoList.TaxInfo.Sum(x => x.Amount)) < 0 ? 0 : amount - salaryInfoList.TaxInfo.Sum(x => x.Amount),
                    SortOrder = salaryHeadingList.Where(x => x.IsSalaryCalculatePoint).ToList()[1].SortOrder
                };

            }
            return salaryInfoList;
        

    }

        public List<LeaveNameDays> GetEmployeeLeaveList(string id, string year, string month)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@employeeid", id);
                    param.Add("@year", year);
                    param.Add("@month", month);
                    List<LeaveNameDays> employeeLeaveList = SqlMapper.Query<LeaveNameDays>(connection, "[dbo].[GetEmployeeLeaveInformation]", param, commandType: CommandType.StoredProcedure).ToList();

                    var totaldays = employeeLeaveList.Count() > 0 ? employeeLeaveList.Sum(x => Convert.ToDecimal(x.Days)) : Convert.ToDecimal("0.00");
                    foreach (var data in employeeLeaveList)
                    {
                        data.Total = totaldays.ToString();
                    }
                    return employeeLeaveList;


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ListSalaryInfoAdd GetEmployeeSalaryInfo(string id)
        {

            ListSalaryInfoAdd salaryInfoList = new ListSalaryInfoAdd();
            using (IDbConnection connection = DBManager.DbConnect())
            {      
                DynamicParameters param = new DynamicParameters();
                param.Add("@employeeid", id);
                               
                List<SalaryInfoDetail> salaryHeadingList = SqlMapper.Query<SalaryInfoDetail>(connection, "[dbo].[GetEmployeeSalaryInfo]", param,commandType: CommandType.StoredProcedure).OrderBy(x=>x.SortOrder).ToList();

                var basicandaddlist = salaryHeadingList.Where(x => x.IsAdd == true || x.IsBasicSalary == true).OrderBy(x => x.SortOrder).Select(x=>new SalaryInfo()
                {
                    SalaryHeadingID=x.SalaryHeadingID,
                    SalHeadingName=x.SalHeadingName,
                    Amount=Convert.ToDecimal(x.Amount),
                    SortOrder=x.SortOrder
                }).ToList();

                var taxSaving = salaryHeadingList.Where(x => x.IsTaxSaving == true).ToList();
                decimal totalTaxSaving = taxSaving.Sum(x => x.Amount);
                salaryInfoList.SalInfoAdd=new List<SalaryInfo>();
                salaryInfoList.SalInfoAdd = basicandaddlist;
                salaryInfoList.SalAddInfoTotal = new SalaryInfo()
                    {
                        SalaryHeadingID = salaryHeadingList.Where(x=>x.IsSalaryCalculatePoint==true).ToList()[0].SalaryHeadingID.ToString(),
                        SalHeadingName = salaryHeadingList.Where(x => x.IsSalaryCalculatePoint==true).ToList()[0].SalHeadingName.ToString(),
                        Amount = salaryInfoList.SalInfoAdd.Sum(x => x.Amount),
                        SortOrder = salaryHeadingList.Where(x => x.IsSalaryCalculatePoint==true).ToList()[0].SortOrder
                    };

                
                var basicandsavinglist = salaryHeadingList.Where(x => x.IsSaving== true).OrderBy(x => x.SortOrder).Select(x => new SalaryInfo()
                {
                    SalaryHeadingID = x.SalaryHeadingID,
                    SalHeadingName = x.SalHeadingName,
                    Amount = Convert.ToDecimal(x.Amount),
                    SortOrder = x.SortOrder
                }).ToList();

                salaryInfoList.SalInfoSaving = new List<SalaryInfo>();
                salaryInfoList.SalInfoSaving = basicandsavinglist;
               
                salaryInfoList.SalInfoSavingTotal = new SalaryInfo()
                {
                    SalaryHeadingID = salaryHeadingList.Where(x => x.IsSalaryCalculatePoint == true).ToList()[0].SalaryHeadingID.ToString(),
                    SalHeadingName = "Total Saving",
                    Amount = salaryInfoList.SalInfoSaving.Sum(x => x.Amount),
                    SortOrder = salaryHeadingList.Where(x => x.IsSalaryCalculatePoint == true).ToList()[0].SortOrder
                };
                var amount= (salaryInfoList.SalAddInfoTotal.Amount - salaryInfoList.SalInfoSavingTotal.Amount);
                int months= (int)GetTotalMonths.Months;
                var totaltaxableamount = ((salaryInfoList.SalAddInfoTotal.Amount - salaryInfoList.SalInfoSavingTotal.Amount) * months)+ salaryHeadingList.FirstOrDefault(x => x.IsBasicSalary == true).Amount-totalTaxSaving;
                decimal remainingtaxableamount = 0 ;
                decimal tax1 =0;
                decimal remainingtax = 0;
                salaryInfoList.TaxInfo = new List<SalaryInfo>();
                List<TaxMaster> taxMasterList = SqlMapper.Query<TaxMaster>(connection, "[dbo].[GetAllTax]", commandType: CommandType.StoredProcedure).OrderBy(x=>x.SortOrder).ToList();
                var taxlist = salaryHeadingList.Where(x => x.IsTax == true).OrderBy(x => x.SortOrder).ToList();
                                                
                   
                    if (totaltaxableamount < (taxMasterList[0].AmountFrom-taxMasterList[0].AmountTo))
                    {
                        tax1 = (totaltaxableamount * taxMasterList[0].TaxPercentage) / (12 * 100);
                        remainingtax = 0;
                      
                    }
                    else
                   {
                   
                        for (int i = 0; i < taxMasterList.Count(); i++)
                        {
                            if (i == 0)
                            {
                                tax1 = (taxMasterList[0].AmountTo * taxMasterList[0].TaxPercentage) / (12 * 100);
                                remainingtaxableamount = totaltaxableamount - taxMasterList[0].AmountTo;
                            }
                            else
                            {
                            if (remainingtaxableamount > 0)
                            {
                                if(remainingtaxableamount>= (taxMasterList[i].AmountTo - taxMasterList[i].AmountFrom))
                                {
                                    remainingtax += ((taxMasterList[i].AmountTo - taxMasterList[i].AmountFrom) * taxMasterList[i].TaxPercentage) / (12 * 100);
                                    remainingtaxableamount = remainingtaxableamount - (taxMasterList[i].AmountTo - taxMasterList[i].AmountFrom);
                                }
                                else
                                {
                                    remainingtax += ((remainingtaxableamount) * taxMasterList[i].TaxPercentage) / (12 * 100);
                                    remainingtaxableamount = remainingtaxableamount - (taxMasterList[i].AmountTo - taxMasterList[i].AmountFrom);
                                }
                              
                            }
                        }
                    }
                  {


                    }
                        
                    }

                salaryInfoList.TaxInfo.Add(new SalaryInfo()
                {
                    SalaryHeadingID = taxlist.Where(x => x.IsTax == true).ToList()[0].SalaryHeadingID.ToString(),
                    SalHeadingName = taxlist.Where(x => x.IsTax == true).ToList()[0].SalHeadingName.ToString(),
                    Amount = Math.Round(tax1,2),
                    SortOrder = taxlist.ToList()[0].SortOrder
                });

                salaryInfoList.TaxInfo.Add(new SalaryInfo()
                {
                    SalaryHeadingID = taxlist.Where(x => x.IsTax == true).ToList()[1].SalaryHeadingID.ToString(),
                    SalHeadingName = taxlist.Where(x => x.IsTax == true).ToList()[1].SalHeadingName.ToString(),
                    Amount = Math.Round(remainingtax,2),
                    SortOrder = taxlist.ToList()[1].SortOrder
                });
                
               
                salaryInfoList.SalInfoFinalTotal = new SalaryInfo()
                {
                    SalaryHeadingID = salaryHeadingList.Where(x => x.IsSalaryCalculatePoint == true).ToList()[1].SalaryHeadingID.ToString(),
                    SalHeadingName = salaryHeadingList.Where(x => x.IsSalaryCalculatePoint == true).ToList()[1].SalHeadingName.ToString(),
                    Amount = (amount - salaryInfoList.TaxInfo.Sum(x => x.Amount))<0?0: amount - salaryInfoList.TaxInfo.Sum(x => x.Amount),
                    SortOrder = salaryHeadingList.Where(x=>x.IsSalaryCalculatePoint).ToList()[1].SortOrder
                };

            }
            return salaryInfoList;
            }

        public int DeleteData(SalaryCalculate salaryHeadAmount)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Year", salaryHeadAmount.Year);
                    param.Add("@Month", salaryHeadAmount.Month);
                    param.Add("@EmployeeID", salaryHeadAmount.EmployeeID);

                    var deletesuccess = connection.Execute("[dbo].[DeleteSalary]", param, commandType: CommandType.StoredProcedure);
                    return deletesuccess;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int SalaryBatchUpload(List<SalaryCalculate> salary)
        {
            try
            {
                using (SqlConnection connection = DBManager.DbConnect1())
                {
                    connection.Open();
                    using (var scope = new TransactionScope())
                    {
                        try
                        {
                            var sqlBulkCopy = new SqlBulkCopy(connection)
                            {
                                DestinationTableName = "[Salary]",
                                BulkCopyTimeout = 6000,
                                BatchSize = salary.Count()
                            };
                            var dataTable = GetDataTableSalaryHeadBatchUpload(salary);
                            sqlBulkCopy.WriteToServer(dataTable);
                            scope.Complete();
                            sqlBulkCopy.Close();
                            return salary.Count();

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable GetDataTableSalaryHeadBatchUpload(List<SalaryCalculate> leaveEntry)
        {
            var table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("EmployeeID", typeof(int));
            table.Columns.Add("SalaryHeadID", typeof(int));
            table.Columns.Add("Amount", typeof(int));
           
            table.Columns.Add("Year", typeof(int));
            table.Columns.Add("Month", typeof(int));
            table.Columns.Add("SortOrder", typeof(int));

            leaveEntry.ForEach(data => table.Rows.Add(
                                                    data.ID,
                                                    data.EmployeeID,
                                                    data.SalaryHeadingID,
                                                   data.Amount,
                                                    data.Year,
                                                    data.Month,
                                                   data.SortOrder                                                


                                                ));
            return table;
        }
    }
    }

