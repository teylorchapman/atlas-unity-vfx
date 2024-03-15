using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class AudioPeer : MonoBehaviour
{
    AudioSource audioSource;
    public static float[] samples = new float[512];
    public static float[] freqBand = new float[8];
    public static float[] bandBuffer = new float[8];
    private float[] bufferDecrease = new float[8];

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        GetAverageInBands();
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    public void BandBuffer()
    {
        for (int g = 0; g < 8; ++g)
        {
            if (freqBand [g] > bandBuffer [g])
            {
                bandBuffer [g] = freqBand [g];
                bufferDecrease [g] = 0.005f;
            }
            if (freqBand [g] < bandBuffer [g])
            {
                bandBuffer [g] -= bufferDecrease [g];
                bufferDecrease [g] *= 1.2f;
            }
        }
    }

    void MakeFrequencyBands()
    {
        float average = 0;
        int count = 0;
        for (int i =0; i < 8; i++)
        {
            int sampleCount = (int)Mathf.Pow (2, i) * 2;
            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                    count++;
            }
            average /= count;

            freqBand[i] = average * 10;
        }
    }
    public float GetAverageInBands()
    {
        float average = 0;
        for (int i = 0; i < 8; i++)
        {
            average += freqBand[i];
        }
        average /= 8;

        return average;
    }
}
