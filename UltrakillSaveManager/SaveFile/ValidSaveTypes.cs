using System;
using System.Collections.Generic;

namespace UltrakillSaveManager.SaveFile;

public static class ValidSaveTypes
{
    public static readonly Dictionary<string, Type> validTypes = new()
    {
        {"RankData", typeof(RankData)},
        {"CyberRankData", typeof(CyberRankData)},
        {"RankScoreData", typeof(RankScoreData)},
        {"GameProgressData", typeof(GameProgressData)},
        {"GameProgressMoneyAndGear", typeof(GameProgressMoneyAndGear)}
    };
}