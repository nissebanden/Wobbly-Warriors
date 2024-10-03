using UnityEngine;

public class Drunk : MonoBehaviour
{
    public Material material; // Reference to your shader material
    private int cupsHitCount = 0; // Counter for cups hit

    // The number of stages (6 stages)
    private const int totalStages = 6;

    // Method to increase drunkenness stage
    public void IncreaseDrunkenness()
    {
        cupsHitCount++;
        ChangeDrunkStage();
    }

    // Method to decrease drunkenness stage
    public void DecreaseDrunkenness()
    {
        if (cupsHitCount > 0)
        {
            cupsHitCount--;
            ChangeDrunkStage();
        }
    }

    private void ChangeDrunkStage()
    {
        // Calculate the current drunk stage based on cups hit
        int drunkStage = Mathf.Clamp(cupsHitCount, 0, totalStages - 1);
        
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
