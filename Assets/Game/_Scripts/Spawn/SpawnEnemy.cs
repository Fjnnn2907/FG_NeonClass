using UnityEngine;

public class SpawnEnemy : SpawnObject
{
    protected override void GetObject(GameObject obj)
    {
        obj.GetComponent<EnemyCtrl>().SetSpawner(this);
    }
}
