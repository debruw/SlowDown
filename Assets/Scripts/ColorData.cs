using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ColorData", menuName = "Color Data", order = 51)]
public class ColorData : ScriptableObject
{
    [SerializeField]
    public ColorGroup[] colorGroups;
}

[System.Serializable]
public class ColorGroup
{
    public Color[] colors;
}
