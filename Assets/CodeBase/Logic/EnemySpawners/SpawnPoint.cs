using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic.EnemySpawners
{
    public class SpawnPoint : MonoBehaviour, ISavedProgress
    {
        public MonsterTypeId MonsterTypeId;
        public string Id { get; set; }

        private bool _slain;
        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;

        public void Construct(IGameFactory gameFactory)
        {
            _factory = gameFactory;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(Id))
                _slain = true;
            else
                Spawn();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_slain)
            {
                progress.KillData.ClearedSpawners.Add(Id);
            }
        }

        private async void Spawn()
        {
            GameObject monster = await _factory.CreateMonster(MonsterTypeId, transform);
            _enemyDeath = monster.GetComponent<EnemyDeath>();
            _enemyDeath.Happened += Slay;
        }

        private void Slay()
        {
            if (_enemyDeath != null)
                _enemyDeath.Happened -= Slay;

            _slain = true;
        }
    }
}