using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Record: IComparable<Record>
{
    public string playerName;
    public float score;

    public int CompareTo(Record other)
    {
        return other.score.CompareTo(score);
    }

}
