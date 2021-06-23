using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mikealpha.AI.FSM
{
    public abstract class Action
    {
        public abstract void Act(StateController controller);
        public abstract void Enter();
        public abstract void Exit();
        public abstract void Draw();
    }
}
