using UnityEngine;
using UnityEngine.UI;

public class SwapButtonSprites : MonoBehaviour
{
    [Header("Sprites 1")]
    [SerializeField] private Sprite _normal1;
    [SerializeField] private Sprite _highlighted1;
    [SerializeField] private Sprite _pressed1;
    [Header("Sprites 2")]
    [SerializeField] private Sprite _normal2;
    [SerializeField] private Sprite _highlighted2;
    [SerializeField] private Sprite _pressed2;
    private Button _button;
    private bool _swap = true;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void SwapSprites()
    {
        if (_swap)
        {
            _swap = false;
            _button.image.sprite = _normal2;
            _button.spriteState = new SpriteState
            {
                highlightedSprite = _highlighted2,
                pressedSprite = _pressed2
            };
        }
        else
        {
            _swap = true;
            _button.image.sprite = _normal1;
            _button.spriteState = new SpriteState
            {
                highlightedSprite = _highlighted1,
                pressedSprite = _pressed1
            };
        }
    }
}
