using System.Collections.Generic;

namespace AssetPath
{
    public struct DataPath
    {
        public static readonly Dictionary<DataType, string> Paths = new Dictionary<DataType, string>
        {
            { DataType.Level, "Data/LevelData"},
            { DataType.Canvas, "Prefabs/GUI/Canvas"},
            { DataType.Character, "Data/CharacterData"},
            { DataType.Environment, "Data/EnvironmentData"},
            { DataType.Settings, "Data/SettingsData"},
        };
    }
}