using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissDelightAIOriginal : EnemyAI
{
    public AISearchRoutine searchForPlayers;
    public override void DoAIInterval()
    {
        base.DoAIInterval(); //Required
        //Don't perform anything if everyone is dead already.
        if (StartOfRound.Instance.allPlayersDead)
        {
            return;
        }
        //Change ownership if needed.
        if (!base.IsServer)
        {
            ChangeOwnershipOfEnemy(StartOfRound.Instance.allPlayerScripts[0].actualClientId);
        }
        switch (currentBehaviourStateIndex)
        {
            //Searching State
            case 0:
                for (int i = 0; i < StartOfRound.Instance.allPlayerScripts.Length; i++)
                {
                    if (PlayerIsTargetable(StartOfRound.Instance.allPlayerScripts[i]) && !Physics.Linecast(base.transform.position + Vector3.up * 0.5f, StartOfRound.Instance.allPlayerScripts[i].gameplayCamera.transform.position, StartOfRound.Instance.collidersAndRoomMaskAndDefault) && Vector3.Distance(base.transform.position, StartOfRound.Instance.allPlayerScripts[i].transform.position) < 30f)
                    {
                        SwitchToBehaviourState(1);
                        return;
                    }
                }
                if (!searchForPlayers.inProgress)
                {
                    movingTowardsTargetPlayer = false;
                    StartSearch(base.transform.position, searchForPlayers);
                }
                break;
            //Targeted player State
            case 1:
                if (!searchForPlayers.inProgress)
                {
                    StopSearch(searchForPlayers);
                }
                if (TargetClosestPlayer())
                {
                    movingTowardsTargetPlayer = true;
                }
                else
                {
                    SwitchToBehaviourState(0);
                }
                break;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();  //Required

        if (isEnemyDead)
        {
            return;
        }
        if (currentBehaviourStateIndex != 1)
        {
            return;
        }


    }
}
