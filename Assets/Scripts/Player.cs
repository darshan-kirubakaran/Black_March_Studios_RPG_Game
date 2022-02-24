using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool canMove = true;
    public bool isMoving = false;

    public void SetPlayerPlatformTagToObsticle()
    {
        RaycastHit hit;
        Physics.Raycast(this.gameObject.transform.position, Vector3.down, out hit);
        Transform currentPlatform = hit.transform;

        currentPlatform.gameObject.tag = "Obsticle";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && canMove)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;

                if (objectHit.gameObject.tag == "Platform")
                {
                    // Do something with the object that was hit by the raycast.
                    canMove = false;
                    isMoving = true;

                    Physics.Raycast(this.gameObject.transform.position, Vector3.down, out hit);
                    Transform currentPlatform = hit.transform;
                    FindObjectOfType<PathFinder>().FindPath(currentPlatform.GetComponent<Platform>().x, currentPlatform.GetComponent<Platform>().y, objectHit.GetComponent<Platform>().x, objectHit.GetComponent<Platform>().y);

                    StartCoroutine(movePlayer());
                }
            }
        }
    }

    IEnumerator movePlayer()
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

        canMove = true;
        isMoving = false;
        FindObjectOfType<Enemy>().MoveEnemy();
    }
}
