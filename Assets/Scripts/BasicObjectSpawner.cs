using UnityEditor;
using UnityEngine;
using System.Collections;

public class BasicObjectSpawner : EditorWindow
{
    int objectX = 1;
    int objectY = 1;
    ObsticleLocations obsticleLocations;

    [MenuItem("Tools/ObsticleSpawner")]
    public static void ShowWindow()
    {
        GetWindow(typeof(BasicObjectSpawner));      //GetWindow is a method inherited from the EditorWindow class
    }

    private void OnGUI()
    {
        GUILayout.Label("Spawn New Obsticle", EditorStyles.boldLabel);
        
        objectX = EditorGUILayout.IntField("Object X Location", objectX);
        objectY = EditorGUILayout.IntField("Object Y Location", objectY);
        obsticleLocations = EditorGUILayout.ObjectField("Obsticle Scriptable Object", obsticleLocations, typeof(ObsticleLocations), false) as ObsticleLocations;

        if (GUILayout.Button("Spawn Obsticle"))
        {
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        if(obsticleLocations == null)
        {
            Debug.LogError("Error: Please assign an object to be spawned.");
            return;
        }

        if(Application.isPlaying)
        {
            return;
        }
        else
        {
            bool alreadyExists = false;

            for(int i = 0; i < obsticleLocations.obsticleLocations.Count; i++)
            {
                if(obsticleLocations.obsticleLocations[i] == objectX + ", " + objectY)
                {
                    alreadyExists = true;
                    break;
                }
            }

            if(!alreadyExists)
            {
                obsticleLocations.obsticleLocations.Add(objectX + ", " + objectY);
            }
            else
            {
                Debug.Log("Already Exists");
            }    
        }
    }
}
