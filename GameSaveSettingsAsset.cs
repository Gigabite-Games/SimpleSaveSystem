using UnityEngine;

namespace SimpleSaveSystem
{
    [CreateAssetMenu(menuName = "Game Save Settings")]
    public class GameSaveSettingsAsset : ScriptableObject
    {
        [SerializeField] protected GameSaveSettings _gameSaveSettings;

        public GameSaveSettings GameSaveSettings
        {
            get => _gameSaveSettings;
            set => _gameSaveSettings = value;
        }
    }
}