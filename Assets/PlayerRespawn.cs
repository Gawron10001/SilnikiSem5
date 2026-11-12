using UnityEngine;

// Sends the player back to the last checkpoint (or the start) when they touch a Hazard.
public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Transform startPoint;

    private Vector3 _respawnPoint;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _respawnPoint = startPoint != null ? startPoint.position : transform.position;
        transform.position = _respawnPoint;
    }

    public void SetCheckpoint(Vector3 position) => _respawnPoint = position;

    public void Die()
    {
        _rigidbody2D.linearVelocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0f;
        transform.SetPositionAndRotation(_respawnPoint, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Hazard _)) Die();
    }
}
