using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public bool isUpgraded = false;
    public Material hoverMaterial;

    MeshRenderer[] meshRenderers;
    List<Material> startMaterials = new List<Material>();

    float delayTime = 0.25f;
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
                StartCoroutine(DelayAction(delayTime));
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
}
