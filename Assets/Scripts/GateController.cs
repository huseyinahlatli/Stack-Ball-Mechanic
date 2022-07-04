using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GateController : MonoBehaviour
{
    [SerializeField] private enum GateType
    {
        RedGate,      
        BlueGate
    }

    [SerializeField] private GateType gateType;
    public int gateNumber;
    [SerializeField] private TMP_Text gateNumberText = null;

    public int GetGateNumber()
    {
        return gateNumber;
    }

    void Start()
    {
        RandomGateNumber();
    }

    private void RandomGateNumber()
    {
        switch(gateType)
        {
            case GateType.BlueGate : gateNumber = Random.Range(1, 10); 
                gateNumberText.text = "+" + gateNumber.ToString();
                break;
            case GateType.RedGate : gateNumber = Random.Range(-10, -1);
                gateNumberText.text = gateNumber.ToString();
                break;
        }
    }
}
