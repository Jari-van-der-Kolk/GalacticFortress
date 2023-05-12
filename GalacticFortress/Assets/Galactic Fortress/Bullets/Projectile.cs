using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour 
{
    [SerializeField] private float _speed;
    [HideInInspector]public Vector3 dir;
    public ParticleSystem particleSystemRoot;
    public GameObject explosionEffect;
	
	void Update () 
    {
       transform.position += dir * _speed * Time.deltaTime;
    }
   

}
