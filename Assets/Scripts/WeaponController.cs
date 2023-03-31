using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("GameObjects")]
    public ParticleSystem projectileExplosion;
    public ParticleSystem pumpkinExplosion;
    public ParticleSystem barrelShootParticles;
    public GameObject barrelPivot;
    public GameObject ingameTxt;
    public GameObject projectilePrefab;
    public GameObject projectileSpawnPoint;
    public GameObject barrel;
    [Header("MortarValues")]
    public int maxShootForce;
    public float rotationSpeed;
    public float tiltSpeed;
    public float minTilt = 18;
    public float maxTilt = 55;
    [Header("Powerups")]
    public int powerPowerUp;
    public int powerUpDurationSec = 4;
    public int timePowerUpSec = 10;
    [Header("Audio")] 
    public AudioSource AudioSource;
    public AudioClip ShootAudio;
    public AudioClip PickupPowerup;
    [Header("Debug")]
    public PowerupTypeEnum currentPowerUp = PowerupTypeEnum.None;
    
    private float _horizontalA;
    private float _verticalA;
    private UiController _uicController;
    private Model _model;

    // Start is called before the first frame update
    void Start()
    {
        _model = Model.Instance();
        _uicController = FindObjectOfType<UiController>();
        _uicController.onShoot.AddListener(OnShoot);
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalA = Input.GetAxis("Horizontal");
        _verticalA = Input.GetAxis("Vertical");

        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime * _horizontalA);
        barrel.transform.Rotate(Vector3.left * tiltSpeed * Time.deltaTime * _verticalA);
        
        if (barrel.transform.eulerAngles.x > maxTilt)
        {
            var maxAngle = barrel.transform.eulerAngles;
            maxAngle.x = maxTilt;
            barrel.transform.eulerAngles = maxAngle;
        }
        else if (barrel.transform.eulerAngles.x < minTilt)
        {
            var minAngle = barrel.transform.eulerAngles;
            minAngle.x = minTilt;
            barrel.transform.eulerAngles = minAngle;
        }
    }

    private void OnShoot(float power)
    {
        Instantiate(barrelShootParticles, barrelPivot.transform.position, barrel.transform.rotation);
        AudioSource.PlayOneShot(ShootAudio);
        if (currentPowerUp == PowerupTypeEnum.MoreProjectiles)
        {
            StartCoroutine(SpawnMultiProjectile(power, 0.1f));
        }
        else
        {
            SpawnProjectile(power);
        }
    }

    IEnumerator SpawnMultiProjectile(float power, float delay)
    {
        SpawnProjectile(power + 0.1f);
        yield return new WaitForSeconds(delay);
        SpawnProjectile(power);
        yield return new WaitForSeconds(delay);
        SpawnProjectile(power - 0.1f);
    }
    
    private void SpawnProjectile(float power)
    {
        var projectile = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, barrel.transform.rotation);
        var shootForce = maxShootForce * power;
        projectile.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * shootForce, ForceMode.Impulse);
        projectile.GetComponent<ProjectileController>().exploded.AddListener(OnProjectileExploded);
    }

    private void OnProjectileExploded(ExplosionData explosionData)
    {
        var fatalExplosionRadiusLocal = currentPowerUp == PowerupTypeEnum.MorePower ? explosionData.FatalExplosionRadius + powerPowerUp : explosionData.FatalExplosionRadius;
        var explosionRadiusLocal = currentPowerUp == PowerupTypeEnum.MorePower ? explosionData.ExplosionRadius + powerPowerUp : explosionData.ExplosionRadius;
        var destroyedEnemies = Physics.OverlapSphere(explosionData.ExplosionCoordinates, fatalExplosionRadiusLocal, LayerMask.GetMask("Targets"));
        var affectedEnemies = Physics
            .OverlapSphere(explosionData.ExplosionCoordinates, explosionRadiusLocal, LayerMask.GetMask("Targets"))
            .Except(destroyedEnemies);
        
        Instantiate(projectileExplosion, explosionData.ExplosionCoordinates, Quaternion.Euler(0, 0, 0));
        foreach (var e in affectedEnemies)
        {
            if (e.gameObject.CompareTag("Enemy"))
            {
                e.GetComponent<Rigidbody>()
                    .AddExplosionForce(explosionData.ExplosionForce, 
                        explosionData.ExplosionCoordinates, 
                        explosionRadiusLocal, 
                        10);
            }
        }
        Array.ForEach(destroyedEnemies, e =>
        {
            if (e.gameObject.CompareTag("Powerup"))
            {
                ProcessPowerup(e.gameObject);
            }
            else
            {
                Instantiate(pumpkinExplosion, e.gameObject.transform.position, Quaternion.Euler(-45, 0, 0));
            }
            Destroy(e.gameObject);
        });
        _model.Destroyed += destroyedEnemies.Length;
    }
    
    private void ProcessPowerup(GameObject powerup)
    {
        AudioSource.PlayOneShot(PickupPowerup);
        ingameTxt.SetActive(true);
        Invoke(nameof(DeactivateIngameText), 2.0f);
        
        var targetedPowerup = powerup.GetComponent<PowerupType>().type;
        switch (targetedPowerup)
        {
            case PowerupTypeEnum.MoreTime:
                _model.RoundTime += timePowerUpSec;
                ingameTxt.GetComponent<TextMeshProUGUI>().text = "+10 sec!";
                break;
            case PowerupTypeEnum.MorePower:
                currentPowerUp = targetedPowerup;
                ingameTxt.GetComponent<TextMeshProUGUI>().text = "Extra power!";
                StartCoroutine(RestoreExplosionPower());
                break;
            case PowerupTypeEnum.MoreProjectiles:
                currentPowerUp = targetedPowerup;
                ingameTxt.GetComponent<TextMeshProUGUI>().text = "Mutishot!";
                StartCoroutine(RevertMultiShot());
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }  
    }

    private void DeactivateIngameText()
    {
        ingameTxt.SetActive(false);
    }
    
    IEnumerator RestoreExplosionPower()
    {
        yield return new WaitForSeconds(powerUpDurationSec);
        if (currentPowerUp == PowerupTypeEnum.MorePower)
        {
            currentPowerUp = PowerupTypeEnum.None;
        }
    }
    
    IEnumerator RevertMultiShot()
    {
        yield return new WaitForSeconds(powerUpDurationSec);
        if (currentPowerUp == PowerupTypeEnum.MoreProjectiles)
        {
            currentPowerUp = PowerupTypeEnum.None;
        }
    }
}
