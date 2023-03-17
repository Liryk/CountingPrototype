using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UiController : MonoBehaviour
{
    public GameObject forceIndicator;
    public float chargeSpeed;
    public float shootIntervalSec = 1;
    
    [Header("Debug")]
    public bool startCharging;
    public float currentForceScale;
    public float currentShootInterval = 0;
    public UnityEvent<float> onShoot;
    
    // Start is called before the first frame update
    void Start()
    {
        currentShootInterval = shootIntervalSec;
        forceIndicator.transform.localScale = new Vector3(
            currentForceScale, 
            forceIndicator.transform.localScale.y, 
            forceIndicator.transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        currentShootInterval += Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.Space) && currentShootInterval >= shootIntervalSec)
        {
            startCharging = true;
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
                startCharging = false;
            }
            
            forceIndicator.transform.localScale = new Vector3(
                currentForceScale, 
                forceIndicator.transform.localScale.y, 
                forceIndicator.transform.localScale.z);
        }
    }
}
