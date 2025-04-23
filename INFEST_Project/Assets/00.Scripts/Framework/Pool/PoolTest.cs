using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class PoolTest : MonoBehaviour
{
    public NetworkRunner runner;
    public List<NetworkObject> objects = new List<NetworkObject>();
    public NetworkObject prefab;

    private System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

    //private bool IsSpawn = true;

    private void Start()
    {
        runner = gameObject.AddGetComponent<NetworkRunner>();
        StartGame();
    }

    public async void StartGame()
    {
        await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
        });
    }

    public void Update()
    {
        //if( runner && IsSpawn)
        //    runner.Spawn(prefab);
        //if (runner && !IsSpawn)
        //    runner.Despawn(prefab);
    }

    public void OnGUI()
    {
        GUILayout.BeginVertical("box", GUILayout.Width(200));

        if (GUILayout.Button("Spawn 1000"))
        {
            //IsSpawn = true ;
            sw.Reset();
            sw.Start();
            // 전체 메모리 사용량 (바이트)
            long previousTotalMemory = Profiler.GetTotalAllocatedMemoryLong();

            // 메가바이트 단위로 변환
            float totalMemoryMB = previousTotalMemory / (1024f * 1024f);

            Debug.Log($"이전 메모리 사용량: {totalMemoryMB:F2} MB");
            for (int i = 0; i < 1000; i++)
            {
                var obj = runner.Spawn(prefab);
                objects.Add(obj);
            }
            // 전체 메모리 사용량 (바이트)
            long afterTotalMemory = Profiler.GetTotalAllocatedMemoryLong();

            // 메가바이트 단위로 변환
            float afterTotalMemoryMB = afterTotalMemory / (1024f * 1024f);

            Debug.Log($"이후 메모리 사용량: {afterTotalMemoryMB:F2} MB");
            sw.Stop();
            UnityEngine.Debug.Log($"Spawned 1000. Total: {objects.Count}. Time: {sw.ElapsedMilliseconds}ms");
        }

        if (GUILayout.Button("Despawn 1000"))
        {
            //IsSpawn = false;
            sw.Reset();
            sw.Start();
            // 전체 메모리 사용량 (바이트)
            long previousTotalMemory = Profiler.GetTotalAllocatedMemoryLong();

            // 메가바이트 단위로 변환
            float totalMemoryMB = previousTotalMemory / (1024f * 1024f);

            Debug.Log($"이전 메모리 사용량: {totalMemoryMB:F2} MB");
            int count = Mathf.Min(1000, objects.Count);
            for (int i = 0; i < count; i++)
            {
                runner.Despawn(objects[0]);
                objects.RemoveAt(0);
            }
            // 전체 메모리 사용량 (바이트)
            long afterTotalMemory = Profiler.GetTotalAllocatedMemoryLong();

            // 메가바이트 단위로 변환
            float afterTotalMemoryMB = afterTotalMemory / (1024f * 1024f);

            Debug.Log($"이후 메모리 사용량: {afterTotalMemoryMB:F2} MB");

            sw.Stop();
            UnityEngine.Debug.Log($"Despawned 1000. Remaining: {objects.Count}. Time: {sw.ElapsedMilliseconds}ms");
        }

        if (GUILayout.Button("Despawn All"))
        {
            sw.Reset();
            sw.Start();
            foreach (var obj in objects)
            {
                runner.Despawn(obj);
            }
            sw.Stop();

            objects.Clear();
            UnityEngine.Debug.Log($"Despawned All. Time: {sw.ElapsedMilliseconds}ms");
        }

        GUILayout.EndVertical();
    }
}
