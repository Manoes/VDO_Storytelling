using UnityEngine;

public class DistantTripodHorn : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource source;

    [Header("Delay Parameters")]
    [SerializeField] private float minDelay = 20f;
    [SerializeField] private float maxDelay = 40f;

    void Awake()
    {
        if(!source) source = GetComponent<AudioSource>();
        SheduleNext();
    }

    void SheduleNext()
    {
        float delay = Random.Range(minDelay, maxDelay);
        Invoke(nameof(PlayHorn), delay);
    }

    void PlayHorn()
    {
        source.Play();
        print("Playing Horn!");
        SheduleNext();
    }
}
