using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    public GameObject[] obstacles; // Array for multiple obstacles
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
            SpawnObstacle();
            spawnTime = Time.time + timeBetweenSpawn;
        }
    }

    void SpawnObstacle()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        GameObject selectedObstacle = obstacles[Random.Range(0, obstacles.Length)]; // Pick random obstacle
        GameObject spawnedObstacle = Instantiate(selectedObstacle, transform.position + new Vector3(randomX, randomY, 0), Quaternion.identity);
        spawnedObstacle.AddComponent<ObstacleMovement>(); // Attach movement script
    }
}

public class ObstacleMovement : MonoBehaviour
{
    private float moveSpeed = -10f; // Match background speed

    void Update()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }
}
