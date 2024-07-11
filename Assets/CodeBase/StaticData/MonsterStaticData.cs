using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
    public class MonsterStaticData : ScriptableObject
    {
        public MonsterTypeId MonsterTypeId;

        [Range(1, 100)]
        public int Hp;

        [Range(1f, 30)]
        public float Damage;

        public int MinLoot;

        public int MaxLoot;

        [Range(1f, 10f)]
        public float MoveSpeed = 5f;

        [Range(0.5f, 1)]
        public float EffectiveDistance;

        [Range(0.5f, 1)]
        public float Cleavage;

        public AssetReferenceGameObject PrefabReference;
    }
}