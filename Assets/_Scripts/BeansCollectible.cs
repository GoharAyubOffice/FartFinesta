using UnityEngine;

public class BeansCollectible : MonoBehaviour
{
    public int fartPowerIncrease = 1;
    public int points = 100; // Increase points by 100

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            JoystickPlayerExample playerController = other.GetComponent<JoystickPlayerExample>();
            if (playerController != null)
            {
                playerController.IncreaseFartPower(fartPowerIncrease);
                playerController.AddPoints(points);
            }

            // Optionally, play a sound or particle effect here

            // Destroy the beans object after collection
            Destroy(gameObject);
        }
    }
}
