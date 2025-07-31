using System.Collections;
using System.Collections.Generic;
using Melon;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMain : GameElement
{
    // MEMO : 게임을 선택 하는 UI를 띄우는 곳

    public UIGameLabelButton uiCubeStackButton;
    
    public override void OnAwakeFunc()
    {
        base.OnAwakeFunc();
        
        uiCubeStackButton.SetClickAction(() =>
        {
            // 씬 전환
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("CubeStack", LoadSceneMode.Single);
        });
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            var gameFramework = root.GetComponentInChildren<GameFramework>(true); // true = 비활성 포함
            if (gameFramework != null)
            {
                gameFramework.InitFramework();
                gameFramework.GameStart();
                break;
            }
        }
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
