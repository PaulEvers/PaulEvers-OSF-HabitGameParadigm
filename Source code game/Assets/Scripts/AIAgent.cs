
using UnityEngine;
using Pathfinding;

public class AIAgent : MonoBehaviour
{
    [HideInInspector] public AIPath path {  get; private set; }
    public Seeker seeker;
    [SerializeField] private Transform target;
    public bool isPathfinding = false;

    [SerializeField]
    CountdownTimer timer;

    private void Start()
    {
        path = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();
    }

    private void Update()
    {
        if (!isPathfinding || AstarPath.active.isScanning) { return; }
        path.destination = target.position;
    }

    public void StartPathFinding()
    {
        isPathfinding = true;
    }

    public void StopPathFinding()
    {
        isPathfinding = false;
    }

    public float GetTotalLength()
    {
        return 0f;
    }
}
