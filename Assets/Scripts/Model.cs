using UnityEngine;

public class Model
{
    public int Shots;
    public int Destroyed;
    public int RoundTime;
    public string PlayerName;

    private static Model _instance;

    public static Model Instance()
    {
        if (_instance == null)
        {
            _instance = new Model();
        }

        return _instance;
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