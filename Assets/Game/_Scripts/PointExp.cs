using UnityEngine;

public class PointExp : MonoBehaviour
{
    private SpawnExp spawner;
    public void SetSpawner(SpawnExp spawnExp)
    {
        spawner = spawnExp;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerStats>();
        if (!player) return;
        player.AddExp(10);
        spawner.OnObjectRemoveList();
        PhotonObjectPool.Instance.Destroy(this.gameObject);
    }
}
