using System;
using System.Net;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Core.Scripts.Movement
{
    public class TouchInput : MonoBehaviour
    {
		public float SecToLongPress = 0.3f;

        public LayerMask LeftLayerMask;
        public LayerMask RightLayerMask;
        public GameObject Elephant;
        public float BoundaryDist;
        public static float Speed;

        public GameObject LeftButton;
        public GameObject RightButton;


		public Transform RotationPoint;

        private float _lastInputDown;
        private ComboHandler _comboHandler;
		private float _timeCounter;

		private bool _firstClick = true;
		private bool _longPressDone;
		private float _startTime;
        private bool _pressingButton;

        private ButtonLook _leftButtonLook;
        private ButtonLook _rightButtonLook;
        private bool _gameOver;

		private bool turnedLeft, turnedRight = false;

		private float span;

        void Start()
        {
            _comboHandler = GameObject.FindGameObjectWithTag(Constants.Tags.GameMaster).GetComponent<ComboHandler>();
			_comboHandler.StartCombo ();
            _leftButtonLook = LeftButton.GetComponent<ButtonLook>();
            _rightButtonLook = RightButton.GetComponent<ButtonLook>();
        }

        // Update is called once per frame
        void Update () {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameOverMaster.MainMenu();
            }

            if (_gameOver && Elephant.transform.position.y > -4.5f)
            {
                Elephant.transform.Translate(Vector3.down*Time.deltaTime*VariableController.DieSpeed);
                return;
            }
            if(_gameOver)
            {
                Elephant.SetActive(false);
                return;
            }

            if(Input.touches.Length > 1)
                return;

            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
            {
                var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var hitColliderLeft = Physics2D.OverlapPoint(mousePosition, LeftLayerMask);
                var hitColliderRight = Physics2D.OverlapPoint(mousePosition, RightLayerMask);

                if (hitColliderLeft) {
                    _pressingButton = true;
					RotationPoint.GetComponent<Animator> ().SetBool ("TurnRight", false);
					RotationPoint.GetComponent<Animator> ().SetBool ("TurnLeft", true);
					//RotationPoint.GetComponent<Animator>().enabled = false;
					turnedLeft = true;
                    GoLeft();
                }
                else if (hitColliderRight) {
                    _pressingButton = true;
					RotationPoint.GetComponent<Animator> ().SetBool ("TurnLeft", false);
					RotationPoint.GetComponent<Animator> ().SetBool ("TurnRight", true);
					//RotationPoint.GetComponent<Animator>().enabled = false;
					turnedRight = true;
                    GoRight();
                }
                else
                {
                    _pressingButton = false;
                    PoseInputDown();
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (_pressingButton)
                {
					//StartCoroutine(WaitForAnimator(.5f));
					RotationPoint.GetComponent<Animator> ().SetBool ("TurnLeft", false);
					RotationPoint.GetComponent<Animator> ().SetBool ("TurnRight", false);
					turnedLeft = false;
					turnedRight = false;
					_pressingButton = false;
                    _rightButtonLook.ButtonNotPressed();
                    _leftButtonLook.ButtonNotPressed();
                } else { 
                    PoseInputUp();
                }
            }
        }

        private void PoseInputDown()
        {
            if (_firstClick)
            {
                _startTime = Time.time;
                _firstClick = false;
            }

            if (_longPressDone) return;

            _timeCounter = Time.time;

			if(_comboHandler.GetCurrentGoalPressNumber() == 0 && _comboHandler.NextGoalPress() == PressType.Long)
			{

				CooldownAnimation(_comboHandler.cooldown1.GetComponent<Animator>());
			}
			else if (_comboHandler.GetCurrentGoalPressNumber() == 1 && _comboHandler.NextGoalPress() == PressType.Long)
			{
				
				CooldownAnimation(_comboHandler.cooldown2.GetComponent<Animator>());
			}
			else if (_comboHandler.GetCurrentGoalPressNumber() == 2 && _comboHandler.NextGoalPress() == PressType.Long)
			{
				
				CooldownAnimation(_comboHandler.cooldown3.GetComponent<Animator>());
			}

            span = (_timeCounter - _startTime);

            if (!(span > SecToLongPress)) return;

            _comboHandler.DoPress(PressType.Long);
            _longPressDone = true;
        }

        private void PoseInputUp()
        {
            if (!_longPressDone)
            {
                _comboHandler.DoPress(PressType.Short);
            }

            _firstClick = true;
            _longPressDone = false;
            _timeCounter = 0;
			
			_comboHandler.cooldown1.GetComponent<Animator> ().SetBool("Active", false);
			_comboHandler.cooldown2.GetComponent<Animator> ().SetBool("Active", false);
			_comboHandler.cooldown3.GetComponent<Animator> ().SetBool("Active", false);

        }


        private void GoLeft()
        {
            _leftButtonLook.ButtonPressed();
            if (-BoundaryDist <= Elephant.transform.position.x)
            {
                Elephant.transform.Translate(Vector3.left * Time.deltaTime * Speed);
            }
        }

        private void GoRight()
        {
            _rightButtonLook.ButtonPressed();
            if (BoundaryDist >= Elephant.transform.position.x)
            {
                Elephant.transform.Translate(Vector3.right * Time.deltaTime * Speed);
            }
        }

        public void Gameover()
        {
            _gameOver = true;
            
        }

        public float GetXPos()
        {
            return Elephant.transform.position.x;
        }

		public void CooldownAnimation(Animator cooldownAnim)
		{
			if (Input.GetMouseButtonDown (0)) {
				
				if(SecToLongPress < 1)
				{
					cooldownAnim.speed = 1f / SecToLongPress;
				}
				else
				{
					cooldownAnim.speed = 1f * SecToLongPress;
				}
				
				//			Debug.Log(cooldownImage.speed);
				
				cooldownAnim.SetBool("Active", true);
			}
		}

    }
}
