using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatforms : MonoBehaviour
{
    public int xLen = 10;
    public int yLen = 10;

    [SerializeField] float padding = 1;

    public Transform platform;
    [SerializeField] Transform obsticlePrefab;
    [SerializeField] Transform platformParent;
    [SerializeField] Transform obsticleParent;

    public ObsticleLocations obsticleLocations;

    public GameObject[,] gridArray;

    private void Awake()
    {
        gridArray = new GameObject[xLen, yLen];
    }

    private void Start()
    {
        Vector3 spawnPos = new Vector3(0, 0, 0);

        for (int y = 0; y < yLen; y++)
        {
            for (int x = 0; x < xLen; x++)
            {
                spawnPos = new Vector3(x * (platform.localScale.x + padding), 0, y * (platform.localScale.z + padding));

                Transform newPlatform = Instantiate(platform, spawnPos, Quaternion.identity);
                newPlatform.gameObject.name = (x + ", " + y).ToString();
                newPlatform.GetComponent<Platform>().x = x;
                newPlatform.GetComponent<Platform>().y = y;
                newPlatform.parent = platformParent;

                gridArray[x, y] = newPlatform.gameObject;
            }
        }

        SpawnObsticles();

        FindObjectOfType<Enemy>().SetEnemyIniLocation();
        FindObjectOfType<Player>().SetPlayerPlatformTagToObsticle();
    }

    public void SpawnObsticles()
    {
        foreach (Transform child in obsticleParent)
        {
            RaycastHit hit;
            Physics.Raycast(child.transform.position, Vector3.down, out hit);
            Transform currentPlatform = hit.transform;
            if(hit.collider != null)
            {
                currentPlatform.tag = "Platform";
            }
            Destroy(child.gameObject);
        }

        foreach (string obsticleLocation in obsticleLocations.obsticleLocations)
        {
            GameObject obsticle = GameObject.Find(obsticleLocation);

            if (obsticle && obsticle.transform.tag == "Platform" && FindObjectOfType<Player>().isMoving == false && FindObjectOfType<Enemy>().isMoving == false)
            {
                obsticle.tag = "Obsticle";
                Transform newObsticle = Instantiate(obsticlePrefab, new Vector3(obsticle.transform.position.x, obsticle.transform.localScale.y / 2, obsticle.transform.position.z), Quaternion.identity);
                newObsticle.parent = obsticleParent;
            }

        }
    }
}
