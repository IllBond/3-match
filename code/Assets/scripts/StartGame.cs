using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void startGameSize(int size) {
        gameManager.instance_gameManager.StartGame(size);
    }
}
