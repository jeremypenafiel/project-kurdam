using System;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class ItemIcon : MonoBehaviour
    {
        //public Image inventoryIcon;
        public int index;

        public event Action<int> onSelected = delegate { };

        public void Initialize(int idx)
        {
            this.index = idx;
        }

        public void Selected(int idx)
        {
            onSelected?.Invoke(idx);
        }

        public void RegisterListener(Action<int> listener)
        {
            onSelected += listener;
        }

        public void UpdateItemIcon(Sprite icon)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = icon;
        }
    }
}