using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfantryGroup : MonoBehaviour
{
    public bool isWalking = true;
    public bool isShooting = false;
    public bool isDead = false;
    public bool isDeadAndMoved = false;
    public bool isCloseToOpposingHQ = false;
    public string associatedBuilding = "";
    public float damage = 0.75f;
    public float health = 5.0f;
    public Vector3 positionOpposingHQ;
    public Vector3 positionTarget;

    public GameObject opposingHQ;
    public GameObject target;

    GameObject allyHQ;
    private Rigidbody rigidBody;
    private Barracks barracks;
    private Barracks upgradedBarracks;

    private Ray opposingHQRay;
    private Ray allyHQRay;

    private float normalHealth = 5.0f;
    private float upgradeHealth = 10.0f;
    private float normalDamage = 0.042f;
    private float upgradeDamage = 0.084f;
    private float infantryGroupSpeed = 3.0f;
    private float deathDelay = 4.5f;
    private float distance;
    private int minDistanceFromAllyHQ = 8;
    private int minDistanceFromOpposingHQ = 10;
    private bool hasAlreadyFoundUpgradedBarracks = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.velocity = transform.forward * infantryGroupSpeed;
        if (this.name.Contains("Blue") || this.name.Contains("Green"))
        {
            opposingHQ = GameObject.Find("EnemyHQ");
            allyHQ = GameObject.Find("PlayerHQ");
            positionOpposingHQ = opposingHQ.transform.position;
        }
        else if (this.name.Contains("Red") || this.name.Contains("Orange"))
        {
            opposingHQ = GameObject.Find("PlayerHQ");
            allyHQ = GameObject.Find("EnemyHQ");
            positionOpposingHQ = opposingHQ.transform.position;
        }

        var buildingObject = GameObject.Find(associatedBuilding);
        barracks = buildingObject.GetComponent<Barracks>();

        opposingHQRay = new Ray(opposingHQ.transform.position, opposingHQ.transform.forward);
        allyHQRay = new Ray(allyHQ.transform.position, allyHQ.transform.forward);

        if (this.name.Contains("Red") || this.name.Contains("Blue"))
        {
            health = normalHealth;
            damage = normalDamage;
        }
        else if (this.name.Contains("Green") || this.name.Contains("Orange"))
        {
            health = upgradeHealth;
            damage = upgradeDamage;
        }

        // Move object off screen and destroy it
        StartCoroutine(MoveAndDestroy(deathDelay));
    }

    // Update is called once per frame
    void Update()
    {
        if (this.health <= 0)
        {
            isDead = true;
        }

        distance = Vector3.Cross(opposingHQRay.direction, this.transform.position - opposingHQRay.origin).magnitude;
        if (distance < minDistanceFromOpposingHQ)
        {
            rigidBody.velocity = transform.forward * 0;
            isWalking = false;
            isShooting = true;
            isCloseToOpposingHQ = true;
        }

        float otherDistance = Vector3.Cross(allyHQRay.direction, this.transform.position - allyHQRay.origin).magnitude;
        if (barracks != null)
        {
            if (otherDistance < minDistanceFromAllyHQ)
            {
                barracks.shouldSpawn = false;
            }
            else
            {
                barracks.shouldSpawn = true;
            }
        }
        
        if (!hasAlreadyFoundUpgradedBarracks && barracks == null)
        {
            var buildingObject = GameObject.Find(associatedBuilding);
            upgradedBarracks = buildingObject.GetComponent<Barracks>();
            hasAlreadyFoundUpgradedBarracks = true;
        }

        if (hasAlreadyFoundUpgradedBarracks && upgradedBarracks != null)
        {
            if (otherDistance < minDistanceFromAllyHQ)
            {
                upgradedBarracks.shouldSpawn = false;
            }
            else
            {
                upgradedBarracks.shouldSpawn = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.CompareTag("BlueTeam") && other.gameObject.CompareTag("BlueTeam"))
        {
            rigidBody.velocity = transform.forward * 0;
            isWalking = false;
        }
        else if (this.gameObject.CompareTag("BlueTeam") && other.gameObject.CompareTag("RedTeam") && !isCloseToOpposingHQ)
        {
            isWalking = false;
            isShooting = true;
            rigidBody.velocity = transform.forward * 0;
            if (other.gameObject.transform.parent != null)
            {
                positionTarget = other.gameObject.transform.parent.gameObject.transform.position;
                target = other.gameObject.transform.parent.gameObject;
            }
            else
            {
                positionTarget = other.gameObject.transform.position;
                target = other.gameObject;
            }
        }
        else if (this.gameObject.CompareTag("RedTeam") && other.gameObject.CompareTag("RedTeam"))
        {
            rigidBody.velocity = transform.forward * 0;
            isWalking = false;
        }
        else if (this.gameObject.CompareTag("RedTeam") && other.gameObject.CompareTag("BlueTeam") && !isCloseToOpposingHQ)
        {
            isWalking = false;
            isShooting = true;
            rigidBody.velocity = transform.forward * 0;
            if (other.gameObject.transform.parent != null)
            {
                positionTarget = other.gameObject.transform.parent.gameObject.transform.position;
                target = other.gameObject.transform.parent.gameObject;
            }
            else
            {
                positionTarget = other.gameObject.transform.position;
                target = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (this.gameObject.CompareTag("BlueTeam") && other.gameObject.CompareTag("BlueTeam"))
        {
            rigidBody.velocity = transform.forward * infantryGroupSpeed;
            isWalking = true;
        }
        else if (this.gameObject.CompareTag("BlueTeam") && other.gameObject.CompareTag("RedTeam"))
        {
            isWalking = true;
            isShooting = false;
            rigidBody.velocity = transform.forward * infantryGroupSpeed;
        }
        else if (this.gameObject.CompareTag("RedTeam") && other.gameObject.CompareTag("RedTeam"))
        {
            rigidBody.velocity = transform.forward * infantryGroupSpeed;
            isWalking = true;
        }
        else if (this.gameObject.CompareTag("RedTeam") && other.gameObject.CompareTag("BlueTeam"))
        {
            isWalking = true;
            isShooting = false;
            rigidBody.velocity = transform.forward * infantryGroupSpeed;
        }
    }

    IEnumerator MoveAndDestroy(float deathDelay)
    {
        yield return new WaitForSeconds(deathDelay);

        if (isDead)
        {
            Vector3 location = this.gameObject.transform.position;
            location.y = 200.0f;
            this.gameObject.transform.position = location;
            this.isDeadAndMoved = true;

            Destroy(this.gameObject, 1.0f);
        }

        StartCoroutine(MoveAndDestroy(deathDelay));
    }
}
