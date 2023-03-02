using System.Collections.Generic;
using UnityEngine;

public static class RandomNumberGenerator
{
    public static List<int> GenerateRandomNums(int range)
    {
        List<int> randomNumbers = new List<int>();
        List<int> availableNumbers = new List<int>();

        // 사용 가능한 수 초기화
        for (int i = 1; i <= range; i++)
        {
            availableNumbers.Add(i);
        }

        // 난수 생성
        for (int i = 1; i <= range; i++)
        {
            int randomIndex = Random.Range(0, availableNumbers.Count);
            randomNumbers.Add(availableNumbers[randomIndex]);
            availableNumbers.RemoveAt(randomIndex);
        }

        return randomNumbers;
    }
}
