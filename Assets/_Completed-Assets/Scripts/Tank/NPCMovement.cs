using UnityEngine;
using UnityEngine.AI;

namespace Complete
{
    public class NPCMovement : MonoBehaviour
    {
        Transform closestTank;
        public Transform[] players;               // Reference to the player's position.
        //TankHealth [] playerHealth;      // Reference to the player's health.
        NPCHealth enemyHealth;        // Reference to this enemy's health.
        NavMeshAgent nav;               // Reference to the nav mesh agent.

        float minDistance = 300;
        /*Animator EnemyAnim;
        bool EnemyMoving;*/
        public float range = 150f;

        TankMovement PlayerMovScript;
        //public Transform Spawn;               // Reference to the spawn's position.
        float nearDistance;

        void Awake()
        {
            // Set up the references.

            //player = GameObject.Find("Player").transform;
            //playerHealth  = players.GetComponent<TankHealth>();
            enemyHealth = GetComponent<NPCHealth>();
            nav = GetComponent<NavMeshAgent>();

            //EnemyAnim = GetComponent<Animator>();

            PlayerMovScript = FindObjectOfType<TankMovement>();

        }
        void checkPositions()
        {
            float distanceTank0 = Vector3.Distance(players[0].position, transform.position);
            float distanceTank2 = Vector3.Distance(players[1].position, transform.position);

            if (distanceTank0 < distanceTank2)
            {
                nearDistance = distanceTank0;
                closestTank = players[0];
            }

            else if (distanceTank0 > distanceTank2)
            {
                nearDistance = distanceTank2;
                closestTank = players[1];
            }


            print("Distancia Tanque 0:"+ distanceTank0+

            "\nDistancia Tanque 1:"+ distanceTank2+

            "\nEl tanque más cercano es: " + closestTank.name);
        }
        /*public Transform nearTank()
        {
            Transform closestTank;
            float minDistance =300;

            for(int i=0; i < players.Length; i++)
            {
                float distance= Vector3.Distance(players[i].position, transform.position);
                if (distance < minDistance) { closestTank = players[i]; }
            }
            return closestTank.transform;
        }*/
        private void FixedUpdate()
        {

            /*for (int i = 0; i < players.Length; i++)
            {
                float distance = Vector3.Distance(players[i].position, transform.position);
                if (distance < minDistance) { closestTank = players[i]; }
            }*/
            checkPositions();

            if (nearDistance <= range)
            {
                nav.isStopped = false;

                nav.SetDestination(closestTank.position);
            }
            else
            {
                //nav.Stop();
                nav.isStopped = true;
            }
            //Transform closePlayer = nearTank();
            //float PlayerDist = Vector3.Distance(closestTank.position, transform.position);

            //nav.SetDestination(closestTank.position);

        }
        /*void Update()

        {
            
            float minDistance = 300;

            for (int i = 0; i < players.Length; i++)
            {
                float distance = Vector3.Distance(players[i].position, transform.position);
                if (distance < minDistance) { closestTank = players[i]; }
            }

            //Transform closePlayer = nearTank();
            float PlayerDist = Vector3.Distance(closestTank.position, transform.position);
            nav.SetDestination(closestTank.position);
            /*float SpawnDist = Vector3.Distance(Spawn.position, transform.position);*/
            //print(dist);
            /*if (enemyHealth.currentHealth > 0 /*&& playerHealth.m_CurrentHealth > 0)
            {
                if (!PlayerMovScript.PlayerIsSafe)  //If the Player is not in the safe zone...
                {
                    if (PlayerDist <= range) //Chase player
                    {
                        //EnemyMoving = true;
                        nav.SetDestination(closestTank.position);
                    }
                    else
                    {
                        //nav.Stop();
                        //EnemyMoving = false;
                    }
                }
                else // If the Player is in the safe zone...
                {
                    //goToSpawn(SpawnDist);
                }
            }*/
            //EnemyAnim.SetBool("InRange", EnemyMoving);

            // If the enemy and the player have health left...

            /*if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
            {
                // ... set the destination of the nav mesh agent to the player.
                nav.SetDestination(player.position);
            }
            // Otherwise...
            else
            {
                // ... disable the nav mesh agent.
                nav.enabled = false;
            }*/

        }

        /*void goToSpawn(float d)
        {
            nav.SetDestination(Spawn.position);
            if (d < 3)
            {
                //EnemyMoving = false;
            }
        }*/
        /*
        public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
        public float m_Speed = 12f;                 // How fast the tank moves forward and back.
        public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.
        public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
        public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
        public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
		public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.

        private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
        private string m_TurnAxisName;              // The name of the input axis for turning.
        private Rigidbody m_Rigidbody;              // Reference used to move the tank.
        private float m_MovementInputValue;         // The current value of the movement input.
        private float m_TurnInputValue;             // The current value of the turn input.
        private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.
        private ParticleSystem[] m_particleSystems; // References to all the particles systems used by the Tanks

        private void Awake ()
        {
            m_Rigidbody = GetComponent<Rigidbody> ();
        }


        private void OnEnable ()
        {
            // When the tank is turned on, make sure it's not kinematic.
            m_Rigidbody.isKinematic = false;

            // Also reset the input values.
            m_MovementInputValue = 0f;
            m_TurnInputValue = 0f;

            // We grab all the Particle systems child of that Tank to be able to Stop/Play them on Deactivate/Activate
            // It is needed because we move the Tank when spawning it, and if the Particle System is playing while we do that
            // it "think" it move from (0,0,0) to the spawn point, creating a huge trail of smoke
            m_particleSystems = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Play();
            }
        }


        private void OnDisable ()
        {
            // When the tank is turned off, set it to kinematic so it stops moving.
            m_Rigidbody.isKinematic = true;

            // Stop all particle system so it "reset" it's position to the actual one instead of thinking we moved when spawning
            for(int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Stop();
            }
        }


        private void Start ()
        {
            // The axes names are based on player number.
            m_MovementAxisName = "Vertical" + m_PlayerNumber;
            m_TurnAxisName = "Horizontal" + m_PlayerNumber;

            // Store the original pitch of the audio source.
            m_OriginalPitch = m_MovementAudio.pitch;
        }


        private void Update ()
        {
            // Store the value of both input axes.
            m_MovementInputValue = Input.GetAxis (m_MovementAxisName);
            m_TurnInputValue = Input.GetAxis (m_TurnAxisName);

            EngineAudio ();
        }


        private void EngineAudio ()
        {
            // If there is no input (the tank is stationary)...
            if (Mathf.Abs (m_MovementInputValue) < 0.1f && Mathf.Abs (m_TurnInputValue) < 0.1f)
            {
                // ... and if the audio source is currently playing the driving clip...
                if (m_MovementAudio.clip == m_EngineDriving)
                {
                    // ... change the clip to idling and play it.
                    m_MovementAudio.clip = m_EngineIdling;
                    m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play ();
                }
            }
            else
            {
                // Otherwise if the tank is moving and if the idling clip is currently playing...
                if (m_MovementAudio.clip == m_EngineIdling)
                {
                    // ... change the clip to driving and play.
                    m_MovementAudio.clip = m_EngineDriving;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
        }


        private void FixedUpdate ()
        {
            // Adjust the rigidbodies position and orientation in FixedUpdate.
            /*Move ();
            Turn ();
        }


        private void Move ()
        {
            // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
            Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

            // Apply this movement to the rigidbody's position.
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }


        private void Turn ()
        {
            // Determine the number of degrees to be turned based on the input, speed and time between frames.
            float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

            // Make this into a rotation in the y axis.
            Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);

            // Apply this rotation to the rigidbody's rotation.
            m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);
        }
    }*/
    
}