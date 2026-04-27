using UnityEngine;

public class AudioReactiveLight : MonoBehaviour
{
    public Light targetLight;
    public float intensityMultiplier = 200f;
    public float threshold = 0.001f;
    [Range(0f, 1f)]
    public float smoothingFactor = 0.1f;

    private float smoothedLevel;
    private AudioSource audioSource;

    private float currentLevel;

    void Awake()
    {
        if(GetComponent<AudioSource>() != null)
        {
            audioSource = GetComponent<AudioSource>();
        } else
        {
            Debug.LogError("Missing audio Source");
        }
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        float sum = 0f;

        for (int i = 0; i < data.Length; i++)
        {
            sum += data[i] * data[i];
        }

        currentLevel = Mathf.Sqrt(sum / data.Length);
    }

    void Update()
    {
        if(!audioSource.isPlaying)
            return;

        float target = currentLevel;

        // soft gate
        target = Mathf.InverseLerp(threshold, threshold * 2f, target);

        smoothedLevel += (target - smoothedLevel) * 0.15f;

        targetLight.intensity = smoothedLevel * intensityMultiplier;
    }
}