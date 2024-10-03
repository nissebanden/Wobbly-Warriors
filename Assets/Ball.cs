using UnityEngine;

public class Ball : MonoBehaviour
{
    private int bounceCount = 0;
    public int maxBounces = 2; 

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);  
            return;  
        }

        bounceCount++;

        if (bounceCount >= maxBounces)
        {
            Destroy(gameObject);
        }
    }
}
