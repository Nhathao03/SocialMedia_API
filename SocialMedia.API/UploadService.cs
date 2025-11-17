namespace SocialMedia.API
{
    public class UploadService
    {
        public async Task<string> UploadImagePostAsync(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var savePath = Path.Combine("wwwroot/posts/image", file.FileName);
                using (var fileStream = new FileStream(savePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                string filePath = "/posts/image" + file.FileName;
                return filePath;
            }
            throw new ArgumentException("File is empty", nameof(file));
        }

        public async Task<string> UploadImageCommentAsync(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var savePath = Path.Combine("wwwroot/comments/images", file.FileName);
                using (var fileStream = new FileStream(savePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                string filePath = "/comments/images/" + file.FileName;
                return filePath;
            }
            throw new ArgumentException("File is empty", nameof(file));
        }

        public async Task<string> UploadAvatarUserAsync(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var savePath = Path.Combine("wwwroot/user/avatar", file.FileName);
                using (var fileStream = new FileStream(savePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                string filePath = "/user/avatar/" + file.FileName;
                return filePath;
            }
            throw new ArgumentException("File is empty", nameof(file));
        }

        public async Task<string> UploadBackgroundUserAsync(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var savePath = Path.Combine("wwwroot/user/background", file.FileName);
                using (var fileStream = new FileStream(savePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                string filePath = "/user/background/" + file.FileName;
                return filePath;
            }
            throw new ArgumentException("File is empty", nameof(file));
        }
    }
}
