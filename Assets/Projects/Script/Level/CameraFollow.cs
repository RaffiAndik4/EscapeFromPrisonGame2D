using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothing = 5f;

    public float minX = -10f;
    public float maxX = 50f;

    public Vector3 offset;   // posisi player relatif terhadap kamera

    private float fixedY;

    void Start()
    {
        fixedY = transform.position.y;  // kunci Y
    }

    void LateUpdate()
    {
        float targetX = player.position.x + offset.x;

        // batasi kamera
        targetX = Mathf.Clamp(targetX, minX, maxX);

        Vector3 targetPos = new Vector3(targetX, fixedY + offset.y, transform.position.z + offset.z);

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
    }
}
