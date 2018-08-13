using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace PointClick.UI
{
    public class GameUI : MonoBehaviour
    {
        public event EventHandler<MessagePopUpEventArgs> OnMessagePopUp;


        [SerializeField] GameObject pauseMenu;
        [SerializeField] private string mainMenuScene = "MenuScene";

        /// <summary>
        /// Unity UI panel that works as a canvas for displaying messages on game
        /// </summary>
        [SerializeField] GameObject messageObject;
        [SerializeField] Text messageText;

        [SerializeField] Sprite lifeBarSprite;
        [SerializeField] GameObject[] lifeBar;
        [SerializeField] Image uiImage;
        private int currentLife;

        // the closeButton object is intended to be a child on messageObject on Unity
        [SerializeField] Button closeMessageButton;
        [SerializeField] Button continueMessageButton;

        private void Start()
        {
            closeMessageButton.onClick.AddListener(() => ClosePopupMessage());
            messageObject.SetActive(false);
            RemoveImage();
        }

        private void OnDestroy()
        {
            closeMessageButton.onClick.RemoveAllListeners();
        }

        /// <summary>
        /// Display multiple popup message
        /// </summary>
        /// <param name="texts"></param>
        public void OpenPopupMessage(string[] texts, int initialIndex = 0)
        {
            StartCoroutine(WaitUntilContinue(texts, initialIndex));
        }
        private IEnumerator WaitUntilContinue(string[] text, int initialIndex)
        {
            closeMessageButton.gameObject.SetActive(false);
            continueMessageButton.gameObject.SetActive(true);
            for(int i = initialIndex; i < text.Length; i ++)
            {
                OpenPopupMessage(text[i]);
                if(i == text.Length-1)
                {
                    closeMessageButton.gameObject.SetActive(true);
                    continueMessageButton.gameObject.SetActive(false);
                }
                else
                {
                    yield return new WaitUntil(() => continueNext == true);
                    continueNext = false;
                }
            }
            continueNext = false;
        }
        /// <summary>
        /// The message is inteded to be contain in only one canvas.
        /// </summary>
        /// <param name="text"></param>
        public void OpenPopupMessage(string text)
        {
            messageObject.SetActive(true);
            messageText.text = text;
            OnMessagePopUp?.Invoke(this, new MessagePopUpEventArgs(true));
        }
        public void ClosePopupMessage()
        {
            messageObject.SetActive(false);
            OnMessagePopUp?.Invoke(this, new MessagePopUpEventArgs(false));
            RemoveImage();
        }

        bool continueNext;
        public void ContinueNextText()
        {
            continueNext = true;
        }

        public void DisplayImage(Sprite image)
        {
            uiImage.sprite = image;
            uiImage.color = new Color(1, 1, 1, 1);
        }
        private void RemoveImage()
        {
            uiImage.sprite = null;
            uiImage.color = new Color(1, 1, 1, 0f);
        }

        public void RemoveLife(ushort amount = 1)
        {
            if (amount == 0) { return; }

            int lifeToSubstract = currentLife - amount;
            if(lifeToSubstract == 0) { return; }

            for (int i = currentLife; i >= 0; i--)
            {
                lifeBar[i].SetActive(false);
            }
            currentLife -= lifeToSubstract;
        }

        public void DisplayPauseMenu()
        {
            pauseMenu.SetActive(true);
            GameController.Instance.PauseGame();
        }
        public void HidePauseMenu()
        {
            pauseMenu.SetActive(true);
            GameController.Instance.UnPauseGame();
        }

        //public void DisplayDocumentInventory()
        //{
        //    GameController.Instance.DocumentInventory.Display(true);
        //}
        //public void HideDocumentInventory()
        //{
        //    GameController.Instance.DocumentInventory.Display(false);
        //}
        public void GoToMainMenu()
        {
            SceneManager.LoadScene(mainMenuScene);
        }
    }

    public class MessagePopUpEventArgs :EventArgs
    {
        public readonly bool isOpen;

        public MessagePopUpEventArgs(bool isOpen)
        {
            this.isOpen = isOpen;
        }
    }
}
