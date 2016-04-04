/**************************************************************************
Copyright:@cartzhang
Author: cartzhang
Date: 2016-04-01
Description:加载关卡，可以分组加载和卸载。使用Unity版本为5.3.0.
因为里面使用了场景管理的一个类，这个类在5.3.0以上版本才添加的。
测试操作：使用空格键来切换场景，然后间隔5秒后才开始卸载。
**************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LevelOrder
{

    [Header("每组关卡名称")]
    public string[] LevelNames;
}

public class ChangLevelsHasMain : MonoBehaviour
{
    [Header("所有关卡列表")]
    public LevelOrder[] levelOrder;
    private static int index;
    private int totalLevels = 0;
    private int levelOrderLength;
    
    void Start ()
    {
        for (int i = 0; i < levelOrder.Length; i++)
        {
            totalLevels += levelOrder[i].LevelNames.Length;
        }

        if (totalLevels != SceneManager.sceneCountInBuildSettings)
        {
            
        }
        levelOrderLength = levelOrder.Length;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool isOk = LoadNextLevels();
            if (isOk)
            {
                InvokeRepeating("UnloadLastLevel", 2.0f, 5);
            }
        }
	}

    bool LoadNextLevels()
    {
        bool bResult = true;
        //index = index % levelOrderLength;
        if (index < 0 || index >= levelOrderLength)
        {
            bResult = false;
            return bResult;
        }

        int LoadTimes = levelOrder[index].LevelNames.Length;
        for (int i = 0; i < LoadTimes; i++)
        {
            SceneManager.LoadSceneAsync(levelOrder[index].LevelNames[i], LoadSceneMode.Additive);
        }
        return bResult;
    }
    
    void UnloadLastLevel()
    {
        if (index == 0)
        {
            index++;
            CancelInvoke("UnloadLastLevel");
            return;
        }
        // 上一組的關卡
        int TmpLast = (index - 1) >= 0 ? (index - 1) : levelOrderLength - 1;
        int LoadTimes = levelOrder[index].LevelNames.Length;
        for (int i = 0; i < LoadTimes; i++)
        {
            Scene Tmp = SceneManager.GetSceneByName(levelOrder[index].LevelNames[i]);
            if (!Tmp.isLoaded)
            {
                return;
            }
        }
        
        // 下一關卡全部加載完畢後，卸載之前關卡
        for (int i = 0; i < levelOrder[TmpLast].LevelNames.Length; i++)
        {
            SceneManager.UnloadScene(levelOrder[TmpLast].LevelNames[i]);
        }
        index++;
        CancelInvoke("UnloadLastLevel");
    }
}
