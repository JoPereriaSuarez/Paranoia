using System;
using UnityEngine;

namespace PointClick
{
    public class TouchController : MonoBehaviour
    {
        Touch theTouch;
        Outline otherOutline;
        Camera cam;

        private void Awake()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            

            if(Input.touchCount > 0)
            {
                theTouch = Input.GetTouch(0);
                if ( theTouch.phase == TouchPhase.Began)
                {
                    Ray ray = cam.ScreenPointToRay(theTouch.position);
                    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000f);
                    if(otherOutline != null)
                    {
                        otherOutline.ShowHide_Outline(false);
                        Debug.Log("CANCEL");

                    }
                    otherOutline = hit.transform?.GetComponent<Outline>();
                    if(otherOutline != null)
                    {
                        otherOutline.ShowHide_Outline(true);
                        Debug.Log("SHOW");
                    }
                }
            }
        }
    }

}
