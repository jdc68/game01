using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public bool phoneCall;
    public string name;

    [TextArea(3, 10)]
    public string[] sentences;

    public VoiceLine[] lines;
}
