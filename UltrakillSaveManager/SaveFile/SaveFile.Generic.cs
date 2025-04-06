using System;
using System.Linq;

namespace UltrakillSaveManager.SaveFile;

public class SaveFile<T> where T : class
{
    private T save;
    private readonly string path;

    public T Save
    {
        get => save;
        set
        {
            save = value;
            Write();
        }
    }

    private static readonly Type[] validTypes = ValidSaveTypes.validTypes.Values.ToArray();

    public SaveFile(string path)
    {
        if (!validTypes.Contains(typeof(T)))
        {
            throw new ArgumentException("Invalid Type");
        }

        this.path = path;
        save = SaveFile.Read(path) as T ?? throw new ArgumentException("SaveData File Not Found");
    }

    public void Write() => SaveFile.Write(path, save);
}