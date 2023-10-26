using System.ComponentModel;

namespace WebApi.Dto
{
    public class ResetPasswordDto
    {
        [DefaultValue("firhan.faisal1995@gmail.com")]
        public string Email { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
