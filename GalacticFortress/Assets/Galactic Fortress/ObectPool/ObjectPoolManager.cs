using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Material playerBulletMaterial;
    [SerializeField] private Material enemyBulletMaterial;

    public ObjectPool<Bullet> playerBulletPool;
    public ObjectPool<Bullet> enemyBulletPool;

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
        Singleton = this;

        playerBulletPool = new ObjectPool<Bullet>(InstantiatePlayerBulletPoolObject, TakeObjectFromPool, ReturnObjectToPool);
        enemyBulletPool = new ObjectPool<Bullet>(InstantiateEnemyBulletPoolObject, TakeObjectFromPool, ReturnObjectToPool);
    }
    
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerBulletPool.Get();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("foo");
        }
    }

    private Bullet InstantiatePlayerBulletPoolObject()
    {
        Bullet playerBullet = new Bullet();
        playerBullet.@object = Instantiate(bulletPrefab, transform);
        playerBullet.SetName("Player" + bulletPrefab.name);
        return playerBullet;
    }
    private Bullet InstantiateEnemyBulletPoolObject()
    {

        return null;
    }
    private void TakeObjectFromPool(PoolingObject poolingObject) => poolingObject.@object.SetActive(true);
    private void ReturnObjectToPool(PoolingObject poolingObject) => poolingObject.@object.SetActive(false);

    
    
   

}

public class Bullet : PoolingObject
{
    public void Shoot(Vector3 pos)
    {
        @object.transform.position = pos;  
    }

    public void SetMaterial(Material material)
    {
        @object.GetComponent<MeshRenderer>().material = material;
    }
}

public abstract class PoolingObject
{
    public GameObject @object;
    public IObjectPool<PoolingObject> pool;

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