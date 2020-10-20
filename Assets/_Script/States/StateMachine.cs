using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public static State currentState;
    public static List<State> allStates = new List<State>();

    public static void GoToState(PlayerBehaviour pb, string newstate)
    {
        if (currentState != null)
        {
            currentState.OnStateExit(pb);
        }
        foreach (var s in allStates)
        {
            if (s.GetType().ToString() == newstate)
            {
                currentState = s;

                pb.ChangeState(currentState);

                s.OnStateEnter(pb);
                Debug.Log("Changed state to " + currentState.GetType().ToString());
                return;
            }
        }
        Debug.LogError("State " + "'" + newstate + "'" + " doesnt exist");
    }

    public static State CurrentState()
    {
        return currentState;
    }
}
