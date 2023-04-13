namespace POS.Application.Dtos.Business.Response
{
    public class BusinessResponseDto
    {
        public int BusinessId { get; set; }
        public string? Code { get; set; }
        public string? Ruc { get; set; }
        public string? BusinessName { get; set; }
        public string? Logo { get; set; }
        public string? District { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Vision { get; set; }
        public string? Mision { get; set; }
        public DateTime AuditCreateDate { get; set; }
        public int State { get; set; }
        public string? StateBusiness { get; set; }
    }
}
