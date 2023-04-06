using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Model
{
    private const string LastPlayerNameKey = "LastPlayer";
    private readonly string ScoresFolderPath = Path.Combine(Application.persistentDataPath, "Scores");
    
    public int Shots;
    public int Destroyed;
    public int RoundTime;

    private string _playerName;
    private string _storageFilePath;
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
            _storageFilePath = Path.Combine(ScoresFolderPath, PlayerName);
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
        if (!Directory.Exists(ScoresFolderPath))
        {
            Directory.CreateDirectory(ScoresFolderPath);
        }

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

    public bool IsHigherScore(int targetScore, Dictionary<string, int> scoreTable)
    {
        var isHigherScore = true;

        foreach(var score in scoreTable.Values)
        {
            if (score >= targetScore)
            {
                isHigherScore = false;
                break;
            }
        }

        return isHigherScore;
    }

    public Dictionary<string, int> LoadOrderedScoreTable(int maxTableSize = -1)
    {
        var scoreTable = new Dictionary<string, int>();
        var scoreFiles = Directory.GetFiles(ScoresFolderPath);
        foreach(string f in scoreFiles)
        {
            var scoreDto = ReadUserDataFromFile(f);
            scoreTable.Add(Path.GetFileName(f), scoreDto.Destroyed);
        }

        var orderedDictionary = scoreTable.OrderByDescending(key => key.Value);

        scoreTable = new Dictionary<string, int>();
        int index = 0;
        foreach (var kv in orderedDictionary)
        {
            index++;
            scoreTable.Add(kv.Key, kv.Value);
            if (index >= maxTableSize && maxTableSize != -1)
            {
                break;
            }
        }


        return scoreTable;
    }

    public void Save()
    {
        var userDataDto = new UserDataDto()
        {
            Destroyed = Destroyed
        };

        var json = JsonUtility.ToJson(userDataDto);
        if (File.Exists(_storageFilePath))
        {
            var oldUserdata = ReadUserDataFromFile(_playerName);
            if (oldUserdata.Destroyed < userDataDto.Destroyed)
            {
                File.WriteAllText(_storageFilePath, json);
            }
        } 
        else
        {
            File.WriteAllText(_storageFilePath, json);
        }
    }

    private UserDataDto ReadUserDataFromFile(string userName)
    {
        var json = File.ReadAllText(Path.Combine(ScoresFolderPath, userName));
        return JsonUtility.FromJson<UserDataDto>(json);
    }
}