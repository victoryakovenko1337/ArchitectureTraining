using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.Randomizer;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.UI;
using System.Collections.Generic;
using CodeBase.Data;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssets _assets;
        private readonly IStaticDataService _staticData;
        private readonly IRandomService _randomService;
        private readonly IPersistentProgressService _progressService;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        private GameObject HeroGameObject { get; set;  }

        public GameFactory(IAssets assets, IStaticDataService staticData, IRandomService random, IPersistentProgressService progressService)
        {
            _assets = assets;
            _staticData = staticData;
            _randomService = random;
            _progressService = progressService;
        }

        public GameObject CreateHero(GameObject at)
        {
            HeroGameObject = InstantiateRegistered(AssetPath.HeroPath, at.transform.position);
            return HeroGameObject;
        }

        public GameObject CreateHud() => InstantiateRegistered(AssetPath.HudPath);

        public GameObject CreateMonster(MonsterTypeId typeId, Transform parent)
        {
            MonsterStaticData monsterData = _staticData.ForMonster(typeId);
            GameObject monster = Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);

            IHealth health = monster.GetComponent<IHealth>();
            health.Current = monsterData.Hp;
            health.Max = monsterData.Hp;

            monster.GetComponent<ActorUI>().Construct(health);
            monster.GetComponent<AgentMoveToPlayer>().Construct(HeroGameObject.transform);
            monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

            LootSpawner lootSpawner = monster.GetComponentInChildren<LootSpawner>();
            lootSpawner.SetLoot(monsterData.MinLoot, monsterData.MaxLoot);
            lootSpawner.Construct(this, _randomService);

            var attack = monster.GetComponent<Attack>();
            attack.Construct(HeroGameObject.transform);
            attack.Damage = monsterData.Damage;
            attack.Cleavage = monsterData.Cleavage;
            attack.EffectiveDistance = monsterData.EffectiveDistance;

            monster.GetComponent<RotateToHero>()?.Construct(HeroGameObject.transform);

            return monster;
        }

        public LootPiece CreateLoot()
        {
             var lootPiece = InstantiateRegistered(AssetPath.Loot).GetComponent<LootPiece>();

             lootPiece.Construct(_progressService.Progress.WorldData);

             return lootPiece;
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        public void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);

            ProgressReaders.Add(progressReader);
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            GameObject gameObject = _assets.Instantiate(prefabPath, at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(string prefabPath)
        {
            GameObject gameObject = _assets.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }
    }
}