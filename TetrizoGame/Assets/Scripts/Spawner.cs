using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Set in the unity editor
    public GameObject[] groups;

    private float spawnDelay = 1.0f;

    void Start() {
        SpawnNext();
    }

    public void SpawnNext() {
        StartCoroutine(spawn());
    }


    IEnumerator spawn() {
        yield return new WaitForSeconds(spawnDelay);

        int i = Random.Range(0, groups.Length);

        // Spawn Group at current Position
        Instantiate(groups[i],
                    transform.position,
                    Quaternion.identity);
    }
}
