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

        public enum FriendsStatus
        {
            [Description("Normal")]
            Normal = 5,
            [Description("Close friends")]
            Close_Friends = 1,
            [Description("Home Town")]
            Home_Town = 2,
        }

        public enum ReportStatus
        {
            [Description("Pending")]
            Pending = 1,
            [Description("Resolved")]
            Resolved = 2,
        }
    }

    public enum FriendShipStatus
    {
        None,
        Pending,
        Friends,
        Rejected
    }

    public enum ReportEntityType
    {
        Post = 1,
        Comment = 2,
        User = 3,
        Reply = 4
    }

    public enum  ReportStatus
    {
        Pending = 1,
        Reviewing = 2,
        Resolved = 3,
        Rejected = 4
    }
}
