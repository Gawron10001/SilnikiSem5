using TMPro;
using UnityEngine;

public class UI_StatisticPanel : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public TextMeshProUGUI healthText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        playerHealth.OnAddHealth += PlayerHealth_OnAddHealth;
        playerHealth.OnHealthInnit += PlayerHealth_OnHealthInnit;
    }

    private void PlayerHealth_OnHealthInnit(int health)
    {
        healthText.text = "Health: " + health.ToString();
    }

    private void PlayerHealth_OnAddHealth(int health)
    {
        healthText.text = "Health: " + health.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
