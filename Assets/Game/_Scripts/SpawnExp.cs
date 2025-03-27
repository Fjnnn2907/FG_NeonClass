using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class SpawnExp : MonoBehaviour
{
    [SerializeField] private GameObject pointExpPrefab;
    [SerializeField] private int maxExpOnMap = 10;
    [SerializeField] private float spawnTimer = 3f; 
    [SerializeField] private Vector2 spawnAreaMin = new Vector2(-14,12);
    [SerializeField] private Vector2 spawnAreaMax = new Vector2(-24, -12);

    [SerializeField] private int maxObject = 15;
    [SerializeField] private int currentObject = 0;

    private void Start()
    {
        StartCoroutine(SpawnExpRoutine());
    }

    private IEnumerator SpawnExpRoutine()
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

        GameObject exp = PhotonObjectPool.Instance.Instantiate(pointExpPrefab.name, randomPosition, Quaternion.identity);
        exp.GetComponent<PointExp>().SetSpawner(this);
        currentObject++;
    }

    public void OnObjectRemoveList()
    {
        currentObject--;
    }
}
