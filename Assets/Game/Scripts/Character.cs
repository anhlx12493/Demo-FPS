using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public enum Behaviour { idle,walk,run,jumpUp,jumpDown,jumpOver};
    public Behaviour behaviour
    {
        get
        {
            return _behaviour;
        }
    }
    Behaviour _behaviour;
    protected bool isBehaviourAbleChange = true;

    public void SetBehaviour(Behaviour behaviour)
    {
        if (isBehaviourAbleChange)
        {
            _behaviour = behaviour;
        }
    }

    public abstract void MoveLeft();

    public abstract void MoveRight();

    public abstract void MoveUp();

    public abstract void MoveDown();

    public abstract void Faster();

    public abstract void Lowwer();
}
