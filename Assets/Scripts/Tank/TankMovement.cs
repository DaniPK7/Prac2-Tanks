using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;         
    public float m_Speed = 12f;            
    public float m_TurnSpeed = 180f;       
    public AudioSource m_MovementAudio;    
    public AudioClip m_EngineIdling;       
    public AudioClip m_EngineDriving;      
    public float m_PitchRange = 0.2f;

    
    private string m_MovementAxisName;     
    private string m_TurnAxisName;         
    private Rigidbody m_Rigidbody;         
    private float m_MovementInputValue;    
    private float m_TurnInputValue;        
    private float m_OriginalPitch;

    TankHealth THScript;

    public float slowAc =0f;
    public float fric=5f;
    public string LastInput;


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable ()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable ()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;

        THScript = FindObjectOfType<TankHealth>();
    }
    

    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);

        EngineAudio();

        //pruebas
        if (Input.GetKey(KeyCode.K)) { THScript.TakeDamage(2f); }

        //if (Input.GetKey(KeyCode.W)) { movement = transform.forward * 1 * slowAc * Time.deltaTime; }
        if (Input.GetKey(KeyCode.S)) { LastInput="S"; }
        if (Input.GetKey(KeyCode.W)) { LastInput="W"; }
    }


    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.

        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)   //Tank is Ideling
        {
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }


    private void FixedUpdate()
    {
        // Move and turn the tank.
        Move();
        
        
        Turn();
    }


    private void Move()
    {
        Vector3 movement = new Vector3();
        if(PressingWorS()) 
        { 
            slowAc = m_Speed;

            movement = transform.forward * m_MovementInputValue * slowAc * Time.deltaTime;
        }
        else 
        {
            if (LastInput == "W")
            {
                if (slowAc >= 0.05f)
                {
                    slowAc -= fric;
                    if (slowAc < 0)
                    {
                        slowAc = 0f;
                    }

                }
            }

            if (LastInput == "S")
            {
                slowAc= -1 * Mathf.Abs(slowAc); 
                if (slowAc <= -0.05f)
                {
                    slowAc += fric;
                    if (slowAc > 0)
                    {
                        slowAc = 0f;
                    }

                }
            }



            movement = transform.forward * slowAc * Time.deltaTime;

        }
        //movement = transform.forward * m_MovementInputValue * slowAc * Time.deltaTime;
    
        // Adjust the position of the tank based on the player's input.      
        

        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);

    }


    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }

    bool PressingWorS()
    {

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) { return true; }
        else return false;
    }
}