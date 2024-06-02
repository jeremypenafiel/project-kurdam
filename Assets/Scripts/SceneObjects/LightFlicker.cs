using Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private Light2D light;

    private int frames = 0;
    Aswang player;
    [SerializeField] private int framesPerRandomize;

    private float minValue;
    private float maxValue;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        frames++;
        if (frames % framesPerRandomize == 0)
        {
            RandomizeIntensity();
        }
    }

    public void setMaxValue(float value)
    {
        maxValue = value;
    }

    void RandomizeIntensity()
    {
        minValue = maxValue - 0.1f;
        // Create an instance of the Random class
        System.Random random = new System.Random();


        float randomValue = (float)(random.NextDouble() * (maxValue - minValue) + minValue);

        light.intensity = randomValue;
    }
}