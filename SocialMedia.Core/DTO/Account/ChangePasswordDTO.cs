namespace SocialMedia.Core.Entities.DTO.Account
{
    public class ChangePasswordDTO
    {
        public string newPass { get; set; }
        public string currentPass { get; set; }
        public string verifyPass { get; set; }
    }
}
