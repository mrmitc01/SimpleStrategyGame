using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VehicleFactory : MonoBehaviour
{
    public bool isUpgraded = false;
    public bool shouldSpawn = true;
    public Material hoverMaterial;

    MeshRenderer[] meshRenderers;
    List<Material> startMaterials = new List<Material>();

    float delayTimeBuild = 0.25f;
    string buildingFullName;
    string buildingName;
    string buildingNumber;

    GameObject canvasObject;
    CanvasInfo canvasInfo;

    GameObject buildPanelObject;
    BuildPanel buildPanel;

    GameObject adjacentBuildPanelObject;
    AdjacentBuildPanel adjacentBuildPanel;

    GameObject upgradePanelObject;
    UpgradePanel upgradePanel;

    GameObject particleGameObject;
    private ParticleSystem selectionParticleSystem;
    private float delayTimeSpawn = 35.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Fetch all child mesh renderer components from the GameObject
        meshRenderers = GetComponentsInChildren<MeshRenderer>();

        // Fetch the original material for all children of the GameObject
        foreach (var meshRenderer in meshRenderers)
        {
            startMaterials.Add(meshRenderer.material);
        }

        if (isUpgraded)
        {
            buildingNumber = this.name.Substring(this.name.Length - 1);

            // Get root objects in scene
            List<GameObject> rootObjects = new List<GameObject>();
            Scene scene = SceneManager.GetActiveScene();
            scene.GetRootGameObjects(rootObjects);

            // Iterate over all root objects
            for (int i = 0; i < rootObjects.Count; ++i)
            {
                GameObject gameObject = rootObjects[i];
                if (gameObject.name.Contains("Vehicle_"))
                {
                    var vehicle = gameObject.GetComponent<Vehicle>();
                    var vehicleAssociatedBuilding = vehicle.associatedBuilding;

                    if (vehicleAssociatedBuilding != "")
                    {
                        var associatedBuildingNumber = vehicleAssociatedBuilding.Substring(vehicleAssociatedBuilding.Length - 1);

                        if (associatedBuildingNumber.ToString() == buildingNumber)
                        {
                            vehicle.associatedBuilding = this.gameObject.name;
                        }
                    }
                }
            }
        }

        // Spawn vehicles
        if (this.name.Contains("VehicleFactory_Blue"))
        {
            StartCoroutine(SpawnBlueVehicle(delayTimeSpawn));
        }
        else if (this.name.Contains("VehicleFactory_Green"))
        {
            StartCoroutine(SpawnGreenVehicle(delayTimeSpawn));
        }
        else if (this.name.Contains("VehicleFactory_Red"))
        {
            StartCoroutine(SpawnRedVehicle(delayTimeSpawn));
        }
        else if (this.name.Contains("VehicleFactory_Orange"))
        {
            StartCoroutine(SpawnOrangeVehicle(delayTimeSpawn));
        }
    }

    void OnMouseOver()
    {
        if (!isUpgraded)
        {
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.material = hoverMaterial;
            }
        }
    }

    void OnMouseExit()
    {
        if (!isUpgraded)
        {
            var startMaterialsArray = startMaterials.ToArray();
            int i = 0;
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.material = startMaterialsArray[i];
                i++;
            }
        }
    }

    private void OnMouseUpAsButton()
    {
        if (!isUpgraded)
        {
            canvasObject = GameObject.Find("Canvas");
            canvasInfo = canvasObject.GetComponent<CanvasInfo>();

            buildPanelObject = canvasInfo.buildPanelObject;
            buildPanel = buildPanelObject.GetComponent<BuildPanel>();

            adjacentBuildPanelObject = canvasInfo.adjacentBuildPanelObject;
            adjacentBuildPanel = adjacentBuildPanelObject.GetComponent<AdjacentBuildPanel>();

            upgradePanelObject = canvasInfo.upgradePanelObject;
            upgradePanel = upgradePanelObject.GetComponent<UpgradePanel>();

            particleGameObject = this.transform.GetChild(0).gameObject;
            selectionParticleSystem = particleGameObject.GetComponent<ParticleSystem>();

            buildingFullName = this.name;
            buildingName = this.name.Substring(0, this.name.Length - 6);
            buildingNumber = this.name.Substring(this.name.Length - 1);

            if (upgradePanelObject.activeSelf == true && upgradePanel.buildingFullName != buildingFullName)
            {
                StartCoroutine(DelayAction(delayTimeBuild));
            }
            else if (upgradePanelObject.activeSelf == true && upgradePanel.buildingFullName == buildingFullName)
            {
                // Make upgrade panel invisible
                upgradePanelObject.SetActive(false);

                // Stop selection particle effect
                selectionParticleSystem.Stop();
            }
            else if (buildPanelObject.activeSelf)
            {
                // Make build panel invisible
                buildPanelObject.SetActive(false);

                // Stop selection particle effect of previously clicked platform
                var oldPlatform = GameObject.Find("Platform" + buildPanel.platformNum);
                var oldParticleGameObject = oldPlatform.transform.GetChild(0).gameObject;
                var oldPlatformParticleSystem = oldParticleGameObject.GetComponent<ParticleSystem>();
                oldPlatformParticleSystem.Stop();

                upgradePanel.buildingFullName = buildingFullName;
                upgradePanel.buildingNumber = buildingNumber;
                upgradePanel.buildingName = buildingName;

                // Make upgrade panel visible
                upgradePanelObject.SetActive(true);

                // Start selection particle effect of newly clicked building
                selectionParticleSystem.Play();

                Debug.Log("Building Clicked!");
            }
            else if (adjacentBuildPanelObject.activeSelf)
            {
                // Make adjacent build panel invisible
                adjacentBuildPanelObject.SetActive(false);

                // Stop selection particle effect of previously clicked adjacent platform
                var oldPlatform = GameObject.Find("Platform_Adjacent" + adjacentBuildPanel.adjacentPlatformNum);
                var oldParticleGameObject = oldPlatform.transform.GetChild(0).gameObject;
                var oldPlatformParticleSystem = oldParticleGameObject.GetComponent<ParticleSystem>();
                oldPlatformParticleSystem.Stop();

                upgradePanel.buildingFullName = buildingFullName;
                upgradePanel.buildingNumber = buildingNumber;
                upgradePanel.buildingName = buildingName;

                // Make upgrade panel visible
                upgradePanelObject.SetActive(true);

                // Start selection particle effect of newly clicked building
                selectionParticleSystem.Play();

                Debug.Log("Building Clicked!");
            }
            // Else no panel at all is selected
            else
            {
                upgradePanel.buildingFullName = buildingFullName;
                upgradePanel.buildingNumber = buildingNumber;
                upgradePanel.buildingName = buildingName;

                // Make upgrade panel visible
                upgradePanelObject.SetActive(true);

                // Start selection particle effect
                selectionParticleSystem.Play();

                Debug.Log("Building Clicked!");
            }
        }
    }

    IEnumerator DelayAction(float delayTime)
    {
        string oldbuildingName = upgradePanel.buildingFullName;
        upgradePanel.buildingFullName = buildingFullName;
        upgradePanel.buildingNumber = buildingNumber;
        upgradePanel.buildingName = buildingName;

        // Make upgrade panel invisible
        upgradePanelObject.SetActive(false);

        // Stop selection particle effect of previously clicked building
        var oldBuilding = GameObject.Find(oldbuildingName);
        var oldParticleGameObject = oldBuilding.transform.GetChild(0).gameObject;
        var oldBuildingParticleSystem = oldParticleGameObject.GetComponent<ParticleSystem>();
        oldBuildingParticleSystem.Stop();

        // Wait for the specified delay time before continuing
        yield return new WaitForSeconds(delayTime);

        // Make upgrade panel visible
        upgradePanelObject.SetActive(true);

        // Start selection particle effect of newly clicked building
        selectionParticleSystem.Play();

        Debug.Log("Building Clicked!");
    }

    IEnumerator SpawnBlueVehicle(float delayTimeSpawn)
    {
        yield return new WaitForSeconds(delayTimeSpawn);

        if (shouldSpawn)
        {
            string prefabPath = "Prefabs/Vehicle_Blue";
            var vehicleBluePrefab = Resources.Load(prefabPath, typeof(GameObject)) as GameObject;
            SpawnVehicle(vehicleBluePrefab);
        }

        StartCoroutine(SpawnBlueVehicle(delayTimeSpawn));
    }

    IEnumerator SpawnGreenVehicle(float delayTimeSpawn)
    {
        yield return new WaitForSeconds(delayTimeSpawn);

        if (shouldSpawn)
        {
            string prefabPath = "Prefabs/Vehicle_Green";
            var vehicleGreenPrefab = Resources.Load(prefabPath, typeof(GameObject)) as GameObject;
            SpawnVehicle(vehicleGreenPrefab);
        }

        StartCoroutine(SpawnGreenVehicle(delayTimeSpawn));
    }

    IEnumerator SpawnRedVehicle(float delayTimeSpawn)
    {
        yield return new WaitForSeconds(delayTimeSpawn);

        if (shouldSpawn)
        {
            string prefabPath = "Prefabs/Vehicle_Red";
            var vehicleRedPrefab = Resources.Load(prefabPath, typeof(GameObject)) as GameObject;
            SpawnVehicle(vehicleRedPrefab);
        }

        StartCoroutine(SpawnRedVehicle(delayTimeSpawn));
    }

    IEnumerator SpawnOrangeVehicle(float delayTimeSpawn)
    {
        yield return new WaitForSeconds(delayTimeSpawn);

        if (shouldSpawn)
        {
            string prefabPath = "Prefabs/Vehicle_Orange";
            var vehicleOrangePrefab = Resources.Load(prefabPath, typeof(GameObject)) as GameObject;
            SpawnVehicle(vehicleOrangePrefab);
        }

        StartCoroutine(SpawnOrangeVehicle(delayTimeSpawn));
    }

    void SpawnVehicle(GameObject prefab)
    {
        float positionY = prefab.transform.position.y;
        Quaternion rotation = prefab.transform.rotation;
        float positionX = 0.0f;
        float positionZ = 0.0f;
        Vector3 position;

        if (this.name.Contains("10"))
        {
            positionX = 758.77f;
            positionZ = 106.45f;
        }
        else if (this.name.Contains("11"))
        {
            positionX = 760.66f;
            positionZ = 109.12f;
        }
        else if (this.name.Contains("12"))
        {
            positionX = 753.07f;
            positionZ = 98.33f;
        }
        else if (this.name.Contains("1"))
        {
            positionX = 702.62f;
            positionZ = 144.89f;
        }
        else if (this.name.Contains("2"))
        {
            positionX = 700.78f;
            positionZ = 142.25f;
        }
        else if (this.name.Contains("3"))
        {
            positionX = 698.85f;
            positionZ = 139.44f;
        }
        else if (this.name.Contains("4"))
        {
            positionX = 696.97f;
            positionZ = 136.76f;
        }
        else if (this.name.Contains("5"))
        {
            positionX = 695.1f;
            positionZ = 134.1f;
        }
        else if (this.name.Contains("6"))
        {
            positionX = 704.5f;
            positionZ = 147.56f;
        }
        else if (this.name.Contains("7"))
        {
            positionX = 751.21f;
            positionZ = 95.69f;
        }
        else if (this.name.Contains("8"))
        {
            positionX = 754.96f;
            positionZ = 101.04f;
        }
        else if (this.name.Contains("9"))
        {
            positionX = 756.9f;
            positionZ = 103.82f;
        }

        position = new Vector3(positionX, positionY, positionZ);
        var vehicleObject = Instantiate(prefab, position, rotation);
        var vehicle = vehicleObject.GetComponent<Vehicle>();
        vehicle.associatedBuilding = this.gameObject.name;
    }
}
