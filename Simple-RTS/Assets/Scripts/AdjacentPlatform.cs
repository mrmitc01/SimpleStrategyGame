using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjacentPlatform : MonoBehaviour
{
    public bool isBuildingOnTop = false;
    public Material hoverMaterial;

    Material startMaterial;
    MeshRenderer meshRenderer;

    float delayTime = 0.25f;
    string adjacentPlatformNum;

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

            adjacentPlatformNum = this.name.Substring(this.name.Length - 1);

            if (adjacentBuildPanelObject.activeSelf == true && adjacentBuildPanel.adjacentPlatformNum != adjacentPlatformNum)
            {
                StartCoroutine(DelayAction(delayTime));
            }
            else if (adjacentBuildPanelObject.activeSelf == true && adjacentBuildPanel.adjacentPlatformNum == adjacentPlatformNum)
            {
                // Make adjacent build panel invisible
                adjacentBuildPanelObject.SetActive(false);

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

                adjacentBuildPanel.adjacentPlatformNum = adjacentPlatformNum;

                // Make adjacent build panel visible
                adjacentBuildPanelObject.SetActive(true);

                // Start selection particle effect of newly clicked adjacent platform
                selectionParticleSystem.Play();

                Debug.Log("Adjacent Platform Clicked!");
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

                adjacentBuildPanel.adjacentPlatformNum = adjacentPlatformNum;

                // Make adjacent build panel visible
                adjacentBuildPanelObject.SetActive(true);

                // Start selection particle effect of newly clicked adjacent platform
                selectionParticleSystem.Play();

                Debug.Log("Adjacent Platform Clicked!");
            }
            // Else no panel at all is selected
            else
            {
                adjacentBuildPanel.adjacentPlatformNum = adjacentPlatformNum;

                // Make build panel visible
                adjacentBuildPanelObject.SetActive(true);

                // Start selection particle effect
                selectionParticleSystem.Play();

                Debug.Log("Adjacent Platform Clicked!");
            }
        }
    }

    IEnumerator DelayAction(float delayTime)
    {
        string oldAdjacentPlatformNum = adjacentBuildPanel.adjacentPlatformNum;
        adjacentBuildPanel.adjacentPlatformNum = adjacentPlatformNum;

        // Make adjacent build panel invisible
        adjacentBuildPanelObject.SetActive(false);

        // Stop selection particle effect of previously clicked adjacent platform
        var oldAdjacentPlatform = GameObject.Find("Platform_Adjacent" + oldAdjacentPlatformNum);
        var oldParticleGameObject = oldAdjacentPlatform.transform.GetChild(0).gameObject;
        var oldAdjacentPlatformParticleSystem = oldParticleGameObject.GetComponent<ParticleSystem>();
        oldAdjacentPlatformParticleSystem.Stop();

        // Wait for the specified delay time before continuing
        yield return new WaitForSeconds(delayTime);

        // Make adjacent build panel visible
        adjacentBuildPanelObject.SetActive(true);

        // Start selection particle effect of newly clicked adjacent platform
        selectionParticleSystem.Play();

        Debug.Log("Adjacent Platform Clicked!");
    }
}
