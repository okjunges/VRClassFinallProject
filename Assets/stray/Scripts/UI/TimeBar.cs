using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeBar : MonoBehaviour
{

    [SerializeField] private Image Timebar;
    [SerializeField] private GM gm;

    void Start()
    {
        if (gm == null)
        {
            gm = GM.Instance;
            if (gm == null) gm = FindObjectOfType<GM>();
        }
    }

    void Update()
    {
        if (gm != null && Timebar != null)
        {
            Timebar.fillAmount = gm.currentTime / gm.maxTime;
        }
    }
}
