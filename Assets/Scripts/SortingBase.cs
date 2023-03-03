using System.Collections.Generic;
using UnityEngine;

public class SortingBase : MonoBehaviour
{
    protected List<ChartItem> ItemList;

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
        
        MainCanvas.Instance.SetPointer(item);
    }

    public virtual void Initialize(List<ChartItem> itemList)
    {
        ItemList = itemList;
    }
}
