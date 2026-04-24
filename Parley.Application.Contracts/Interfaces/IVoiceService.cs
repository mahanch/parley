namespace Parley.Application.Contracts.Interfaces;

public interface IVoiceService
{
    string GenerateJoinToken(string channelId, string userId, string userName);
}