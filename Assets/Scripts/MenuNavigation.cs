using UnityEngine.SceneManagement;
using UnityEngine;


public class MenuNavigation : MonoBehaviour
{
    public void GoToScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
