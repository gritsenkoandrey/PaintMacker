using UnityEngine;

namespace Utils
{
    public static class U
    {
        public const string Level = "Level";
        
        public static float Remap(float iMin, float iMax, float oMin, float oMax, float value)
        {
            return Mathf.Lerp(oMin, oMax, Mathf.InverseLerp(iMin, iMax, value));
        }

        public static float Random(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }
        
        public static int Random(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }
        
        public static Transform[] ChildPoints(this Transform parent)
        {
            int count = parent.childCount;

            Transform[] points = new Transform[count]; 

            for (int i = 0; i < count; i++)
            {
                points[i] = parent.GetChild(i);
            }

            return points;
        }
    }
}