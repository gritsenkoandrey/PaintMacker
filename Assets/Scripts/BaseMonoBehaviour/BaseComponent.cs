using UnityEngine;

namespace BaseMonoBehaviour
{
    public abstract class BaseComponent : MonoBehaviour
    {
        protected virtual void Awake() { }
        protected virtual void OnEnable() { }
        protected virtual void Start() { }
        protected virtual void OnDisable() { }
    }
}