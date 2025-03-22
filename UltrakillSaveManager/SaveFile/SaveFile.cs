using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace UltrakillSaveManager.SaveFile;
public partial class SaveFile
{
    private object _saveData;
    private readonly string path;
    public object SaveData
    {
        get => _saveData;
        set
        {
            _saveData = value;
            Write();
        }
    }

    public SaveFile(string path, object? saveData = null)
    {
        // TODO: Add type validation
        this.path = path;
        _saveData = saveData ?? Read(path) ?? throw new ArgumentException("SaveData File Not Found");
    }
    public void Write() => Write(path, _saveData);

    public static void Write(string path, object data)
    {
        Console.WriteLine("[FS] Writing To " + path);
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        using FileStream fileStream = new(path, FileMode.OpenOrCreate);
        BinaryFormatter binaryFormatter = new();
        try
        {
            binaryFormatter.Serialize(fileStream, data);
            fileStream.Close();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
    public static object? Read(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        using FileStream fileStream = new(path, FileMode.Open);
        if (fileStream.Length == 0L)
        {
            throw new Exception("Stream Length 0");
        }

        return new BinaryFormatter
        {
            Binder = new RestrictedSerializationBinder
            {
                AllowedTypes =
            {
                typeof(RankData),
                typeof(CyberRankData),
                typeof(RankScoreData),
                typeof(GameProgressData),
                typeof(GameProgressMoneyAndGear)
            }
            }
        }.Deserialize(fileStream);
    }
}
