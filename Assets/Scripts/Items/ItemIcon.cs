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
        public event Action<int, int> onActionSelected = delegate { };

        public void Initialize(int idx)
        {
            this.index = idx;
        }

        public void Selected(int idx)
        {
            onSelected?.Invoke(idx);
        }
        
        public void ActionSelected(int idx, int action)
        {
            onActionSelected?.Invoke(idx, action);
        }

        public void RegisterSelectedListener(Action<int> listener)
        {
            onSelected += listener;
        }
        
        public void RegisterActionSelectedListener(Action<int, int> listener)
        {
            onActionSelected += listener;
        }

        public void UpdateItemIcon(Sprite icon)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = icon;
        }
    }
}