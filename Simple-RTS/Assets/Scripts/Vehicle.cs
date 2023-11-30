using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public string associatedBuilding = "";
    public float health = 10.0f;
    public bool isDeadAndMoved = false;

    private float normalHealth = 10.0f;
    private float upgradeHealth = 20.0f;
    private float normalDamage = 0.75f;
    private float upgradeDamage = 1.25f;
    private float explosionDelay = 1.0f;
    private float moveDelay = 2.0f;
    private float deathDelay = 1.0f;
    private float shootDelay = 1.0f;
    private float vehicleSpeed = 1.5f;
    private float damage = 0.75f;
    private float minDistanceFromAllyHQ = 6.0f;
    private float minDistanceFromOpposingHQ = 8.4f;
    private bool isCloseToOpposingHQ = false;
    private bool hasAlreadyFoundUpgradedVehicleFactory = false;
    private int shootingDamping = 120;
    private Rigidbody rigidBody;
    private VehicleFactory vehicleFactory;
    private VehicleFactory upgradedVehicleFactory;
    private AudioSource explosionAudioSource;
    private AudioSource shootEnemyAudioSource;
    private AudioSource shootHQAudioSource;
    private ParticleSystem explosionParticleSystem;
    private ParticleSystem enemyParticleSystem;
    private ParticleSystem HQParticleSystem;
    private GameObject target;
    private GameObject opposingHQ;
    private GameObject allyHQ;
    private Vector3 positionOpposingHQ;
    private Vector3 positionTarget;
    private Ray opposingHQRay;
    private Ray allyHQRay;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.velocity = transform.right * vehicleSpeed;
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
        vehicleFactory = buildingObject.GetComponent<VehicleFactory>();

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

        explosionParticleSystem = this.gameObject.transform.GetChild(3).gameObject.GetComponent<ParticleSystem>();
        explosionAudioSource = this.gameObject.transform.GetChild(2).gameObject.GetComponent<AudioSource>();

        var plasmaShotEnemyObject = this.gameObject.transform.GetChild(4).gameObject;
        var particleEnemyObject = plasmaShotEnemyObject.transform.GetChild(0).gameObject;
        enemyParticleSystem = particleEnemyObject.GetComponent<ParticleSystem>();
        shootEnemyAudioSource = plasmaShotEnemyObject.GetComponent<AudioSource>();

        var plasmaShotHQObject = this.gameObject.transform.GetChild(5).gameObject;
        var particleHQObject = plasmaShotHQObject.transform.GetChild(0).gameObject;
        HQParticleSystem = particleHQObject.GetComponent<ParticleSystem>();
        shootHQAudioSource = plasmaShotHQObject.GetComponent<AudioSource>();

        StartCoroutine(ShootEnemy());
        StartCoroutine(ShootHQ());

        // Play explosion particle effect and sound effect when health <= 0
        // Move off screen, then destroy object
        StartCoroutine(IsDefeated());
    }

    // Update is called once per frame
    void Update()
    {
        float opposingHQDistance = Vector3.Cross(opposingHQRay.direction, this.transform.position - opposingHQRay.origin).magnitude;
        if (opposingHQDistance < minDistanceFromOpposingHQ)
        {
            rigidBody.velocity = transform.right * 0;
            this.isCloseToOpposingHQ = true;
        }

        float allyHQDistance = Vector3.Cross(allyHQRay.direction, this.transform.position - allyHQRay.origin).magnitude;
        if (vehicleFactory != null)
        {
            if (allyHQDistance < minDistanceFromAllyHQ)
            {
                vehicleFactory.shouldSpawn = false;
            }
            else
            {
                vehicleFactory.shouldSpawn = true;
            }
        }

        if (!hasAlreadyFoundUpgradedVehicleFactory && vehicleFactory == null)
        {
            var buildingObject = GameObject.Find(associatedBuilding);
            upgradedVehicleFactory = buildingObject.GetComponent<VehicleFactory>();
            hasAlreadyFoundUpgradedVehicleFactory = true;
        }

        if (hasAlreadyFoundUpgradedVehicleFactory && upgradedVehicleFactory != null)
        {
            if (allyHQDistance < minDistanceFromAllyHQ)
            {
                upgradedVehicleFactory.shouldSpawn = false;
            }
            else
            {
                upgradedVehicleFactory.shouldSpawn = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.CompareTag("BlueTeam") && other.gameObject.CompareTag("BlueTeam"))
        {
            rigidBody.velocity = transform.right * 0;
        }
        else if (this.gameObject.CompareTag("BlueTeam") && other.gameObject.CompareTag("RedTeam") && !isCloseToOpposingHQ)
        {
            rigidBody.velocity = transform.right * 0;
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
            rigidBody.velocity = transform.right * 0;
        }
        else if (this.gameObject.CompareTag("RedTeam") && other.gameObject.CompareTag("BlueTeam") && !isCloseToOpposingHQ)
        {
            rigidBody.velocity = transform.right * 0;
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
            rigidBody.velocity = transform.right * vehicleSpeed;
        }
        else if (this.gameObject.CompareTag("BlueTeam") && other.gameObject.CompareTag("RedTeam"))
        {
            rigidBody.velocity = transform.right * vehicleSpeed;
        }
        else if (this.gameObject.CompareTag("RedTeam") && other.gameObject.CompareTag("RedTeam"))
        {
            rigidBody.velocity = transform.right * vehicleSpeed;
        }
        else if (this.gameObject.CompareTag("RedTeam") && other.gameObject.CompareTag("BlueTeam"))
        {
            rigidBody.velocity = transform.right * vehicleSpeed;
        }
    }

    IEnumerator IsDefeated()
    {
        if (this.health <= 0)
        {
            explosionAudioSource.PlayOneShot(explosionAudioSource.clip);
            explosionParticleSystem.Play();
            StartCoroutine(MoveAndDestroy());
        }
        else
        {
            yield return new WaitForSeconds(explosionDelay);
            StartCoroutine(IsDefeated());
        }
    }

    IEnumerator MoveAndDestroy()
    {
        yield return new WaitForSeconds(moveDelay);

        Vector3 location = this.gameObject.transform.position;
        location.y = 200.0f;
        this.gameObject.transform.position = location;
        this.isDeadAndMoved = true;

        Destroy(this.gameObject, deathDelay);
    }

    IEnumerator ShootEnemy()
    {
        yield return new WaitForSeconds(shootDelay);

        if (this.target != null && this.health > 0)
        {
            if (this.target.name.Contains("InfantryGroup") && this.target.GetComponent<InfantryGroup>().health > 0)
            {
                // Aim particle towards opposingHQ
                var lookPos = this.positionTarget - enemyParticleSystem.transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                enemyParticleSystem.transform.rotation = Quaternion.Slerp(enemyParticleSystem.transform.rotation, rotation, Time.deltaTime * shootingDamping);

                this.target.GetComponent<InfantryGroup>().health -= this.damage;
                enemyParticleSystem.Play();
                shootEnemyAudioSource.PlayOneShot(shootEnemyAudioSource.clip);
            }
            else if (this.target.name.Contains("Vehicle") && this.target.GetComponent<Vehicle>().health > 0)
            {
                // Aim particle towards opposingHQ
                var lookPos = this.positionTarget - enemyParticleSystem.transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                enemyParticleSystem.transform.rotation = Quaternion.Slerp(enemyParticleSystem.transform.rotation, rotation, Time.deltaTime * shootingDamping);

                enemyParticleSystem.Play();
                shootEnemyAudioSource.PlayOneShot(shootEnemyAudioSource.clip);
                this.target.GetComponent<Vehicle>().health -= this.damage;
            }
        }

        StartCoroutine(ShootEnemy());
    }

    IEnumerator ShootHQ()
    {
        yield return new WaitForSeconds(shootDelay);

        if (this.isCloseToOpposingHQ && this.health > 0)
        {
            if (this.opposingHQ.name == "EnemyHQ" && this.opposingHQ.GetComponent<EnemyHQ>().health > 0)
            {
                // Aim particle towards opposingHQ
                var lookPos = this.positionOpposingHQ - HQParticleSystem.transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                HQParticleSystem.transform.rotation = Quaternion.Slerp(HQParticleSystem.transform.rotation, rotation, Time.deltaTime * shootingDamping);

                HQParticleSystem.Play();
                shootHQAudioSource.PlayOneShot(shootHQAudioSource.clip);

                if (this.opposingHQ.GetComponent<EnemyHQ>().health - this.damage < 0)
                {
                    this.opposingHQ.GetComponent<EnemyHQ>().health = 0;
                }
                else
                {
                    this.opposingHQ.GetComponent<EnemyHQ>().health -= this.damage;
                }
            }
            else if (this.opposingHQ.name == "PlayerHQ" && this.opposingHQ.GetComponent<PlayerHQ>().health > 0)
            {
                // Aim particle towards opposingHQ
                var lookPos = this.positionOpposingHQ - HQParticleSystem.transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                HQParticleSystem.transform.rotation = Quaternion.Slerp(HQParticleSystem.transform.rotation, rotation, Time.deltaTime * shootingDamping);

                HQParticleSystem.Play();
                shootHQAudioSource.PlayOneShot(shootHQAudioSource.clip);

                if (this.opposingHQ.GetComponent<PlayerHQ>().health - this.damage < 0)
                {
                    this.opposingHQ.GetComponent<PlayerHQ>().health = 0;
                }
                else
                {
                    this.opposingHQ.GetComponent<PlayerHQ>().health -= this.damage;
                }
            }
        }

        StartCoroutine(ShootHQ());
    }
}
