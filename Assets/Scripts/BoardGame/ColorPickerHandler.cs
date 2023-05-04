using ConnectFour.BoardGame;
using ConnectFour.Game;
using ConnectFour.Hovering;
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
        _hoverPawnRed.gameObject.SetActive(false);
        _hoverPawnYellow.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
