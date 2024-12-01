using UnityEngine;

public class TractorInteraction : MonoBehaviour
{
    public GameObject player;         // Oyuncu nesnesi
    public GameObject tractor;        // Traktör nesnesi
    public Transform tractorSeat;     // Traktörün oturma noktası
    public GameObject interactionUI;  // "E'ye bas" mesajı için UI
    public Camera playerCamera;       // Oyuncunun kamerası
    public Camera tractorCamera;      // Traktör kamerası

    private bool isNearTractor = false;  // Oyuncu traktörün yakınında mı?
    private bool isDriving = false;      // Oyuncu traktör sürüyor mu?
    private TractorController tractorController;

    private void Start()
    {
        // Traktör kontrol scriptine referans al
        tractorController = tractor.GetComponent<TractorController>();

        // Başlangıçta sürüş kapalı
        if (tractorController != null)
            tractorController.isDriving = false;

        // UI ve kamerayı başlangıçta kapalı yap
        if (interactionUI != null)
            interactionUI.SetActive(false);

        if (tractorCamera != null)
            tractorCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Eğer oyuncu traktörün yakınındaysa ve E tuşuna basarsa
        if (isNearTractor && Input.GetKeyDown(KeyCode.E))
        {
            if (!isDriving)
                EnterTractor();  // Traktöre bin
            else
                ExitTractor();   // Traktörden in
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Oyuncu traktör algılama alanına girdiğinde
        if (other.gameObject == player)
        {
            isNearTractor = true;
            if (interactionUI != null && !isDriving)
                interactionUI.SetActive(true);  // UI mesajını göster
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Oyuncu traktör algılama alanından çıktığında
        if (other.gameObject == player)
        {
            isNearTractor = false;
            if (interactionUI != null)
                interactionUI.SetActive(false);  // UI mesajını gizle
        }
    }

    private void EnterTractor()
    {
        isDriving = true;

        // UI'yi gizle
        if (interactionUI != null)
            interactionUI.SetActive(false);

        // Oyuncuyu traktörün oturma noktasına taşı
        player.SetActive(false);  // Oyuncuyu gizle
        player.transform.position = tractorSeat.position;
        player.transform.rotation = tractorSeat.rotation;

        // Kameraları değiştir
        if (playerCamera != null)
            playerCamera.gameObject.SetActive(false);

        if (tractorCamera != null)
        {
            tractorCamera.gameObject.SetActive(true);

            // Kamera kontrolünü aktif hale getir
            TractorCameraController cameraController = tractorCamera.GetComponent<TractorCameraController>();
            if (cameraController != null)
                cameraController.enabled = true;
        }

        // Traktör sürüş modunu aç
        if (tractorController != null)
            tractorController.isDriving = true;

        Debug.Log("Traktöre bindiniz!");
    }

    private void ExitTractor()
    {
        isDriving = false;

        // UI'yi tekrar etkinleştir
        if (interactionUI != null)
            interactionUI.SetActive(true);

        // Oyuncuyu traktörün dışına taşı
        player.SetActive(true);  // Oyuncuyu görünür yap
        player.transform.position = tractor.transform.position + Vector3.right * 2;  // Traktörün yanına bırak

        // Kameraları değiştir
        if (tractorCamera != null)
        {
            // Kamera kontrolünü devre dışı bırak
            TractorCameraController cameraController = tractorCamera.GetComponent<TractorCameraController>();
            if (cameraController != null)
                cameraController.enabled = false;

            tractorCamera.gameObject.SetActive(false);
        }

        if (playerCamera != null)
            playerCamera.gameObject.SetActive(true);

        // Traktör sürüş modunu kapat
        if (tractorController != null)
            tractorController.isDriving = false;

        Debug.Log("Traktörden indiniz!");
    }
}
