using UnityEngine;

public class PoliceGhostFollower : MonoBehaviour
{
    public PlayerPathRecorder playerRecorder;
    public float followSpeed = 8f;
    public int delayFrames = 20; // semakin besar → polisi makin telat

    void Update()
    {
        if (playerRecorder == null) return;
        if (playerRecorder.positions.Count <= delayFrames) return;

        Vector3 targetPos = playerRecorder.positions[
            playerRecorder.positions.Count - 1 - delayFrames
        ];

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            followSpeed * Time.deltaTime
        );

        // flip sprite otomatis
        if (targetPos.x != transform.position.x)
        {
            float dir = targetPos.x - transform.position.x;
            transform.localScale = new Vector3(
                Mathf.Sign(dir),
                1f,
                1f
            );
        }
    }
}
