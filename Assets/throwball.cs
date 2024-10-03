using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform playerSpawnPoint;
    public float throwForce = 10f;
    public float throwCooldown = 1f;

    private GameObject currentBall = null;
    private bool isPlayerTurn = true;
    private bool canThrow = true;

    public AIController aiController;

    void Update()
    {
        if (isPlayerTurn && canThrow && Input.GetMouseButtonDown(0))
        {
            ThrowBall(playerSpawnPoint);
            StartCoroutine(HandleTurnTransition());
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
    }
}
