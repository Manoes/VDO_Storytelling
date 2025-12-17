using UnityEngine;

public class HouseBlastTrigger : MonoBehaviour
{
    [SerializeField] private TripodShotEventReceiver tripodReceiver;

    [Header("Tripod Control")]
    [SerializeField] private Animator tripodAnimator;
    [SerializeField] private string shootHouseTrigger = "PlaceHolder";
    [SerializeField] private BurnableHouse targetHouse;
    [SerializeField] private MonoBehaviour randomShootScriptToDisable;

    private bool fired;

    private void OTriggerEnter(Collider other)
    {
        if(fired) return;
        if(!other.CompareTag("Tripod")) return;

        fired = true;

        if(randomShootScriptToDisable) randomShootScriptToDisable.enabled = false;

        tripodReceiver.currentTarget = targetHouse;

        tripodAnimator.SetTrigger(shootHouseTrigger);
    }
}
