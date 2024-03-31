using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieSpawnController : MonoBehaviour
{
    public int initialZombiesPerWave = 5;
    public int currentZombiesPerWave;

    public float spawnDelay = 0.5f; //delay between spawning each zombie in a wave

    public int currentWave = 0;
    public float waveCooldown = 5f; // time in seconds between waves

    public bool inCooldown;
    public float cooldownCounter = 0; // We only use this for testing and the UI

    public List<Enemy> currentZombiesAlive;

    public GameObject zombiePrefab;

    public TextMeshProUGUI waveUI;
    public TextMeshProUGUI currentWaveUI;

    private void Start()
    {
        currentZombiesPerWave = initialZombiesPerWave;
        GlobalReferences.Instance.waveNumber = currentWave;

        StartNextWave();
    }

    private void StartNextWave()
    {
        currentZombiesAlive.Clear();
        currentWave++;

        GlobalReferences.Instance.waveNumber = currentWave;
        currentWaveUI.text = "Round " + currentWave;
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            //Generate a random offset within a specified range
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;


            // Instantiate the zombie
            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

            Enemy enemyScript = zombie.GetComponent<Enemy>();

            currentZombiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);

        }
    }

    private void Update()
    {
        // get all dead zombies
        List<Enemy> zombiesToRemove = new List<Enemy>();
        foreach (var zombie in currentZombiesAlive)
        {
            if (zombie.isDead)
            {
                zombiesToRemove.Add(zombie);
            }
        }

        // Actually remove all dead zombies
        foreach (Enemy zombie in zombiesToRemove)
        {
            currentZombiesAlive.Remove(zombie);
        }

        zombiesToRemove.Clear();

        //start cooldown if all zombies are dead
        if (currentZombiesAlive.Count == 0 && inCooldown == false)
        {
            //Start cooldown for next wave
            StartCoroutine(WaveCooldown());
        }

        // Run the cooldown counter
        if (inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        if(!inCooldown)
        {
            // Reset the counter
            cooldownCounter = waveCooldown;
        }

        
    }

    private IEnumerator WaveCooldown()
    {
        
        waveUI.gameObject.SetActive(true);
        int nextWave = currentWave + 1;
        waveUI.text = "Round " + nextWave;
        inCooldown = true;

        yield return new WaitForSeconds(waveCooldown);

        inCooldown = false;

        waveUI.gameObject.SetActive(false);


        currentZombiesPerWave *= 2; 
        StartNextWave();
    }
}
