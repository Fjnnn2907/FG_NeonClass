using Photon.Pun;
using UnityEngine;

public class PointExp : MonoBehaviourPun
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

        // Hủy đối tượng trên tất cả Client
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
