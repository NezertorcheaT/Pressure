using System.Collections;
using UnityEngine;

public class NothingMouseTrigger : MouseTrigger
{
    [SerializeField] private TwoSideMouseTrigger twoSideTrigger;
    public TwoSideMouseTrigger TwoSideTrigger => twoSideTrigger;
    public TwoSideMouseTrigger.Side side;
}