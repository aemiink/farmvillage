using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TractorController : MonoBehaviour
{
    public float moveSpeed = 10f;       // İleri geri hareket hızı
    public float turnSpeed = 50f;      // Dönüş hızı
    public bool isDriving = false;     // Oyuncu traktörü sürüyor mu?

    public Animator wheelAnimator;     // Tekerlek Animator referansı

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Eğer sürüş modunda değilsek hareketleri engelle
        if (!isDriving)
        {
            if (wheelAnimator != null)
            {
                wheelAnimator.SetBool("Turn", false); // Animasyonu durdur
            }
            return;
        }

        // İleri veya geri hareket (W/S tuşları)
        float moveInput = Input.GetAxis("Vertical") * moveSpeed;
        Vector3 move = transform.forward * moveInput * Time.fixedDeltaTime;

        // Sağa veya sola dönüş (A/D tuşları)
        float turnInput = Input.GetAxis("Horizontal") * turnSpeed * Time.fixedDeltaTime;
        Quaternion turn = Quaternion.Euler(0f, turnInput, 0f);

        // Hareket ve dönüşü uygula
        rb.MovePosition(rb.position + move);
        rb.MoveRotation(rb.rotation * turn);

        // Tekerlek animasyonu kontrolü (tuş girişine göre)
        if (wheelAnimator != null)
        {
            // W, A, S, D tuşlarına basılı olup olmadığını kontrol et
            bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || 
                            Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

            // Animator'a bilgiyi gönder
            wheelAnimator.SetBool("Turn", isMoving);
        }
    }
}
