using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneController : MonoBehaviour
{
    public TextMeshProUGUI ShotsDestroyedTxt;
    
    private Model _model;

    // Start is called before the first frame update
    void Start()
    {
        _model = Model.Instance();
        ShotsDestroyedTxt.text = $"Destroyed: {_model.Destroyed}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayAgain()
    {
        _model.Reset();
        SceneManager.LoadScene(1);
    }
}
