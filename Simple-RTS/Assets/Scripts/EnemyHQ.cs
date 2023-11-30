using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EnemyHQ : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    public float health = 100.0F;

    GameObject playerHQObject;
    PlayerHQ playerHQ;

    GameObject explosionSoundGameObject;
    AudioSource explosionAudioSource;

    GameObject explosionParticleGameObject;
    GameObject flameParticleGameObject;
    private ParticleSystem explosionParticleSystem;
    private ParticleSystem flameParticleSystem;

    private float explosionWait = 1.0f;
    private float victoryWait = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerHQObject = GameObject.Find("PlayerHQ");
        playerHQ = playerHQObject.GetComponent<PlayerHQ>();

        explosionParticleGameObject = this.gameObject.transform.GetChild(2).gameObject;
        explosionParticleSystem = explosionParticleGameObject.GetComponent<ParticleSystem>();

        flameParticleGameObject = this.gameObject.transform.GetChild(3).gameObject;
        flameParticleSystem = flameParticleGameObject.GetComponent<ParticleSystem>();

        explosionSoundGameObject = this.gameObject.transform.GetChild(1).gameObject;
        explosionAudioSource = explosionSoundGameObject.GetComponent<AudioSource>();

        healthText = GameObject.Find("EnemyHQHealth").GetComponent<TextMeshProUGUI>();
        SetHealthText();

        StartCoroutine(IsVictory());
    }

    // Update is called once per frame
    void Update()
    {
        SetHealthText();
    }

    void SetHealthText()
    {
        healthText.text = "Health: " + Convert.ToInt32(Math.Floor(health)).ToString() + "%";
    }

    IEnumerator IsVictory()
    {
        if (health <= 0 && playerHQ.health > 0)
        {
            explosionAudioSource.PlayOneShot(explosionAudioSource.clip);
            explosionParticleSystem.Play();
            flameParticleSystem.Play();
            StartCoroutine(ShowVictory());
        }
        else
        {
            yield return new WaitForSeconds(explosionWait);
            StartCoroutine(IsVictory());
        }
    }

    IEnumerator ShowVictory()
    {
        yield return new WaitForSeconds(victoryWait);
        GameObject.Find("Canvas").transform.GetChild(6).gameObject.SetActive(true);

        // Freeze game
        Time.timeScale = 0;
    }
}
