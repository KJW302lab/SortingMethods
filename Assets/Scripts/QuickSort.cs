using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class QuickSort : SortingBase
{
    private float _switchSec = 1f;
    private float _selectSec = 1f;
    private bool _jobFinished;
    private List<ChartItem> _sorted;

    public override void Initialize(List<ChartItem> itemList, float speedRate)
    {
        base.Initialize(itemList, speedRate);
        
        _jobFinished = false;

        if (SpeedRate > 40)
        {
            SpeedRate = 40;
        }
        
        _switchSec /= SpeedRate;
        _selectSec /= SpeedRate;

        _sorted = new();

        _sorted = ItemList.OrderBy(item => item.Number).ToList();

        StartCoroutine(StartSorting());
    }

    IEnumerator StartSorting()
    {
        yield return StartCoroutine(QuickSorting(ItemList));

        foreach (var item in _sorted)
        {
            item.OnRightPosition();
            item.PlaySound();
            item.PointItem();
            yield return AddWait(_selectSec);
        }
    }

    IEnumerator QuickSorting(List<ChartItem> list)
    {
        if (list == null || list.Count <= 1)
        {
            yield break;
        }

        // 리스트 중 하나의 요소를 pivot으로 정하고, 리스트의 맨 처음 요소나 맨 마지막 요소와 Swap.
        // 리스트의 맨 처음 요소를 left로 정하고, 마지막 요소를 right으로 정함.
        int left = 1;
        int right = list.Count - 1;

        int pivot = list.Count / 2;
        
        yield return SelectItem(list[pivot]);

        SwapItem(list[pivot], list[0], list);
        yield return AddStep(ProcessSec);
        pivot = 0;

        while (true)
        {
            // left와 right이 교차되었을때, pivot과 right를 swap하고 pivot을 기준으로 2개의 리스트를 새로 만들어(Divide) 각각을 Quick 정렬함
            if (left > right)
            {
                SwapItem(list[pivot], list[right], list);
                pivot = right;
                yield return AddStep(ProcessSec);

                List<ChartItem> leftList = new();
                List<ChartItem> rightList = new();

                for (int i = 0; i < pivot; i++)
                {
                    leftList.Add(list[i]);
                }

                for (int i = pivot + 1; i < list.Count; i++)
                {
                    rightList.Add(list[i]);
                }

                yield return StartCoroutine(QuickSorting(leftList));
                yield return StartCoroutine(QuickSorting(rightList));
                yield break;
            }
            
            SelectItem(list[left]);
            yield return SelectItem(list[right]);

            // left가 pivot보다 작다면, left의 인덱스 +
            if (list[left].Number < list[pivot].Number)
            {
                list[left].CancelSelect();
                left++;
            }

            // right가 pivot보다 작다면,
            else if (list[right].Number > list[pivot].Number)
            {
                list[right].CancelSelect();
                right--;
            }

            // left와 right가 멈추었다면, 둘을 swap하고 각각의 iterator를 진행시킨다
            else
            {
                SwapItem(list[left], list[right], list);
                
                list[left].CancelSelect();
                list[right].CancelSelect();

                left++;
                right--;
                
                yield return AddStep(ProcessSec);
            }
        }
    }
}
