using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentRound : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI roundText;
    private GameManager gm;

    void Start()
    {
        gm = GameManager.Instance;
    }

    void Update()
    {
        if (roundText != null)
        {
            roundText.text = "Round " + GameManager.Instance.roundCount.ToString() + " / " + GameManager.Instance.maxRound.ToString();
        }
    }
}
