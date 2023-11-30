using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildPanel : MonoBehaviour
{
    public string platformNum = "0";
    public int barracksCost = 250;
    public int vehicleFactoryCost = 500;

    Button barracksButton;
    Button vehicleFactoryButton;
    GameControl gameControl;

    // Start is called before the first frame update
    void Start()
    {
        barracksButton = this.transform.GetChild(1).GetComponent<Button>();
        vehicleFactoryButton = this.transform.GetChild(2).GetComponent<Button>();
        gameControl = GameObject.FindObjectOfType<GameControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameControl.energyCount < barracksCost && barracksButton.interactable)
        {
            barracksButton.interactable = false;
        }
        else if (gameControl.energyCount >= barracksCost && !barracksButton.interactable)
        {
            barracksButton.interactable = true;
        }

        if (gameControl.energyCount < vehicleFactoryCost && vehicleFactoryButton.interactable)
        {
            vehicleFactoryButton.interactable = false;
        }
        else if (gameControl.energyCount >= vehicleFactoryCost && !vehicleFactoryButton.interactable)
        {
            vehicleFactoryButton.interactable = true;
        }
    }
}
