using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VoiceLine
{
    [TextArea(3, 10)]
    public string sentence;
    public Sound audio;
}
