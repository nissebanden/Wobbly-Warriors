using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform spawnPoint;
    public float throwForce = 10f;
    public float minThrowForce = 8f; 
    public string cupTag = "Cup"; 

    private GameObject currentBall = null;
    private GameObject[] cups;
    private Drunk drunkEffect; // Reference to the Drunk script

    void Start()
    {
        cups = GameObject.FindGameObjectsWithTag(cupTag);
        drunkEffect = FindObjectOfType<Drunk>(); // Find the Drunk script
    }

    public void ThrowAI()
    {
        if (currentBall != null)
        {
            Destroy(currentBall);  
        }

        if (cups.Length == 0) return; 

        GameObject targetCup = cups[Random.Range(0, cups.Length)];

        currentBall = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
        currentBall.tag = "AI_Ball"; // Set the tag for AI balls
        Rigidbody rb = currentBall.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 direction = (targetCup.transform.position - spawnPoint.position).normalized;

            float adjustedThrowForce = Mathf.Max(throwForce, minThrowForce);

            rb.AddForce(direction * adjustedThrowForce, ForceMode.Impulse);
        }

        Destroy(currentBall, 5f); 

        // Check if the ball hits a cup after a short delay
        StartCoroutine(CheckIfHitCup(currentBall, targetCup));
    }

    private IEnumerator CheckIfHitCup(GameObject ball, GameObject targetCup)
    {
        // Wait for a short duration to allow the ball to travel
        yield return new WaitForSeconds(0.5f);

        // Check for collision with the cup
        if (ball != null && targetCup != null)
        {
            // Simple check for collision (you may want to use actual collision detection)
            if (Vector3.Distance(ball.transform.position, targetCup.transform.position) < 0.5f) // Adjust the distance as needed
            {
                // Call AIHitCup() when the AI successfully hits a cup
                drunkEffect.AIHitCup(); 
                Debug.Log("AI hit the cup!");
            }
        }
    }
}
