using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace StudioScor.InventorySystem
{
    public interface ISlot<T>
    {
        public T GetData { get; }
        public int GetInventoryLayer { get; }
        public bool HasData { get; }
        public void SetData(T data);
        public void ResetData();
    }
}