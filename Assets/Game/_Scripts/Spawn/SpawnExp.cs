using UnityEngine;

public class SpawnExp : SpawnObject
{
    protected override void GetObject(GameObject obj)
    {
        obj.GetComponent<PointExp>().SetSpawner(this);
    }
}
