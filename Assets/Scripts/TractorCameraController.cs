using UnityEngine;

public class TractorCameraController : MonoBehaviour
{
    public Transform target;  // Kameranın takip edeceği traktör veya başka bir hedef
    public float distance = 5.0f;  // Kameranın hedeften uzaklığı
    public float height = 2.0f;    // Kameranın yükseklik farkı
    public float rotationSpeed = 100.0f;  // Fare hassasiyeti

    private float currentX = 0.0f;
    private float currentY = 0.0f;

    private void LateUpdate()
    {
        // Fare hareketlerini al
        currentX += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        currentY -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        // Kameranın konumunu ve rotasyonunu hesapla
        Vector3 direction = new Vector3(0, height, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = target.position + rotation * direction;

        // Kamerayı hedefe çevir
        transform.LookAt(target);
    }
}
