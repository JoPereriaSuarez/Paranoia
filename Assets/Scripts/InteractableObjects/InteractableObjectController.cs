using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PointClick.Interactable
{
    /// <summary>
    /// Add one for every scene (to make it work smoothly)
    /// </summary>
    public class InteractableObjectController : MonoBehaviour
    {
        [SerializeField]private InteractableObject[] allInteractables;

        private void Awake()
        {
            allInteractables = FindObjectsOfType<InteractableObject>();
            if(allInteractables.Length <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                for (int i = 0; i < allInteractables.Length; i++)
                {
                    allInteractables[i].OnClick += InteractableObjectController_OnClick;
                }
            }
        }

        private void InteractableObjectController_OnClick(object sender, EventArgs e)
        {
            InteractableObject interactable = (InteractableObject)sender;
            for (int i = 0; i < allInteractables.Length; i++)
            {
                if(allInteractables[i] != interactable)
                {
                    allInteractables[i].CancelClick();
                }
            }
        }
    }

}


