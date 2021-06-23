using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mikealpha.AI.FSM;

[System.Serializable]
public class Transition
{
    public Decision decision;
    public State trueState;
    public State falseState;
}
    
