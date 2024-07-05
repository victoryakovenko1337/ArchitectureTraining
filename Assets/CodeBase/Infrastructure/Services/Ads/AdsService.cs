using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeBase.Infrastructure.Services.Ads
{
    public class AdsService : IAdsService, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private const string AndroidGameId = "5651185";
        private const string IOSGameId = "5651184";

        private const string RewardedVideoIdAndroid = "Rewarded_Android";
        private const string RewardedVideoIdIOS = "Rewarded_iOS";

        private const bool TestMode = true;

        public event Action RewardedVideoReady;

        public bool IsRewardedVideoReady => _adReady;
        public int Reward => 20;

        private Action _onVideoFinished;

        private bool _adReady;
        private string _gameId;
        private string _placementId;

        public void Initialize()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _gameId = AndroidGameId;
                    _placementId = RewardedVideoIdAndroid;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    _gameId = IOSGameId;
                    _placementId = RewardedVideoIdIOS;
                    break;
                case RuntimePlatform.WindowsEditor:
                    _gameId = IOSGameId;
                    _placementId = RewardedVideoIdIOS;
                    break;
                default:
                    Debug.Log("Unsupported platform for ads");
                    break;
            }

            Advertisement.Initialize(_gameId, TestMode, this);
            _adReady = true;
        }

        public void ShowRewardedVideo(Action onVideoFinished)
        {
            _adReady = false;
            Advertisement.Load(_placementId, this);
            Advertisement.Show(_placementId, this);

            _onVideoFinished = onVideoFinished;
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log($"OnUnityAdsAdLoaded {placementId}");

            if (placementId == _placementId)
            {
                _adReady = true;
                RewardedVideoReady?.Invoke();
            }
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            switch (showCompletionState)
            {
                case UnityAdsShowCompletionState.SKIPPED:
                    Debug.Log($"OnUnityAdsShowComplete {showCompletionState}");
                    break;
                case UnityAdsShowCompletionState.COMPLETED:
                    _onVideoFinished?.Invoke();
                    break;
                case UnityAdsShowCompletionState.UNKNOWN:
                    Debug.Log($"OnUnityAdsShowComplete {showCompletionState}");
                    break;
                default:
                    Debug.Log($"OnUnityAdsShowComplete {showCompletionState}");
                    break;
            }

            _onVideoFinished = null;
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) =>
            Debug.Log($"OnUnityAdsShowFailure {placementId} - {error.ToString()} - message: {message}");

        public void OnUnityAdsShowStart(string placementId) =>
            Debug.Log($"OnUnityAdsShowStart {placementId}");

        public void OnUnityAdsShowClick(string placementId) =>
            Debug.Log($"OnUnityAdsShowClick {placementId}");

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) =>
            Debug.Log($"Error loading Ad Unit: {placementId} - {error.ToString()} - {message}");

        public void OnInitializationFailed(UnityAdsInitializationError error, string message) =>
            Debug.Log($"Error initialize Ad Unit: {error.ToString()} - {message}");

        public void OnInitializationComplete() { }
    }
}
