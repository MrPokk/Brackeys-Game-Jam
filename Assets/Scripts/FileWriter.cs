using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

public static class FileWriter
{
    private const char separator = ';';
    private static string path = "/DebugInfo/";
    private const string fileType = ".dat";

    private static string pathOrder = "/DebugInfo/";
    private static string sesionID = string.Empty;
    public static async void Write(BasePeople people, Potion potion)
    {
        if (sesionID == string.Empty) {
            pathOrder = Directory.GetCurrentDirectory() + path;
            Directory.CreateDirectory(pathOrder);
            sesionID = Random.Range(10000, 99999).ToString();
            pathOrder += sesionID + fileType;
        }
        FileStream fstream = new FileStream(pathOrder, FileMode.OpenOrCreate);
        fstream.Seek(0, SeekOrigin.End);
        StringBuilder sb = new StringBuilder(people.DataComponent.TypePoison.ID.ToString(), 64); 
        sb.Append(separator);
        sb.Append(people.DataComponent.Name.text); sb.Append(separator);
        if (potion != null) {
            sb.Append(potion.ID); sb.Append(separator);
            sb.Append(potion.IDIngredients.Count); sb.Append(separator);
            sb.AppendJoin(separator, potion.IDIngredients);
        }
        sb.Append('\n');

        byte[] input = Encoding.Default.GetBytes(sb.ToString());
        await fstream.WriteAsync(input, 0, input.Length);
        fstream.Close();
        Debug.Log("FileWriter people");
    }
    public static async void Write(Dictionary<SamplePotion, int> pull)
    {
        string _path = Directory.GetCurrentDirectory() + path;
        Directory.CreateDirectory(_path);
        _path += "PullPotions" + fileType;

        FileStream fstream = new FileStream(_path, FileMode.OpenOrCreate);
        StringBuilder sb = new StringBuilder();
        foreach (var potion in pull) {
            sb.Append($"{potion.Key.ID} {(int)potion.Key.Difity} {6 - (int)potion.Key.Difity} {potion.Value}\n");
        }
        byte[] input = Encoding.Default.GetBytes(sb.ToString());
        await fstream.WriteAsync(input, 0, input.Length);
        fstream.Close();
        Debug.Log("FileWriter pull potions");
    }
    public static async void WriteWin()
    {
        FileStream fstream = new FileStream(pathOrder, FileMode.OpenOrCreate);
        fstream.Seek(0, SeekOrigin.End);
        byte[] input = Encoding.Default.GetBytes("win\n");
        await fstream.WriteAsync(input, 0, input.Length);
        fstream.Close();
        Debug.Log("FileWriter Win");
    }
    public static async void WriteLoss()
    {
        FileStream fstream = new FileStream(pathOrder, FileMode.OpenOrCreate);
        fstream.Seek(0, SeekOrigin.End);

        byte[] input = Encoding.Default.GetBytes($"loss\n");
        await fstream.WriteAsync(input, 0, input.Length);
        fstream.Close();
        Debug.Log("FileWriter Win");
    }
}