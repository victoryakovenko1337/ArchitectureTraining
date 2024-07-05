using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeBase.Infrastructure.Services.Ads
{
    public class AdsService : IAdsService, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private const string AndroidGameId = "5651185";
        private const string IOSGameId = "5651184";

        private const string RewardedVideoPlacementId = "";
        private const bool TestMode = true;

        public event Action RewardedVideoReady;

        public bool IsRewardedVideoReady => true;
        public int Reward => 13;

        private Action _onVideoFinished;

        private bool _adReady;
        private string _gameId;


        public void Initialize()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _gameId = AndroidGameId;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    _gameId = IOSGameId;
                    break;
                case RuntimePlatform.WindowsEditor:
                    _gameId = IOSGameId;
                    break;
                default:
                    Debug.Log("Unsupported platform for ads");
                    break;
            }

            Advertisement.Initialize(_gameId, TestMode);
        }

        public void LoadRewardedVideo()
        {
            Debug.Log("Loading Ad: " + _gameId);
            Advertisement.Load(_gameId, this);
        }

        public void ShowRewardedVideo(Action onVideoFinished)
        {
            Debug.Log("Showing Ad: " + _gameId);
            Advertisement.Show(_gameId, this);

            _onVideoFinished = onVideoFinished;
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) =>
            Debug.Log($"OnUnityAdsShowFailure {placementId}");

        public void OnUnityAdsShowStart(string placementId) =>
            Debug.Log($"OnUnityAdsShowStart {placementId}");

        public void OnUnityAdsShowClick(string placementId) =>
            Debug.Log($"OnUnityAdsShowClick {placementId}");

        public void OnUnityAdsAdLoaded(string placementId) =>
            Debug.Log($"OnUnityAdsAdLoaded {placementId}");

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) =>
            Debug.Log($"Error loading Ad Unit: {placementId} - {error.ToString()} - {message}");

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
    }
}
