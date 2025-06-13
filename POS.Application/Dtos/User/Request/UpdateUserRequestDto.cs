

namespace POS.Application.Dtos.User.Request
{
    public class UpdateUserRequestDto
    {
        public int UserId { get; set; } 
        public string Email { get; set; } = string.Empty;
        public List<int> Roles { get; set; } = new List<int>();

    }
}