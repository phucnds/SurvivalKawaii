using UnityEngine;

public class InfiniteChildMover : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float mapChunkSize;
    [SerializeField] private float distanceThreshold = 1.5f;

    private void Update()
    {
        UpdateChildren();
    }

    private void UpdateChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            Vector3 distance = playerTransform.position - child.position;
            float calculatedDistanceThreshold = mapChunkSize * distanceThreshold;

            if (Mathf.Abs(distance.x) > calculatedDistanceThreshold)
            {
                child.position += Vector3.right * calculatedDistanceThreshold * 2 * Mathf.Sign(distance.x);
            }

            if (Mathf.Abs(distance.y) > calculatedDistanceThreshold)
            {
                child.position += Vector3.up * calculatedDistanceThreshold * 2 * Mathf.Sign(distance.y);
            }
        }
    }
}
