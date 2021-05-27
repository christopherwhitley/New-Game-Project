using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*[System.Serializable]*/
[CreateAssetMenu(menuName = "State", order = 1)]

public class State : ScriptableObject
{

    [SerializeField] public string name;
    [SerializeField] public GameObject questItem;

    [TextArea(10, 14)]
    [SerializeField] public string[] sentences;

 




}
