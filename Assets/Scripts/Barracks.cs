using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Barracks : MonoBehaviour
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

    private float delayTimeSpawn = 25.0f;

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
                if (gameObject.name.Contains("InfantryGroup"))
                {
                    var infantryGroup = gameObject.GetComponent<InfantryGroup>();
                    var infantryGroupAssociatedBuilding = infantryGroup.associatedBuilding;

                    if (infantryGroupAssociatedBuilding != "")
                    {
                        var associatedBuildingNumber = infantryGroupAssociatedBuilding.Substring(infantryGroupAssociatedBuilding.Length - 1);

                        if (associatedBuildingNumber.ToString() == buildingNumber)
                        {
                            infantryGroup.associatedBuilding = this.gameObject.name;
                        }
                    }
                }
            }
        }

        // Spawn infantry
        if (this.name.Contains("Barracks_Blue"))
        {
            StartCoroutine(SpawnBlueInfantry(delayTimeSpawn));
        }
        else if (this.name.Contains("Barracks_Green"))
        {
            StartCoroutine(SpawnGreenInfantry(delayTimeSpawn));
        }
        else if (this.name.Contains("Barracks_Red"))
        {
            StartCoroutine(SpawnRedInfantry(delayTimeSpawn));
        }
        else if (this.name.Contains("Barracks_Orange"))
        {
            StartCoroutine(SpawnOrangeInfantry(delayTimeSpawn));
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

    IEnumerator DelayAction(float delayTimeBuild)
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
        yield return new WaitForSeconds(delayTimeBuild);

        // Make upgrade panel visible
        upgradePanelObject.SetActive(true);

        // Start selection particle effect of newly clicked building
        selectionParticleSystem.Play();

        Debug.Log("Building Clicked!");
    }

    IEnumerator SpawnBlueInfantry(float delayTimeSpawn)
    {
        yield return new WaitForSeconds(delayTimeSpawn);

        if (shouldSpawn)
        {
            string prefabPath = "Prefabs/InfantryGroup_Blue";
            var infantryBluePrefab = Resources.Load(prefabPath, typeof(GameObject)) as GameObject;
            SpawnInfantry(infantryBluePrefab);
        }
        
        StartCoroutine(SpawnBlueInfantry(delayTimeSpawn));
    }

    IEnumerator SpawnGreenInfantry(float delayTimeSpawn)
    {
        yield return new WaitForSeconds(delayTimeSpawn);

        if (shouldSpawn)
        {
            string prefabPath = "Prefabs/InfantryGroup_Green";
            var infantryGreenPrefab = Resources.Load(prefabPath, typeof(GameObject)) as GameObject;
            SpawnInfantry(infantryGreenPrefab);
        }

        StartCoroutine(SpawnGreenInfantry(delayTimeSpawn));
    }

    IEnumerator SpawnRedInfantry(float delayTimeSpawn)
    {
        yield return new WaitForSeconds(delayTimeSpawn);

        if (shouldSpawn)
        {
            string prefabPath = "Prefabs/InfantryGroup_Red";
            var infantryRedPrefab = Resources.Load(prefabPath, typeof(GameObject)) as GameObject;
            SpawnInfantry(infantryRedPrefab);
        }

        StartCoroutine(SpawnRedInfantry(delayTimeSpawn));
    }

    IEnumerator SpawnOrangeInfantry(float delayTimeSpawn)
    {
        yield return new WaitForSeconds(delayTimeSpawn);

        if (shouldSpawn)
        {
            string prefabPath = "Prefabs/InfantryGroup_Orange";
            var infantryOrangePrefab = Resources.Load(prefabPath, typeof(GameObject)) as GameObject;
            SpawnInfantry(infantryOrangePrefab);
        }

        StartCoroutine(SpawnOrangeInfantry(delayTimeSpawn));
    }

    void SpawnInfantry(GameObject prefab)
    {
        float positionY = prefab.transform.position.y;
        Quaternion rotation = prefab.transform.rotation;
        float positionX = 0.0f;
        float positionZ = 0.0f;
        Vector3 position;

        if (this.name.Contains("10"))
        {
            positionX = 757.16f;
            positionZ = 105.05f;
        }
        else if (this.name.Contains("11"))
        {
            positionX = 759.03f;
            positionZ = 107.7f;
        }
        else if (this.name.Contains("12"))
        {
            positionX = 751.6907f;
            positionZ = 97.19416f;
        }
        else if (this.name.Contains("1"))
        {
            positionX = 702.36f;
            positionZ = 145.47f;
        }
        else if (this.name.Contains("2"))
        {
            positionX = 700.5515f;
            positionZ = 142.8808f;
        }
        else if (this.name.Contains("3"))
        {
            positionX = 698.7601f;
            positionZ = 140.301f;
        }
        else if (this.name.Contains("4"))
        {
            positionX = 696.95f;
            positionZ = 137.67f;
        }
        else if (this.name.Contains("5"))
        {
            positionX = 695.1f;
            positionZ = 135f;
        }
        else if (this.name.Contains("6"))
        {
            positionX = 704.1932f;
            positionZ = 148.1242f;
        }
        else if (this.name.Contains("7"))
        {
            positionX = 749.8207f;
            positionZ = 94.48415f;
        }
        else if (this.name.Contains("8"))
        {
            positionX = 753.51f;
            positionZ = 99.79f;
        }
        else if (this.name.Contains("9"))
        {
            positionX = 755.3f;
            positionZ = 102.41f;
        }

        position = new Vector3(positionX, positionY, positionZ);
        var infantryGroupObject = Instantiate(prefab, position, rotation);
        var infantryGroup = infantryGroupObject.GetComponent<InfantryGroup>();
        infantryGroup.associatedBuilding = this.gameObject.name;
    }
}
