using System.Collections;
using UnityEngine;

public class HoleControl : MonoBehaviour
{
    public LastNailControl lastNailControl;  // Reference to the LastNailControl script
    private Rigidbody rb;                  // Rigidbody component of the object
    private float moveSpeed = 10f;           // Speed at which the nail moves down and back up
    public float correctYCoordinate = -2f;  // Y-coordinate for correct movement
    public float initialDownwardSpeed = 10f; // Initial downward speed after movement

    private Vector3 initialPosition;       // Initial position of the nail
    private Vector3 correctTargetPosition; // Target position when clicked in the correct order

    private bool isMoving = false;         // Flag to check if the nail is moving

    void Start()
    {
        // Store the initial position of the nail
        initialPosition = transform.position;

        // Correct target position (this happens when clicked in the right order)
        correctTargetPosition = new Vector3(initialPosition.x, correctYCoordinate, initialPosition.z);
        
        // Ensure the object starts in a stationary state, disable Rigidbody physics initially
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Disable physics, object will be manually controlled initially
    }

    void Update()
    {
        // If the nail is moving, it will handle movement in the coroutines
    }

    // Detect mouse click on the nail
    private void OnMouseDown()
    {
        if (!isMoving)
        {
            isMoving = true; // Move the nail down
            lastNailControl.holeNailed = true;  // Trigger the condition
            StartCoroutine(MoveNail(correctTargetPosition)); // Move nail to correct position
        }
    }

    // Move the nail down towards the correct target position
    private IEnumerator MoveNail(Vector3 targetPosition)
    {
        isMoving = true;
        
        // Move the nail towards the target position
        while (Vector3.Distance(transform.position, targetPosition) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // After moving down, enable gravity and allow the object to be controlled by physics
        rb.isKinematic = false; // Enable Rigidbody physics, allowing gravity and collisions
        rb.useGravity = true;    // Enable gravity

        // Give the object an initial downward speed
        rb.velocity = new Vector3(0, -initialDownwardSpeed, 0);

        // Remove any constraints if needed (optional)
        rb.constraints = RigidbodyConstraints.None;

        isMoving = false; // Reset the moving flag
    }
}
