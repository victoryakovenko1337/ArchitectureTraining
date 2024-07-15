using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }

        Task<GameObject> CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
        Task<GameObject> CreateHero(Vector3 at);
        Task<GameObject> CreateHud();
        Task<LootPiece> CreateLoot();
        Task CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);

        void CleanUp();
        Task WarmUp();
    }
}