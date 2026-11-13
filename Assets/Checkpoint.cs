using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerRespawn respawn))
        {
            respawn.SetCheckpoint(transform);
        }
    }
}
