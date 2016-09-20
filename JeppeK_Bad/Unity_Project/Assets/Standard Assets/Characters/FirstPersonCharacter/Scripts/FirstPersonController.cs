using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class FirstPersonController : MonoBehaviour
    {
        //[SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        //[SerializeField] private float m_RunSpeed;
        //[SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        //[SerializeField] private float m_StickToGroundForce;
        //[SerializeField] private float m_GravityMultiplier;
        [SerializeField] private MouseLook m_MouseLook;
        //[SerializeField] private bool m_UseFovKick;
        //[SerializeField] private FOVKick m_FovKick = new FOVKick();

        private Camera m_Camera;
        private Transform m_transform;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        //private Vector3 m_MoveDir = Vector3.zero;
        //private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;

		//PowerUp
		public static bool doubleSpeed = false;
		public float speed;
		float time = 5;
		float timeElapsed = 0;

		// Use this for initialization
        private void Start()
        {
            //m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_transform = GetComponent<Transform>();
            //m_FovKick.Setup(m_Camera);
            m_MouseLook.Init(transform , m_Camera.transform);
        }


        // Update is called once per frame
        private void Update()
        {
            RotateView();

            //m_transform.localRotation = Quaternion.Euler(0f, 1.0f, 0f);
            //localRotation.eulerAngles = new Vector3(0, 0, 0);
        }

        /*private void FixedUpdate()
        {
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height/2f);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x*speed;
            m_MoveDir.z = desiredMove.z*speed;
   
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);

            UpdateCameraPosition(speed);
        }*/

        /*private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
  
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                newCameraPosition = m_Camera.transform.localPosition;
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
			}
            m_Camera.transform.localPosition = newCameraPosition;
        }*/


        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

			speed = m_WalkSpeed;
            
			//Apply double movement speedbuff
			if (doubleSpeed) {
				speed = doubleMovementSpeed(speed);
			}

			m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }
        }


        public void RotateView()
        {
            m_MouseLook.LookRotation (transform, m_Camera.transform);
        }


        /*private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }*/

		private float doubleMovementSpeed(float speed)
		{
			if (timeElapsed < time) {
				speed = speed * 2;
				timeElapsed += Time.deltaTime;
			} else {
				speed = speed/2;
				doubleSpeed = false;
				timeElapsed = 0;
			}
			return speed;
		}

    }
}
