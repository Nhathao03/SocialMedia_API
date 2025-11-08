using SocialMedia.Infrastructure.Repositories;
using System.Text;
using SocialMedia.Core.Entities.Entity;

namespace SocialMedia.Core.Services
{
    public class SearchService :ISearchService
    {
        private readonly ISearchRepository _searchRepository;
        public SearchService(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        // Normalize string by removing diacritics and converting to lowercase
        private static string NormalizeString(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            input = input.ToLowerInvariant();
            var normalized = input.Normalize(System.Text.NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }
            return sb.ToString().Normalize(System.Text.NormalizationForm.FormC);
        }

    }
}
