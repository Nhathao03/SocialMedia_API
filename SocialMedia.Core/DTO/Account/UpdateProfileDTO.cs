namespace SocialMedia.Core.Entities.DTO.Account
{
    public class UpdateProfileDTO
    {
        public string fullname { get; set; }
        public int? addressId { get; set; }
        public DateTime? Birth {  get; set; }
        public string gender { get; set; }
        public string avatar { get; set; }

    }
}
