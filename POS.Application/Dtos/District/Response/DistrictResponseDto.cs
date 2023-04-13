namespace POS.Application.Dtos.District.Response
{
    public class DistrictResponseDto
    {
        public int DistrictId { get; set; }
        public string? Province { get; set; }
        public string? Name { get; set; }
        public DateTime AuditCreateDate { get; set; }
        public int State { get; set; }
        public string? StateDistrict { get; set; }
    }
}
