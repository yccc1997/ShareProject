using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using OpenCvSharp;
using OpenCvSharp.Aruco;
using Rect = UnityEngine.Rect;

public class AtlasParser : MonoBehaviour
{
    public static string Rootpath = Application.dataPath + "/Res/bottomScene/";
    public static string path = Rootpath + "scene.atlas";


    [MenuItem("Tools/转换数据")]
    public static void ChangeData()
    {
        List<AtlasData> resultList = ParseAtlas(path);
        for (int i = 0; i < resultList.Count; i++)
        {
            CuttingImages(resultList[i]);
        }
       
    }

    public static List<AtlasData> ParseAtlas(string filePath)
    {
        List<AtlasData> dataList = new List<AtlasData>();
        using (StreamReader reader = new StreamReader(filePath))
        {
            string data = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                data = reader.ReadLine().Trim();
                if (data == null || data == "")
                {
                   break;
                }
                AtlasData pngContent = new AtlasData();
                List<ImageData> tempContent = new List<ImageData>();
                pngContent.name = data;
                string temp = reader.ReadLine().Trim();
                int[] size = Array.ConvertAll(temp.Split(":")[^1].Trim().Split(","), int.Parse);
                Vector2 sizeV2 = new Vector2();
                if (size.Length == 2)
                {
                    sizeV2.x = size[0];
                    sizeV2.y = size[1];
                }
                pngContent.size = sizeV2;
                pngContent.format = reader.ReadLine().Trim().Split(":")[^1].Trim();
                pngContent.filter = reader.ReadLine().Trim().Split(":")[^1].Trim();
                pngContent.repeat = reader.ReadLine().Trim().Split(":")[^1].Trim();
                pngContent.content = tempContent;
                while (true)
                {
                    data = reader.ReadLine();
                    if (data == null) break;
                    if (data == "") break;
                    if (data == "\n") break;
                    ImageData imgData = new ImageData();
                    imgData.name = data;
                    imgData.rotate = reader.ReadLine().Trim().Split(":")[^1].Trim();
                    imgData.xy = Array.ConvertAll(reader.ReadLine().Trim().Split(":")[^1].Trim().Split(","), int.Parse);
                    imgData.size = Array.ConvertAll(reader.ReadLine().Trim().Split(":")[^1].Trim().Split(","), int.Parse);
                    imgData.orig = reader.ReadLine().Trim().Split(":")[^1].Trim();
                    imgData.offset = reader.ReadLine().Trim().Split(":")[^1].Trim();
                    imgData.index = reader.ReadLine().Trim().Split(":")[^1].Trim();
                    tempContent.Add(imgData);
                }

                dataList.Add(pngContent);
            }
        }

        return dataList;
    }
    
    


    private static void CuttingImages(AtlasData result)
    {
        string name = result.name;
        string path = Rootpath + result.name;
        string DirectoryName = name.Remove(name.LastIndexOf(".", StringComparison.Ordinal));
        print(DirectoryName);
        byte[] imageBytes = System.IO.File.ReadAllBytes(path);
        Texture2D imgTexture = new Texture2D((int) result.size.x, (int) result.size.y);
        imgTexture.LoadImage(imageBytes);
        foreach (ImageData card in result.content)
        {
            ProcessCardImage(card, imgTexture,DirectoryName);
        }
    }

    static void ProcessCardImage(ImageData card, Texture2D imgTexture,string DirectoryName)
    {
        // 创建一个RenderTexture来保存图像
        RenderTexture renderTexture = new RenderTexture(imgTexture.width, imgTexture.height, 0);
        Graphics.Blit(imgTexture, renderTexture);

        // 将RenderTexture转换为Texture2D
        Texture2D processedTexture = new Texture2D(renderTexture.width, renderTexture.height);
        RenderTexture.active = renderTexture;
        processedTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        processedTexture.Apply();

        // 裁剪图像
        float posx = card.xy[0];
        float posY = imgTexture.height - (card.xy[1] + card.size[1]);
        Rect cardRect = new Rect(posx, posY, card.size[0], card.size[1]);
        Texture2D croppedTexture = new Texture2D((int) cardRect.width, (int) cardRect.height);
        Color[] pixels = processedTexture.GetPixels((int) cardRect.x, (int) cardRect.y, (int) cardRect.width,
            (int) cardRect.height);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();

        // 将裁剪后的图像保存为PNG文件
        byte[] pngBytes = croppedTexture.EncodeToPNG();

        string outPath = Rootpath + DirectoryName+"/";
        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }

        string path = outPath + card.name.Replace("/", "_") + ".png";
        File.WriteAllBytes(path, pngBytes);
    }
}

public class AtlasData
{
    public string name;
    public Vector2 size;
    public string format;
    public string filter;
    public string repeat;
    public List<ImageData> content;
}

public class ImageData
{
    public string name;
    public string rotate;
    public int[] xy;
    public int[] size;
    public string orig;
    public string offset;
    public string index;
}