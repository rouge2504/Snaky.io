using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    private string URL_EDITOR;
    private string URL_ANDROID;

    public static StorageManager Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = new StorageManager();
            }
            return instance;
        }
    }

    private static StorageManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        URL_EDITOR = Application.streamingAssetsPath;
        URL_ANDROID = Application.persistentDataPath;
        instance = this;
    }

    public void ClearJSON()
    {
        string json = null;

        if (Application.platform == RuntimePlatform.Android)
        {
            File.WriteAllText(URL_ANDROID + "/SnakeStorage.json", json);
        }
        else
        {

            File.WriteAllText(Application.streamingAssetsPath + "/SnakeStorage.json", json);
        }
    }

    public void SaveColors(List<Color> colors)
    {
        List<SnakeStorage> snakeStorage = new List<SnakeStorage>();

        foreach (Color color in colors)
        {
            snakeStorage.Add(new SnakeStorage(color));
        }
        string json = JsonHelper.ToJson<SnakeStorage>(snakeStorage.ToArray());

        if (Application.platform == RuntimePlatform.Android)
        {
            File.WriteAllText(URL_ANDROID + "/SnakeStorage.json", json);
        }
        else
        {
            File.WriteAllText(Application.streamingAssetsPath + "/SnakeStorage.json", json);

        }
    }

    public List<Color> LoadColors()
    {
        List<SnakeStorage> snakeStorages = new List<SnakeStorage>();

        if (!File.Exists(URL_ANDROID + "/SnakeStorage.json") && Application.platform == RuntimePlatform.Android)
        {
            return null;
        }

        string json = null;


        if (Application.platform == RuntimePlatform.Android)
        {
            json = File.ReadAllText(URL_ANDROID + "/SnakeStorage.json");
        }
        else
        {
            json = File.ReadAllText(Application.streamingAssetsPath + "/SnakeStorage.json");

        }

        if (String.IsNullOrEmpty(json))
        {
            return null;
        }

        snakeStorages = JsonHelper.FromJson<SnakeStorage>(json).ToList();

        List<Color> colorStorage = new List<Color>();

        foreach(SnakeStorage snakeStorage in snakeStorages)
        {
            colorStorage.Add(snakeStorage.color);
        }

        return colorStorage;




    }
}

[Serializable]
public class SnakeStorage
{
    public Color color;

    public SnakeStorage(Color color)
    {
        this.color = color;
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }

}