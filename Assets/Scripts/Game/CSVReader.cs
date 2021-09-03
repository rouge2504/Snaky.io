﻿using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public class CSVReader
{
    private string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    private string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    private char[] TRIM_CHARS = { '\"' };
    private TextAsset data;
    private TextAsset data_result;
    private string[] lines;

    private static CSVReader instance;
    public static CSVReader Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new CSVReader("Skins");
            }
            return instance;
        }
    }



    public CSVReader(string fileName)
    {
        data = Resources.Load(fileName) as TextAsset;
        lines = Regex.Split(data.text, LINE_SPLIT_RE);



    }

    public List<SkinMask> Read()
    {
        var list = new List<SkinMask>();
        if (lines.Length > 1)
        {

            var header = Regex.Split(lines[0], SPLIT_RE);
            for (var i = 1; i < lines.Length; i++)
            {
                var values = Regex.Split(lines[i], SPLIT_RE);
                if (values.Length == 0 || values[0] == "") continue;

                SkinMask entry = new SkinMask();
                for (var j = 0; j < header.Length && j < values.Length; j++)
                {
                    string value = values[j];
                    value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");

                    switch (j)
                    {
                        case 0:
                            entry.nameMask = value;
                            break;
                        case 1:
                            entry.url = value;
                            entry.SetMask();
                            //entry.SetMaterial();
                            break;
                        case 2:
                            
                            entry.price = int.Parse(string.IsNullOrEmpty(value) ? "0" : value);
                            break;
                        case 3:
                            entry.cube = value;
                            break;
                        case 4:
                            entry.eggValue = int.Parse(string.IsNullOrEmpty(value) ? "0" : value);
                            break;
                        case 5:
                            entry.type = int.Parse(string.IsNullOrEmpty(value) ? "0" : value);
                            break;


                    }
                }
                list.Add(entry);
            }
        }
        return list;
    }

}

[Serializable]
public class SkinMask
{
    public string nameMask;
    public string url;
    public int price;
    public string cube;
    public int eggValue;
    public int type;
    public Sprite maskSprite;
    public Material maskMaterial;

    public void SetMask()
    {
        string url = "Masks/" + this.url + "/" + nameMask;
        maskSprite = Resources.Load<Sprite>(url);
    }

    public void SetMaterial()
    {
        string url = "Masks/" + this.url + "/" + nameMask;
        Material material = new Material(Shader.Find("Unlit/Transparent Cutout"));
        Texture2D texture = Resources.Load<Texture2D>(url);
        material.mainTexture = texture;
        string savePath = UnityEditor.AssetDatabase.GetAssetPath(texture);
        savePath = savePath.Substring(0, savePath.LastIndexOf('/') + 1);
        maskMaterial = material;
        string newAssetName = savePath + texture.name + ".mat";

        UnityEditor.AssetDatabase.CreateAsset(material, newAssetName);

        UnityEditor.AssetDatabase.SaveAssets();
    }
}