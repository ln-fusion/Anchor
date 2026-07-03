using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;        // 跟踪的目标
    public Vector3 offset = new Vector3(0, 0, -10); // 相机偏移
    public float smoothSpeed = 5f;  // 平滑速度，越小越慢

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position + offset;

        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
    }
}
