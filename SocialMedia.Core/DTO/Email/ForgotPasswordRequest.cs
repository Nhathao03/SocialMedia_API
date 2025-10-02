using Microsoft.Extensions.Primitives;

namespace SocialMedia.Core.Entities.Email
{
    public class ForgotPasswordRequest
    {
        public string Email { get; set; }
    }
}
