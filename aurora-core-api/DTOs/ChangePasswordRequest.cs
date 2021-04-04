namespace aurora_core_api.DTOs
{
    public class ChangePasswordRequest
    {

        public string Current { get; set; }

        public string New { get; set; }

        public string Confirmation { get; set; }
    }
}
