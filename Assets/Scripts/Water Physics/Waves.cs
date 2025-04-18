using UnityEngine;

public class Waves : MonoBehaviour
{
    public static Waves instance;

    [SerializeField] private float amplitude = 0.05f; // Wave height
    [SerializeField] private float frequency = 0.2f;
    [SerializeField] private float speed = 1f; // Wave speed

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public float GetWaveHeight(float xPos, float zPos)
    {
        float waveX = Mathf.Sin(frequency * xPos + Time.time * speed);
        float waveZ = Mathf.Sin(frequency * zPos + Time.time * speed);
        return amplitude * (waveX + waveZ) / 2f;
    }
}
