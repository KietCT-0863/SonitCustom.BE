using Microsoft.AspNetCore.Http;

namespace SonitCustom.BLL.Interface
{
    /// <summary>
    /// Service quản lý lưu trữ file trên Cloudflare R2
    /// </summary>
    public interface IR2Service
    {
        /// <summary>
        /// Tải lên một file lên R2 bucket
        /// </summary>
        /// <param name="file">File được tải lên</param>
        /// <param name="key">Tên file trên R2 (nếu null, sẽ sử dụng tên file gốc)</param>
        /// <returns>URL công khai của file</returns>
        Task<string> UploadFileAsync(IFormFile file, string? key = null);

        /// <summary>
        /// Xóa file khỏi R2 bucket
        /// </summary>
        /// <param name="key">Tên file cần xóa</param>
        /// <returns>Kết quả xóa file</returns>
        Task<bool> DeleteFileAsync(string key);
    }
} 