using UnityEngine;
using UnityEngine.UI;
using PointClick.Inventory;
using System.Text;
using PointClick;
using System;

public class BriefcaseController : MonoBehaviour
{
    public event EventHandler<bool> OnConfirm;

    public string puzzleId = "puzzle.briefcase.initial";
    [SerializeField] string initialPassword = "1496";
    [SerializeField] string password = "1234";
    [SerializeField] InputField[] passwordContainers = new InputField[4];
    [SerializeField] InventoryItemModel[] rewards;
    [SerializeField] GameObject puzzleContainer;

    public void DisplayPuzzle()
    {
        puzzleContainer.SetActive(true);
        for (int i = 0; i < initialPassword.Length; i++)
        {
            passwordContainers[i].text = initialPassword[i].ToString();
        }
    }
    public void HidePuzzle()
    {
        puzzleContainer.SetActive(false);
    }

    public void Confirm()
    {
        StringBuilder userCode = new StringBuilder(password.Length);
        for(int i = 0; i < passwordContainers.Length; i++)
        {
            userCode.Append(passwordContainers[i].text);
        }

        if(password == userCode.ToString())
        {
            Debug.Log("CODE CORRECT");
            for (int i = 0; i < rewards.Length; i++)
            {
                GameController.Instance.NormalInventory.AddToInventory(rewards[i]);
            }
            HidePuzzle();
            OnConfirm?.Invoke(this, true);
            return;
        }

        OnConfirm?.Invoke(this, false);
    }
    
}
