using UnityEngine;
using UnityEngine.EventSystems;



namespace Minesweeper
{
    public class ResetButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        public Minesweep instance;
        public SmileySprites sprites;
        internal SpriteRenderer renderer;

        public void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
            instance.resetButton = this;
        }

        [System.Serializable]
        public class SmileySprites
        {
            public Sprite normal;
            public Sprite clicked;
            public Sprite lose;
            public Sprite shock;
            public Sprite win;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            renderer.sprite = sprites.clicked;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            instance.Reset();
            renderer.sprite = sprites.normal;
        }

        public void Lose()
        {
            renderer.sprite = sprites.lose;
        }

        public void Win()
        {
            renderer.sprite = sprites.win;
        }
    }
}