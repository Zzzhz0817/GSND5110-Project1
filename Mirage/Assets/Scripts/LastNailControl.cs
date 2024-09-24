using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastNailControl : MonoBehaviour
{
    private float moveSpeed = 10f;           // Speed at which the nail moves
    public float correctYCoordinate = 0.995f;  // Y-coordinate for correct movement
    private float incorrectYOffset = -2f;     // Offset for incorrect movement (distance the nail moves down from the original position)
    public float shatteredYCoordinate = -1.0f;  // Y-coordinate for the shattering position

    private Vector3 initialPosition;         // Initial position of the nail
    private Vector3 correctTargetPosition;   // Target position when clicked in the correct order
    private Vector3 incorrectTargetPosition; // Target position when clicked in the wrong order
    private Vector3 shatterTargetPosition;   // Target position for shattering when condition is not fulfilled

    private bool isMoving = false;           // Flag to check if the nail is moving
    public bool allOtherNailed = false;  // Condition set by another script
    public bool holeNailed = false;  // Condition set by another script

    // Reference to the original (intact) nail object
    public GameObject originalObject;

    // Reference to the shattered version of the nail
    public GameObject shatteredObject;

    private bool isShattered = false;        // Flag to check if the nail has shattered

    void Start()
    {
        // Store the initial position of the nail
        initialPosition = transform.position;

        // Set correct target position
        correctTargetPosition = new Vector3(initialPosition.x, correctYCoordinate, initialPosition.z);

        // Set incorrect target position for when clicked in the wrong order
        incorrectTargetPosition = new Vector3(initialPosition.x, initialPosition.y + incorrectYOffset, initialPosition.z);

        // Set the position where the nail should shatter
        shatterTargetPosition = new Vector3(initialPosition.x, shatteredYCoordinate, initialPosition.z);

        // Ensure the shattered object is initially inactive
        shatteredObject.SetActive(false);
    }

    void Update()
    {
        // If the nail is moving, it will handle movement in the coroutines
    }

    // Detect mouse click on the nail
    private void OnMouseDown()
    {
        // Check if this nail is the next in the correct order
        if (allOtherNailed && !isMoving && !isShattered)
        {
            isMoving = true; // Start moving the nail
            if (holeNailed)
            {
                StartCoroutine(MoveNail(correctTargetPosition)); // Move to correct position
            }
            else
            {
                StartCoroutine(MoveNail(shatterTargetPosition)); // Move to shatter position
            }
        }
        else if (!isMoving && !isShattered)
        {
            // If it's the wrong order, trigger the incorrect movement
            StartCoroutine(WrongOrderMotion());
        }
    }

    // Move the nail down towards the specified target position
    private IEnumerator MoveNail(Vector3 targetPosition)
    {
        isMoving = true;
        // Move the nail towards the target position
        while (Vector3.Distance(transform.position, targetPosition) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Nail reaches the correct position or shattered position
        isMoving = false;

        // If we moved to the shatter position, trigger the shattering effect
        if (transform.position == shatterTargetPosition && !holeNailed)
        {
            ShatterNail(); // Shatter at the shattered position
        }
    }

    // Handle incorrect movement: Nail moves down and then returns to the original position
    private IEnumerator WrongOrderMotion()
    {
        // Move down to the incorrect target position
        yield return StartCoroutine(MoveNail(incorrectTargetPosition));

        // Move back up to the original position
        yield return StartCoroutine(MoveNail(initialPosition));
    }

    // Function to handle the nail shattering
    private void ShatterNail()
    {
        if (!isShattered)
        {
            // Disable the original intact object
            originalObject.SetActive(false);

            // Activate the shattered object and let the pieces fall
            shatteredObject.SetActive(true);

            // Optionally, add a force to the shattered pieces to create an explosion effect
            //foreach (Rigidbody rb in shatteredObject.GetComponentsInChildren<Rigidbody>())
            //{
            //    rb.AddExplosionForce(500f, shatteredObject.transform.position, 5f); // Example explosion force
            //}

            isShattered = true;  // Mark the nail as shattered
        }
    }
}
