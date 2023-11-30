using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySchedule : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // At 5 seconds into the game
        // Build energy generator at adjacent platform 7
        string buildingName = "EnergyGenerator_Red7";
        GameObject prefab = Resources.Load("Prefabs/EnergyGenerator_Red", typeof(GameObject)) as GameObject;
        AdjacentPlatform adjacentPlatform = GameObject.Find("Platform_Adjacent7").GetComponent<AdjacentPlatform>();
        float positionY = prefab.transform.position.y;
        float positionX = adjacentPlatform.transform.position.x;
        float positionZ = adjacentPlatform.transform.position.z;
        Quaternion rotation = prefab.transform.rotation;
        Vector3 position = new Vector3(positionX, positionY, positionZ);
        StartCoroutine(Build(5.0f, buildingName, prefab, position, rotation));

        // At 10 seconds
        // Build energy generator at adjacent platform 8
        buildingName = "EnergyGenerator_Red8";
        prefab = Resources.Load("Prefabs/EnergyGenerator_Red", typeof(GameObject)) as GameObject;
        adjacentPlatform = GameObject.Find("Platform_Adjacent8").GetComponent<AdjacentPlatform>();
        positionY = prefab.transform.position.y;
        positionX = adjacentPlatform.transform.position.x;
        positionZ = adjacentPlatform.transform.position.z;
        rotation = prefab.transform.rotation;
        position = new Vector3(positionX, positionY, positionZ);
        StartCoroutine(Build(10.0f, buildingName, prefab, position, rotation));

        // Wait 40 seconds
        // Build vehicle factory at platform 12
        buildingName = "VehicleFactory_Red12";
        prefab = Resources.Load("Prefabs/VehicleFactory_Red", typeof(GameObject)) as GameObject;
        Platform platform = GameObject.Find("Platform12").GetComponent<Platform>();
        positionY = prefab.transform.position.y;
        positionX = platform.transform.position.x;
        positionZ = platform.transform.position.z;
        rotation = prefab.transform.rotation;
        position = new Vector3(positionX, positionY, positionZ);
        StartCoroutine(Build(40.0f, buildingName, prefab, position, rotation));

        // Wait 50 seconds
        // Build barracks at platform 8
        buildingName = "Barracks_Red8";
        prefab = Resources.Load("Prefabs/Barracks_Red", typeof(GameObject)) as GameObject;
        platform = GameObject.Find("Platform8").GetComponent<Platform>();
        positionY = prefab.transform.position.y;
        positionX = platform.transform.position.x;
        positionZ = platform.transform.position.z;
        rotation = prefab.transform.rotation;
        position = new Vector3(positionX, positionY, positionZ);
        StartCoroutine(Build(50.0f, buildingName, prefab, position, rotation));

        // Wait 80 seconds
        // Build turret at adjacent platform 5
        buildingName = "Turret_Red5";
        prefab = Resources.Load("Prefabs/Turret_Red", typeof(GameObject)) as GameObject;
        adjacentPlatform = GameObject.Find("Platform_Adjacent5").GetComponent<AdjacentPlatform>();
        positionY = prefab.transform.position.y;
        positionX = adjacentPlatform.transform.position.x;
        positionZ = adjacentPlatform.transform.position.z;
        rotation = prefab.transform.rotation;
        position = new Vector3(positionX, positionY, positionZ);
        StartCoroutine(Build(80.0f, buildingName, prefab, position, rotation));

        // Wait 100 seconds
        // Build vehicle factory at platform 9
        buildingName = "VehicleFactory_Red9";
        prefab = Resources.Load("Prefabs/VehicleFactory_Red", typeof(GameObject)) as GameObject;
        platform = GameObject.Find("Platform9").GetComponent<Platform>();
        positionY = prefab.transform.position.y;
        positionX = platform.transform.position.x;
        positionZ = platform.transform.position.z;
        rotation = prefab.transform.rotation;
        position = new Vector3(positionX, positionY, positionZ);
        StartCoroutine(Build(100.0f, buildingName, prefab, position, rotation));

        // Wait 130 seconds
        // Upgrade energy generator at adjacent platform 7
        string oldBuildingName = "EnergyGenerator_Red7";
        string newBuildingName = "EnergyGenerator_Orange7";
        prefab = Resources.Load("Prefabs/EnergyGenerator_Orange", typeof(GameObject)) as GameObject;
        adjacentPlatform = GameObject.Find("Platform_Adjacent7").GetComponent<AdjacentPlatform>();
        positionY = prefab.transform.position.y;
        positionX = adjacentPlatform.transform.position.x;
        positionZ = adjacentPlatform.transform.position.z;
        rotation = prefab.transform.rotation;
        position = new Vector3(positionX, positionY, positionZ);
        StartCoroutine(Upgrade(130.0f, oldBuildingName, newBuildingName, prefab, position, rotation));

        // Wait 150 seconds
        // Build turret at adjacent platform 6
        buildingName = "Turret_Red6";
        prefab = Resources.Load("Prefabs/Turret_Red", typeof(GameObject)) as GameObject;
        adjacentPlatform = GameObject.Find("Platform_Adjacent6").GetComponent<AdjacentPlatform>();
        positionY = prefab.transform.position.y;
        positionX = adjacentPlatform.transform.position.x;
        positionZ = adjacentPlatform.transform.position.z;
        rotation = prefab.transform.rotation;
        position = new Vector3(positionX, positionY, positionZ);
        StartCoroutine(Build(150.0f, buildingName, prefab, position, rotation));

        // Wait 190 seconds
        // Upgrade energy generator at adjacent platform 8
        oldBuildingName = "EnergyGenerator_Red8";
        newBuildingName = "EnergyGenerator_Orange8";
        prefab = Resources.Load("Prefabs/EnergyGenerator_Orange", typeof(GameObject)) as GameObject;
        adjacentPlatform = GameObject.Find("Platform_Adjacent8").GetComponent<AdjacentPlatform>();
        positionY = prefab.transform.position.y;
        positionX = adjacentPlatform.transform.position.x;
        positionZ = adjacentPlatform.transform.position.z;
        rotation = prefab.transform.rotation;
        position = new Vector3(positionX, positionY, positionZ);
        StartCoroutine(Upgrade(190.0f, oldBuildingName, newBuildingName, prefab, position, rotation));

        // Wait 230 seconds
        // Upgrade barracks at platform 8
        oldBuildingName = "Barracks_Red8";
        newBuildingName = "Barracks_Orange8";
        prefab = Resources.Load("Prefabs/Barracks_Orange", typeof(GameObject)) as GameObject;
        platform = GameObject.Find("Platform8").GetComponent<Platform>();
        positionY = prefab.transform.position.y;
        positionX = platform.transform.position.x;
        positionZ = platform.transform.position.z;
        rotation = prefab.transform.rotation;
        position = new Vector3(positionX, positionY, positionZ);
        StartCoroutine(Upgrade(230.0f, oldBuildingName, newBuildingName, prefab, position, rotation));

        // Wait 250 seconds
        // Upgrade vehicle factory at platform 12
        oldBuildingName = "VehicleFactory_Red12";
        newBuildingName = "VehicleFactory_Orange12";
        prefab = Resources.Load("Prefabs/VehicleFactory_Orange", typeof(GameObject)) as GameObject;
        platform = GameObject.Find("Platform12").GetComponent<Platform>();
        positionY = prefab.transform.position.y;
        positionX = platform.transform.position.x;
        positionZ = platform.transform.position.z;
        rotation = prefab.transform.rotation;
        position = new Vector3(positionX, positionY, positionZ);
        StartCoroutine(Upgrade(250.0f, oldBuildingName, newBuildingName, prefab, position, rotation));

        // Wait 270 seconds
        // Build barracks at platform 11
        buildingName = "Barracks_Red11";
        prefab = Resources.Load("Prefabs/Barracks_Red", typeof(GameObject)) as GameObject;
        platform = GameObject.Find("Platform11").GetComponent<Platform>();
        positionY = prefab.transform.position.y;
        positionX = platform.transform.position.x;
        positionZ = platform.transform.position.z;
        rotation = prefab.transform.rotation;
        position = new Vector3(positionX, positionY, positionZ);
        StartCoroutine(Build(270.0f, buildingName, prefab, position, rotation));
    }

    // Update is called once per frame
    void Update()
    {
        // Check if EnemyHQ exists, if it doesn't set a bool to false
        // This bool will be used to stop all coroutines
    }

    private IEnumerator Build(float delayTime, string buildingName, GameObject prefab, Vector3 position, Quaternion rotation)
    {
        yield return new WaitForSeconds(delayTime);

        //var buildingCheck = GameObject.Find("buildingName");
        GameObject building = Instantiate(prefab, position, rotation);
        building.name = buildingName;

        // Play sound effect
        var buildObject = GameObject.Find("Build");
        var audioSource = buildObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);

        Debug.Log("Built " + buildingName + " after " + delayTime + " seconds");
    }

    private IEnumerator Upgrade(float delayTime, string oldBuildingName, string newBuildingName, GameObject prefab, Vector3 position, Quaternion rotation)
    {
        yield return new WaitForSeconds(delayTime);

        GameObject oldBuilding = GameObject.Find(oldBuildingName);
        Destroy(oldBuilding);

        GameObject building = Instantiate(prefab, position, rotation);
        building.name = newBuildingName;

        // Play sound effect
        var buildObject = GameObject.Find("Build");
        var audioSource = buildObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);

        Debug.Log("Built " + newBuildingName + " after " + delayTime + " seconds");
    }
}
