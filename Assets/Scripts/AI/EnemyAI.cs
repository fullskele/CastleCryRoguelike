using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour {

    [SerializeField]
    private List<SteeringBehaviour> steeringBehaviours;

    [SerializeField]
    private List<Detector> detectors;

    [SerializeField]
    private AIData aiData;

    [SerializeField]
    private float detectionDelay = 0.05f;

    private void Start() {
        //Detecting Player and Obstacles around
        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }

    private void PerformDetection() {

        foreach (Detector detector in detectors) {
            detector.Detect(aiData);
        }

        float[] danger = new float[8];
        float[] interest = new float[8];

        foreach (SteeringBehaviour behavior in steeringBehaviours) {
            (danger, interest) = behavior.GetSteering(danger, interest, aiData);
        }
    }
}