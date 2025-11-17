namespace SocialMedia.Core.DTO.Friend
{
    public class FriendDTO
    {
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public int Type_FriendsId { get; set; }
    }
}
