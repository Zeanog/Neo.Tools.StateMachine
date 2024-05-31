using System.Collections.Generic;
using UnityEngine;

public class InsertShellsAnimatorEventHandler : AnimatorEventHandler
{
    protected List<int> insertShellCounts = new List<int>();

    protected override void Awake()
    {
        base.Awake();

        AddHandler("ReloadStart.Exit", "ReloadStartCompleted");
        AddHandler("ReloadEnd.Exit", "OnReloadComplete");

        AddHandler("InsertShell.Enter", (Animator animator, int layerIndex) => {
            while (layerIndex >= insertShellCounts.Count)
            {
                insertShellCounts.Add(0);
            }

            insertShellCounts[layerIndex] = 0;
        });

        AddHandler("InsertShell.Update", (Animator animator, int layerIndex) => {
            while (layerIndex >= insertShellCounts.Count)
            {
                insertShellCounts.Add(0);
            }

            var stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
            var count = (int)stateInfo.normalizedTime;
            if (count > insertShellCounts[layerIndex])
            {
                weapon.TriggerStateEvent("InsertCompleted", layerIndex);
            }
            insertShellCounts[layerIndex] = count;
        });
    }
}