using UnityEngine;

public class TripodShotEventReceiver : MonoBehaviour
{
    [Header("Set by Trigger")]
    public BurnableHouse currentTarget;

    [Header("Impact VFX/Audio")]
    [SerializeField] private GameObject impactVFXPrefab;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip impactClip;

    public void AE_HeatRayImpact()
    {
        if(currentTarget == null) return;

        currentTarget.Ignite();

        if (impactVFXPrefab)
        {
            var point = currentTarget.impactPoint ? currentTarget.impactPoint.position : currentTarget.transform.position;
            Instantiate(impactVFXPrefab, point, Quaternion.identity);
        }

        if(source && impactClip)
        {
            source.PlayOneShot(impactClip);
        }

        currentTarget = null;
    }
}
