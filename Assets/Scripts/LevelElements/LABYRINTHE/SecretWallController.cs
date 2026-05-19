using UnityEngine;

public class SecretWallController : MonoBehaviour
{
    [SerializeField] private float topY = 0f;
    [SerializeField] private float bottomY = -3f;

    public void ToggleYPosition()
    {
        Vector3 position = transform.position;
        float targetY = Mathf.Approximately(position.y, topY) ? bottomY : topY;
        transform.position = new Vector3(position.x, targetY, position.z);
    }
}
