namespace Frapper.ViewModel.Reports
{
    public class UserSummaryReport
    {
        public string TotalUsers { get; set; }
        public string RoleName { get; set; }
        public string CreatedOn { get; set; }
    }

    public class UserDetailsReport
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string Gender { get; set; }
        public string RoleName { get; set; }
        public string CreatedOn { get; set; }
        public string IsFirstLoginDate { get; set; }
    }
}