using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBuilding : MonoBehaviour
{
    string buildingFullName;

    GameControl gameControl;

    UpgradePanel upgradePanel;
    GameObject buildingObject;

    GameObject canvasObject;
    CanvasInfo canvasInfo;
    GameObject upgradePanelObject;

    public void UpgradeExistingBuilding()
    {
        upgradePanel = GetComponentInParent<UpgradePanel>();
        buildingFullName = upgradePanel.buildingFullName;

        buildingObject = GameObject.Find(buildingFullName);
        Debug.Log("Found " + buildingFullName);

        if (buildingFullName.Contains("EnergyGenerator"))
        {
            var building = buildingObject.GetComponent<EnergyGenerator>();

            // Destroy original building and spawn upgraded building
            string prefabPath = "Prefabs/" + upgradePanel.buildingName + "_Green";
            var buildingGreenPrefab = Resources.Load(prefabPath, typeof(GameObject)) as GameObject;
            float positionY = buildingGreenPrefab.transform.position.y;
            float positionX = buildingObject.transform.position.x;
            float positionZ = buildingObject.transform.position.z;
            Quaternion rotation = buildingGreenPrefab.transform.rotation;
            Vector3 position = new Vector3(positionX, positionY, positionZ);
            Object.Destroy(buildingObject);
            GameObject newBuildingGreen = Instantiate(buildingGreenPrefab, position, rotation);
            newBuildingGreen.name = upgradePanel.buildingName + "_Green" + upgradePanel.buildingNumber;

            // Stop selection particle effect
            var particleGameObject = building.transform.GetChild(0).gameObject;
            var platformParticleSystem = particleGameObject.GetComponent<ParticleSystem>();
            platformParticleSystem.Stop();
        }
        else if (buildingFullName.Contains("VehicleFactory"))
        {
            var building = buildingObject.GetComponent<VehicleFactory>();

            // Destroy original building and spawn upgraded building
            string prefabPath = "Prefabs/" + upgradePanel.buildingName + "_Green";
            var buildingGreenPrefab = Resources.Load(prefabPath, typeof(GameObject)) as GameObject;
            float positionY = buildingGreenPrefab.transform.position.y;
            float positionX = buildingObject.transform.position.x;
            float positionZ = buildingObject.transform.position.z;
            Quaternion rotation = buildingGreenPrefab.transform.rotation;
            Vector3 position = new Vector3(positionX, positionY, positionZ);
            Object.Destroy(buildingObject);
            GameObject newBuildingGreen = Instantiate(buildingGreenPrefab, position, rotation);
            newBuildingGreen.name = upgradePanel.buildingName + "_Green" + upgradePanel.buildingNumber;

            // Stop selection particle effect
            var particleGameObject = building.transform.GetChild(0).gameObject;
            var platformParticleSystem = particleGameObject.GetComponent<ParticleSystem>();
            platformParticleSystem.Stop();
        }
        else if (buildingFullName.Contains("Barracks"))
        {
            var building = buildingObject.GetComponent<Barracks>();

            // Destroy original building and spawn upgraded building
            string prefabPath = "Prefabs/" + upgradePanel.buildingName + "_Green";
            var buildingGreenPrefab = Resources.Load(prefabPath, typeof(GameObject)) as GameObject;
            float positionY = buildingGreenPrefab.transform.position.y;
            float positionX = buildingObject.transform.position.x;
            float positionZ = buildingObject.transform.position.z;
            Quaternion rotation = buildingGreenPrefab.transform.rotation;
            Vector3 position = new Vector3(positionX, positionY, positionZ);
            Object.Destroy(buildingObject);
            GameObject newBuildingGreen = Instantiate(buildingGreenPrefab, position, rotation);
            newBuildingGreen.name = upgradePanel.buildingName + "_Green" + upgradePanel.buildingNumber;

            // Stop selection particle effect
            var particleGameObject = building.transform.GetChild(0).gameObject;
            var platformParticleSystem = particleGameObject.GetComponent<ParticleSystem>();
            platformParticleSystem.Stop();
        }
        else if (buildingFullName.Contains("Turret"))
        {
            var building = buildingObject.GetComponent<Turret>();

            // Destroy original building and spawn upgraded building
            string prefabPath = "Prefabs/" + upgradePanel.buildingName + "_Green";
            var buildingGreenPrefab = Resources.Load(prefabPath, typeof(GameObject)) as GameObject;
            float positionY = buildingGreenPrefab.transform.position.y;
            float positionX = buildingObject.transform.position.x;
            float positionZ = buildingObject.transform.position.z;
            Quaternion rotation = buildingGreenPrefab.transform.rotation;
            Vector3 position = new Vector3(positionX, positionY, positionZ);
            Object.Destroy(buildingObject);
            GameObject newBuildingGreen = Instantiate(buildingGreenPrefab, position, rotation);
            newBuildingGreen.name = upgradePanel.buildingName + "_Green" + upgradePanel.buildingNumber;

            // Stop selection particle effect
            var particleGameObject = building.transform.GetChild(0).gameObject;
            var platformParticleSystem = particleGameObject.GetComponent<ParticleSystem>();
            platformParticleSystem.Stop();
        }

        // Play sound effect
        var buildObject = GameObject.Find("Build");
        var audioSource = buildObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);

        // Subtract energy
        gameControl = GameObject.FindObjectOfType<GameControl>();
        gameControl.energyCount -= upgradePanel.upgradeCost;

        // Make panel invisible
        canvasObject = GameObject.Find("Canvas");
        canvasInfo = canvasObject.GetComponent<CanvasInfo>();
        upgradePanelObject = canvasInfo.upgradePanelObject;
        upgradePanelObject.SetActive(false);

        Debug.Log("Button Clicked!");
    }
}
