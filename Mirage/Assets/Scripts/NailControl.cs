using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailControl : MonoBehaviour
{
    public LastNailControl lastNailControl;  // Reference to the LastNailControl script
    private float moveSpeed = 10f;           // Speed at which the nail moves down and back up
    public float correctYCoordinate = 0.995f;  // Y-coordinate for correct movement
    private float incorrectYOffset = -2f;   // Offset for incorrect movement (distance the nail moves down from the original position)

    private Vector3 initialPosition;       // Initial position of the nail
    private Vector3 correctTargetPosition; // Target position when clicked in the correct order
    private Vector3 incorrectTargetPosition; // Target position when clicked in the wrong order

    private bool isMoving = false;         // Flag to check if the nail is moving

    public static int currentNailIndex = 0;    // Index of the nail that should be clicked next in the correct order
    public int nailOrderIndex;                 // This nail's index in the correct order

    void Start()
    {
        // Store the initial position of the nail
        initialPosition = transform.position;

        // Correct target position (this happens when clicked in the right order)
        correctTargetPosition = new Vector3(initialPosition.x, correctYCoordinate, initialPosition.z);

        // Incorrect target position (this happens when clicked in the wrong order)
        incorrectTargetPosition = new Vector3(initialPosition.x, initialPosition.y + incorrectYOffset, initialPosition.z);
    }

    void Update()
    {
        // If the nail is moving, it will handle movement in the coroutines
    }

    // Detect mouse click on the nail
    private void OnMouseDown()
    {
        // Check if this nail is the next in the correct order
        if (nailOrderIndex == currentNailIndex && !isMoving)
        {
            isMoving = true; // Move the nail down
            StartCoroutine(MoveNail(correctTargetPosition)); // Move nail to correct position
            currentNailIndex++; // Increment the current nail index
            if (currentNailIndex == 6)
            {
                lastNailControl.allOtherNailed = true;  // Trigger the condition
            }
        }
        else if (!isMoving)
        {
            // If it's the wrong order, trigger the incorrect movement
            StartCoroutine(WrongOrderMotion());
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
        isMoving = false;
    }

    // Handle incorrect movement: Nail moves down and then returns to the original position
    private IEnumerator WrongOrderMotion()
    {
        // Move down to the incorrect target position
        yield return StartCoroutine(MoveNail(incorrectTargetPosition));

        // Move back up to the original position
        yield return StartCoroutine(MoveNail(initialPosition));
    }
}
