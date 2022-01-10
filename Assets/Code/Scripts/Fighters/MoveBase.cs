using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Move/Create new move")]

public class MoveBase : ScriptableObject
{

    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] FighterType type;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int pp;

    [SerializeField] bool isMagick;

    public string Name
    {
        get { return name; }
    }

    public string Description
    {
        get { return description; }
    }

    public FighterType Type
    {
        get { return type; }
    }

    public int Power
    {
        get { return power; }
    }

    public int Accuracy
    {
        get { return accuracy; }
    }

    public int PP
    {
        get { return pp; }
    }

    public bool IsMagick
    {
        get
        {
            return isMagick;
        }
    }
}
