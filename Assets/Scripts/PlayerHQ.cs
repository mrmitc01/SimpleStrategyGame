using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class PlayerHQ : MonoBehaviour
{
    public TextMeshProUGUI countText;
    public TextMeshProUGUI healthText;

    public int energyStart = 1000;
    public int secondsToWait = 15;
    public int energyIncrementAmount = 40;
    public float health = 100.0F;

    GameControl gameControl;
    GameObject enemyHQObject;
    EnemyHQ enemyHQ;

    GameObject explosionSoundGameObject;
    AudioSource explosionAudioSource;

    GameObject explosionParticleGameObject;
    GameObject flameParticleGameObject;
    private ParticleSystem explosionParticleSystem;
    private ParticleSystem flameParticleSystem;

    private float explosionWait = 1.0f;
    private float defeatWait = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        gameControl = GameObject.FindObjectOfType<GameControl>();
        gameControl.energyCount = energyStart;

        enemyHQObject = GameObject.Find("EnemyHQ");
        enemyHQ = enemyHQObject.GetComponent<EnemyHQ>();

        explosionParticleGameObject = this.gameObject.transform.GetChild(2).gameObject;
        explosionParticleSystem = explosionParticleGameObject.GetComponent<ParticleSystem>();

        flameParticleGameObject = this.gameObject.transform.GetChild(3).gameObject;
        flameParticleSystem = flameParticleGameObject.GetComponent<ParticleSystem>();

        explosionSoundGameObject = this.gameObject.transform.GetChild(1).gameObject;
        explosionAudioSource = explosionSoundGameObject.GetComponent<AudioSource>();

        countText = GameObject.Find("EnergyCount").GetComponent<TextMeshProUGUI>();
        SetCountText();

        healthText = GameObject.Find("PlayerHQHealth").GetComponent<TextMeshProUGUI>();
        SetHealthText();

        StartCoroutine(Incremental());
        StartCoroutine(IsDefeated());
    }

    private void Update()
    {
        SetCountText();
        SetHealthText();
    }

    void SetCountText()
    {
        countText.text = "Energy: " + gameControl.energyCount.ToString();
    }

    void SetHealthText()
    {
        healthText.text = "Health: " + Convert.ToInt32(Math.Floor(health)).ToString() + "%";
    }

    void IncrementEnergyCount()
    {
        gameControl.energyCount += energyIncrementAmount;
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

    IEnumerator IsDefeated()
    {
        if (health <= 0 && enemyHQ.health > 0)
        {
            explosionAudioSource.PlayOneShot(explosionAudioSource.clip);
            explosionParticleSystem.Play();
            flameParticleSystem.Play();
            StartCoroutine(ShowDefeat());
        }
        else
        {
            yield return new WaitForSeconds(explosionWait);
            StartCoroutine(IsDefeated());
        }
    }

    IEnumerator ShowDefeat()
    {
        yield return new WaitForSeconds(defeatWait);
        GameObject.Find("Canvas").transform.GetChild(7).gameObject.SetActive(true);

        // Freeze game
        Time.timeScale = 0;
    }
}
