using UnityEngine;

public class ExplosionData
{
    public Vector3 ExplosionCoordinates { get; }
    public float ExplosionRadius { get; }
    public float FatalExplosionRadius { get; }
    public float ExplosionForce { get; }

    public ExplosionData(Vector3 explosionCoordinates, float explosionRadius, float fatalExplosionRadius, float explosionForce)
    {
        ExplosionCoordinates = explosionCoordinates;
        ExplosionRadius = explosionRadius;
        FatalExplosionRadius = fatalExplosionRadius;
        ExplosionForce = explosionForce;
    }
}