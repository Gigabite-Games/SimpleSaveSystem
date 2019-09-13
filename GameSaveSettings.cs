using System;
using UnityEngine;

namespace SimpleSaveSystem
{
    [Serializable]
    public class GameSaveSettings
    {
        [SerializeField] protected string _name;
        [SerializeField] protected string _filePath;
        [SerializeField] protected string _extension;
        [SerializeField] protected SaveDataType _dataType;
        [SerializeField] protected bool _fireEvents;
        [SerializeField] protected bool _throwExceptions;

        public GameSaveSettings()
        {
            _name = "save";
            _filePath = "/";
            _extension = "json";
            _dataType = SaveDataType.Json;
            _fireEvents = true;
            _throwExceptions = false;
        }

        #region Accessors

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string FilePath
        {
            get => _filePath;
            set => _filePath = value;
        }

        public string Extension
        {
            get => _extension;
            set => _extension = value;
        }

        public SaveDataType DataType
        {
            get => _dataType;
            set => _dataType = value;
        }

        public bool FireEvents
        {
            get => _fireEvents;
            set => _fireEvents = value;
        }

        public bool ThrowExceptions
        {
            get => _throwExceptions;
            set => _throwExceptions = value;
        }

        #endregion
    }
}