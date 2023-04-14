namespace POS.Application.Dtos.BranchOffice.Response
{
    public class BranchOfficeResponseDto
    {
        public int BranchOfficeId { get; set; }
        public string? Code { get; set; }
        public string? Business { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? District { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime AuditCreateDate { get; set; }
        public int State { get; set; }
        public string? StateBranchOffice { get; set; }
    }
}
