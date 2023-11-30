using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjacentBuildPanel : MonoBehaviour
{
    public string adjacentPlatformNum = "0";
    public int turretCost = 250;
    public int energyGeneratorCost = 500;

    Button turretButton;
    Button energyGeneratorButton;
    GameControl gameControl;

    // Start is called before the first frame update
    void Start()
    {
        turretButton = this.transform.GetChild(1).GetComponent<Button>();
        energyGeneratorButton = this.transform.GetChild(2).GetComponent<Button>();
        gameControl = GameObject.FindObjectOfType<GameControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameControl.energyCount < turretCost && turretButton.interactable)
        {
            turretButton.interactable = false;
        }
        else if (gameControl.energyCount >= turretCost && !turretButton.interactable)
        {
            turretButton.interactable = true;
        }

        if (gameControl.energyCount < energyGeneratorCost && energyGeneratorButton.interactable)
        {
            energyGeneratorButton.interactable = false;
        }
        else if (gameControl.energyCount >= energyGeneratorCost && !energyGeneratorButton.interactable)
        {
            energyGeneratorButton.interactable = true;
        }
    }
}
