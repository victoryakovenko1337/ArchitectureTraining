using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace CodeBase.UI
{
    public class LootCounter : MonoBehaviour
    {
        public TextMeshProUGUI Counter;

        private WorldData _worldData;

        public void Construct(WorldData data)
        {
            _worldData = data;
            _worldData.LootData.Changed += UpdateCounter;
        }

        private void Start()
        {
            UpdateCounter();
        }

        private void OnDestroy()
        {
            _worldData.LootData.Changed -= UpdateCounter;
        }

        private void UpdateCounter()
        {
            Counter.text = $"{_worldData.LootData.Collected}";
        }
    }
}
