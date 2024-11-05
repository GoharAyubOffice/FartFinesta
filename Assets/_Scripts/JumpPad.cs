using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpForce = 10f; // The force with which the player will be launched
    public Vector3 jumpDirection = Vector3.up; // The direction of the jump, default is upward

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.collider.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // Apply a force to the player's rigidbody
                playerRb.linearVelocity = Vector3.zero; // Reset velocity to ensure consistent jump
                playerRb.AddForce(jumpDirection.normalized * jumpForce, ForceMode.VelocityChange);

                // Play the bounce animation
                animator.SetTrigger("Bounce");
            }
        }
    }
}
