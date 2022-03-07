using UnityEngine;

namespace Utils
{
    public sealed class EnemyPath : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(transform.GetChild(i).position, 0.25f);
            }
        }
    }
}