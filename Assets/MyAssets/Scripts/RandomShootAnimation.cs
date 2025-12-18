using UnityEngine;

public class RandomShootAnimation : MonoBehaviour
{
    [Header("Animations")]
    [SerializeField] private Animator animator;
    [SerializeField] private float minDelay = 2f;
    [SerializeField] private float maxDelay = 6f;

    [Header("Audio Clips")]
    [SerializeField] private AudioSource leftHeatRay;
    [SerializeField] private AudioSource rightHeatRay;

    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0f)
        {
            if(Random.value > 0.5f)
            {
                print("Shooting Left!");
                animator.SetTrigger("ShootLeft");
            }

            else
            {
                print("Shooting Right!");
                animator.SetTrigger("ShootRight");
            }                
            
            ResetTimer();
        }
    }

    void ResetTimer()
    {
        timer = Random.Range(minDelay, maxDelay);
    }

    public void PlayHeatRaySoundLeft()
    {
        leftHeatRay.Play();
    }

    public void PlayHeatRaySoundRight()
    {
        rightHeatRay.Play();
    }
}
