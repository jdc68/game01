using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneButton : MonoBehaviour
{
    public void AcceptCall()
    {
        PhoneManager.Instance.AcceptCall();
        gameObject.SetActive(false);
    }
}
