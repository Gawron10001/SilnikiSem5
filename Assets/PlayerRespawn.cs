using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Transform respawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<PlayerHealth>().Death += PlayerRespawn_Death;
    }

    private void PlayerRespawn_Death()
    {
        transform.SetPositionAndRotation(respawnPoint.position, Quaternion.identity);
    }

    public void SetCheckpoint(Transform newCheckpoint)
    {
        respawnPoint = newCheckpoint;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
