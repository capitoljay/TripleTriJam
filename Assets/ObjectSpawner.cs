using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public float seed = 239.1902f;
    public Rect spawnBoundary = new Rect(0, 0, 74, 20);
    public ScoreManager scoreMgr;
    public int objectCount = 10;
    public GameObject[] spawnPrefabs;
    public bool respawn = true;
    public float randomnessMultiplier = 1f;
    public float countMultiplierPerLevel = 1.1f;
    private long currentLevel = 0;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    void Start()
    {
        if (scoreMgr == null)
            scoreMgr = FindObjectOfType<ScoreManager>();


        //Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (respawn)
        {
            respawn = false;
            Spawn(currentLevel);
        }
    }

    public void Spawn(long level)
    {
        int seedIdx = 0;
        int prefabIdx = 0;
        float randomizer = 0;
        currentLevel = level;

        respawn = false;
        for (int x = this.transform.childCount - 1; x >= 0; x--)
            Destroy(this.transform.GetChild(x).gameObject);

        if (scoreMgr != null && spawnedObjects.Count == 0 && spawnPrefabs != null && spawnPrefabs.Length > 0)
        {
            for(int x = 0; x < objectCount + (int)(level * countMultiplierPerLevel); x++)
            {
                if (seedIdx >= scoreMgr.seedVals.Count - 1)
                    seedIdx = 0;

                var idxX = scoreMgr.seedVals[seedIdx] * randomnessMultiplier + seed * level / 10f;
                var idxY = scoreMgr.seedVals[seedIdx + 1] * randomnessMultiplier + seed * level / 10f;
                randomizer += scoreMgr.seedVals[seedIdx + 2] * randomnessMultiplier + seed * level / 10f;
                seedIdx += 3;

                var random = Mathf.PerlinNoise(idxX + randomizer, idxY + randomizer);
                var random2 = Mathf.PerlinNoise(idxY + randomizer, idxX + randomizer);

                //Figure out if we're going to go to the next prefab
                if (random > 0.5f)
                    prefabIdx++;

                //Reset the prefab IDX if we go out of bounds.
                if (prefabIdx >= spawnPrefabs.Length)
                    prefabIdx = 0;

                var newObj = Instantiate(spawnPrefabs[prefabIdx]);
                newObj.transform.localPosition = new Vector3(
                    Mathf.Clamp(Mathf.Lerp(spawnBoundary.width / -2.75f, spawnBoundary.width / 2.75f, random), spawnBoundary.width / -2f, spawnBoundary.width / 2f) + spawnBoundary.x,
                    Mathf.Clamp(Mathf.Lerp(spawnBoundary.height / -2.75f, spawnBoundary.height / 2.75f, random2), spawnBoundary.height / -2f, spawnBoundary.height / 2f) + spawnBoundary.y,
                    1f);



                newObj.transform.SetParent(transform);
            }
        }
    }
}