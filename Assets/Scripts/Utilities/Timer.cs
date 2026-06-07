using System;

namespace Core.Utilities
{
    /// <summary>
    /// A modular and decoupled timer that can be updated manually.
    /// Does not inherit from MonoBehaviour to ensure it can be used anywhere.
    /// </summary>
    public class Timer
    {
        public float Duration { get; private set; }
        public float CurrentTime { get; private set; }
        public bool IsRunning { get; private set; }
        
        /// <summary>
        /// Returns the progress of the timer from 0 to 1.
        /// 0 means just started, 1 means completed.
        /// </summary>
        public float Progress => Duration > 0 ? 1f - (CurrentTime / Duration) : 0f;

        public event Action OnTimerStart;
        public event Action OnTimerComplete;
        public event Action<float> OnTimerTick;

        public Timer(float duration)
        {
            Duration = duration;
            CurrentTime = duration;
            IsRunning = false;
        }

        public void Start()
        {
            if (Duration > 0 && !IsRunning)
            {
                IsRunning = true;
                OnTimerStart?.Invoke();
            }
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void Pause()
        {
            IsRunning = false;
        }

        public void Resume()
        {
            if (CurrentTime > 0)
            {
                IsRunning = true;
            }
        }

        public void Reset()
        {
            CurrentTime = Duration;
        }

        public void Reset(float newDuration)
        {
            Duration = newDuration;
            CurrentTime = Duration;
        }

        /// <summary>
        /// Call this method from an external Update loop (e.g., GameManager or EffectManager)
        /// passing in Time.deltaTime.
        /// </summary>
        /// <param name="deltaTime">The time passed since the last frame.</param>
        public void Update(float deltaTime)
        {
            if (!IsRunning) return;

            CurrentTime -= deltaTime;
            OnTimerTick?.Invoke(CurrentTime);

            if (CurrentTime <= 0f)
            {
                CurrentTime = 0f;
                IsRunning = false;
                OnTimerComplete?.Invoke();
            }
        }
    }
}
