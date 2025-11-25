using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentRound : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private GM gm;

    
    void Start()
    {
        if (gm == null)
        {
            gm = GM.Instance;
            if (gm == null) gm = FindObjectOfType<GM>();
        }

        if (gm != null && roundText != null)
        {
            roundText.text = "Round " + gm.roundCount + " / " + gm.maxRound;
        }
    }
}
