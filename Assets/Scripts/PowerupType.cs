using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupType : MonoBehaviour
{
    public PowerupTypeEnum type;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum PowerupTypeEnum
{
    None,
    MoreTime,
    MorePower,
    MoreProjectiles,
}
