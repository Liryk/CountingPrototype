using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public int waveSize = 1;
    [FormerlySerializedAs("minEdge")] public float minRadius;
    [FormerlySerializedAs("maxEdge")] public float maxRadius;
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnWave();
    }

    private void SpawnWave()
    {
        for (int i = 0; i < waveSize; i++)
        {
            var randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            var randomPosition = RandomBetweenRadius2D(minRadius, maxRadius);
            randomPosition.y = randomEnemy.GetComponent<MeshRenderer>().bounds.size.y / 2;
            Instantiate(randomEnemy, randomPosition, randomEnemy.transform.rotation);
        }

        waveSize++;
    }
    
    Vector3 RandomBetweenRadius3D(float minRad, float maxRad)
    {
        float diff = maxRad - minRad;
        Vector3 point = Vector3.zero;
        while(point == Vector3.zero)
        {
            point = Random.insideUnitSphere;
        }
        point = point.normalized * (Random.value * diff + minRad);
        return point;  
    }
    
    Vector3 RandomBetweenRadius2D(float minRad, float maxRad)
    {
        float radius = Random.Range(minRad, maxRad);
        float angle = Random.Range(0, 360);
            
        float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
        float z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
        float y = Mathf.Tan(Mathf.Deg2Rad * angle) * radius;

        return new Vector3(z, 0, x);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(Vector3.zero, minRadius);
        Gizmos.DrawWireSphere(Vector3.zero, maxRadius);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            SpawnWave();
        }
    }
}
