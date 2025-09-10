using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Create_BinaryFile : Editor
{
    private const string SavePath = "Assets/Resources/BinaryFile/{0}.binary";

    [MenuItem("Assets/Table/Create_BinaryTable")]
    public static void Create()
    {
        if (Selection.objects.Length <= 0)
        {
            return;
        }

        foreach (var obj in Selection.objects)
        {
            var textAsset = obj as TextAsset;
            if (textAsset is null)
                return;

            var path = string.Format(SavePath, textAsset.name);
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(textAsset.text);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        Debug.Log("官捞呈府 颇老 积己 场~!");
    }
}
