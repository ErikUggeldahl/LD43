using UnityEngine;

public class NPCSpawnLocation : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 4f);
    }
}
