using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mikealpha.AI.FSM
{

    [CreateAssetMenu(menuName = "Arte De MikeAlpha/AI/State")]
    public class State : ScriptableObject
    {
        public Action[] actions;
        public Transition[] transitions;

        public void UpdateState(StateController controller)
        {
            DoAction(controller);
            CheckForTransition(controller);
        }

        public void DoAction(StateController controller)
        {
            Debug.Log(actions.Length);
            for (int i = 0; i < actions.Length; i++)
            {
                actions[i].Act(controller);
            }
        }

        public void CheckForTransition(StateController controller)
        {
            for (int i = 0; i < transitions.Length; i++)
            {
                bool transitionSuceeded = transitions[i].decision.Decide(controller);
                if (transitionSuceeded)
                {
                    controller.TransitionToState(transitions[i].trueState);
                }
                else
                {
                    controller.TransitionToState(transitions[i].falseState);
                }
            }
        }
    }
}
