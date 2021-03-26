using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public int coins;
    public Text coinsText;

    public void AddCoin() {
        coins++;
        coinsText.text = "" + coins;
    }
}
