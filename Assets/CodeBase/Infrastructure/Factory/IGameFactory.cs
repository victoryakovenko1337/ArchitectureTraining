using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }

        void Register(ISavedProgressReader savedProgress);

        GameObject CreateHero(GameObject at);
        GameObject CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
        GameObject CreateLoot();
        GameObject CreateHud();

        void Cleanup();
    }
}