using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Save"))
        {
            SaveLevel();
        }
    }

    private void SaveLevel()
    {
        GameObject[] objectArray = Selection.gameObjects;

        foreach (GameObject gameObject in objectArray)
        {
            string localPath = "Assets/Resources/Prefabs/Levels/" + gameObject.GetComponent<Level>().levelName + ".prefab";
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
            PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, localPath, InteractionMode.UserAction);
        }
    }
}
