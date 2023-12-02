using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretShooting : MonoBehaviour
{
    //private GameObject target;
    private GameObject turretObject;
    private AudioSource shootEnemyAudioSource;
    private ParticleSystem enemyParticleSystem;
    private float damage = 0.168f;
    private float normalDamage = 0.168f;
    private float upgradeDamage = 0.336f;
    private float shootDelay = 1.0f;
    private int shootingDamping = 250;
    private List<GameObject> targetList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        turretObject = this.gameObject.transform.parent.gameObject;

        if (turretObject.name.Contains("Red") || turretObject.name.Contains("Blue"))
        {
            damage = normalDamage;
        }
        else if (turretObject.name.Contains("Green") || turretObject.name.Contains("Orange"))
        {
            damage = upgradeDamage;
        }

        if (turretObject.CompareTag("RedTurret"))
        {
            var turretTowerObject = this.gameObject.transform.parent.gameObject.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject;
            var plasmaShotEnemyObject = turretTowerObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
            var particleEnemyObject = plasmaShotEnemyObject.transform.GetChild(0).gameObject;
            enemyParticleSystem = particleEnemyObject.GetComponent<ParticleSystem>();
            shootEnemyAudioSource = plasmaShotEnemyObject.GetComponent<AudioSource>();
        }
        else if (turretObject.CompareTag("BlueTurret"))
        {
            var turretTowerObject = this.gameObject.transform.parent.gameObject.transform.GetChild(2).gameObject.transform.GetChild(2).gameObject;
            var plasmaShotEnemyObject = turretTowerObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
            var particleEnemyObject = plasmaShotEnemyObject.transform.GetChild(0).gameObject;
            enemyParticleSystem = particleEnemyObject.GetComponent<ParticleSystem>();
            shootEnemyAudioSource = plasmaShotEnemyObject.GetComponent<AudioSource>();
        }

        StartCoroutine(ShootEnemy());
    }

    private void OnTriggerEnter(Collider other)
    {
        // Add target to target list
        if (turretObject.CompareTag("BlueTurret") && other.gameObject.CompareTag("RedTeam"))
        {
            if (other.gameObject.name.Contains("InfantryGroup") || other.gameObject.name.Contains("Vehicle_"))
            {
                targetList.Add(other.gameObject);
            }
        }
        else if (turretObject.CompareTag("RedTurret") && other.gameObject.CompareTag("BlueTeam"))
        {
            if (other.gameObject.name.Contains("InfantryGroup") || other.gameObject.name.Contains("Vehicle_"))
            {
                targetList.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove target from target list
        if (turretObject.CompareTag("BlueTurret") && other.gameObject.CompareTag("RedTeam"))
        {
            if (other.gameObject.name.Contains("InfantryGroup") || other.gameObject.name.Contains("Vehicle_"))
            {
                targetList.Remove(other.gameObject);
            }
        }
        else if (turretObject.CompareTag("RedTurret") && other.gameObject.CompareTag("BlueTeam"))
        {
            if (other.gameObject.name.Contains("InfantryGroup") || other.gameObject.name.Contains("Vehicle_"))
            {
                targetList.Remove(other.gameObject);
            }
        }

    }

    IEnumerator ShootEnemy()
    {
        yield return new WaitForSeconds(shootDelay);

        // Look at first in target list
        if (targetList.Any())
        {
            var target = targetList.First();
            if (target.name.Contains("InfantryGroup") && target.GetComponent<InfantryGroup>().health > 0)
            {
                // Aim particle towards InfantryGroup
                var positionTarget = target.transform.position;
                var lookPos = positionTarget - enemyParticleSystem.transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                enemyParticleSystem.transform.rotation = Quaternion.Slerp(enemyParticleSystem.transform.rotation, rotation, Time.deltaTime * shootingDamping);
                
                target.GetComponent<InfantryGroup>().health -= this.damage;
                enemyParticleSystem.Play();
                shootEnemyAudioSource.PlayOneShot(shootEnemyAudioSource.clip);
            }
            else if (target.name.Contains("Vehicle") && target.GetComponent<Vehicle>().health > 0)
            {
                // Aim particle towards Vehicle
                var positionTarget = target.transform.position;
                var lookPos = positionTarget - enemyParticleSystem.transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                enemyParticleSystem.transform.rotation = Quaternion.Slerp(enemyParticleSystem.transform.rotation, rotation, Time.deltaTime * shootingDamping);
                
                enemyParticleSystem.Play();
                shootEnemyAudioSource.PlayOneShot(shootEnemyAudioSource.clip);
                target.GetComponent<Vehicle>().health -= this.damage;
            }
        }

        StartCoroutine(ShootEnemy());
    }
}
