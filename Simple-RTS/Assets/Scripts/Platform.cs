using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool isBuildingOnTop = false;
    public Material hoverMaterial;

    Material startMaterial;
    MeshRenderer meshRenderer;

    float delayTime = 0.25f;
    string platformNum;

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

    void Start()
    {
        //Fetch the mesh renderer component from the GameObject
        meshRenderer = GetComponent<MeshRenderer>();

        //Fetch the original material of the GameObject
        startMaterial = meshRenderer.material;
    }

    void OnMouseOver()
    {
        if (!isBuildingOnTop)
        {
            meshRenderer.material = hoverMaterial;
        }
    }

    void OnMouseExit()
    {
        if (!isBuildingOnTop)
        {
            meshRenderer.material = startMaterial;
        }
    }

    private void OnMouseUpAsButton()
    {
        if (!isBuildingOnTop)
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

            platformNum = this.name.Substring(this.name.Length - 1);

            if (buildPanelObject.activeSelf == true && buildPanel.platformNum != platformNum)
            {
                StartCoroutine(DelayAction(delayTime));
            }
            else if (buildPanelObject.activeSelf == true && buildPanel.platformNum == platformNum)
            {
                // Make build panel invisible
                buildPanelObject.SetActive(false);

                // Stop selection particle effect
                selectionParticleSystem.Stop();
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

                buildPanel.platformNum = platformNum;

                // Make build panel visible
                buildPanelObject.SetActive(true);

                // Start selection particle effect of newly clicked platform
                selectionParticleSystem.Play();

                Debug.Log("Platform Clicked!");
            }
            else if (upgradePanelObject.activeSelf)
            {
                // Make upgrade panel invisible
                upgradePanelObject.SetActive(false);

                // Stop selection particle effect of previously clicked building
                var oldBuilding = GameObject.Find(upgradePanel.buildingFullName);
                var oldParticleGameObject = oldBuilding.transform.GetChild(0).gameObject;
                var oldBuildingParticleSystem = oldParticleGameObject.GetComponent<ParticleSystem>();
                oldBuildingParticleSystem.Stop();

                buildPanel.platformNum = platformNum;

                // Make build panel visible
                buildPanelObject.SetActive(true);

                // Start selection particle effect of newly clicked platform
                selectionParticleSystem.Play();

                Debug.Log("Platform Clicked!");
            }
            // Else no panel at all is selected
            else
            {
                buildPanel.platformNum = platformNum;

                // Make build panel visible
                buildPanelObject.SetActive(true);

                // Start selection particle effect
                selectionParticleSystem.Play();

                Debug.Log("Platform Clicked!");
            }
        }
    }

    IEnumerator DelayAction(float delayTime)
    {
        string oldPlatformNum = buildPanel.platformNum;
        buildPanel.platformNum = platformNum;

        // Make build panel invisible
        buildPanelObject.SetActive(false);

        // Stop selection particle effect of previously clicked platform
        var oldPlatform = GameObject.Find("Platform" + oldPlatformNum);
        var oldParticleGameObject = oldPlatform.transform.GetChild(0).gameObject;
        var oldPlatformParticleSystem = oldParticleGameObject.GetComponent<ParticleSystem>();
        oldPlatformParticleSystem.Stop();

        // Wait for the specified delay time before continuing
        yield return new WaitForSeconds(delayTime);

        // Make build panel visible
        buildPanelObject.SetActive(true);

        // Start selection particle effect of newly clicked platform
        selectionParticleSystem.Play();

        Debug.Log("Platform Clicked!");
    }
}
