using System;

namespace CodeBase.Infrastructure.Services.Ads
{
    public interface IAdsService : IService
    {
        event Action RewardedVideoReady;
        bool IsRewardedVideoReady { get; }
        int Reward { get; }
        void Initialize();
        void LoadRewardedVideo();
        void ShowRewardedVideo(Action onVideoFinished);
    }
}
