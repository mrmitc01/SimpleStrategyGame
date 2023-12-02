using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePanel : MonoBehaviour
{
    GameObject panelObject;

    public void Close()
    {
        panelObject = this.transform.parent.gameObject;

        if (panelObject.name == "BuildPanel")
        {
            // Make BuildPanel invisible
            panelObject.SetActive(false);

            // Stop selection particle effect
            var platformName = "Platform" + panelObject.GetComponent<BuildPanel>().platformNum;
            var platformObject = GameObject.Find(platformName);
            var particleGameObject = platformObject.transform.GetChild(0).gameObject;
            var platformParticleSystem = particleGameObject.GetComponent<ParticleSystem>();
            platformParticleSystem.Stop();
        }

        else if (panelObject.name == "AdjacentBuildPanel")
        {
            // Make AdjacentBuildPanel invisible
            panelObject.SetActive(false);

            // Stop selection particle effect
            var adjacentPlatformName = "Platform_Adjacent" + panelObject.GetComponent<AdjacentBuildPanel>().adjacentPlatformNum;
            var adjacentPlatformObject = GameObject.Find(adjacentPlatformName);
            var particleGameObject = adjacentPlatformObject.transform.GetChild(0).gameObject;
            var adjacentPlatformParticleSystem = particleGameObject.GetComponent<ParticleSystem>();
            adjacentPlatformParticleSystem.Stop();
        }

        else if (panelObject.name == "UpgradePanel")
        {
            // Make UpgradePanel invisible
            panelObject.SetActive(false);

            // Stop selection particle effect
            var buildingFullName = panelObject.GetComponent<UpgradePanel>().buildingFullName;
            var buildingObject = GameObject.Find(buildingFullName);
            var particleGameObject = buildingObject.transform.GetChild(0).gameObject;
            var buildingParticleSystem = particleGameObject.GetComponent<ParticleSystem>();
            buildingParticleSystem.Stop();
        }
    }
}
