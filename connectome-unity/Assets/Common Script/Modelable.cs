using UnityEngine;
using System.Collections;
using System;

public enum AnimationDirection : int { ToRight, ToLeft, ToUp, ToDown };

namespace Connectome.Unity.Common
{
    public class Modelable : MonoBehaviour
    {
        public AnimationDirection Direction;
        public Transform target;
        public int Steps;

        private bool animate;

        private Vector2 pointFrom;
        private Vector2 pointTo;

        private bool isHiding;

        void Start()
        {
            animate = false;

            Rect rect = target.GetComponent<RectTransform>().rect;

            pointFrom = new Vector2(target.localPosition.x, target.localPosition.y);

            // 1 right, -1 left, 0 none 
            int horizantalDirection = (1 - (int)Direction / 2) * (((int)Direction * -2) + 1);

            // 1 up, -1 down, 0 none 
            int verticalDirection = ((int)Direction / 2) * (((int)Direction * -2) + 5);

            float newX = horizantalDirection * rect.width;
            float newY = verticalDirection * rect.height;

            //Debug.Log("newX :" + newX);


            pointTo = new Vector2(target.localPosition.x + newX, target.localPosition.y + newY);

            isHiding = false;
        }

        void Update()
        {
            if (animate)
            {
                Step();
            }
        }

        public void Toggle()
        {
            if (isHiding)
                Show();
            else
                Hide();
        }

        public void Hide()
        {
            isHiding = true;
            animate = true;
        }

        public void Show()
        {
            isHiding = false;
            animate = true;
        }


        private void Step()
        {
            float destanceX = (isHiding ? -1 : 1) * (pointFrom.x - pointTo.x);
            float destanceY = (isHiding ? -1 : 1) * (pointFrom.y - pointTo.y);

            float stepX = destanceX / Steps;
            float stepY = destanceY / Steps;


            target.localPosition += new Vector3(stepX, stepY);

            float currentX = target.localPosition.x;
            float currentY = target.localPosition.y;

            stepX = Math.Abs(stepX);
            stepY = Math.Abs(stepY);

            //Debug.Log("step ("+ stepX + "): delta " + Math.Abs(currentX - pointTo.x) + " cur " + currentX + "  to.X "+ pointTo.x); 

            if (isHiding)
            {
                if (Math.Abs(currentX - pointTo.x) <= stepX && Math.Abs(currentY - pointTo.y) <= stepY)
                {
                    animate = false;
                    //Debug.Log("ended " + gameObject.name); 
                    //snap 
                    target.localPosition = new Vector3(pointTo.x, pointTo.y);
                }
            }
            else
            {
                if (Math.Abs(currentX - pointFrom.x) <= stepX && Math.Abs(currentY - pointFrom.y) <= stepY)
                {
                    animate = false;
                    //Debug.Log("ended "  +gameObject.name);
                    //snap
                    target.localPosition = new Vector3(pointFrom.x, pointFrom.y);
                }
            }
        }

    }
}