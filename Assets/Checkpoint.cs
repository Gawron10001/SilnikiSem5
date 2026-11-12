using UnityEngine;

// Touch it once to make it the new respawn point. The flag changes colour when active.
public class Checkpoint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer flag;
    [SerializeField] private Color activeColor = new Color(0.3f, 0.85f, 0.4f);

    private bool _activated;

    private void Awake()
    {
        if (flag == null) flag = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_activated) return;
        if (other.TryGetComponent(out PlayerRespawn player))
        {
            player.SetCheckpoint(transform.position);
            _activated = true;
            if (flag != null) flag.color = activeColor;
        }
    }
}
