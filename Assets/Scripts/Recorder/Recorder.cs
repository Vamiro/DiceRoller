using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class Recorder<TValue> : MonoBehaviour, IRecorder
{
    private int _numberOfSkippedFrames = 5;
    private int _recordFramesCounter = 0;
    private int _replayFramesCounter = 0;

    public event Action<TValue> InterpolatedValue;
    public event Action RestoredValue;
    public event Action DataHasEnded;

    private TValue _currentValue;
    private TValue _targetValue;

    private RecordBuffer<TValue> _buffer;

    private bool _isRecording;
    public bool IsRecording => _isRecording;
    
    private bool _isReplaying;

    [Inject]
    public void Construct(ReplayConfig replayConfig)
    {
        float recordsPerSecond = 1 / Time.fixedDeltaTime / replayConfig.NumberOfSkippedFrames;
        int capacity = Mathf.RoundToInt(replayConfig.MaximumSecondsToRecord * recordsPerSecond);
        _buffer = new RecordBuffer<TValue>(capacity);
    }

    private void FixedUpdate()
    {
        if (_isRecording)
        {
            if (_recordFramesCounter < _numberOfSkippedFrames)
            {
                _recordFramesCounter++;
            }
            else
            {
                _recordFramesCounter = 0;
                var recordValue = GetRecordValue();
                _buffer.Write(recordValue);
            }
        }
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

    public virtual void StartRecord()
    {
        _buffer.ResetValues();
        _recordFramesCounter = 0;
        _isRecording = true;
    }

    public virtual void StopRecord()
    {
        _isRecording = false;
    }

    public virtual void StartReplay()
    {
        _replayFramesCounter = 0;
        _isReplaying = true;
    }

    public void SetNumberOfSkippedFrames(int num)
    {
        _numberOfSkippedFrames = num;
    }
    
    public void CopyValues(Recorder<TValue> other)
    {
        _buffer.ResetValues();
        _buffer.RecordableValues = new List<TValue>(other._buffer.RecordableValues);
    }
    
    protected abstract void RestoreValue(ref TValue currentValue, ref TValue targetValue, float progress);
    
    protected virtual void OnInterpolated(TValue value) => InterpolatedValue?.Invoke(value);
    
    protected abstract TValue GetRecordValue();
    
    protected abstract bool IsDataValuesChanged(TValue recordableValue);
}
