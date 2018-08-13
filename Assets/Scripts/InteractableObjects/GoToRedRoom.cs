using System;
using UnityEngine;

namespace PointClick.Interactable
{
    public class GoToRedRoom : InteractableObject
    {
        [SerializeField] AudioClip sfx;
        [SerializeField] PaddlePuzleController puzzle;
        string normalRoom = "room03";
        string redRoom = "room03red";
        bool hasDisplayed = false;

        protected override void Start()
        {
            base.Start();
            puzzle.OnPuzzleConfirm += Puzzle_OnPuzzleConfirm;
            coll.enabled = false;
            GameController.Instance.OnRoomChanged += Instance_OnRoomChanged;
        }

        private void Instance_OnRoomChanged(object sender, RoomChangedEventArgs e)
        {
            if(e.roomName == NextDoor.ToLower())
            {
                if (hasDisplayed) { return; }
                SoundManager.Instance.SfxController.Play(sfx, true);
                hasDisplayed = true;
                NextDoor = normalRoom;
            }
            else if(e.previousRoom == redRoom)
            {
                print("Go to " + e.roomName);
                SoundManager.Instance.SfxController.Stop();
            }
        }

        private void Puzzle_OnPuzzleConfirm(object sender, PuzzleConfirmEventArgs e)
        {
            if(e.acoplished)
            {
                coll.enabled = true;
            }
        }
    }
}
