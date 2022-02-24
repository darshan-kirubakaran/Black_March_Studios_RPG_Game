using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObsticleLocations", menuName = "ScriptableObjects/ObsticleLocations", order = 1)]
public class ObsticleLocations : ScriptableObject
{
    public List<string> obsticleLocations;
}
