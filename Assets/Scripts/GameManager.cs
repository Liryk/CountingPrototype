using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AudioClip ClickSound;
    public AudioSource AudioSource;
    public AudioSource ClickAudioSource;
    public GameObject gameOverPanel;
    public GameObject startPanel;
    public Button startBtn;
    public TextMeshProUGUI shotsDestroyedTxt;
    public Button playAgainBtn;
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
        playAgainBtn.onClick.AddListener(PlayAgain);
        if (Model.FirstStart)
        {
            startBtn.onClick.AddListener(OnStartClick);
            startPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void OnShoot(float arg0)
    {
        _model.Shots++;
    }

    private void RoundIsOver()
    {
        AudioSource.Stop();
        Time.timeScale = 0;
        shotsDestroyedTxt.text = $"Destroyed: {_model.Destroyed}";
        gameOverPanel.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _model.Reset();
        Time.timeScale = 1;
        ClickAudioSource.PlayOneShot(ClickSound);
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeIsRunningOut() && !AudioSource.isPlaying && Time.timeScale != 0)
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

    public void OnStartClick()
    {
        startPanel.SetActive(false);
        Time.timeScale = 1;
        Model.FirstStart = false;
        ClickAudioSource.PlayOneShot(ClickSound);
    }
}
