using SocialMedia.Core.Entities.Entity;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface ISearchRepository
    {
        Task<IEnumerable<User>> FindUser(string stringData, string CurrentUserIdSearch);
    }
}
