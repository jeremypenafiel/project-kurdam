using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
   public SerializableGuid Id;
   public ItemsBase details;
   public int quantity;

   public Item(ItemsBase details, int quantity)
   {
      Id = SerializableGuid.NewGuid();
      this.details = details;
      this.quantity = quantity;
   }
}
