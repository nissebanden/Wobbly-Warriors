using UnityEngine;

public class Drunk : MonoBehaviour
{
    public Material material; // Reference to your shader material
    private int cupsHitCount = 0; // Counter for cups hit

    // The number of stages (you mentioned 6 stages)
    private const int totalStages = 6;

    // Method to call when the AI hits a cup
    public void AIHitCup()
    {
        cupsHitCount++;

        // Change the drunk stage based on the number of cups hit
        ChangeDrunkStage();
    }

    private void ChangeDrunkStage()
    {
        int drunkStage = cupsHitCount / 1; // Change stage every cup hit

        if (drunkStage >= totalStages) // Limit to max stages
        {
            drunkStage = totalStages - 1;
        }

        // Update the material's shader property based on the drunk stage
        ApplyDrunkEffect(drunkStage);
    }

    private void ApplyDrunkEffect(int stage)
    {
        // Map stage to a value between 0 and 1
        float drunkIntensity = (float)stage / (totalStages - 1);
        
        // Set the _DrunkStage property on the material
        material.SetFloat("_DrunkStage", drunkIntensity);

        Debug.Log("Current Drunk Stage: " + stage + ", Intensity: " + drunkIntensity);
    }
}
