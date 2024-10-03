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
    public float accuracyFactor = 1.0f; // AI's accuracy, decreases as AI gets drunk

    private GameObject currentBall = null;
    private GameObject[] cups;
    private Drunk drunkEffect;

    void Start()
    {
        cups = GameObject.FindGameObjectsWithTag(cupTag);
        drunkEffect = FindObjectOfType<Drunk>();
    }

public void ThrowAI()
{
    if (currentBall != null)
    {
        Destroy(currentBall);
    }

    // Refresh the cups array to get the current state of cups in the scene
    cups = GameObject.FindGameObjectsWithTag(cupTag);

    if (cups.Length == 0) 
    {
        Debug.LogWarning("No cups available to throw at.");
        return; // Exit early if there are no cups
    }

    // Select a random cup, ensuring it's valid
    GameObject targetCup = cups[Random.Range(0, cups.Length)];

    if (targetCup == null)
    {
        Debug.LogWarning("Target cup is null, cannot proceed with the throw.");
        return;
    }

    currentBall = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
    currentBall.tag = "AI_Ball";
    Rigidbody rb = currentBall.GetComponent<Rigidbody>();

    if (rb != null)
    {
        // Calculate direction towards the cup's center
        Vector3 direction = (targetCup.transform.position - spawnPoint.position).normalized;

        // Add an arc by adjusting the y-component of the direction to give the ball some height
        float heightOffset = 0.5f; // Adjust for how much arc you want
        direction.y += heightOffset;

        // Adjust throw force based on AI's drunkenness (accuracyFactor)
        float adjustedThrowForce = Mathf.Max(throwForce * accuracyFactor, minThrowForce);

        // Randomize direction slightly for accuracy variation
        direction += Random.insideUnitSphere * (1.0f - accuracyFactor); // Less accurate when drunk
        direction = direction.normalized;

        // Apply force to the ball
        rb.AddForce(direction * adjustedThrowForce, ForceMode.Impulse);

        // Aim correction during the throw
        StartCoroutine(CorrectAim(rb, targetCup));

        // Destroy the ball after 5 seconds
        Destroy(currentBall, 5f);
    }
    else
    {
        Debug.LogError("Rigidbody component missing from the instantiated ball.");
    }

    // Start the coroutine to check if the AI hits the cup
    StartCoroutine(CheckIfHitCup(currentBall, targetCup));
}

    private IEnumerator CorrectAim(Rigidbody rb, GameObject targetCup)
    {
        float correctionTime = 0.5f; // How long we want to apply aim correction
        float timeElapsed = 0f;

        while (timeElapsed < correctionTime && rb != null && targetCup != null)
        {
            timeElapsed += Time.deltaTime;

            // Adjust the direction slightly towards the target cup during flight
            Vector3 correctionDirection = (targetCup.transform.position - rb.position).normalized;
            rb.velocity = Vector3.Lerp(rb.velocity, correctionDirection * rb.velocity.magnitude, timeElapsed / correctionTime);

            yield return null;
        }
    }

    private IEnumerator CheckIfHitCup(GameObject ball, GameObject targetCup)
    {
        yield return new WaitForSeconds(0.8f);

        if (ball != null && targetCup != null)
        {
            if (Vector3.Distance(ball.transform.position, targetCup.transform.position) < 0.5f)
            {
                drunkEffect.AIHitCup();
                Debug.Log("AI hit the cup!");
            }
        }
        else
        {
            if (ball == null)
            {
                Debug.LogWarning("The ball has been destroyed or is null.");
            }
            if (targetCup == null)
            {
                Debug.LogWarning("The target cup is null.");
            }
        }
    }
}
