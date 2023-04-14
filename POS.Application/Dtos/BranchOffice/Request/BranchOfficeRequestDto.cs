namespace POS.Application.Dtos.BranchOffice.Request
{
    public class BranchOfficeRequestDto
    {
        public string? Code { get; set; }
        public int BusinessId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int DistrictId { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; } 

    }
}
