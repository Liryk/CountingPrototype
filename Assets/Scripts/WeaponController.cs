using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float rotationSpeed = 0;
    
    private float _horizontalA;
    private float _verticalA;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalA = Input.GetAxis("Horizontal");
        _verticalA = Input.GetAxis("Vertical");
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Down");
        }
        
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime * _horizontalA);
    }
}
