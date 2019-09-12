using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

public static class GameSave
{
    public static UnityEvent OnSaveSuccessful { get; set; } = new UnityEvent();
    public static UnityEvent OnSaveFailed { get; set; } = new UnityEvent();
    public static UnityEvent OnLoadSuccessful { get; set; } = new UnityEvent();
    public static UnityEvent OnLoadFailed { get; set; } = new UnityEvent();

    public static void Save<T>(T obj, string name = "save", string filePath = "/", string extension = "json",
        SaveDataType dataType = SaveDataType.Json, bool fireEvents = true, bool throwExceptions = false)
    {
        var fullFilepath = BuildFullFilepath(filePath, name, extension);

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

            if (fireEvents) OnSaveSuccessful?.Invoke();
        }
        catch
        {
            if (fireEvents) OnSaveFailed?.Invoke();
            if (throwExceptions) throw;
        }
    }

    public static T Load<T>(string name = "save", string filePath = "/", string extension = "json",
        SaveDataType dataType = SaveDataType.Json, bool fireEvents = true, bool throwExceptions = false)
    {
        var fullFilepath = BuildFullFilepath(filePath, name, extension);

        try
        {
            if (File.Exists(fullFilepath))
                switch (dataType)
                {
                    default:
                        throw new NotImplementedException(
                            $"The selected SaveDataType '{dataType.ToString()}' is not implemented");
                    case SaveDataType.Json:
                        var json = File.ReadAllText(fullFilepath);
                        var obj = JsonConvert.DeserializeObject<T>(json);
                        if (fireEvents) OnLoadSuccessful?.Invoke();
                        return obj;
                    case SaveDataType.Binary:
                        var formatter = new BinaryFormatter();
                        var file = File.Open(fullFilepath, FileMode.Open);
                        file.Position = 0;
                        var data = (T) formatter.Deserialize(file);
                        file.Close();
                        if (fireEvents) OnLoadSuccessful?.Invoke();
                        return data;
                }
        }
        catch
        {
            if (fireEvents) OnLoadFailed?.Invoke();
            if (throwExceptions) throw;
        }

        return default;
    }

    public static string BuildFullFilepath(string filePath, string name, string extension)
    {
        return $"{Application.persistentDataPath}{filePath}{name}.{extension}";
    }
}