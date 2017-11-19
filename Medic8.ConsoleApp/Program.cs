using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using Medic8.ConsoleApp.Models;

namespace Medic8.ConsoleApp
{
    /// <summary>
    /// Console App that displays diagnosis results for a Member Id input by user.
    /// </summary>
    class Program
    {
        static void Main()
        {
            try
            {
                while (true)
                {
                    Console.Write("Enter Member Id (or 0 to exit):");
                    var input = Console.ReadLine();

                    if (!string.IsNullOrEmpty(input) && input.Equals("0", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    int memberId;
                    if (Int32.TryParse(input, out memberId))
                    {
                        using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["Medic8DbContext"].ConnectionString))
                        {

                            var sql = Constants.AllMembersDiagnosisSqlStatement;

                            var memberDiagnosis =
                                ((List<MemberDiagnosisSummaryDto>)db.Query<MemberDiagnosisSummaryDto>(
                                    sql,
                                    new { TheMemberId = memberId })
                                )
                                .FirstOrDefault();

                            if (memberDiagnosis != null)
                            {
                                DisplayDiagnosis(memberDiagnosis);
                            }
                            else
                            {
                                Console.WriteLine($"No diagnosis records found for Member with Member Id = {memberId}");
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                // TODO: Log exception
                Console.WriteLine($"Oops! An Error Occurred: {exception.Message}");                
            }

        }

        /// <summary>
        /// Displays the Diagnosis summary data on the console, for a <see cref="MemberDiagnosisSummaryDto"/>  passed in as parameter
        /// </summary>
        /// <param name="diagnosisSummary">The diagnosis that you want to display</param>
        private static void DisplayDiagnosis(MemberDiagnosisSummaryDto diagnosisSummary)
        {
            if (diagnosisSummary != null)
            {
                var sb = new StringBuilder();

                // TODO: Convert these checks into Helper method that checks for default value of type, and returns "" or the real value
                string diagnosisId = diagnosisSummary.MostSevereDiagnosisId == 0 ? "" : diagnosisSummary.MostSevereDiagnosisId.ToString();
                string categoryId = diagnosisSummary.MostSevereCategoryId == 0 ? "" : diagnosisSummary.MostSevereCategoryId.ToString();

                sb.AppendLine($"Medic8 - Diagnosis Report Summary::");
                sb.AppendLine($"                        Member Id: {diagnosisSummary.MemberId}");
                sb.AppendLine($"                       First Name: {diagnosisSummary.FirstName}");
                sb.AppendLine($"                        Last Name: {diagnosisSummary.LastName}");
                sb.AppendLine($"         Most Severe Diagnosis Id: {diagnosisId}");
                sb.AppendLine($"Most Severe Diagnosis Description: {diagnosisSummary.MostSevereDiagnosisDescription}");
                sb.AppendLine($"                      Category ID: {categoryId}");
                sb.AppendLine($"             Category Description: {diagnosisSummary.CategoryDescription}");
                sb.AppendLine($"                   Category Score: {diagnosisSummary.CategoryScore }");
                sb.AppendLine($"          Is Most Severe Category: {diagnosisSummary.IsMostSevereCategory}");

                Console.WriteLine(sb.ToString());
                sb.Clear();
            }
        }
    }
}
