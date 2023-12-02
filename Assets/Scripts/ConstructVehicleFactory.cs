using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructVehicleFactory : MonoBehaviour
{
    string platformNum;

    GameControl gameControl;

    BuildPanel buildPanel;
    GameObject platformObject;
    Platform platform;

    GameObject canvasObject;
    CanvasInfo canvasInfo;
    GameObject buildPanelObject;

    public GameObject vehicleFactoryBlue;

    public void BuildVehicleFactory()
    {
        buildPanel = GetComponentInParent<BuildPanel>();
        platformNum = buildPanel.platformNum;
        string platformName = "Platform" + platformNum;

        platformObject = GameObject.Find(platformName);
        platform = platformObject.GetComponent<Platform>();
        Debug.Log("Found Platform" + platformNum);

        // Spawn vehicle factory
        float positionY = vehicleFactoryBlue.transform.position.y;
        float positionX = platformObject.transform.position.x;
        float positionZ = platformObject.transform.position.z;
        Quaternion rotation = vehicleFactoryBlue.transform.rotation;
        Vector3 position = new Vector3(positionX, positionY, positionZ);
        GameObject newVehicleFactoryBlue = Instantiate(vehicleFactoryBlue, position, rotation);
        newVehicleFactoryBlue.name = "VehicleFactory_Blue" + platformNum;

        // Play sound effect
        var buildObject = GameObject.Find("Build");
        var audioSource = buildObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);

        // Subtract energy
        gameControl = GameObject.FindObjectOfType<GameControl>();
        gameControl.energyCount -= buildPanel.vehicleFactoryCost;

        // Make panel invisible
        platform.isBuildingOnTop = true;
        canvasObject = GameObject.Find("Canvas");
        canvasInfo = canvasObject.GetComponent<CanvasInfo>();
        buildPanelObject = canvasInfo.buildPanelObject;
        buildPanelObject.SetActive(false);

        // Stop selection particle effect
        var particleGameObject = platformObject.transform.GetChild(0).gameObject;
        var platformParticleSystem = particleGameObject.GetComponent<ParticleSystem>();
        platformParticleSystem.Stop();

        Debug.Log("Button Clicked!");
    }
}
