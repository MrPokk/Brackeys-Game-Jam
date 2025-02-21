using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

public static class FileWriter
{
    private const char separator = ';';
    private static string path = "/DebugInfo/";
    private const string fileType = ".dat";
    private static string sesionID = string.Empty;
    public static async void Write(BasePeople people, Potion potion)
    {
        if (sesionID == string.Empty) {
            path = Directory.GetCurrentDirectory() + path;
            Directory.CreateDirectory(path);
            sesionID = Random.Range(10000, 99999).ToString();
            path += sesionID + fileType;
        }
        FileStream fstream = new FileStream(path, FileMode.OpenOrCreate);
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
        Debug.Log("FileWriter");
    }
}