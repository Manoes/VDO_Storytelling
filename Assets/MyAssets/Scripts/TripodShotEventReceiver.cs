using UnityEngine;

public class TripodShotEventReceiver : MonoBehaviour
{
    [Header("References")]
    public TripodLocomotionInPlace walkScript;

    [Header("Set by Trigger")]
    public BurnableHouse currentTarget;


    public void AE_HeatRayImpact()
    {
        if (currentTarget == null) return;

        // Blow up House
        currentTarget.Ignite();

        // Walk Again after blowing up House
        walkScript.canWalk = true;
        walkScript.WalkAgain();        
    }
}
