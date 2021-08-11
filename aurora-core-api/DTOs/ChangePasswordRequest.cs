namespace AuroraCore.Web.DTOs
{
    public class ChangePasswordRequest
    {
        public string Current { get; set; }

        public string New { get; set; }

        public string Confirmation { get; set; }
    }
}