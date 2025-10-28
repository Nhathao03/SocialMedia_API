namespace SocialMedia.Core.DTO.Friend
{
    public class FriendDTO
    {
        public string UserID { get; set; }
        public string FriendID { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public int Type_FriendsID { get; set; }
    }
}
