using System;
using System.Linq;

namespace UltrakillSaveManager;

public class Slot(int slot)
{
    public readonly string prefix = $"/Slot{slot}";
    private const string ext = ".bepis";
    public string CyberGrindHiScore => $"{prefix}/cybergrindhighscore{ext}";

    public Func<int, string> DifficultyProgress =>
        (int difficultyIndex) => $"{prefix}/difficulty{difficultyIndex}progress{ext}";

    public string GeneralProgress => $"{prefix}/generalprogress{ext}";

    public static readonly int[] ValidLevelNums = [-1, .. Enumerable.Range(1, 29), 100, 101, 500, 501, 666, 667];

    public string LvlProgress(int num)
    {
        if (!ValidLevelNums.Contains(num))
        {
            throw new ArgumentOutOfRangeException("slot", "Level must be between 0 and 5");
        }
        return $"{prefix}/lvl{num}progress{ext}";
    }
}