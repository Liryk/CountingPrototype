using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class UiController : MonoBehaviour
{
    public TextMeshProUGUI shotsTxt;
    public TextMeshProUGUI destroyedTxt;
    public GameObject forceIndicator;
    public float chargeSpeed;
    public float shootIntervalSec = 1;
    public AudioSource ChargingWeaponAudioSource;
    
    [Header("Debug")]
    public bool startCharging;
    public float currentForceScale;
    public float currentShootInterval = 0;
    public UnityEvent<float> onShoot;

    private Model _model;
    
    // Start is called before the first frame update
    void Start()
    {
        _model = Model.Instance();
        currentShootInterval = shootIntervalSec;
        forceIndicator.transform.localScale = new Vector3(
            currentForceScale, 
            forceIndicator.transform.localScale.y, 
            forceIndicator.transform.localScale.z);
        ChargingWeaponAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        currentShootInterval += Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.Space) && currentShootInterval >= shootIntervalSec)
        {
            startCharging = true;
            ChargingWeaponAudioSource.Play();
        }

        if (startCharging)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                currentForceScale = currentForceScale >= 1 ? 1 : currentForceScale + Time.deltaTime * chargeSpeed;
            }
            
            if (Input.GetKeyUp(KeyCode.Space))
            {
                onShoot.Invoke(currentForceScale);
                currentForceScale = 0;
                currentShootInterval = 0;
                ChargingWeaponAudioSource.Stop();
                startCharging = false;
            }
            
            forceIndicator.transform.localScale = new Vector3(
                currentForceScale, 
                forceIndicator.transform.localScale.y, 
                forceIndicator.transform.localScale.z);
        }

        UpdateTxt();
    }

    private void UpdateTxt()
    {
        shotsTxt.text = $"Shots: {_model.Shots}";
        destroyedTxt.text = $"Destroyed: {_model.Destroyed}";
    }
}
