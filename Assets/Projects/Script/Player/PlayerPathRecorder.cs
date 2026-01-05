using System.Collections.Generic;
using UnityEngine;

public class PlayerPathRecorder : MonoBehaviour
{
    [Header("Record Settings")]
    public float recordInterval = 0.05f; // semakin kecil, semakin akurat
    public int maxRecords = 1000;         // batas data (biar tidak berat)

    [HideInInspector]
    public List<Vector3> positions = new List<Vector3>();

    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= recordInterval)
        {
            timer = 0f;

            positions.Add(transform.position);

            if (positions.Count > maxRecords)
            {
                positions.RemoveAt(0); // buang data lama
            }
        }
    }
}
