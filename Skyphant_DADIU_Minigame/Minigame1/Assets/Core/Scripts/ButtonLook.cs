using UnityEngine;

namespace Assets.Core.Scripts
{
    public class ButtonLook : MonoBehaviour
    {

        private SpriteRenderer sr;

        void Start()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        public void ButtonPressed()
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.7f);
        }

        public void ButtonNotPressed()
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.37f);
        }
    }
}

