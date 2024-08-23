using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class Replayer<TValue> : MonoBehaviour
{
    private int _numberOfSkippedFrames = 5;
    private int _replayFramesCounter = 0;

    public event Action<TValue> InterpolatedValue;
    public event Action RestoredValue;
    public event Action DataHasEnded;

    private TValue _currentValue;
    private TValue _targetValue;

    private RecordBuffer<TValue> _buffer;

    public RecordBuffer<TValue> Buffer => _buffer;

    private bool _isReplaying;

    [Inject]
    public void Construct(RecorderConfig recorderConfig)
    {
        float recordsPerSecond = 1 / Time.fixedDeltaTime / recorderConfig.NumberOfSkippedFrames;
        int capacity = Mathf.RoundToInt(recorderConfig.MaximumSecondsToRecord * recordsPerSecond);
        _buffer = new RecordBuffer<TValue>(capacity);
    }

    private void FixedUpdate()
    {
        if (_isReplaying)
        {
            if (_replayFramesCounter > 0)
            {
                _replayFramesCounter--;
            }
            else
            {
                _replayFramesCounter = _numberOfSkippedFrames - 1;
                RestoredValue?.Invoke();
                ReadNewValue();
            }

            float progress = _replayFramesCounter / (float)_numberOfSkippedFrames;
            RestoreValue(ref _currentValue, ref _targetValue, progress);
        }
    }

    private void ReadNewValue()
    {
        if (_buffer.TryReadNextValue(out TValue currentValue))
        {
            _currentValue = currentValue;

            if (_buffer.RecordableValues.Count > 0)
            {
                _targetValue = _buffer.RecordableValues[0];
            }
        }
        else
        {
            DataHasEnded?.Invoke();
            _isReplaying = false;
        }
    }

    public virtual void StartReplay(RecordBuffer<TValue> other)
    {
        _buffer.ResetValues();
        _buffer.RecordableValues = new List<TValue>(other.RecordableValues);
        _replayFramesCounter = 0;
        _isReplaying = true;
    }
    
    public virtual void StartReplay()
    {
        if (_buffer.RecordableValues.Count == 0) return;
        _replayFramesCounter = 0;
        _isReplaying = true;
    }
    
    public virtual void StopReplay()
    {
        _isReplaying = false;
    }
    
    public virtual void ContinueReplay()
    {
        _isReplaying = true;
    }

    public void SetNumberOfSkippedFrames(int num)
    {
        _numberOfSkippedFrames = num;
    }
    
    protected abstract void RestoreValue(ref TValue currentValue, ref TValue targetValue, float progress);
    
    protected virtual void OnInterpolated(TValue value) => InterpolatedValue?.Invoke(value);
}
