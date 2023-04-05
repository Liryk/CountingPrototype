using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioSource AudioSource;
    private RoundTimerController _roundTimerController;
    private UiController _uicController;

    private Model _model;
    
    // Start is called before the first frame update
    void Start()
    {
        _model = Model.Instance();
        _uicController = FindObjectOfType<UiController>();
        _uicController.onShoot.AddListener(OnShoot);
        _roundTimerController = FindObjectOfType<RoundTimerController>();
        _roundTimerController.timeIsOver.AddListener(RoundIsOver);
    }

    private void OnShoot(float arg0)
    {
        _model.Shots++;
    }

    private void RoundIsOver()
    {
        SceneManager.LoadScene(2);
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeIsRunningOut() && !AudioSource.isPlaying)
        {
            AudioSource.Play();
        } else if (!TimeIsRunningOut() && AudioSource.isPlaying)
        {
            AudioSource.Stop();
        }
    }

    private bool TimeIsRunningOut()
    {
        return _model.RoundTime < 10;
    }
}
