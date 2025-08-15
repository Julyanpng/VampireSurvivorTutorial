using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemsScribtableObject", menuName = "ScriptableObjects/Passive Item")]
public class PassiveItemScribtableScript : ScriptableObject
{
    [SerializeField]
    float multiplier;
    public float Multiplier { get => multiplier; private set => multiplier = value; }

    [SerializeField]
    int level; //Not meant to be modified in the game (only in editor)
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    GameObject nextLevelPrefab; //Prefab of the next level
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

    [SerializeField]
    new string name;
    public string Name { get => name; private set => name = value; }

    [SerializeField]
    string description;
    public string Description { get => description; private set => description = value; }

    [SerializeField]
    Sprite icon;
    public Sprite Icon { get => icon; private set => icon = value; }
}
