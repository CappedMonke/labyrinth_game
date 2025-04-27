using UnityEngine;

public class DestinationTile : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<Renderer>().material.color = Color.green;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MapGenerator mapGenerator = FindFirstObjectByType<MapGenerator>();
            if (mapGenerator != null)
            {
                mapGenerator.RegenerateLabyrinth();
            }
        }
    }
}
