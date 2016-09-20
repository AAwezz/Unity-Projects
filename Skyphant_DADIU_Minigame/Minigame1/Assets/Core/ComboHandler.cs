using System.Collections.Generic;
using Assets.Characters.Elephant.Scripts;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Core
{
	public class ComboHandler : MonoBehaviour
	{
		public GameObject Player;
		public GameObject BackGround;
		public float TapTapTapFrequency = 0f;
		public Text GoalText;
		public float SecondsBeforeNewCombo = 2f;
		public Image OneImg, TwoImg, ThreeImg, cooldown1, cooldown2, cooldown3;
		public Sprite ShortSprite, LongSprite, SuccessShortSprite, SuccessLongSprite, FailShortSprite, FailLongSprite;
		//public Animator cooldownAnim;
		public GameObject PoseButton;
		//public MeshRenderer ElephantColor; 
		
		private List<PressType> _currentPresses;
		private List<PressType> _currentGoal;
		private int _goalId = -1;
		private AnimationControl _playerAnimationControl;
		private bool _isCalledInThisFrame;
		
		private bool _isFirstPose = true;

		private AudioManager AudioMngr;

		void Awake()
		{
			_playerAnimationControl = Player.GetComponent<AnimationControl>();
		}

		void Start()
		{
			AudioMngr = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager>();
		}
		
		public void DoPress(PressType press)
		{
			/*
			if (_isCalledInThisFrame)
			{
				_isCalledInThisFrame = false;
				return;
			}
			*/
			
			if (_goalId < 0) return;
			
			//Debug.Log("Pressed: " + press);
			if (NextGoalPress() == press)
			{
				_currentPresses.Add(press);

				if(_currentPresses.Count == 1)
				{

					//AudioMngr.success.pitch = 1f;
					AudioMngr.ChangeSuccessPitch(1f);
					AudioMngr.SuccessPlay();

					if(OneImg.sprite == ShortSprite)
					{
						OneImg.sprite = SuccessShortSprite;
					}
					else
					{
						OneImg.sprite = SuccessLongSprite;
					}
				}
				else if(_currentPresses.Count == 2)
				{
					//AudioMngr.success.pitch = 1.2f;
					AudioMngr.ChangeSuccessPitch(1.15f);
					AudioMngr.SuccessPlay();
					if(TwoImg.sprite == ShortSprite)
					{
						TwoImg.sprite = SuccessShortSprite;
					}
					else
					{
						TwoImg.sprite = SuccessLongSprite;
					}
				}
				else if(_currentPresses.Count == 3)
				{
					//AudioMngr.success.pitch = 1.4f;
					AudioMngr.ChangeSuccessPitch(1.4f);
					AudioMngr.SuccessPlay();
					if(ThreeImg.sprite == ShortSprite)
					{
						ThreeImg.sprite = SuccessShortSprite;
					}
					else
					{
						ThreeImg.sprite = SuccessLongSprite;
					}
				}

				if (_currentPresses.Count != _currentGoal.Count) return;
				PoseSucceeded();
			}
			else
			{
				PoseFailed();
                ScoreSystem.comboCount = 0;
                ScoreSystem.comboTimeReset();
			}
		}
		
		public PressType? NextGoalPress()
		{
			if (_goalId != -1) {
				return _currentGoal [_currentPresses.Count];
			}
			return null;
		}

		public int GetCurrentGoalPressNumber()
		{
			return _currentPresses.Count;
		}
		
		public void StartCombo()
		{
			_isCalledInThisFrame = true;
			
			// Reset current presses 
			_currentPresses = new List<PressType>();

			// Choose new goal
			var combos = Constants.Combos;
			
			if (!_isFirstPose) {
				int random = Random.Range (0, 100);
				
				if(random <= TapTapTapFrequency)
				{
					_goalId = 0;
					_currentGoal = combos [_goalId];
				}
				else
				{
					_goalId = new System.Random().Next(0, combos.Count);
					_currentGoal = combos[_goalId];
				}
				
			} else {
				_goalId = 0;
				_currentGoal = combos [_goalId];
				_isFirstPose = false;
			}

		    _playerAnimationControl.PickAnimation();

			//PoseButton.SetActive(false);
			
			// Print goal
			GoalText.text = "Goal:";
			foreach (var goal in _currentGoal)
			{
				GoalText.text += " " + goal;
			}

			/*
			for (int i = 0; i < _currentGoal.Count; i++) {
				if(_currentGoal[0] == PressType.Long)
			}
			*/

			if (_currentGoal [0] == PressType.Short) {
				OneImg.sprite = ShortSprite;
			}
			else 
			{
				OneImg.sprite = LongSprite;
			}

			if (_currentGoal [1] == PressType.Short) {
				TwoImg.sprite = ShortSprite;
			}
			else 
			{
				TwoImg.sprite = LongSprite;
			}

			if (_currentGoal [2] == PressType.Short) {
				ThreeImg.sprite = ShortSprite;
			}
			else 
			{
				ThreeImg.sprite = LongSprite;
			}


			//_playerAnimationControl.ReadyToPose();
			
			//_control.DisableMovement();
		}
		
		public void PoseFailed()
		{
			AudioMngr.FailPlay();
			int count = _currentPresses.Count;

			if( count == 0 )
			{
				if(OneImg.sprite == ShortSprite)
				{
					OneImg.sprite = FailShortSprite;
				}
				else
				{
					OneImg.sprite = FailLongSprite;
				}
			}
			if(count == 1)
			{
				if(TwoImg.sprite == ShortSprite)
				{
					TwoImg.sprite = FailShortSprite;
				}
				else
				{
					TwoImg.sprite = FailLongSprite;
				}
			}
			if(count == 2)
			{
				if(ThreeImg.sprite == ShortSprite)
				{
					ThreeImg.sprite = FailShortSprite;
				}
				else
				{
					ThreeImg.sprite = FailLongSprite;
				}
			}

			_playerAnimationControl.FailedPose();
			ScoreSystem.poseFail = true;
			Debug.Log ("PoseFailed");
			Reset();
		}
		
		public void PoseSucceeded()
		{
			AudioMngr.PosePlay();
			_playerAnimationControl.DidPose(_goalId);
			ScoreSystem.poseComplete = true;
			Debug.Log ("PoseSucceeded");
            
			//ElephantColor.material.color = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f)); 
			Reset();
		}
		
		public void Reset()
		{
			_goalId = -1;
			_currentPresses = new List<PressType> ();
			_currentGoal = new List<PressType> ();
			GoalText.text = "";
			//PoseButton.SetActive(true);
			//_control.EnableMovement();
			StartCoroutine (WaitForNewCombo(SecondsBeforeNewCombo));
		}

		IEnumerator WaitForNewCombo(float seconds)
		{
			//OneImg.sprite = null;
			//TwoImg.sprite = null;
			//ThreeImg.sprite = null;

			yield return new WaitForSeconds(seconds);
			StartCombo ();
		}
	}
	
	public enum PressType
	{
		Short, Long
	}
}
