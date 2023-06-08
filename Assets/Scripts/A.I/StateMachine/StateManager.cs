using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] private States currentstates;

    // Update is called once per frame
    void Update()
    {
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        States nextState = currentstates?.RunCurrentState(); //Checking if it's null, if not then ignore

        if(nextState != null)
        {
            //Switch to the next state
            SwitchtoNextState(nextState);
        }
    }

    private void SwitchtoNextState(States nextState)
    {
        currentstates = nextState;
    }
}
