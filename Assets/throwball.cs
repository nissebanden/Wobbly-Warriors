using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform playerSpawnPoint;
    public float minThrowForce = 5f; // Minimum throw force
    public float maxThrowForce = 20f; // Maximum throw force
    public float throwCooldown = 1f;

    private GameObject currentBall = null;
    private bool isPlayerTurn = true;
    private bool canThrow = true;

    private float throwForce; // Current throw force
    private float holdTime; // Time the button is held down

    public AIController aiController;

    void Update()
    {
        if (isPlayerTurn && canThrow)
        {
            // Increase hold time while the button is held down
            if (Input.GetMouseButton(0))
            {
                holdTime += Time.deltaTime;
                throwForce = Mathf.Clamp(minThrowForce + holdTime * 5f, minThrowForce, maxThrowForce); // Increase throw force based on hold time
            }

            // Throw the ball when the button is released
            if (Input.GetMouseButtonUp(0))
            {
                ThrowBall(playerSpawnPoint);
                holdTime = 0; // Reset hold time
                StartCoroutine(HandleTurnTransition());
            }
        }
    }

    void ThrowBall(Transform spawnPoint)
    {
        if (currentBall != null)
        {
            Destroy(currentBall);
        }

        currentBall = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
        currentBall.tag = "Player_Ball";
        Rigidbody rb = currentBall.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(10);
            }

            Vector3 direction = (targetPoint - spawnPoint.position).normalized;
            rb.AddForce(direction * throwForce, ForceMode.Impulse);
        }

        Destroy(currentBall, 5f);
        canThrow = false;
    }

    IEnumerator HandleTurnTransition()
    {
        yield return new WaitForSeconds(throwCooldown);

        isPlayerTurn = false;

        aiController.ThrowAI();

        yield return new WaitForSeconds(throwCooldown);

        isPlayerTurn = true;
        canThrow = true;
        throwForce = minThrowForce; // Reset throw force for the next turn
    }
}
