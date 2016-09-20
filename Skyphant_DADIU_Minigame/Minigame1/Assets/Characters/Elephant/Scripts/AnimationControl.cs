using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Characters.Elephant.Scripts
{
    public class AnimationControl : MonoBehaviour
    {

        private Material _material;
        private DateTime? _goBackToNeutral;

        public Image Image;
        public Sprite[] PoseIcons;

        private int poseIndex;

        private float timer = 0f;

        public void PickAnimation()
        {
            var oldPoseIndex = poseIndex;

            //We choose a poseIndex based on the animations/icons we have.
            while (oldPoseIndex == poseIndex && PoseIcons.Length > 1)
                poseIndex = Random.Range(0, PoseIcons.Length);
            
            //We set the image of the animation at the top of the screen
            Image.sprite = PoseIcons[poseIndex];

            //We need to pick the animation here!
            

        }


        // Use this for initialization
        void Start ()
        {
            _material = gameObject.GetComponent<Renderer>().material;
        }
	
        // Update is called once per frame
        void Update () {
            //timer for increaing the length of time the pose is held


            timer += Time.deltaTime;
            gameObject.GetComponentInChildren<Animator>().SetFloat("timer", timer);

            if (_goBackToNeutral.HasValue && _goBackToNeutral < DateTime.Now)
            {
                _goBackToNeutral = null;
                
            }
        }
        
        public void DidPose(int animationId)
        {
            ReadyToPose();
            gameObject.GetComponentInChildren<Animator>().SetInteger("poseID", poseIndex);
            Debug.Log(poseIndex);
            ReturnToNeutral();
        }

        public void FailedPose()
        {
            
            ReturnToNeutral();
        }

        public void ReadyToPose()
        {
            // CancelReturnToNeutral();
            //gameObject.GetComponentInChildren<Animator>().SetBool("doingPose", false);
            gameObject.GetComponentInChildren<Animator>().SetTrigger("doingPose");
            timer = 0;
        }

        public void ReturnToNeutral()
        {
          //  _goBackToNeutral = DateTime.Now.AddSeconds(2);
            //gameObject.GetComponentInChildren<Animator>().SetBool("doingPose", true);
        }

        public void CancelReturnToNeutral()
        {
            _goBackToNeutral = null;
        }

    }
}
