namespace Medic8.ConsoleApp.Models
{
    public class MemberDiagnosisSummaryDto
    {
        public int MemberId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int MostSevereDiagnosisId { get; set; }

        public string MostSevereDiagnosisDescription { get; set; }

        public int MostSevereCategoryId { get; set; }

        public string CategoryDescription { get; set; }

        public int CategoryScore { get; set; }

        public int IsMostSevereCategory { get; set; }
    }
}
