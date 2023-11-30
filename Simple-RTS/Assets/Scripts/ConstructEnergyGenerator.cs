using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructEnergyGenerator : MonoBehaviour
{
    string adjacentPlatformNum;

    GameControl gameControl;

    AdjacentBuildPanel adjacentBuildPanel;
    GameObject adjacentPlatformObject;
    AdjacentPlatform adjacentPlatform;

    GameObject canvasObject;
    CanvasInfo canvasInfo;
    GameObject adjacentBuildPanelObject;

    public GameObject energyGeneratorBlue;

    public void BuildEnergyGenerator()
    {
        adjacentBuildPanel = GetComponentInParent<AdjacentBuildPanel>();
        adjacentPlatformNum = adjacentBuildPanel.adjacentPlatformNum;
        string adjacentPlatformName = "Platform_Adjacent" + adjacentPlatformNum;

        adjacentPlatformObject = GameObject.Find(adjacentPlatformName);
        adjacentPlatform = adjacentPlatformObject.GetComponent<AdjacentPlatform>();
        Debug.Log("Found Adjacent Platform" + adjacentPlatformNum);

        // Spawn energy generator
        float positionY = energyGeneratorBlue.transform.position.y;
        float positionX = adjacentPlatformObject.transform.position.x;
        float positionZ = adjacentPlatformObject.transform.position.z;
        Quaternion rotation = energyGeneratorBlue.transform.rotation;
        Vector3 position = new Vector3(positionX, positionY, positionZ);
        GameObject newEnergyGeneratorBlue = Instantiate(energyGeneratorBlue, position, rotation);
        newEnergyGeneratorBlue.name = "EnergyGenerator_Blue" + adjacentPlatformNum;

        // Play sound effect
        var buildObject = GameObject.Find("Build");
        var audioSource = buildObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);

        // Subtract energy
        gameControl = GameObject.FindObjectOfType<GameControl>();
        gameControl.energyCount -= adjacentBuildPanel.energyGeneratorCost;

        // Make panel invisible
        adjacentPlatform.isBuildingOnTop = true;
        canvasObject = GameObject.Find("Canvas");
        canvasInfo = canvasObject.GetComponent<CanvasInfo>();
        adjacentBuildPanelObject = canvasInfo.adjacentBuildPanelObject;
        adjacentBuildPanelObject.SetActive(false);

        // Stop selection particle effect
        var particleGameObject = adjacentPlatformObject.transform.GetChild(0).gameObject;
        var platformParticleSystem = particleGameObject.GetComponent<ParticleSystem>();
        platformParticleSystem.Stop();

        Debug.Log("Button Clicked!");
    }
}
