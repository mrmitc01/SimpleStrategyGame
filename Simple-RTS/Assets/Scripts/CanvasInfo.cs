using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInfo : MonoBehaviour
{
    public GameObject buildPanelObject;
    public GameObject adjacentBuildPanelObject;
    public GameObject upgradePanelObject;
    public GameObject victoryObject;
    public GameObject defeatObject;

    // Start is called before the first frame update
    void Start()
    {
        buildPanelObject = GameObject.Find("BuildPanel");
        buildPanelObject.SetActive(false);

        adjacentBuildPanelObject = GameObject.Find("AdjacentBuildPanel");
        adjacentBuildPanelObject.SetActive(false);

        upgradePanelObject = GameObject.Find("UpgradePanel");
        upgradePanelObject.SetActive(false);

        victoryObject = this.gameObject.transform.GetChild(6).gameObject;
        victoryObject.SetActive(false);

        defeatObject = this.gameObject.transform.GetChild(7).gameObject;
        defeatObject.SetActive(false);
    }
}
