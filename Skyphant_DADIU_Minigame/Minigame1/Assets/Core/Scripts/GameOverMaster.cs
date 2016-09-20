using System.Net.Configuration;
using System.Security.Policy;
using Assets.Core.Scripts.Movement;
using UnityEngine;

namespace Assets.Core.Scripts
{
    public class GameOverMaster : MonoBehaviour
    {

        public GameObject Ground;
        public GameObject Hole;
        public GameObject FrontCylinder;
        public GameObject BackCylinder;

        public GameObject MainMenuButton;
        public GameObject RestartLevelButton;
        public GameObject TuturialButtton;
        
        public GameObject NavButtingLeft;
        public GameObject NavButtingRight;


        private TouchInput _elephantMovement;

        private Transform _groundTransform;
        private Transform _holeTransform ;
        private bool _gameIsOver;
		private AudioManager AudioMngr;

        void Start()
        {
            _groundTransform = Ground.GetComponent<Transform>();
            _elephantMovement = GetComponent<TouchInput>();
            Hole.GetComponent<SpriteRenderer>().enabled = false;
			AudioMngr = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager>();
        }

        void Update () {
            // Shoud move ground up
            if(_groundTransform.position.y < -0.27 && _gameIsOver) { 
                _groundTransform.Translate(Vector3.up * Time.deltaTime * VariableController.DieSpeed);
                _elephantMovement.Gameover();
                _holeTransform = Hole.GetComponent<Transform>();
            }
            //Ground is now moved up. Add hole. 
            else if (_gameIsOver)
            {
                _holeTransform.position = new Vector3(_elephantMovement.GetXPos(), _holeTransform.position.y, _holeTransform.position.z);
                StopSpawns();
            }
        }

        public void GameOver()
        {
			AudioMngr.HitGroundPlay();
			AudioMngr.MusicStop ();
			AudioMngr.WindStop ();
			AudioMngr.FlappingStop ();
			GameObject.FindGameObjectWithTag ("Player").GetComponent<BoxCollider>().enabled = false;
            _gameIsOver = true;

        }

        private void StopSpawns()
        {
            Hole.GetComponent<SpriteRenderer>().enabled = true;
            Camera.main.GetComponent<ShakeCam>().StopShaking();

            var clouds = FindObjectsOfType<CloudMovement>();
            foreach (var cloud in clouds)
            {
                cloud.enabled = false;
            }
            
            var obstacles = GameObject.FindGameObjectsWithTag(Constants.Tags.Obstacle);
            foreach (var obstacle in obstacles)
            {
                obstacle.GetComponent<obstacleMovement>().StopSpawning();
            }

            MainMenuButton.SetActive(true);
            RestartLevelButton.SetActive(true);
            TuturialButtton.SetActive(true);
            Destroy(NavButtingLeft);
            Destroy(NavButtingRight);
        }

        public static void RestartScene()
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        public static void MainMenu()
        {
            Application.LoadLevel(Constants.Scenes.MainMenuSceneName);
        }

        // STUPOID UNITY!!!!
        public void MainMenu2()
        {
			GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager> ().ButtonClickPlay ();
            MainMenu();
        }

        public void RestartScene2()
        {
			GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager> ().ButtonClickPlay ();
            RestartScene();
        }



    }
}

