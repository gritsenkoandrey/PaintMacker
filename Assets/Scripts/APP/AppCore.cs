using BaseMonoBehaviour;
using UnityEngine;

namespace APP
{
    public sealed class AppCore : BaseComponent
    {
        protected override void Awake()
        {
            base.Awake();
            
            DontDestroyOnLoad(this);
        }

        protected override void Start()
        {
            base.Start();
            
            Input.multiTouchEnabled = false;
        }
    }
}