using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneController : MonoBehaviour
{

    public TMP_InputField PlayerNameTxt;
    private Model _model;

    // Start is called before the first frame update
    void Start()
    {
        _model = Model.Instance();
        PlayerNameTxt.text = _model.PlayerName ?? PlayerNameTxt.text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStart()
    {
        _model.PlayerName = PlayerNameTxt.text;
        SceneManager.LoadScene(1);
    }
}
