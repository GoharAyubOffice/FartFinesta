using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public class PlayerTrigger : MonoBehaviour
{
    //public Animator playerAnimator; // Reference to the player's Animator component

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("finish"))
        {
            // Stop player movement (assuming you have a script controlling movement)
            GetComponent<JoystickPlayerExample>().enabled = false; // Disable movement script
            GetComponent<FartPropulsion>().enabled = false;

            // Trigger final animation
            //if (playerAnimator != null)
            //{
            //    playerAnimator.SetTrigger("finish"); // Assuming you have a trigger parameter named "Finish" in your Animator
            //}
            //SceneManager.LoadScene(0);
            // Optionally, perform other actions such as showing UI for game over or level completion
            Debug.Log("Game Over - Player reached the finish point!");
        }
    }
}
