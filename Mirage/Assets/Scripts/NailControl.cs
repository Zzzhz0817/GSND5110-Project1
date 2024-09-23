using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailControl : MonoBehaviour
{
    public float moveSpeed = 5f;        // Speed at which the nail moves down
    public float targetYCoordinate = 0f; // The specific Y-coordinate the nail will move to
    private bool isMoving = false;      // Flag to check if the nail is moving
    private Vector3 targetPosition;     // The target position where the nail will stop moving

    void Start()
    {
        // Calculate the target position using the same X and Z coordinates but the new Y value
        targetPosition = new Vector3(transform.position.x, targetYCoordinate, transform.position.z);
    }

    void Update()
    {
        // If the nail is moving, move it down
        if (isMoving)
        {
            MoveNailDown();
        }
    }

    // Detect mouse click on the nail
    private void OnMouseDown()
    {
        // If the player clicks the nail and it isn't already moving, start moving
        if (!isMoving)
        {
            isMoving = true;
        }
    }

    // Move the nail down towards the target position
    private void MoveNailDown()
    {
        // Move the nail towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // If the nail reaches the target position, stop moving
        if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
        {
            isMoving = false;
        }
    }
}