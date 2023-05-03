using ConnectFour.BoardGame;
using ConnectFour.Game;
using ConnectFour.Hovering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickerHandler : MonoBehaviour
{
    [SerializeField]
    private SelectionHover _hoverPawnRed;
    [SerializeField]
    private SelectionHover _hoverPawnYellow;
    [SerializeField]
    private TurnManager _turnManager;
 
    private void OnEnable()
    {
        _hoverPawnRed.OnSelected += Choose;
        _hoverPawnYellow.OnSelected += Choose;
    }

    private void OnDisable()
    {
        _hoverPawnRed.OnSelected -= Choose;
        _hoverPawnYellow.OnSelected -= Choose;
    }
    public void Choose(PawnOwner playerChoice)
    {
        _turnManager.PlayerChoose(playerChoice);
        gameObject.SetActive(true);
    }
}
