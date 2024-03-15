using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NoiseFlowField))]
public class AudioFlowField : MonoBehaviour
{
    NoiseFlowField noiseFlowField;
    public AudioPeer audioPeer;
    // Speed Stuff
    public bool useSpeed;
    public Vector2 moveSpeedMinMax, rotateSpeedMinMax;
    // Scale Stuff
    public bool useScale;
    public Vector2 scaleMinMax;
    
    void Start()
    {
        noiseFlowField = GetComponent<NoiseFlowField>();
        int countBand = 0;
        for (int i = 0; i < noiseFlowField.amountOfParticles; i++)
        {
            int band = countBand % 8;
            noiseFlowField.particles[i].audioBand = band;
            countBand++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (useSpeed)
        {
            noiseFlowField.particleMoveSpeed = Mathf.Lerp(moveSpeedMinMax.x, moveSpeedMinMax.y, audioPeer.GetAverageInBands());
            noiseFlowField.particleRotateSpeed = Mathf.Lerp(rotateSpeedMinMax.x, rotateSpeedMinMax.y, audioPeer.GetAverageInBands());
        }
        for (int i = 0; i < noiseFlowField.amountOfParticles; i++)
        {
            if (useScale)
            {
                float scale = Mathf.Lerp(scaleMinMax.x, scaleMinMax.y, AudioPeer.bandBuffer[noiseFlowField.particles[i].audioBand]);
                noiseFlowField.particles[i].transform.localScale = new Vector3(scale, scale, scale);
            }
        }
    }
}
