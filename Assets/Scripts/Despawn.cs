using UnityEngine;

public class Despawn : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Despawner"))
        {
            Destroy(collision.gameObject); 
        }
    }
}
