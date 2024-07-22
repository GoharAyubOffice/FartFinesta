using UnityEngine;


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

            Debug.Log("Game Over - Player reached the finish point!");
        }
    }
}
