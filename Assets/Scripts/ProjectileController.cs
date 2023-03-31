using UnityEngine;
using UnityEngine.Events;

public class ProjectileController : MonoBehaviour
{
    public float explosionForce;
    public float explosionRadius;
    public float fatalExplosionRadius;
    public UnityEvent<ExplosionData> exploded;
    public AudioClip ShootImpactAudio;

    private string[] _destroyOnCollideTags;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _destroyOnCollideTags = new[] { "Enemy", "Ground" };
        _audioSource = GetComponent<AudioSource>();
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
                exploded.Invoke(
                    new ExplosionData(transform.position, explosionRadius, fatalExplosionRadius, explosionForce));
                _audioSource.PlayOneShot(ShootImpactAudio);
                //Remove visible part and let object play audio till the end
                Destroy(gameObject.GetComponent<Rigidbody>());
                Destroy(gameObject.GetComponentInChildren<MeshRenderer>());
                Destroy(gameObject, ShootImpactAudio.length);
                break;
            }
        }
    }
}
