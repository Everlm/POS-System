namespace POS.Application.Dtos.Business.Request
{
    public class BusinessRequestDto
    {
        public string? Code { get; set; }
        public string? Ruc { get; set; }
        public string? BusinessName { get; set; }
        public string? Logo { get; set; }
        public int DistrictId { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Vision { get; set; }
        public string? Mision { get; set; }
        public int State { get; set; }
    }
}
