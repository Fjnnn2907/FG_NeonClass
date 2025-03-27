using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var hitBox = collision.GetComponent<Stats>();

        if (hitBox != null)
        {
            hitBox.TakeDamage(10);
        }
    }
}
