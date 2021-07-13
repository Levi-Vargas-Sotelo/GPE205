using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ScoreData : IComparable<ScoreData>
{
    // The value of the score
    public float score;
    // Tha name of who has the score
    public string name;

    //Function used to see and compare the score with another score and be able to sort it out automatically
    public int CompareTo (ScoreData other) 
    {
        if (other == null) 
        {
            return 1;
        }
        if (this.score > other.score) 
        {
            return 1;
        }
        if (this.score < other.score) 
        {
          return -1;
        }
        return 0;
    }
}
