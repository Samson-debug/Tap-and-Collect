using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO scoreIncreaseEvent;
    private int score;

    private void Start()
    {
        Debug.Log("Score: " + score);
    }

    private void OnEnable()
    {
        if (scoreIncreaseEvent != null){
            scoreIncreaseEvent.OnEventRaised += IncrementScore;
        }
    }

    private void OnDisable()
    {
        if (scoreIncreaseEvent != null){
            scoreIncreaseEvent.OnEventRaised -= IncrementScore;
        }
    }

    private void IncrementScore()
    {
        score++;
        Debug.Log("Score: " + score);
    }
}
