using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{

    public TextMeshProUGUI PlayerNameTxt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStart()
    {
        Model.Instance().PlayerName = PlayerNameTxt.text;
        SceneManager.LoadScene(1);
    }
}
