using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using SonitCustom.BLL.Interface;
using SonitCustom.BLL.Settings;
using System.Net;

namespace SonitCustom.BLL.Services
{
    /// <summary>
    /// Service triển khai các thao tác lưu trữ file trên Cloudflare R2
    /// </summary>
    public class R2Service : IR2Service
    {
        private readonly IAmazonS3 _r2Client;
        private readonly string _bucketName;
        private readonly string _publicUrl;

        /// <summary>
        /// Khởi tạo đối tượng R2Service
        /// </summary>
        /// <param name="r2Settings">Cấu hình kết nối đến Cloudflare R2</param>
        public R2Service(R2Settings r2Settings)
        {
            _bucketName = r2Settings.BucketName;
            _publicUrl = r2Settings.PublicUrl;

            _r2Client = CreateR2Client(r2Settings);
        }

        /// <inheritdoc />
        public async Task<string> UploadFileAsync(IFormFile file, string? key = null)
        {
            string finalKey = GenerateFileKey(file, key);

            using Stream inputStream = file.OpenReadStream();

            PutObjectRequest putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = finalKey,
                InputStream = inputStream,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.PublicRead,
                UseChunkEncoding = false
            };

            PutObjectResponse response = await _r2Client.PutObjectAsync(putRequest);

            if (response.HttpStatusCode != HttpStatusCode.OK)
                throw new Exception("Failed to upload file to R2");

            return $"{_publicUrl}/{finalKey}";
        }

        /// <inheritdoc />
        public async Task<bool> DeleteFileAsync(string key)
        {
            DeleteObjectRequest deleteRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            DeleteObjectResponse response = await _r2Client.DeleteObjectAsync(deleteRequest);
            return response.HttpStatusCode == HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Tạo client kết nối đến Cloudflare R2
        /// </summary>
        /// <param name="settings">Cấu hình kết nối</param>
        /// <returns>Client Amazon S3 được cấu hình cho Cloudflare R2</returns>
        private static IAmazonS3 CreateR2Client(R2Settings settings)
        {
            BasicAWSCredentials credentials = new BasicAWSCredentials(
                settings.AccessKey.Trim(),
                settings.SecretKey.Trim()
            );

            AmazonS3Config config = new AmazonS3Config
            {
                ServiceURL = $"https://{settings.AccountId}.r2.cloudflarestorage.com",
                ForcePathStyle = true,
                UseHttp = false,
                DisableHostPrefixInjection = true
            };

            return new AmazonS3Client(credentials, config);
        }

        /// <summary>
        /// Tạo khóa (key) cho file khi lưu trữ
        /// </summary>
        /// <param name="file">File được tải lên</param>
        /// <param name="key">Khóa được chỉ định (nếu có)</param>
        /// <returns>Khóa duy nhất cho file</returns>
        private static string GenerateFileKey(IFormFile file, string? key)
        {
            key ??= Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return $"{DateTime.UtcNow:yyyyMMddHHmmss}-{key}{extension}";
        }
    }
}
