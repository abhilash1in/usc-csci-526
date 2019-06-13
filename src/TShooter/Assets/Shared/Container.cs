using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Container : MonoBehaviour
{
    [System.Serializable]
    public class ContainerItem
    {
        public System.Guid Id;
        public string Name;
        public int Maximum;
        public int Remaining
        {
            get
            {
                return Maximum - amountTaken;
            }
        }

        public int amountTaken;

        public ContainerItem()
        {
            Id = System.Guid.NewGuid();
        }

        public int Get(int value)
        {
            if((amountTaken + value) > Maximum)
            {
                int tooMuch = (amountTaken + value) - Maximum;
                amountTaken = Maximum;
                return value - tooMuch;
            }

            amountTaken += value;
            return value;
        }

        public void Set(int amount)
        {
            amountTaken -= amount;
            if(amountTaken < 0)
            {
                amountTaken = 0; 
            }
        }
    }

    public List<ContainerItem> items;

    public System.Action OnContainerReady;

    private void Awake()
    {
        items = new List<ContainerItem>();
        if (OnContainerReady != null)
            OnContainerReady();
    } 

    public System.Guid Add(string name, int maximum)
    {
        items.Add(new ContainerItem
        {
            Maximum = maximum,
            Name = name
        });
        return items.Last().Id;
    }

    public void Put(string name, int amount)
    {
        var containerItem = items.FirstOrDefault(x => x.Name == name);
        if(containerItem == null)
            return;
        containerItem.Set(amount);
    }

    public int TakeFromContainer(System.Guid id, int amount)
    {
        ContainerItem containerItem = GetContainerItem(id);

        if (containerItem == null)
            return -1;

        return containerItem.Get(amount);
    }

    public int GetAmountRemaining(System.Guid id)
    {
        ContainerItem containerItem = GetContainerItem(id);

        if (containerItem == null)
            return -1;

        return containerItem.Remaining;
    }

    private ContainerItem GetContainerItem(System.Guid id)
    {
        return items.FirstOrDefault(x => x.Id == id);
    }

}
