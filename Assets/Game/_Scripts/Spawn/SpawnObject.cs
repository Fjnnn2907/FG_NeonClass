using Photon.Pun;
using System.Collections;
using UnityEngine;

public class SpawnObject : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject objPrefab;
    [SerializeField] private float spawnTimer = 3f;
    [SerializeField] private Vector2 spawnAreaMin = new Vector2(-11, 11);
    [SerializeField] private Vector2 spawnAreaMax = new Vector2(14, 8);

    [SerializeField] private int maxObject = 15;
    [SerializeField] private int currentObject = 0;

    protected void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnExpRoutine());
        }
    }

    protected IEnumerator SpawnExpRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTimer);

            if (currentObject < maxObject)
            {
                Spawn();
            }
        }
    }

    private void Spawn()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y),
            0f
        );

        photonView.RPC("RPC_SpawnObeject", RpcTarget.AllBuffered, randomPosition);
    }

    [PunRPC]
    protected void RPC_SpawnObeject(Vector3 position)
    {
        GameObject obj = PhotonNetwork.Instantiate(objPrefab.name, position, Quaternion.identity);
        GetObject(obj);
        currentObject++;
    }

    protected virtual void GetObject(GameObject obj)
    {
       //TODO: Setup Object
    }

    public void OnObjectRemoveList()
    {
        currentObject--;
    }
}
