using UnityEngine;

namespace APP
{
    public sealed class APPCore : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            Input.multiTouchEnabled = false;
        }
    }
}