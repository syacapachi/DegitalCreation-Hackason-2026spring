using System.IO;
using UnityEngine;

public static class PhotoSaver
{
    public static void SavePNG(Texture2D tex, string fileName)
    {
        byte[] bytes = tex.EncodeToPNG();
        SavePNG(bytes, fileName);
    }
    public static void SavePNG(byte[] bytes, string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".png";
        File.WriteAllBytes(path, bytes);
        Debug.Log("保存先: " + path);
    }

    public static void SaveJPG(Texture2D tex, string fileName)
    {
        byte[] bytes = tex.EncodeToJPG();
        SaveJPG(bytes, fileName);
    }
    public static void SaveJPG(byte[] bytes, string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".jpg";
        File.WriteAllBytes(path, bytes);
        Debug.Log("保存先: " + path);
    }
}