using System;
using UnityEngine;

public class InfiniteMap : MonoBehaviour
{
    [SerializeField] private GameObject mapChunkPrefab;
    [SerializeField] private float mapChunkSize;

    private void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                GenerateMapChunk(x, y);
            }
        }
    }

    private void GenerateMapChunk(int x, int y)
    {
        Vector3 spawnPos = new Vector3(x, y, 0) * mapChunkSize;
        Instantiate(mapChunkPrefab, spawnPos, Quaternion.identity, transform);
    }
}
