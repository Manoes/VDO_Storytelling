using UnityEngine;

public class TripodLocomotionInPlace : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;    
    [SerializeField] private string walkAgainTrigger = "WalkAgain";
    [SerializeField] private AnimationClip walkClip;

    [Tooltip("Meters traveled per FULL walk loop (at normal walk speed).")]
    [SerializeField] private float metersPerCycle = 2f;

    [Tooltip("Scales ONLY movement, not animation.")]
    [SerializeField] private float moveMultiplier = 1f;

    private float cycleDuration = 1f;
    public bool canWalk = true;

    void Awake()
    {
        if (!animator) animator = GetComponentInChildren<Animator>();
        if (walkClip) cycleDuration = Mathf.Max(0.0001f, walkClip.length);
    }

    public void WalkAgain()
    {
        animator.SetTrigger(walkAgainTrigger);
    }

    void Update()
    {
        if (!walkClip || !canWalk) return;

        float moveSpeed = (metersPerCycle / cycleDuration) * moveMultiplier;
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
