using PointClick.Interactable;
using UnityEngine;

/// <summary>
/// Shows the puzzle Container and plays the falling animation.
/// </summary>
[RequireComponent(typeof(Animator))]
public class TheScream : MonoBehaviour
{
    InteractableObject interactable;
    Animator anim;
    [SerializeField] GameObject puzleContainer;

    private void Start()
    {
        interactable = GetComponent<InteractableObject>();
        anim = GetComponent<Animator>();
        interactable.OnInteract += Interactable_OnInteract;
    }

    private void Interactable_OnInteract(object sender, InteractEventArgs e)
    {
        anim.SetFloat("sp", 1.0f);
        puzleContainer.GetComponent<BoxCollider2D>().enabled = true;
        puzleContainer.GetComponent<SpriteRenderer>().enabled = true;
    }
}
