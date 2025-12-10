using UnityEngine;   // <- WAJIB ADA

public class CameraFollowVertical : MonoBehaviour
{
    public Transform player;
    public float smoothing = 3f;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPos = new Vector3(
            player.position.x,
            player.position.y,
            transform.position.z
        );

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
    }
}
