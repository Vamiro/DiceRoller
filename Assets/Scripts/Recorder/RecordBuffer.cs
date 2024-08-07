using System;
using System.Collections.Generic;

public class RecordBuffer<TValue>
{
    private List<TValue> _recordableValues;

    public List<TValue> RecordableValues
    {
        get => _recordableValues;
        set => _recordableValues = value;
    }

    private int _capacity;

    public RecordBuffer(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity));

        _capacity = capacity;
        _recordableValues = new List<TValue>(capacity);
    }
    

    public void Write(TValue recordableValue)
    {
        if (_capacity <= _recordableValues.Count)
            _recordableValues.RemoveAt(0);

        _recordableValues.Add(recordableValue);
    }

    public bool TryReadNextValue(out TValue value)
    {
        if (_recordableValues.Count == 0)
        {
            value = default;
            return false;
        }

        value = _recordableValues[0];

        _recordableValues.RemoveAt(0);

        return true;
    }

    public void ResetValues()
    {
        _recordableValues.Clear();
    }
}
