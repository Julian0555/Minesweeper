using System;
using UnityEngine;
using UnityEngine.UI;

namespace Minesweeper
{
    public class MineUI : MonoBehaviour
    {
        public Minesweep instance;
        public GameObject bottomBar;
        public GameObject leftBar;
        public GameObject rightBar;
        public GameObject topPanel;
        public GameObject smiley;

        public void Start()
        {
            int tilesWidth = instance.width * 16;
            int tilesHeight = instance.height * 16;
            int gameWidth = tilesWidth + 12 + 8;
            int gameHeight = tilesHeight + 55 + 8;

            bottomBar.transform.localScale = new Vector3(tilesWidth - 1, 1, 1);
            topPanel.transform.localPosition = new Vector3(0, tilesHeight + 8, 0);
            rightBar.transform.localPosition = new Vector3(tilesWidth + 12, 0, 0);
            rightBar.transform.Find("RightBar").transform.localScale = new Vector3(1, tilesHeight, 1);
            leftBar.transform.Find("LeftBar").transform.localScale = new Vector3(1, tilesHeight, 1);
            smiley.transform.localPosition = new Vector3(gameWidth / 2 - 10, smiley.transform.localPosition.y, smiley.transform.localPosition.z);
            topPanel.transform.Find("TopCenter").transform.localScale = new Vector3(tilesWidth - 94, 1, 1);
            topPanel.transform.Find("TopRight").transform.localPosition = new Vector3(tilesWidth - 36, 0, 0);

            instance.camera.UpdateResolution(gameWidth, gameHeight);
#if !UNITY_ANDROID
            Screen.SetResolution(gameWidth, gameHeight, FullScreenMode.Windowed);
#endif
        }
    }
}
