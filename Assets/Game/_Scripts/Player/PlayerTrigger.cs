using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public PlayerStats playerStats;

    private void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var hitBox = collision.GetComponent<Stats>();

        if (hitBox != null)
        {
            hitBox.TakeDamage(playerStats.Damage);
            if (hitBox.CurrentHealth <= 0)
                playerStats.AddExp(40);
        }
    }
}
