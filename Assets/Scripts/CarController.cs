using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class CarroController : MonoBehaviour
{
    public float forca = 3000f;
    public float giro = 80f;
    private int count;
    Rigidbody rb;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
   

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        count = 0;

    }

    void FixedUpdate()
    {
        var k = Keyboard.current;
        float acel = (k.wKey.isPressed ? 1f : 0f) - (k.sKey.isPressed ? 1f : 0f);
        float dir = (k.dKey.isPressed ? 1f : 0f) - (k.aKey.isPressed ? 1f : 0f);

        rb.AddForce(transform.forward * acel * forca);
        rb.AddTorque(Vector3.up * dir * giro);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
            other.gameObject.SetActive(false);
        count = count + 1;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 4)
        {
            winTextObject.SetActive(true);
        }
    }

}
