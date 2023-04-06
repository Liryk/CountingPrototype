using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneController : MonoBehaviour
{
    public TextMeshProUGUI ShotsDestroyedTxt;
    public TextMeshProUGUI GameOverTxt;
    public TextMeshProUGUI ScoreBoard;
    
    private Model _model;

    // Start is called before the first frame update
    void Start()
    {
        _model = Model.Instance();
        GameOverTxt.text = $"Game Over {_model.PlayerName}!";
        ShotsDestroyedTxt.text = $"Destroyed: {_model.Destroyed}";

        var scoreTable = _model.LoadOrderedScoreTable(5);

        PrintScoreBoard(scoreTable);

        if (_model.IsHigherScore(_model.Destroyed, scoreTable))
        {
            ShotsDestroyedTxt.text += ". Best score!!";
        }

        _model.Save();
    }

    private void PrintScoreBoard(Dictionary<string, int> scoreTable)
    {
        foreach (KeyValuePair<string, int> kv in scoreTable)
        {
            ScoreBoard.text += $"{Environment.NewLine}{kv.Key}: {kv.Value}";
        }
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
