using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedObjectScroller : ObjectScroller
{
    [Space]
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private float _despawnTime;
    void Start()
    {
        StartCoroutine(TimedSpawn(_spawnDelay)); 
    }

    private IEnumerator TimedSpawn(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject scrollingObject = Instantiate(_prefab, GetRandomRadius(), Quaternion.LookRotation(dir));
        
        AddScrollingObject(scrollingObject);
        StartCoroutine(TimedSpawn(time));
        
        yield return new WaitForSeconds(_despawnTime);

        RemoveScrollingObject(scrollingObject);
        //Destroy(_prefab);
    }
}
