using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rules : MonoBehaviour
{
    private string _ruleName;

    public List<RuleType> rules = new List<RuleType>();

    public enum RuleType
    {
        appearOnce,
        appearMultipleTimes,
        appearInFirstLast,
        NextRoomIs,
        TotheLeftRight
    }

    void ExecuteRules(ref Object var)
    {
        foreach(RuleType rule in rules)
        {
            switch (rule)
            {
                case RuleType.appearOnce:
                    ///Execute code here
                    break; 

                case RuleType.appearMultipleTimes:
                    ///Execute code here
                    break;

                case RuleType.appearInFirstLast:
                    ///Execute code here
                    break;

                case RuleType.NextRoomIs:
                    ///Execute code here
                    break;
                
                case RuleType.TotheLeftRight:
                    ///Execute code here
                    break;
            }
        }
    }
}
