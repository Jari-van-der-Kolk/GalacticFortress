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
        ShootBullet();
    }

    private void ShootBullet()
    {
        _timer += _shootingSpeed * Time.deltaTime;

        if (Input.GetMouseButton(0) && _timer >= 1f)
        {
            Bullet bullet = ObjectPoolManager.Singleton.playerBulletPool.Get();
            bullet.Shoot(_shootPos.transform.position, transform.right);
        }

    }

}
