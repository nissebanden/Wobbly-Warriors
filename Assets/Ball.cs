using UnityEngine;

public class Ball : MonoBehaviour
{
    private int bounceCount = 0;

    void OnCollisionEnter(Collision collision)
    {
        // Increment bounce count only for ground or floor collisions
        // You might want to customize this based on your specific requirements
        if (collision.gameObject.CompareTag("Ground"))
        {
            bounceCount++;
            if (bounceCount > 1)
            {
                Destroy(gameObject);
            }
        }
    }
}