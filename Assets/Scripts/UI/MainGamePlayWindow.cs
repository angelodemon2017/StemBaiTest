using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainGamePlayWindow : MainWindow
    {
        public Button ButtonMix;
        public Transform SlotsParent;
        public UiSlotItem SlotPrefab;
        public List<UiSlotItem> Slots = new();
    }
}