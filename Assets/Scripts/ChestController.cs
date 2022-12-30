using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public ItemController[] items;
    private bool _open;

    public bool IsOpen() {
        return _open;
    }

    public void SetOpen(bool b) {
        _open = b;
    }

    public bool Contains(string name) {
        return Get(name)!=null;
    }

    public ItemController Get(string name){
        if(_open)
            foreach (var VAR in items)
                if (VAR.itemname == name)
                    return VAR;
        return null;
    }

    public void Putt(ItemController item) {
        if(_open){
            ItemController[] newItems = new ItemController[items.Length + 1];
            for (int i = 0; i < items.Length; i++)
                newItems[i] = items[i];
            newItems[items.Length] = item;
            items = newItems;
        }
    }
}
