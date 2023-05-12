using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;

    [SerializeField] private GameObject[] _randomScrollingObjects; 

    public ObjectPool<Bullet> playerBulletPool;
    public ObjectPool<Bullet> enemyBulletPool;
    public ObjectPool<PoolingObject> scrollingObjectsPool;

    private static ObjectPoolManager singleton;
    public static ObjectPoolManager Singleton
    {
        get
        {
            return singleton;
        }

        private set
        {
            if (singleton)
            {
                Destroy(value);
                Debug.LogError("We have more than one ObjectPoolManager!!!");
                return;
            }

        }
    }
    private void Awake()
    {
        singleton = this;

        playerBulletPool = new ObjectPool<Bullet>(InstantiatePlayerBulletPoolObject, TakeObjectFromPool, ReturnObjectToPool);
        enemyBulletPool = new ObjectPool<Bullet>(InstantiateEnemyBulletPoolObject, TakeObjectFromPool, ReturnObjectToPool);
        scrollingObjectsPool = new ObjectPool<PoolingObject>(InstantiateRandomScrollingPoolObject, TakeObjectFromPool, ReturnObjectToPool);
    }
    
    void Start()
    {
        
    }

    private void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("foo");
        }
    }

    private Bullet InstantiatePlayerBulletPoolObject()
    {
        Bullet playerBullet = new Bullet();
        playerBullet.@object = Instantiate(_bulletPrefab, transform);
        playerBullet.SetName("Player" + _bulletPrefab.name);
        return playerBullet;
    }
    private Bullet InstantiateEnemyBulletPoolObject()
    {
        return null;
    }

    private PoolingObject InstantiateRandomScrollingPoolObject()
    {
        PoolingObject scrollingObject = new PoolingObject();
        scrollingObject.@object = Instantiate(_randomScrollingObjects[Random.Range(0, _randomScrollingObjects.Length)], transform);
        return scrollingObject;
    }

    private void TakeObjectFromPool(PoolingObject poolingObject) => poolingObject.@object.SetActive(true);
    private void ReturnObjectToPool(PoolingObject poolingObject) => poolingObject.@object.SetActive(false);

    
}

public class Bullet : PoolingObject
{
    public void Shoot(Vector3 pos, Vector3 dir)
    {
        @object.transform.position = pos;
        @object.GetComponent<Projectile>().dir = dir;
        @object.transform.rotation = Quaternion.LookRotation(dir);
    }

    public void SetMaterial(Material material)
    {
        @object.GetComponent<MeshRenderer>().material = material;
    }
}


public class PoolingObject
{
    public GameObject @object;
    internal IObjectPool<PoolingObject> pool;

    public void SetPool(IObjectPool<PoolingObject> pool)
    {
        this.pool = pool;
    }

    public void SetName(string name)
    {
        @object.name = name;
    }

    public void ReturnToPool()
    {
        pool.Release(this);
    }

    public IEnumerator ReturnToPoolTimer(float time)
    {
        yield return new WaitForSeconds(time);
        pool.Release(this);

    }
}