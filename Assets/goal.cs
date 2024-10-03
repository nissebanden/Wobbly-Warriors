using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goal : MonoBehaviour
{
    private Drunk drunkEffect; // Reference to the Drunk script

    void Start()
    {
        // Find and cache the Drunk script at the start
        drunkEffect = FindObjectOfType<Drunk>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            // Call AIHitCup to increase the drunk stage
            drunkEffect.AIHitCup(); 

            Destroy(other.gameObject); 
            
            // Destroy the cup (or the parent object if it has one)
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
