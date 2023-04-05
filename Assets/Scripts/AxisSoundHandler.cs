using UnityEngine;

public class AxisSoundHandler : MonoBehaviour
{
    public string AxisName;
    public AudioClip AxisSound;
    public AudioSource AudioSource;
    
    private float _axisValue;
    private float _verticalA;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource.clip = AxisSound;
    }

    // Update is called once per frame
    void Update()
    {
        _axisValue = Input.GetAxis(AxisName);

        if (_axisValue != 0 && !AudioSource.isPlaying)
        {
            AudioSource.Play();
        }
        else if (_axisValue == 0 && AudioSource.isPlaying)
        {
            AudioSource.Stop();
        }
    }
}
