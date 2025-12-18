using UnityEngine;

public class HouseBlastTrigger : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] private TripodShotEventReceiver tripodReceiver;
    [SerializeField] private TripodLocomotionInPlace tripodWalking;

    [Header("Tripod Control")]
    [SerializeField] private Animator tripodAnimator;
    [SerializeField] private string shootHouseTrigger = "PlaceHolder";
    [SerializeField] private BurnableHouse targetHouse;
    [SerializeField] private MonoBehaviour randomShootScriptToDisable;

    private bool fired;

    private void OnTriggerEnter(Collider other)
    {
        print("Tripod has triggered the Trigger!");

        // Stand in Place, get Ready to Shoot
        if (tripodWalking) tripodWalking.canWalk = false;
        tripodAnimator.SetTrigger(shootHouseTrigger);

        // Disable Randomshoot Script
        if(randomShootScriptToDisable) randomShootScriptToDisable.enabled = false;
    }
}
