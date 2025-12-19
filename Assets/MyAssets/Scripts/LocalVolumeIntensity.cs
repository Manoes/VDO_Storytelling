using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Rendering;

public class LocalVolumeIntensity : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Volume volume;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform[] tripods;

    [Header("Distance -> Base Intensity")]
    [SerializeField] private float farDistance = 220f;
    [SerializeField] private float nearDistance = 35f;
    [SerializeField] private float baseSmooth = 3f;

    [Header("Heartbeat")]
    [Tooltip("Heartbeat speed in BPM")]
    [SerializeField] private float bpm = 78f;

    [Tooltip("How strong the Heartbeat affect the Volume Weight")]
    [SerializeField] private float pulseWeight = 0.25f;

    [Tooltip("How Much the Volume moves toward the Camera on each pulse")]
    [SerializeField] private float pulseMove = 1.2f;

    [Tooltip("Adds subtle irregularity to Heartbeat timing")]
    [SerializeField] private float jitter = 0.07f;

    [Header("Volume Position along Camera forward")]
    [SerializeField] private float baseForwardOffset = 0.8f;
    [SerializeField] private float positionSmooth = 8f;

    [Header("Clamp")]
    [SerializeField] private float minWeight = 0f;
    [SerializeField] private float maxWeight = 1f;

    float _jitterSeed;
    float _phase;
    float _base01;
    float _posPulse;

    void Awake()
    {
        if (!volume) volume = GetComponent<Volume>();
        if (!cam && Camera.main) cam = Camera.main.transform;
        _jitterSeed = Random.value * 1000f;
    }

    void Update()
    {
        if (!volume || !cam) return;

        // Base Intensity from nearest tripod Distance
        float nearest = float.PositiveInfinity;
        if (tripods != null)
        {
            for (int i = 0; i < tripods.Length; i++)
            {
                if (!tripods[i]) return;
                float d = Vector3.Distance(cam.position, tripods[i].position);
                if (d < nearest) nearest = d;
            }
        }

        float targetBase = (nearest < float.PositiveInfinity)
            ? Mathf.Clamp01(Mathf.InverseLerp(farDistance, nearDistance, nearest))
            : 0f;

        // Smooth base so it doesn't jitter
        _base01 = Damp(_base01, targetBase, baseSmooth);

        // HeartBeat Pulse
        float hz = bpm / 60f;

        // Add slight irregularity (subtle)
        float j = (Mathf.PerlinNoise(_jitterSeed, Time.time * 0.5f) - 0.5f) * 2f * jitter;
        float heartBeatHZ = Mathf.Max(0.1f, hz * (1f + j));

        _phase += heartBeatHZ * Time.deltaTime;

        // Two Pulses per Beat: Main Thump + Smaller Follow-up
        float p1 = Pulse01(_phase, 0.00f, 0.045f);
        float p2 = Pulse01(_phase, 0.18f, 0.035f);
        float pulse01 = Mathf.Clamp01(p1 + 0.6f * p2);

        // Pulse Strength scales with Base Intensity
        float pulseScaled = pulse01 * _base01;

        // Apply to Weight
        float targetWeight = _base01 + pulseScaled * pulseWeight;
        targetWeight = Mathf.Clamp(targetWeight, minWeight, maxWeight);
        volume.weight = targetWeight;

        // Apply to Position (Volume Breathes toward Camera)
        float targetPosPulse = pulseScaled * pulseMove;
        _posPulse = Damp(_posPulse, targetPosPulse, positionSmooth);

        Vector3 desiredPos = cam.position + cam.forward * (baseForwardOffset - _posPulse);
        transform.position = Vector3.Lerp(transform.position, desiredPos, 1f - Mathf.Exp(-positionSmooth * Time.deltaTime));

        // Keep orientation aligned 
        transform.rotation = cam.rotation;
    }

    // Smooth Damping helper
    float Damp(float current, float target, float speed)
    {
        return Mathf.Lerp(current, target, 1f - Mathf.Exp(-speed * Time.deltaTime));
    }

    // Returns a Sharp Pulse once per Cycle
    float Pulse01(float phase, float center, float width)
    {
        float x = Mathf.Repeat(phase, 1f);
        float d = Mathf.Abs(x - center);
        d = Mathf.Min(d, 1f - d);
        float t = Mathf.Clamp01(1f - d / Mathf.Max(0.0001f, width));
        return t * t;
    }
}
