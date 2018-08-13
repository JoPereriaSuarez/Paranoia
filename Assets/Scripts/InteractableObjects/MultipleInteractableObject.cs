using UnityEngine;

namespace PointClick.Interactable
{
    [RequireComponent(typeof(Animator))]
    public class MultipleInteractableObject : InteractableObject
    {
        Animator anim;
        const string FINAL_STATE_TAG = "endState";
        [SerializeField] InteractableObject[] resultedObjects;
        protected bool IsOpen
        {
            get { return anim.GetBool(FINAL_STATE_TAG); }
            set
            {
                anim.SetBool(FINAL_STATE_TAG, value);
            }
        }

        protected override void Start()
        {
            anim = GetComponent<Animator>();
            base.Start();
            for (int i = 0; i < resultedObjects.Length; i++)
            {
                resultedObjects[i].gameObject.SetActive(false);
            }
        }

        public override void Interact()
        {
            if(!IsOpen) { IsOpen = true; }
        }

    }

}
