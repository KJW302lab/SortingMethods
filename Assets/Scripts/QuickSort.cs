using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class QuickSort : SortingBase
{
    private float _switchSec = 1f;
    private float _selectSec = 1f;

    private ChartItem _pivot;
    private int _high;
    private int _low;
        
    public override void Initialize(List<ChartItem> itemList, float speedRate)
    {
        base.Initialize(itemList, speedRate);

        _switchSec /= speedRate;
        _selectSec /= speedRate;

        StartCoroutine(nameof(StartQuickSort));
    }

    IEnumerator StartQuickSort()
    {
        _pivot = ItemList[Random.Range(0, ItemList.Count)];
        
        SelectItem(_pivot);
        yield return AddStep(_selectSec);

        _pivot.Switch(ItemList[^1], _switchSec, ItemList);
        yield return AddStep(_switchSec);
        
        CancelAllSelect();

        _low = 0;
        _high = ItemList.Count - 2;
        
        SelectItem(ItemList[_low]);
        SelectItem(ItemList[_high]);
        yield return AddStep(_selectSec);

        while (true)
        {
            yield return null;
            
            if (ItemList[_low].Number < _pivot.Number)
            {
                ItemList[_low].CancelSelect();
                
                _low++;
                SelectItem(ItemList[_low]);
                yield return AddStep(_selectSec);
            }

            if (ItemList[_high].Number >= _pivot.Number)
            {
                ItemList[_high].CancelSelect();
                
                _high--;
                SelectItem(ItemList[_high]);
                yield return AddStep(_selectSec);
            }
            else
            {
                ItemList[_high].Switch(ItemList[_low], _switchSec, ItemList);
                yield return AddStep(_switchSec);
            }

            if (_low >= _high)
            {
                ItemList[_high].Switch(_pivot, _switchSec, ItemList);
                _pivot = ItemList[^1];
                _low = 0;
                _high = ItemList.Count - 2;
                yield return AddStep(_switchSec);
            }
        }
    }

    private void Update()
    {
        if (_pivot != null)
        {
            print($"high : {_high} low : {_low} pivot : {_pivot.Number}");
        }
    }
}
