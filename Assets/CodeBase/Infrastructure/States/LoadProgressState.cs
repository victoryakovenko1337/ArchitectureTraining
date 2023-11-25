﻿using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;

namespace CodeBase.Infrastructure.States
{
    internal class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;

        public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();

            _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
        }


        public void Exit()
        {
        }

        private void LoadProgressOrInitNew() => 
            _progressService.Progress =
                _saveLoadService.LoadProgress()
                ?? NewProgress();

        private PlayerProgress NewProgress()
        {
            var progress = new PlayerProgress(initialLevel: "Main");

            progress.HeroState.MaxHP = 50;
            progress.HeroState.ResetHP();

            return progress;
        }
    }
}