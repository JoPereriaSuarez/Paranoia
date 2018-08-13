using System;
using UnityEngine;

namespace PointClick.Interactable
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Paddle : MonoBehaviour
    {
        [SerializeField] Paddle[] otherPaddles;
        SpriteRenderer rend;
        [SerializeField] bool startOn = false;

        Sprite onSprite;
        public  Sprite OnSprite
        {
            get { return onSprite; }
            set
            {
                if(onSprite != null || value == null) { return; }
                onSprite = value;
            }
        }
        Sprite offSprite;
        public Sprite OffSprite
        {
            get { return offSprite; }
            set
            {
                if (offSprite != null || value == null) { return; }
                offSprite = value;
            }
        }

        /// <summary>
        /// True paddle is on. False paddle is off
        /// </summary>
        public bool State
        {
            get { return startOn; }
            private set
            {
                if(value == startOn) { return; }
                startOn = value;
            }
        }

        private void Start()
        {
            rend = GetComponent<SpriteRenderer>();
            rend.sprite = (startOn) ? onSprite : offSprite;
        }

        public void Switch()
        {
            State = !State;
            rend.sprite = (State) ? onSprite : offSprite;
            if(otherPaddles.Length > 0)
            {
                for (int i = 0; i < otherPaddles.Length; i++)
                {
                    otherPaddles[i].Switch();
                }
            }
        }

        private void OnMouseDown()
        {
            Switch();
        }

    }

}
