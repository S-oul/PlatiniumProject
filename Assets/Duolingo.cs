using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duolingo : NPC
{
    [SerializeField] GameObject _task;
    [SerializeField] DataManager.TaskEnum _typeTask;
    public GameObject Task { get => _task; }

    [SerializeField] DeskDuolingo _leftDesk;
    [SerializeField] DeskDuolingo _rightDesk;

    public DeskDuolingo LeftDesk { get => _leftDesk; set =>  _leftDesk = value; }
    public DeskDuolingo RightDesk { get => _rightDesk; set => _rightDesk = value; }

    private void Awake()
    {
        _task = Instantiate(DataManager.Instance.AllTasks[(int)_typeTask], this.transform);
        _leftDesk.TaskDuolingo = _task;
        _rightDesk.TaskDuolingo = _task;
        _task.GetComponent<DuolingoTask>().NPCDuolingo = this;
    }

    public void Talk(string text)
    {
        NPCUI.DisplayTalkingBubble(true);
        NPCUI.ChangeBubbleContent(text);
    }
}
