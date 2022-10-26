using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTips : MonoBehaviour
{
    public GameObject level_1_TipsPanel;
    public GameObject level_2_TipsPanel;
    public GameObject level_4_TipsPanel;
    public GameObject level_6_TipsPanel;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        OpenTipsPanel(LevelManager.currentLevelId);
    }

    public void OpenTipsPanel(int _currentLevel)
    {
        switch (_currentLevel)
        {
            case 1:
                UIManager.instance.PopShowPanel(level_1_TipsPanel);
                UIManager.instance.isShowingTips = true;
                break;
            case 2:
                UIManager.instance.PopShowPanel(level_2_TipsPanel);
                UIManager.instance.isShowingTips = true;
                break;
            case 4:
                UIManager.instance.PopShowPanel(level_4_TipsPanel);
                UIManager.instance.isShowingTips = true;
                break;
            case 6:
                UIManager.instance.PopShowPanel(level_6_TipsPanel);
                UIManager.instance.isShowingTips = true;
                break;
        }
    }

    public void CloseTipsPanel(int _currentLevel)
    {
        switch (_currentLevel)
        {
            case 1:
                UIManager.instance.PopHidePanel(level_1_TipsPanel);
                UIManager.instance.isShowingTips = false;
                break;
            case 2:
                UIManager.instance.PopHidePanel(level_2_TipsPanel);
                UIManager.instance.isShowingTips = false;
                break;
            case 4:
                UIManager.instance.PopHidePanel(level_4_TipsPanel);
                UIManager.instance.isShowingTips = false;
                break;
            case 6:
                UIManager.instance.PopHidePanel(level_6_TipsPanel);
                UIManager.instance.isShowingTips = false;
                break;
        }
    }
}