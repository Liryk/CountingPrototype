using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class RoundTimerController : MonoBehaviour
{
    public int roundTime;
    public TextMeshProUGUI counterTxt;
    public UnityEvent timeIsOver;
    
    private Model _model;

    void Start()
    {
        _model = Model.Instance();
        _model.RoundTime = roundTime;
        counterTxt.text = _model.RoundTime.ToString();
        InvokeRepeating(nameof(Tick), 1, 1);
    }

    private void Tick()
    {
        _model.RoundTime--;
        counterTxt.text = _model.RoundTime.ToString();

        if (_model.RoundTime == 0)
        {
            CancelInvoke();
            timeIsOver.Invoke();
        }
    }
}
