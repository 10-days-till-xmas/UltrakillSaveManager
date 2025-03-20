using System;
using System.Reflection;

namespace UltrakillSaveManager.SaveFile;
public class TypedSaveFile(string path, Type type) : SaveFile(path)
{
    public Type Type = type;
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
    public U GetFieldValue<U>(string fieldName) => (U)GetFieldValue(fieldName);
    public object GetFieldValue(string fieldName) => Type.GetField(fieldName).GetValue(SaveData);

    public static TypedSaveFile GetSaveFile(string path)
    {
        var saveFile = new SaveFile(path);

        return saveFile.SaveData switch
        {
            RankData => new RankDataSaveFile(path),
            CyberRankData => new CyberRankDataSaveFile(path),
            RankScoreData => new RankScoreDataSaveFile(path),
            GameProgressData => new GameProgressDataSaveFile(path),
            GameProgressMoneyAndGear => new GameProgressMoneyAndGearSaveFile(path),
            _ => throw new ArgumentException("Invalid Type"),
        };
    }
}
public sealed class RankDataSaveFile(string path) : TypedSaveFile(path, typeof(RankData));
public sealed class CyberRankDataSaveFile(string path) : TypedSaveFile(path, typeof(CyberRankData));
public sealed class RankScoreDataSaveFile(string path) : TypedSaveFile(path, typeof(RankScoreData));
public sealed class GameProgressDataSaveFile(string path) : TypedSaveFile(path, typeof(GameProgressData));
public sealed class GameProgressMoneyAndGearSaveFile(string path) : TypedSaveFile(path, typeof(GameProgressMoneyAndGear));



