using UnityEngine;

public class ElectricalRotatableElement : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField, Range(0, 11)] private int currentPosition;
    [SerializeField, Range(0, 11)] private int targetPosition;
    [SerializeField] private float angleOffset;

    public int CurrentPosition => currentPosition;
    public int TargetPosition => targetPosition;

    private void Awake()
    {
        ApplyRotationImmediate();
    }

    public void RotateNextPosition()
    {
        currentPosition = (currentPosition + 1) % 12;
        ApplyRotationImmediate();
    }

    public bool IsInCorrectPosition()
    {
        return currentPosition == targetPosition;
    }

    private void ApplyRotationImmediate()
    {
        float angle = (currentPosition * 30f) + angleOffset;
        transform.localRotation = Quaternion.Euler(0f, angle, 0f);
    }
}
