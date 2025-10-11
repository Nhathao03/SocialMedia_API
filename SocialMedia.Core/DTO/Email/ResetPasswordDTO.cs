namespace SocialMedia.Core.Entities.Email
{
    public class ResetPasswordDTO
    {
        public string Otp { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
