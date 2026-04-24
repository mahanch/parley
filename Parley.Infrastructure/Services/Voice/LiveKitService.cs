using Livekit.Server.Sdk.Dotnet;
using Parley.Application.Contracts.Interfaces;

namespace Parley.Infrastructure.Services.Voice;

public class LiveKitService : IVoiceService
{
    private readonly string _apiKey = "devkey"; // از AppSettings خوانده شود
    private readonly string _apiSecret = "secret"; 

    public string GenerateJoinToken(string channelId, string userId, string userName)
    {
        var tokenGen = new AccessToken(_apiKey, _apiSecret);
        tokenGen.WithName(userName);
        tokenGen.WithIdentity(userId);
        tokenGen.WithGrants(new VideoGrants 
        { 
            RoomJoin = true, 
            Room = channelId 
        });

        return tokenGen.ToJwt();
    }
}