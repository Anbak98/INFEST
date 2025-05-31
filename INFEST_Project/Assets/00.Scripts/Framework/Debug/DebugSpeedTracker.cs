using Fusion;
using UnityEngine;

public class DebugSpeedTracker : NetworkBehaviour
{
    private Vector3 lastPosition;
    private float speed; // m/s 단
    private float totalDistance;
    private float totalTime;

    void Start()
    {
        lastPosition = transform.position;
    }

    public override void FixedUpdateNetwork()
    {
        float distance = Vector3.Distance(transform.position, lastPosition);
        float deltaTime = Runner.DeltaTime;

        speed = distance / deltaTime; // 초당 이동 거리 (m/s)
        lastPosition = transform.position;
        totalDistance += distance;
        totalTime += deltaTime;

        Debug.Log($"{gameObject.name} Speed: {speed:F2} m/s {totalTime} : {totalDistance}");
    }
}