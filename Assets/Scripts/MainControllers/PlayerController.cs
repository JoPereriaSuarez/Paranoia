using UnityEngine;

/// <summary>
/// Can interact with Interactable Objects
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Camera on Game
    /// </summary>
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

}
