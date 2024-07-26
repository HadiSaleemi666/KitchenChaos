using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private AudioSource audioSource;
    private float warningBurnTimer;
    private bool playWarningSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float progressBeforeWarning = .6f;

        playWarningSound = stoveCounter.IsFried() && e.progressNormalized >= progressBeforeWarning;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        if (playSound)
            audioSource.Play();
        else
            audioSource.Pause();
    }

    private void Update()
    {
        if (playWarningSound)
        {
            warningBurnTimer -= Time.deltaTime;

            if (warningBurnTimer <= 0)
            {
                float warningBurnTimerMax = .2f;
                warningBurnTimer = warningBurnTimerMax;
                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }
    }
}
