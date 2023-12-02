using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyGeneratorUpgraded : MonoBehaviour
{
    private int secondsToWait = 10;
    private int energyIncrementAmount = 75;

    GameControl gameControl;

    // Start is called before the first frame update
    void Start()
    {
        gameControl = GameObject.FindObjectOfType<GameControl>();
        StartCoroutine(Incremental());
    }

    IEnumerator Incremental()
    {
        while (true)
        {
            // Wait for specified number of seconds
            yield return new WaitForSeconds(secondsToWait);

            //Increment energyCount
            IncrementEnergyCount();
        }
    }

    void IncrementEnergyCount()
    {
        gameControl.energyCount += energyIncrementAmount;
    }
}
