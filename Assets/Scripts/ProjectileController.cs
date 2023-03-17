using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float explosionForce;
    public float explosionRadius;
    public float fatalExplosionRadius;
    
    private string[] _destroyOnCollideTags;
    private Rigidbody _rb;
    
    // Start is called before the first frame update
    void Start()
    {
        _destroyOnCollideTags = new[] { "Enemy", "Ground" };
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, fatalExplosionRadius);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        foreach (var t in _destroyOnCollideTags)
        {
            if (collision.gameObject.CompareTag(t))
            {
                var destroyedEnemies = Physics.OverlapSphere(transform.position, fatalExplosionRadius, LayerMask.GetMask("Enemy"));
                var affectedEnemies = Physics
                    .OverlapSphere(transform.position, explosionRadius, LayerMask.GetMask("Enemy"))
                    .Except(destroyedEnemies);
                
                foreach (var e in affectedEnemies)
                {
                    e.GetComponent<Rigidbody>()
                        .AddExplosionForce(explosionForce, 
                            transform.position, 
                            explosionRadius, 
                            10);
                }
                Array.ForEach(destroyedEnemies, e => Destroy(e.gameObject));
                Destroy(gameObject);
                break;
            } 
        }
    }
}
