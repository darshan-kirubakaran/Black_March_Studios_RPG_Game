using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public int startX = 0;
    public int startY = 0;
    public int endX = 2;
    public int endY = 2;

    public GameObject[,] gridArray;
    public List<GameObject> path = new List<GameObject>();

    int xLen;
    int yLen;

    SpawnPlatforms spawnPlatforms;

    private void Start()
    {
        spawnPlatforms = FindObjectOfType<SpawnPlatforms>();

        xLen = spawnPlatforms.xLen;
        yLen = spawnPlatforms.yLen;
        gridArray = spawnPlatforms.gridArray;
}

    public void FindPath(int startx, int starty, int endx, int endy)
    {
        startX = startx;
        startY = starty;
        endX = endx;
        endY = endy;


        SetDistance();
        SetPath();

        path.Reverse();
    }

    void SetDistance()
    {
        InitialSetUp();
        int x = startX;
        int y = startY;
        for (int step = 1; step < xLen * yLen; step++)
        {
            foreach (GameObject plat in gridArray)
            {
                if (plat.GetComponent<Platform>().visited == step - 1)
                {
                    TestFourDirections(plat.GetComponent<Platform>().x, plat.GetComponent<Platform>().y, step);
                }
            }
        }
    }

    private void SetPath()
    {
        int step;
        int x = endX;
        int y = endY;
        List<GameObject> tempList = new List<GameObject>();
        path.Clear();
        if (gridArray[endX, endY] != null && gridArray[endX, endY].GetComponent<Platform>().visited > 0)
        {
            path.Add(gridArray[x, y]);
            step = gridArray[x, y].GetComponent<Platform>().visited - 1;
        }
        else
        {
            print("Cant reach the desired location");
            return;
        }

        for (int i = step; step > 0; step--)
        {
            if (TestDirection(x, y, step, 1))
                tempList.Add(gridArray[x, y + 1]);
            if (TestDirection(x, y, step, 2))
                tempList.Add(gridArray[x + 1, y]);
            if (TestDirection(x, y, step, 3))
                tempList.Add(gridArray[x, y - 1]);
            if (TestDirection(x, y, step, 4))
                tempList.Add(gridArray[x - 1, y]);

            GameObject temObj = FindClosest(gridArray[endX, endY].transform, tempList);
            path.Add(temObj);
            x = temObj.GetComponent<Platform>().x;
            y = temObj.GetComponent<Platform>().y;
            tempList.Clear();
        }
    }

    private void InitialSetUp()
    {
        foreach (GameObject plat in gridArray)
        {
            if (plat != null)
            {
                plat.GetComponent<Platform>().visited = -1;
            }
        }

        gridArray[startX, startY].GetComponent<Platform>().visited = 0;
    }

    bool TestDirection(int x, int y, int step, int direction)
    {
        //1 is up, 2 is right, 3 is down, 4 is left

        switch (direction)
        {
            case 1:
                if (y + 1 < yLen && gridArray[x, y + 1] && gridArray[x, y + 1].GetComponent<Platform>().visited == step && gridArray[x, y + 1].tag == "Platform")
                    return true;
                else
                    return false;
            case 2:
                if (x + 1 < xLen && gridArray[x + 1, y] && gridArray[x + 1, y].GetComponent<Platform>().visited == step && gridArray[x + 1, y].tag == "Platform")
                    return true;
                else
                    return false;
            case 3:
                if (y - 1 > -1 && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<Platform>().visited == step && gridArray[x, y - 1].tag == "Platform")
                    return true;
                else
                    return false;
            case 4:
                if (x - 1 > -1 && gridArray[x - 1, y] && gridArray[x - 1, y].GetComponent<Platform>().visited == step && gridArray[x - 1, y].tag == "Platform")
                    return true;
                else
                    return false;

        }
        return false;
    }

    private void TestFourDirections(int x, int y, int step)
    {
        if (TestDirection(x, y, -1, 1))
        {
            Setvisited(x, y + 1, step);
        }
        if (TestDirection(x, y, -1, 2))
        {
            Setvisited(x + 1, y, step);
        }
        if (TestDirection(x, y, -1, 3))
        {
            Setvisited(x, y - 1, step);
        }
        if (TestDirection(x, y, -1, 4))
        {
            Setvisited(x - 1, y, step);
        }
    }

    private void Setvisited(int x, int y, int step)
    {
        if (gridArray[x, y])
        {
            gridArray[x, y].GetComponent<Platform>().visited = step;
        }
    }

    GameObject FindClosest(Transform targetLocation, List<GameObject> list)
    {
        float currentDistance = spawnPlatforms.platform.localScale.x * xLen * yLen;
        int indexNumber = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
            {
                currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                indexNumber = i;
            }
        }

        return list[indexNumber];
    }

}
