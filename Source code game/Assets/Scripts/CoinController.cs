using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField]
    private Seeker startAISeeker;

    [SerializeField]
    private AIAgent finishAI;


    [SerializeField]
    private GameObject coinPrefab;
    private GameObject coinInstance;

    [SerializeField]
    private CountdownTimer timer;

    [SerializeField]
    private int lowerPercentageLimit, upperPercentageLimit = 0;

    [SerializeField]
    private int nodesDistance;

    private DataController dataController;

    public int spawnPercentage {  get; private set; }
    public bool hasSpawned = false;
    private bool isDistanceCalculated = false;

    public float totalDistance;
    public float spawnDistance;

    public decimal spawnTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        startAISeeker.pathCallback += OnPathComplete;
        dataController = GameManager.Instance.dataController;

        StartCoroutine(WaitBeforeCheckTotalDistance());
    }

    private int maxTries = 50;
    IEnumerator CheckTotalDistanceCoroutine()
    {
        int attempts = 0;
        float distance = 0;

        while (attempts < maxTries)
        {
            if (finishAI.path == null)
            {
                Debug.Log($"Attempt {attempts + 1}: Condition not met yet.");
                attempts++;
                yield return new WaitForSecondsRealtime(0.05f);
            }
            distance = finishAI.path.remainingDistance;

            if (distance > 50f && !float.IsInfinity(distance))
            {
                totalDistance = distance;
                isDistanceCalculated = true;
                break;
            }

            // If not met, wait for the interval and try again
            Debug.Log($"Attempt {attempts + 1}: Condition not met yet.");
            attempts++;
            yield return new WaitForSecondsRealtime(0.08f);
        }

        if (attempts >= maxTries)
        {
            Debug.Log("Condition not met after max attempts.");
        }

        spawnPercentage = GetSpawnPercentage();
    }

    private int GetSpawnPercentage()
    {
        return UnityEngine.Random.Range(lowerPercentageLimit, upperPercentageLimit);
    }
    private void OnPathComplete(Path p)
    {
        if (p.error)
        {
            Debug.LogError("Error calculating path: " + p.errorLog);
            return;
        }

        if (AstarPath.active.graphs.Length == 0) { return; }

        SpawnCoin();
    }

    void OnDestroy()
    {
        // Unsubscribe
        startAISeeker.pathCallback -= OnPathComplete;
    }

    void SpawnCoin()
    {
        if (GameManager.Instance.GetCurrentGameState() == "Practice") { return; }
        if (hasSpawned || !isDistanceCalculated) { return; }

        //if (timer.timeRemaining >= spawnPercentage) { return; }

        AIPath finishPath = finishAI.path;
        spawnDistance = totalDistance - ((totalDistance / 100) * spawnPercentage);

        float currentPathLength = finishPath.remainingDistance;

        if (float.IsInfinity(currentPathLength)) { return;  }

        if (currentPathLength > spawnDistance) {  return; }


        Path currentPath = startAISeeker.GetCurrentPath();
        List<GraphNode> nodes = currentPath.path;

        if (nodes.Count <= nodesDistance)
        {
            return;
        }

        double percentage = 60; // Enter the percentage as a whole number

        // Calculate the percentage
        double result = (nodes.Count * percentage) / 100.0;

        //GraphNode node = nodes[nodes.Count - nodesDistance - 1];
        GraphNode node = nodes[(int)Math.Round(result)];

        coinInstance = Instantiate(coinPrefab, (Vector3)node.position, Quaternion.identity);

        hasSpawned = true;
        GameManager.Instance.pickedUpCoin = false;
        spawnTime = GameManager.Instance.dataController.time;
        //dataController.Begin();
    }

    public void Reset()
    {
        isDistanceCalculated = false;
        StartCoroutine(WaitBeforeCheckTotalDistance());
        Destroy(coinInstance);
        hasSpawned = false;
    }

    private IEnumerator WaitBeforeCheckTotalDistance()
    {
        yield return new WaitForSeconds(.1f); // Wait for 2 seconds
        StartCoroutine(CheckTotalDistanceCoroutine());
    }
}
