using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed; // The default movement speed of the player
    private bool isMoving; // private to prevent input when the player is already moving
    private Vector2 input;
    private Animator animator;

    // Start is called before the first frame update, when the game object is created
    void Start()
    {
        // Find and assign the Animator component attached to the player
        animator = GetComponent<Animator>();

    }

    // Update is called once every frame
    void Update()
    {
        // Ensures new input is only processed when the player has finished moving
        if (!isMoving)
        {
            // Capturing input from the player
            // GetAxisRaw provides values of -1, 0, or 1 based on input
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // Removes diagonal movement; if x has an input, make the y input nonexistent/0
            if (input.x != 0) input.y = 0;
        

            // Calculates new target position if player is not standing still
            if (input != Vector2.zero) {
                // Set the movement values in the animator
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);
                // Capturing the target position to which the player should move
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                // Start moving the player and set isMoving to true to prevent further input until movement is complete
                StartCoroutine(Move(targetPos));
                isMoving = true;
            }
        }

        // Set the boolean for isMoving in the animator
        animator.SetBool("isMoving", isMoving);
    }

    // Special function with IEnumerator as a return type; coroutine handles movement over time that span multiple frames
    // Examples: smooth movements, animations, or timed events
    IEnumerator Move(Vector3 targetPos)
    {
        // While there is still a significant distance between the current position and the target position...
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) //sqrMagnitude avoids the computational cost of actual magnitude due to square root
        {
            // Move towards the target position at a fixed speed (built-in Unity function) from current position of transform.position to that target position of targetPos
            // moveSpeed = value that can be set in the Unity Inspector
            // Time.deltaTime =  amount of time in seconds that has passed since last frame; ensures that movement is frame-rate independent
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            // Pause the execution here and resume in the next frame, when Update is called again
            yield return null;
        }

        // Ensure the player ends exactly at the target position
        transform.position = targetPos;
        isMoving = false;
    }
}
