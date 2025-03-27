using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class PhotonObjectPool : MonoBehaviour, IPunPrefabPool
{
    private Dictionary<string, Queue<GameObject>> poolDictionary = new();

    [SerializeField] private int maxCount = 10;

    public static PhotonObjectPool instance;
    public static PhotonObjectPool Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance.gameObject);
    }

    /// <summary>
    /// Tạo hoặc lấy object từ pool
    /// </summary>
    public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        if (poolDictionary.ContainsKey(prefabId) && poolDictionary[prefabId].Count > 0)
        {
            GameObject obj = poolDictionary[prefabId].Dequeue();
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);
            return obj;
        }

        GameObject newObj = Instantiate(Resources.Load<GameObject>(prefabId), position, rotation);
        newObj.name = prefabId;
        return newObj;
    }

    /// <summary>
    /// Đưa object vào pool
    /// </summary>
    public void Destroy(GameObject gameObject)
    {
        string prefabID = gameObject.name;

        if (!poolDictionary.ContainsKey(prefabID))
        {
            poolDictionary[prefabID] = new Queue<GameObject>();
        }

        if (poolDictionary[prefabID].Count >= maxCount)
        {
            GameObject oldestObj = poolDictionary[prefabID].Dequeue();
            Destroy(oldestObj);
        }

        gameObject.SetActive(false);
        poolDictionary[prefabID].Enqueue(gameObject);
    }
}
