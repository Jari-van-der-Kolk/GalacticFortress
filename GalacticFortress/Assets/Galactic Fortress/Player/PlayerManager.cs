using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject _shootPos;
    [Space]

    [SerializeField] private float _shootingSpeed;
    private float _timer;
    
    void Start()
    {

    }

    
    void Update()
    {

        
    }

    private void ShootBullet()
    {
        _timer += _shootingSpeed * Time.deltaTime;

        if (_timer >= 1f)
        {
            Bullet bullet = ObjectPoolManager.Singleton.playerBulletPool.Get();
            bullet.Shoot(_shootPos.transform.position);
        }

    }

}
