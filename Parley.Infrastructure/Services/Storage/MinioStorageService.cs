using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;
using Parley.Application.Contracts.Interfaces.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Parley.Infrastructure.Services.Storage;

public class MinioStorageService : IStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;

    public MinioStorageService(IMinioClient minioClient, IConfiguration configuration)
    {
        _minioClient = minioClient ?? throw new ArgumentNullException(nameof(minioClient));
        _bucketName = configuration["Storage:BucketName"] ?? "parley-attachments";
    }

    public async Task<(string UploadUrl, string ObjectKey)> GeneratePresignedUploadUrlAsync(string fileName, string contentType, TimeSpan expiration, CancellationToken cancellationToken = default)
    {
        // تولید یک شناسه یکتا برای فایل
        var fileId = Guid.NewGuid().ToString("N");
        
        // استخراج پسوند فایل
        var extension = System.IO.Path.GetExtension(fileName);
        
        // مسیر نهایی فایل در باکت (مثال: attachments/2026/05/18/guid.jpg)
        var datePath = DateTime.UtcNow.ToString("yyyy/MM/dd");
        var objectKey = $"attachments/{datePath}/{fileId}{extension}";

        // پارامترهای تولید لینک در MinIO
        var args = new PresignedPutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectKey)
            .WithExpiry((int)expiration.TotalSeconds);

        // تولید لینک
        var url = await _minioClient.PresignedPutObjectAsync(args).ConfigureAwait(false);
        
        return (url, objectKey);
    }
}
