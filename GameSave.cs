using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleSaveSystem
{
    public static class GameSave
    {
        #region Events

        public static UnityEvent OnSaveSuccessful { get; set; } = new UnityEvent();
        public static UnityEvent OnSaveFailed { get; set; } = new UnityEvent();
        public static UnityEvent OnLoadSuccessful { get; set; } = new UnityEvent();
        public static UnityEvent OnLoadFailed { get; set; } = new UnityEvent();

        #endregion


        #region Primary Methods

        public static void Save<T>(T obj, string name = "save", string filePath = "/", string extension = "json",
            SaveDataType dataType = SaveDataType.Json, bool fireEvents = true, bool throwExceptions = false)
        {
            var fullFilepath = BuildFullFilepath(filePath, name, extension);
            var successful = false;

            try
            {
                switch (dataType)
                {
                    default:
                        throw new NotImplementedException(
                            $"The selected SaveDataType '{dataType.ToString()}' is not implemented");
                    case SaveDataType.Json:
                        var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                        File.WriteAllText(fullFilepath, json);
                        break;
                    case SaveDataType.Binary:
                        var formatter = new BinaryFormatter();
                        var file = File.Create(fullFilepath);
                        formatter.Serialize(file, obj);
                        file.Close();
                        break;
                }

                successful = true;
            }
            catch
            {
                if (throwExceptions) throw;
            }
            finally
            {
                if (fireEvents)
                {
                    if (successful)
                        OnSaveSuccessful?.Invoke();
                    else
                        OnSaveFailed?.Invoke();
                }
            }
        }

        public static T Load<T>(string name = "save", string filePath = "/", string extension = "json",
            SaveDataType dataType = SaveDataType.Json, bool fireEvents = true, bool throwExceptions = false)
        {
            var fullFilepath = BuildFullFilepath(filePath, name, extension);
            var successful = false;

            try
            {
                if (File.Exists(fullFilepath))
                {
                    T obj;

                    switch (dataType)
                    {
                        default:
                            throw new NotImplementedException(
                                $"The selected SaveDataType '{dataType.ToString()}' is not implemented");

                        case SaveDataType.Json:
                            var json = File.ReadAllText(fullFilepath);
                            obj = JsonConvert.DeserializeObject<T>(json);
                            break;

                        case SaveDataType.Binary:
                            var formatter = new BinaryFormatter();
                            var file = File.Open(fullFilepath, FileMode.Open);
                            file.Position = 0;
                            obj = (T) formatter.Deserialize(file);
                            file.Close();
                            break;
                    }

                    successful = true;
                    return obj;
                }
            }
            catch
            {
                if (throwExceptions) throw;
            }
            finally
            {
                if (fireEvents)
                {
                    if (successful) OnLoadSuccessful?.Invoke();
                    OnLoadFailed?.Invoke();
                }
            }

            return default;
        }

        #endregion


        #region Helper Methods

        public static string BuildFullFilepath(string filePath, string name, string extension)
        {
            return $"{Application.persistentDataPath}{filePath}{name}.{extension}";
        }

        public static void Save<T>(T obj, GameSaveSettings settings)
        {
            Save(obj, settings.Name, settings.FilePath, settings.Extension, settings.DataType, settings.FireEvents,
                settings.ThrowExceptions);
        }

        public static T Load<T>(GameSaveSettings settings)
        {
            return Load<T>(settings.Name, settings.FilePath, settings.Extension, settings.DataType, settings.FireEvents,
                settings.ThrowExceptions);
        }

        #endregion
    }
}