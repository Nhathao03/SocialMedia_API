using SocialMedia.Core.Entities.Entity;

namespace SocialMedia.Core.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<User>> FindUserAsync(string stringData, string CurrentUserIdSearch);
    }
}
