using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlatformSelecter : MonoBehaviour
{
    [SerializeField] Text selecetedPlatformPosText;

    [SerializeField] Transform Player;

    [SerializeField] Material highlightMat;
    [SerializeField] Material defaultMat;

    [SerializeField] Transform oldHitObj;

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if(hit.collider.gameObject.tag == "Platform" || hit.collider.gameObject.tag == "Obsticle")
            {
                Transform objectHit = hit.transform;

                if (oldHitObj != objectHit && objectHit.gameObject.tag == "Platform")
                {
                    // Do something with the object that was hit by the raycast.
                    OnHit(objectHit);
                }
                else if (objectHit.gameObject.tag != "Platform")
                {
                    if (oldHitObj != null)
                    {
                        oldHitObj.GetComponent<MeshRenderer>().material = defaultMat;
                    }
                }
            }
        }
        else
        {
            if (oldHitObj != null)
            {
                oldHitObj.GetComponent<MeshRenderer>().material = defaultMat;
            }

            oldHitObj = null;
            selecetedPlatformPosText.text = "None";
        }
    }

    private void OnHit(Transform objectHit)
    {
        selecetedPlatformPosText.text = objectHit.gameObject.name;
        objectHit.GetComponent<MeshRenderer>().material = highlightMat;

        if(oldHitObj != null)
        {
            oldHitObj.GetComponent<MeshRenderer>().material = defaultMat;
        }

        oldHitObj = objectHit;
    }
}
