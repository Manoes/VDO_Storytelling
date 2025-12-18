using Unity.Mathematics;
using UnityEngine;

public class BurnableHouse : MonoBehaviour
{
    [Header("VFX")]
    public Transform impactPoint;
    [SerializeField] private GameObject explosionVFXPrefab;
    [SerializeField] private GameObject firePrefabToEnable;
    [SerializeField] private GameObject debrisVFXPrefab;

    [Header("Audio")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip explosionClip;

    [Header("One-shot")]
    [SerializeField] private bool alreadyBurned;

    public void Ignite()
    {
        if(alreadyBurned) return;
        alreadyBurned = true;

        var point = impactPoint ? impactPoint.position : transform.position;

        if (explosionVFXPrefab) explosionVFXPrefab.SetActive(true);
        if(debrisVFXPrefab) Instantiate(debrisVFXPrefab, point, Quaternion.identity);
        if(firePrefabToEnable) firePrefabToEnable.SetActive(true);

        if(source && explosionClip)
            source.PlayOneShot(explosionClip);
    }
}
