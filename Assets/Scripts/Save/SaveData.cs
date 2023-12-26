using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public int DaTime;
    public bool isSucess;

    public SaveData(int disTime, bool daSucess)
    {
        DaTime = disTime;
        isSucess = daSucess;
    }
}
