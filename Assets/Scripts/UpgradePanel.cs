using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    // Example: Barracks, VehicleFactory, Turret, EnergyGenerator
    public string buildingName = "";
    public string buildingNumber = "";
    public string buildingFullName = "";
    public int upgradeCost = 750;

    Button upgradeButton;
    GameControl gameControl;

    // Start is called before the first frame update
    void Start()
    {
        upgradeButton = this.transform.GetChild(1).GetComponent<Button>();
        gameControl = GameObject.FindObjectOfType<GameControl>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (gameControl.energyCount < upgradeCost && upgradeButton.interactable)
        {
            upgradeButton.interactable = false;
        }
        else if (gameControl.energyCount >= upgradeCost && !upgradeButton.interactable)
        {
            upgradeButton.interactable = true;
        }
    }
}
