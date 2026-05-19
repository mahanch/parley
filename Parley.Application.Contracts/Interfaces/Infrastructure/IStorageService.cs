using System;
using System.Threading;
using System.Threading.Tasks;

namespace Parley.Application.Contracts.Interfaces.Infrastructure;

public interface IStorageService
{
    /// <summary>
    /// Generates a presigned URL for direct file upload by the client.
    /// </summary>
    /// <param name="fileName">The original name of the file.</param>
    /// <param name="contentType">The MIME type of the file.</param>
    /// <param name="expiration">How long the URL is valid for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A tuple containing the presigned UploadUrl and the generated ObjectKey.</returns>
    Task<(string UploadUrl, string ObjectKey)> GeneratePresignedUploadUrlAsync(string fileName, string contentType, TimeSpan expiration, CancellationToken cancellationToken = default);
}
