using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfantrySolo : MonoBehaviour
{
    private InfantryGroup infantryGroup;
    private Animator Animator;
    private GameObject assaultRifle;
    private GameObject plasmaShotHQ;
    private GameObject plasmaShotEnemy;
    private GameObject particleGameObjectHQ;
    private GameObject particleGameObjectEnemy;
    private AudioSource shootAudioSourceHQ;
    private AudioSource shootAudioSourceEnemy;
    private float delayTime = 1.0f;
    private int shootingDamping = 120;

    private ParticleSystem particleSystemHQ;
    private ParticleSystem particleSystemEnemy;
    
    // Start is called before the first frame update
    void Start()
    {
        infantryGroup = GetComponentInParent<InfantryGroup>();
        Animator = GetComponent<Animator>();

        assaultRifle = this.transform.GetChild(1).gameObject;
        plasmaShotEnemy = assaultRifle.transform.GetChild(0).gameObject;
        particleGameObjectEnemy = plasmaShotEnemy.transform.GetChild(0).gameObject;
        particleSystemEnemy = particleGameObjectEnemy.GetComponent<ParticleSystem>();
        shootAudioSourceEnemy = plasmaShotEnemy.GetComponent<AudioSource>();

        plasmaShotHQ = assaultRifle.transform.GetChild(1).gameObject;
        particleGameObjectHQ = plasmaShotHQ.transform.GetChild(0).gameObject;
        particleSystemHQ = particleGameObjectHQ.GetComponent<ParticleSystem>();
        shootAudioSourceHQ = plasmaShotHQ.GetComponent<AudioSource>();

        StartCoroutine(ShootEnemy(delayTime));
        StartCoroutine(ShootHQ(delayTime));
    }

    // Update is called once per frame
    void Update()
    {
        if (infantryGroup.isWalking && !infantryGroup.isShooting && !infantryGroup.isDead)
        {
            Animator.SetBool("Walking", true);
            Animator.SetBool("Shooting", false);
        }
        else if (!infantryGroup.isWalking && !infantryGroup.isShooting && !infantryGroup.isDead)
        {
            Animator.SetBool("Walking", false);
            Animator.SetBool("Shooting", false);
        }
        else if (infantryGroup.isShooting && !infantryGroup.isWalking && !infantryGroup.isDead)
        {
            Animator.SetBool("Shooting", true);
            Animator.SetBool("Walking", false);
        }
        else if (infantryGroup.isDead)
        {
            Animator.SetBool("Dead", true);
        }
    }

    IEnumerator ShootEnemy(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (infantryGroup.target != null && !infantryGroup.isDead)
        {
            if (infantryGroup.target.name.Contains("InfantryGroup"))
            {
                if (infantryGroup.target.GetComponent<InfantryGroup>().health > 0)
                {
                    // Aim particle towards opposingHQ
                    var lookPos = infantryGroup.positionTarget - particleSystemHQ.transform.position;
                    lookPos.y = 0;
                    var rotation = Quaternion.LookRotation(lookPos);
                    particleSystemHQ.transform.rotation = Quaternion.Slerp(particleSystemHQ.transform.rotation, rotation, Time.deltaTime * shootingDamping);

                    infantryGroup.target.GetComponent<InfantryGroup>().health -= infantryGroup.damage;
                    particleSystemEnemy.Play();
                    shootAudioSourceEnemy.PlayOneShot(shootAudioSourceEnemy.clip);
                }
                else
                {
                    if (!infantryGroup.target.GetComponent<InfantryGroup>().isDeadAndMoved)
                    {
                        infantryGroup.isWalking = false;
                        infantryGroup.isShooting = false;
                    }
                }
            }
            else if (infantryGroup.target.name.Contains("Vehicle"))
            {
                if (infantryGroup.target.GetComponent<Vehicle>().health > 0)
                {
                    // Aim particle towards opposingHQ
                    var lookPos = infantryGroup.positionTarget - particleSystemHQ.transform.position;
                    lookPos.y = 0;
                    var rotation = Quaternion.LookRotation(lookPos);
                    particleSystemHQ.transform.rotation = Quaternion.Slerp(particleSystemHQ.transform.rotation, rotation, Time.deltaTime * shootingDamping);

                    particleSystemEnemy.Play();
                    shootAudioSourceEnemy.PlayOneShot(shootAudioSourceEnemy.clip);
                    infantryGroup.target.GetComponent<Vehicle>().health -= infantryGroup.damage;
                }
                else
                {
                    if (!infantryGroup.target.GetComponent<Vehicle>().isDeadAndMoved)
                    {
                        infantryGroup.isWalking = false;
                        infantryGroup.isShooting = false;
                    }
                }
            }
        }

        StartCoroutine(ShootEnemy(delayTime));
    }

    IEnumerator ShootHQ(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (infantryGroup.isCloseToOpposingHQ && !infantryGroup.isDead)
        {
            if (infantryGroup.opposingHQ.name == "EnemyHQ")
            {
                if (infantryGroup.opposingHQ.GetComponent<EnemyHQ>().health > 0)
                {
                    // Aim particle towards opposingHQ
                    var lookPos = infantryGroup.positionOpposingHQ - particleSystemHQ.transform.position;
                    lookPos.y = 0;
                    var rotation = Quaternion.LookRotation(lookPos);
                    particleSystemHQ.transform.rotation = Quaternion.Slerp(particleSystemHQ.transform.rotation, rotation, Time.deltaTime * shootingDamping);

                    particleSystemHQ.Play();
                    shootAudioSourceHQ.PlayOneShot(shootAudioSourceHQ.clip);

                    if (infantryGroup.opposingHQ.GetComponent<EnemyHQ>().health - infantryGroup.damage < 0)
                    {
                        infantryGroup.opposingHQ.GetComponent<EnemyHQ>().health = 0;
                    }
                    else
                    {
                        infantryGroup.opposingHQ.GetComponent<EnemyHQ>().health -= infantryGroup.damage;
                    }
                }
                else
                {
                    infantryGroup.isWalking = false;
                    infantryGroup.isShooting = false;
                    Animator.SetBool("Walking", false);
                    Animator.SetBool("Shooting", false);
                }
            }
            else if (infantryGroup.opposingHQ.name == "PlayerHQ")
            {
                if (infantryGroup.opposingHQ.GetComponent<PlayerHQ>().health > 0)
                {
                    // Aim particle towards opposingHQ
                    var lookPos = infantryGroup.positionOpposingHQ - particleSystemHQ.transform.position;
                    lookPos.y = 0;
                    var rotation = Quaternion.LookRotation(lookPos);
                    particleSystemHQ.transform.rotation = Quaternion.Slerp(particleSystemHQ.transform.rotation, rotation, Time.deltaTime * shootingDamping);

                    particleSystemHQ.Play();
                    shootAudioSourceHQ.PlayOneShot(shootAudioSourceHQ.clip);

                    if (infantryGroup.opposingHQ.GetComponent<PlayerHQ>().health - infantryGroup.damage < 0)
                    {
                        infantryGroup.opposingHQ.GetComponent<PlayerHQ>().health = 0;
                    }
                    else
                    {
                        infantryGroup.opposingHQ.GetComponent<PlayerHQ>().health -= infantryGroup.damage;
                    }
                }
                else
                {
                    infantryGroup.isWalking = false;
                    infantryGroup.isShooting = false;
                    Animator.SetBool("Walking", false);
                    Animator.SetBool("Shooting", false);
                }
            }
        }

        StartCoroutine(ShootHQ(delayTime));
    }
}
