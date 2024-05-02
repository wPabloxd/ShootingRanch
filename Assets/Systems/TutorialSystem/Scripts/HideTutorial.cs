using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HideTutorial : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    public void HideTutorials()
    {
        text.text = null;
    }
}
