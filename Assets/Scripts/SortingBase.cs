using System.Collections.Generic;
using UnityEngine;

public class SortingBase : MonoBehaviour
{
    protected List<ChartItem> ItemList;
    protected float SpeedRate;

    private ChartItem _selectedItem;
    protected ChartItem SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;

            foreach (var item in ItemList)
            {
                item.Select(item == value);
            }
        }
    }

    protected void SelectItem(ChartItem item)
    {
        SelectedItem = item;
        
        MainCanvas.Instance.SetPointerPosition(item);
    }

    public virtual void Initialize(List<ChartItem> itemList, float speedRate)
    {
        ItemList = itemList;
        SpeedRate = speedRate;
    }
}
