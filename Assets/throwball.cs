    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class GameController : MonoBehaviour
    {
        public GameObject ballPrefab;
        public Transform spawnPoint;
        public float throwForce = 10f;
        public float throwCooldown = 1f; 
    
        private float nextThrowTime = 0f; 
    
        void Update()
        {
            if (Input.GetMouseButtonDown(0) && Time.time >= nextThrowTime)
            {
                nextThrowTime = Time.time + throwCooldown; 
                ThrowBall();
            }
        }
    
        void ThrowBall()
        {
            GameObject newBall = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
            newBall.AddComponent<Ball>(); // Attach Ball script if not already attached
            Rigidbody rb = newBall.GetComponent<Rigidbody>();
        
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
            
            Destroy(newBall, 10);
        }
    }
