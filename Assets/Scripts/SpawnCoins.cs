using UnityEngine;

public class SpawnCoins : MonoBehaviour
{
    public GameObject coinPrefab; 
    public float maxX;
    public float minX;
    public float maxY;
    public float minY;
    public float timeBetweenSpawn; 
    private float spawnTime;

    void Update()
    {
        if (Time.time > spawnTime)
        {
            SpawnCoin();
            spawnTime = Time.time + timeBetweenSpawn;
        }
    }

    void SpawnCoin()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        GameObject spawnedCoin = Instantiate(coinPrefab, transform.position + new Vector3(randomX, randomY, 0), Quaternion.identity);
        spawnedCoin.AddComponent<CoinMovement>(); 
    }
}

public class CoinMovement : MonoBehaviour
{
    private float moveSpeed = 10f;

    void Update()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }
}
