using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goalMovment : MonoBehaviour
{
    public float minX = -7f; 
    public float maxX = 7f; 
    public float minZ = -4f; 
    public float maxZ = 12f; 
    public float moveSpeed = 2f; 

    void Start()
    {
        StartCoroutine(RandomXMoveCoroutine());
    }

    IEnumerator RandomXMoveCoroutine()
    {
        while (true)
        {
            // Generate random movement along the X-axis
            float randomX = Random.Range(-0.25f, 0.25f); 
            Vector3 newPosition = transform.localPosition + new Vector3(randomX, 0f, 0f) * moveSpeed * Time.deltaTime;

            // Clamp the new X position within the specified range
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

            // Update the object's position
            transform.localPosition = newPosition;

            // Wait for a random interval before generating the next movement
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        }
    }
    

    // void Update()
    // {
    //     // Generate random movement within the XZ plane
    //     Vector3 randomDirection = new Vector3(Random.Range(-0.25f, 0.25f), 0f, Random.Range(-0.25f, 0.24f));
    //     Vector3 newPosition = transform.localPosition + randomDirection * moveSpeed * Time.deltaTime;

    //     // Clamp the new position within the specified range
    //     newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
    //     newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);

    //     // Update the object's position
    //     transform.localPosition = newPosition;
    // }
}
