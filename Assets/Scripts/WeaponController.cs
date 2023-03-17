using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject directionIndicator;
    public GameObject projectilePrefab;
    public GameObject projectileSpawnPoint;
    public GameObject barrel;
    [Header("MortarValues")]
    public int maxShootForce;
    public float rotationSpeed = 0;
    public float tiltSpeed = 0;
    public float minTilt = 18;
    public float maxTilt = 55;
    
    private float _horizontalA;
    private float _verticalA;
    private UiController _uicController;

    // Start is called before the first frame update
    void Start()
    {
        _uicController = FindObjectOfType<UiController>();
        _uicController.onShoot.AddListener(OnShoot);
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalA = Input.GetAxis("Horizontal");
        _verticalA = Input.GetAxis("Vertical");

        directionIndicator.SetActive(_horizontalA != 0);
        
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime * _horizontalA);
        barrel.transform.Rotate(Vector3.forward * tiltSpeed * Time.deltaTime * _verticalA);
        
        if (barrel.transform.eulerAngles.z > maxTilt)
        {
            var maxAngle = barrel.transform.eulerAngles;
            maxAngle.z = maxTilt;
            barrel.transform.eulerAngles = maxAngle;
        }
        else if (barrel.transform.eulerAngles.z < minTilt)
        {
            var minAngle = barrel.transform.eulerAngles;
            minAngle.z = minTilt;
            barrel.transform.eulerAngles = minAngle;
        }
    }

    private void OnShoot(float power)
    {
        SpawnProjectile(power);
    }

    private void SpawnProjectile(float power)
    {
        var projectile = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, barrel.transform.rotation);
        var shootForce = maxShootForce * power;
        Debug.Log($"shootForce: {shootForce}");
        projectile.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * shootForce, ForceMode.Impulse);
    }
}
