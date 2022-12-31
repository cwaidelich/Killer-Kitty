using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kuehlschrank : MonoBehaviour
{
    public GameManager gameManager;
    public void gameover()
    {
        gameManager.PlayerWin();
    }

}
