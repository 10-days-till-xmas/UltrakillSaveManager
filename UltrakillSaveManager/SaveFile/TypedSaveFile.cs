using System;
using System.IO;
using System.Reflection;

namespace UltrakillSaveManager.SaveFile;
public class TypedSaveFile(string path, Type type, object? saveData = null) : SaveFile(path, saveData)
{
    public Type Type = type;

    // TODO: Make this all into a dynamic class to wrap the saveData object
    // Also, it could be possible to make custom saves this way?
    public bool GetField<U>(string fieldName, out U? value)
    {
        var field = Type.GetField(fieldName);
        if (field == null)
        {
            value = default;
            return false;
        }
        value = (U)field.GetValue(SaveData);
        return true;
    }
    public bool SetField<U>(string fieldName, U value)
    {
        var field = Type.GetField(fieldName);
        if (field == null)
        {
            return false;
        }
        field.SetValue(SaveData, value);
        return true;

    }

    public bool GetField(string fieldName, out FieldInfo fieldInfo)
    {
        fieldInfo = Type.GetField(fieldName);
        return fieldInfo != null;
    }

    public FieldInfo[] GetFields() => Type.GetFields();
    public string[] GetFieldNames() => Array.ConvertAll(GetFields(), field => field.Name);
    public U GetFieldValue<U>(string fieldName) => (U)GetFieldValue(fieldName);
    public object GetFieldValue(string fieldName) => Type.GetField(fieldName).GetValue(SaveData);

    public static TypedSaveFile GetSaveFile(string path)
    {
        var saveFile = new SaveFile(path);

        return saveFile.SaveData switch
        {
            RankData save => new RankDataSaveFile(path, save) {},
            CyberRankData save => new CyberRankDataSaveFile(path, save),
            RankScoreData save => new RankScoreDataSaveFile(path, save),
            GameProgressData save => new GameProgressDataSaveFile(path, save),
            GameProgressMoneyAndGear save => new GameProgressMoneyAndGearSaveFile(path, save),
            _ => throw new ArgumentException("Invalid Type"),
        };
    }
}
public sealed class RankDataSaveFile(string path, object? saveData = null) : TypedSaveFile(path, typeof(RankData), saveData);
public sealed class CyberRankDataSaveFile(string path, object? saveData = null) : TypedSaveFile(path, typeof(CyberRankData), saveData);
public sealed class RankScoreDataSaveFile(string path, object? saveData = null) : TypedSaveFile(path, typeof(RankScoreData), saveData);
public sealed class GameProgressDataSaveFile(string path, object? saveData = null) : TypedSaveFile(path, typeof(GameProgressData), saveData);
public sealed class GameProgressMoneyAndGearSaveFile(string path, object? saveData = null) : TypedSaveFile(path, typeof(GameProgressMoneyAndGear), saveData);



