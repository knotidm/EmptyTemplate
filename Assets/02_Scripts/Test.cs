using UnityEngine;

public class Test : MonoBehaviour
{
    [Header("States")]
    [TypeFilter(typeof(IState))]
    [SerializeField] SerializableType startingState;

    [Header("Events")]
    [TypeFilter(typeof(IEvent))]
    [SerializeField] SerializableType gameOverEvent;

    [TypeFilter(typeof(IEvent))]
    [SerializeField] SerializableType levelCompleteEvent;
}
