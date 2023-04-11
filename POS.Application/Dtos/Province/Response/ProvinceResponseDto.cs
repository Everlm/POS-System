namespace POS.Application.Dtos.Province.Response
{
    public class ProvinceResponseDto
    {
        public int ProvinceId { get; set; }
        public string? Name { get; set; }
        public string? Department { get; set; }
        public DateTime AuditCreateDate { get; set; }
        public int State { get; set; }
        public string? StateProvince { get; set; }
    }
}
