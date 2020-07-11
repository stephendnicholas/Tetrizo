using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    // Set in the unity editor
    public Sprite[] groupSprites;

    // Set in the unity editor
    public GameObject[] groups;

    // Set in the unity editor
    public GameObject nextImage;

    private float spawnDelay = 0.5f;

    private int nextBlock;

    void Start() {
        nextBlock = Random.Range(0, groups.Length);
        SpawnNext();
    }

    public void SpawnNext() {
        StartCoroutine(spawn());
    }


    IEnumerator spawn() {
        yield return new WaitForSeconds(spawnDelay);

        // Spawn Group at current Position
        Instantiate(groups[nextBlock],
                    transform.position,
                    Quaternion.identity);

        nextBlock = Random.Range(0, groups.Length);

        nextImage.SetActive(true);
        nextImage.GetComponent<Image>().sprite = groupSprites[nextBlock];
    }
}
