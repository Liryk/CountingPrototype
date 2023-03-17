using UnityEngine;

public class DestroyOnCollide : MonoBehaviour
{
    public string[] tags;

    private void OnCollisionEnter(Collision collision)
    {
        foreach (var t in tags)
        {
            if (collision.gameObject.CompareTag(t))
            {
                Destroy(gameObject);
            } 
        }
    }
}
