using UnityEditor;
using UnityEngine;
using Utils;

namespace Editor
{
    public struct EditorUtils
    {
        [MenuItem("Tools/Clear Prefs")]
        public static void ClearPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            
            CustomDebug.Log("Clear Prefs");
        }
    }
}