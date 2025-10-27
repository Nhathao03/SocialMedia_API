using System.ComponentModel;

namespace Social_Media.Helpers
{
    public class Constants
    {
        public enum FriendRequestStatus
        {
            [Description("Đã gửi lời mời, đang chờ người kia phản hồi")]
            Pending = 1,
            [Description("Người kia đã chấp nhận lời mời")]
            Accepted = 2,
            [Description("Người kia đã từ chối lời mời")]
            Rejected = 3,
            [Description("Người gửi đã hủy lời mời trước khi người kia phản hồi")]
            Canceled = 4,
            [Description("Một trong hai người đã chặn nhau")]
            Blocked = 5,
            [Description("Đã từng là bạn, nhưng sau đó hủy kết bạn")]
            Removed = 6     
        }

        public enum FriendsEnum
        {
            [Description("Normal")]
            Normal = 5,
            [Description("Close friends")]
            Close_Friends = 1,
            [Description("Home Town")]
            Home_Town = 2,
        }

        public enum NotificationTypeEnum
        {
            [Description("Like")]
            Like = 1,
            [Description("Comment")]
            Comment = 2,
            [Description("FriendRequest")]
            FriendRequest = 3,
            [Description("PostShare")]
            PostShare = 4,
            [Description("Message")]
            Message = 5,
        }

        public enum ReportStatusEnum
        {
            [Description("Pending")]
            Pending = 1,
            [Description("Resolved")]
            Resolved = 2,
        }
    }
}
