using UnityEngine;

public class Model
{
    private const string LastPlayerNameKey = "LastPlayer";
    
    public int Shots;
    public int Destroyed;
    public int RoundTime;

    private string _playerName;
    public string PlayerName
    {
        get => _playerName;
        set
        {
            if (_playerName != value)
            {
                _playerName = value;
                PlayerPrefs.SetString(LastPlayerNameKey, _playerName);
            }
        }
    }

    private static Model _instance;

    public static Model Instance()
    {
        if (_instance == null)
        {
            _instance = new Model();
            _instance.Initialize();
        }

        return _instance;
    }

    private void Initialize()
    {
        if (PlayerPrefs.HasKey(LastPlayerNameKey))
        {
            _playerName = PlayerPrefs.GetString(LastPlayerNameKey);
        }
    }

    public float GetDestroyedPerShotAverage()
    {
        Debug.Log($"Destroyed: {Destroyed}");
        Debug.Log($"Shots: {Shots}");
        return Shots > 0 ? (float)Destroyed / Shots : 0;
    }

    public void Reset()
    {
        Shots = 0;
        Destroyed = 0;
    }
}