using UnityEngine;
using UnityEngine.EventSystems;

namespace Minesweeper
{
    public class MineTile : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        public int x;
        public int y;
        public int type;
        public bool hidden = true;
        public bool flagged = false;
        //public GameObject gameObject;
        new public SpriteRenderer renderer;
        public Minesweep instance;

        public void Setup(int x, int y, int type, SpriteRenderer renderer, Minesweep instance) 
        { 
            this.x = x;
            this.y = y;
            this.type = type;
            this.renderer = renderer;
            this.instance = instance;
        }

        private void Update()
        {
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (instance.gameState == 2) return;
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                instance.startTime = Time.time;
                instance.keyHeld = true;
                instance.lastHeld = this;
                instance.resetButton.renderer.sprite = instance.resetButton.sprites.shock;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (instance.gameState == 2) return;
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                instance.resetButton.renderer.sprite = instance.resetButton.sprites.normal;
                if (instance.lastHeld == this && !instance.keyHeld) return;
                instance.keyHeld = false;
                Click();
            }
            else if (eventData.button == PointerEventData.InputButton.Right) Flag();
        }

        public void UpdateTile()
        {
            if (hidden)
            {
                if (flagged) renderer.sprite = instance.sprites.flag;
                else renderer.sprite = instance.sprites.unclicked;
                //renderer.sortingOrder = 0;
            }
            else
            {
                if (type == -1) renderer.sprite = instance.sprites.bomb;
                else
                {
                    renderer.sprite = instance.numberSprites[type];
                }
            }
        }

        public void Click(bool clearBombs = true)
        {
            if (instance.gameState == 0)
            {
                if (type == -1) { instance.MoveBomb(x, y); Debug.Log("First click bomb!"); }
                instance.gameState = 1;
            }
            if (flagged || !hidden) return;
            hidden = false;
            UpdateTile();
            instance.tilesClicked++;
            if (instance.tilesClicked >= instance.width * instance.height - instance.bombLimit) instance.CheckWin();
            if (type == -1) instance.bombClicked(this);
            else if (type == 0)
            {
                instance.checkEmpties(this);
            }
            if (clearBombs) instance.checkedBombs.Clear();
        }

        public void Flag()
        {
            if (!hidden) return;
            flagged = !flagged;
            if (flagged) instance.activeBombs--;
            else instance.activeBombs++;
            instance.BombCounterUpdate();
            UpdateTile();
        }
    }
}
