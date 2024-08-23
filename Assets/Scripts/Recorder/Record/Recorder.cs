using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class Recorder<TValue> : MonoBehaviour
{
    private int _numberOfSkippedFrames;
    private int _recordFramesCounter;
    
    private RecordBuffer<TValue> _buffer;
    public RecordBuffer<TValue> Buffer => _buffer;

    private bool _isRecording;
    public bool IsRecording => _isRecording;

    [Inject]
    public void Construct(RecorderConfig recorderConfig)
    {
        float recordsPerSecond = 1 / Time.fixedDeltaTime / recorderConfig.NumberOfSkippedFrames;
        int capacity = Mathf.RoundToInt(recorderConfig.MaximumSecondsToRecord * recordsPerSecond);
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

    public void SetNumberOfSkippedFrames(int num)
    {
        _numberOfSkippedFrames = num;
    }
    
    protected abstract TValue GetRecordValue();
    
    protected abstract bool IsDataValuesChanged(TValue recordableValue);
}
