using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class CarControllerWC : MonoBehaviour
{

    private int count;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public GameObject winPanel;
    public VideoPlayer winVideo;
    public AudioSource src;
    public AudioClip sfx1, sfx2;
    private bool venceu = false;
    public float motorForca = 100f;
    public float brakeForca = 1000f;
    public float maxAnguloSteer = 30f;

    public WheelCollider frenteEsquerdaWheelCollider;
    public WheelCollider frenteDireitaWheelCollider;
    public WheelCollider traseiraEsquerdaWheelCollider;
    public WheelCollider traseiraDireitaWheelCollider;

    public Transform frenteEsquerdaWheelTransform;
    public Transform frenteDireitaWheelTransform;
    public Transform traseiraEsquerdaWheelTransform;
    public Transform traseiraDireitaWheelTransform;

    private float horizontalInput;
    private float verticalInput;
    private float atualAnguloSteer;
    private float atualBrakeForca;
    private bool isBraking;
    private Rigidbody rb;

    private void GetInput() // define os inputs (teclas de movimentação, setinhas, wasd)
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor() // aplica força vertical nas rodas, se o carro frear, as rodas vao perdendo força tbm
    {
        frenteDireitaWheelCollider.motorTorque = -verticalInput * motorForca;
        frenteEsquerdaWheelCollider.motorTorque = -verticalInput * motorForca;

        atualBrakeForca = isBraking ? brakeForca : 0f;
        Frear();
    }

    private void Frear() // método de freio ne vei, se frear freou
    {
        frenteEsquerdaWheelCollider.brakeTorque = atualBrakeForca;
        frenteDireitaWheelCollider.brakeTorque = atualBrakeForca;
        traseiraEsquerdaWheelCollider.brakeTorque = atualBrakeForca;
        traseiraDireitaWheelCollider.brakeTorque = atualBrakeForca;
    }

    private void HandleSteering() // método p mudar o angulo da roda, se o carro virar, a roda vira tbm, pega o angulo do input horizontal e vai atualizando a roda
    {
        atualAnguloSteer = maxAnguloSteer * horizontalInput;
        frenteEsquerdaWheelCollider.steerAngle = atualAnguloSteer;
        frenteDireitaWheelCollider.steerAngle = atualAnguloSteer;
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform) // atualiza rotação e posição da roda e aplica no modelo 3D
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    private void UpdateWheels() // é pras rodas acompanhar o carro e virar junto dele²
    {
        UpdateSingleWheel(frenteDireitaWheelCollider, frenteDireitaWheelTransform);
        UpdateSingleWheel(frenteEsquerdaWheelCollider, frenteEsquerdaWheelTransform);
        UpdateSingleWheel(traseiraDireitaWheelCollider, traseiraDireitaWheelTransform);
        UpdateSingleWheel(traseiraEsquerdaWheelCollider, traseiraEsquerdaWheelTransform);
    }
    void Start()
    { 
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        count = 0;
        winTextObject.SetActive(false);
        winPanel.SetActive(false);

        winVideo.source = VideoSource.Url;
        winVideo.url = Application.streamingAssetsPath + "/carspin.mp4";
        winVideo.Prepare();
    }

    void Update()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        SetCountText();
    }

    public void Coin()
    {
        src.PlayOneShot(sfx1);
        src.Play();
    }

    public void Yippie() 
    {
        src.PlayOneShot(sfx2);
        src.Play();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            Coin();
        }
        SetCountText();
    }

    void SetCountText()
    {
        countText.text = "Colecionáveis: " + count.ToString() + " / 36";

        if (count >= 36 && !venceu)
        {
            venceu = true;
            winTextObject.SetActive(true);
            winPanel.SetActive(true);
            winVideo.Play();
            Yippie();
        }

    }
}
