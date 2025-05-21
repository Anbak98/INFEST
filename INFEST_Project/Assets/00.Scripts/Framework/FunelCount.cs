using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class FunelCount : NetworkBehaviour
{
    [SerializeField] private int FunelKey = -1;
    [SerializeField] private int DurationSec = -1;

    bool IsCounted = false;

    public override void FixedUpdateNetwork()
    {
        if (Runner.SimulationTime > DurationSec && IsCounted == false)
        {
            AnalyticsManager.SendFunnelStep(FunelKey);
            IsCounted = true;
            this.enabled = false;
        }
    }
}
