using UnityEngine;

public class TransformRecorder : Recorder<TransformValues>
{
    private Quaternion _rotationShift;

    public Quaternion RotationShift
    {
        get => _rotationShift;
        set => _rotationShift = value;
    }

    protected override TransformValues GetRecordValue()
        => new TransformValues(transform.position, transform.rotation);

    protected override bool IsDataValuesChanged(TransformValues lastValue) 
        => lastValue.Position != transform.position || lastValue.Rotation != transform.rotation;

    protected override void RestoreValue(TransformValues currentValue, TransformValues targetValue, float progress)
    {
        transform.position = Vector3.Lerp(targetValue.Position, currentValue.Position, progress);
        transform.rotation = Quaternion.Lerp(targetValue.Rotation * _rotationShift, currentValue.Rotation * _rotationShift, progress);
        OnInterpolated(new TransformValues(transform.position, transform.rotation));
    }

    protected override void OnInterpolated(TransformValues value) => base.OnInterpolated(value);
}
