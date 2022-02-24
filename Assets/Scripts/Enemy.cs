using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject player;
    public bool isMoving = false;

    int startX = 9;
    int startY = 9;

    int xLen;
    int yLen;

    GameObject[,] gridArray;

    SpawnPlatforms spawnPlatforms;

    private void Start()
    {
        spawnPlatforms = FindObjectOfType<SpawnPlatforms>();

        xLen = spawnPlatforms.xLen;
        yLen = spawnPlatforms.yLen;
        gridArray = spawnPlatforms.gridArray;
    }

    public void SetEnemyIniLocation()
    {
        Transform iniPlatform = FindObjectOfType<SpawnPlatforms>().gridArray[startX, startY].transform;

        this.gameObject.transform.position = new Vector3(iniPlatform.position.x, (iniPlatform.transform.localScale.y / 2) + (this.gameObject.transform.localScale.y / 2), iniPlatform.position.z);

        RaycastHit hit;
        Physics.Raycast(this.gameObject.transform.position, Vector3.down, out hit);
        Transform currentPlatform = hit.transform;

        currentPlatform.gameObject.tag = "Obsticle";
    }

    public void MoveEnemy()
    {
        player.GetComponent<Player>().canMove = false;
        isMoving = true;

        RaycastHit hit;
        Physics.Raycast(this.gameObject.transform.position, Vector3.down, out hit);
        Transform currentPlatform = hit.transform;

        // Do something with the object that was hit by the raycast.
        TestFourDirections(currentPlatform);

        StartCoroutine(moveEnemyByFrame());

    }

    IEnumerator moveEnemyByFrame()
    {
        Transform currentPlatform;
        RaycastHit hit;

        foreach (GameObject obj in FindObjectOfType<PathFinder>().path)
        {
            Physics.Raycast(this.gameObject.transform.position, Vector3.down, out hit);
            currentPlatform = hit.transform;

            currentPlatform.gameObject.tag = "Obsticle";

            yield return new WaitForSeconds(1);
            this.gameObject.transform.position = new Vector3(obj.transform.position.x, (obj.transform.localScale.y / 2) + (this.gameObject.transform.localScale.y / 2), obj.transform.position.z);

            currentPlatform.gameObject.tag = "Platform";

        }

        Physics.Raycast(this.gameObject.transform.position, Vector3.down, out hit);
        currentPlatform = hit.transform;

        currentPlatform.gameObject.tag = "Obsticle";

        player.GetComponent<Player>().canMove = true;
        isMoving = false;
    }

    float TestDirection(int x, int y, int direction)
    {
        //1 is up, 2 is right, 3 is down, 4 is left

        switch (direction)
        {
            case 1:
                if (y + 1 < yLen && gridArray[x, y + 1] && gridArray[x, y + 1].tag == "Platform")
                    return Vector3.Distance(gridArray[x, y + 1].transform.position, this.transform.position);
                else
                    return 0;
            case 2:
                if (x + 1 < xLen && gridArray[x + 1, y] && gridArray[x + 1, y].tag == "Platform")
                    return Vector3.Distance(gridArray[x + 1, y].transform.position, this.transform.position);
                else
                    return 0;
            case 3:
                if (y - 1 > -1 && gridArray[x, y - 1] && gridArray[x, y - 1].tag == "Platform")
                    return Vector3.Distance(gridArray[x, y - 1].transform.position, this.transform.position);
                else
                    return 0;
            case 4:
                if (x - 1 > -1 && gridArray[x - 1, y] && gridArray[x - 1, y].tag == "Platform")
                    return Vector3.Distance(gridArray[x - 1, y].transform.position, this.transform.position);
                else
                    return 0;

        }
        return 0;
    }

    private void TestFourDirections(Transform startPlatform)
    {
        RaycastHit hit;
        Physics.Raycast(player.gameObject.transform.position, Vector3.down, out hit);
        Transform playerPlatform = hit.transform;
        int x = playerPlatform.GetComponent<Platform>().x;
        int y = playerPlatform.GetComponent<Platform>().y;

        float pos1 = TestDirection(x, y, 1);
        float pos2 = TestDirection(x, y, 2);
        float pos3 = TestDirection(x, y, 3);
        float pos4 = TestDirection(x, y, 4);

        float[] posses = { pos1, pos2, pos3, pos4 };

        float value = float.PositiveInfinity;
        int index = -1;
        for (int i = 0; i < posses.Length; i++)
        {
            if (posses[i] < value && posses[i] != 0)
            {
                index = i;
                value = posses[i];
            }
        }

        if(posses[index] == pos1)
        {
            MoveEnemyTowardRightDir(startPlatform, gridArray[x, y + 1].transform);
        }
        else if(posses[index] == pos2)
        {
            MoveEnemyTowardRightDir(startPlatform, gridArray[x + 1, y].transform);
        }
        else if(posses[index] == pos3)
        {
            MoveEnemyTowardRightDir(startPlatform, gridArray[x, y - 1].transform);
        }
        else if(posses[index] == pos4)
        {
            MoveEnemyTowardRightDir(startPlatform, gridArray[x - 1, y].transform);
        }
    }

    private void MoveEnemyTowardRightDir(Transform startPlatform, Transform endPlatform)
    {
        FindObjectOfType<PathFinder>().FindPath(startPlatform.GetComponent<Platform>().x, startPlatform.GetComponent<Platform>().y, endPlatform.GetComponent<Platform>().x, endPlatform.GetComponent<Platform>().y);
    }
}
