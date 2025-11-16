using UnityEngine;
using TMPro;

public class ControlLabel : MonoBehaviour
{
    public PlayerMovement player;              
    public TextMeshProUGUI uiText;             
    public string buttonVariableName;          

    void Start()
    {
        string inputName = (string)typeof(PlayerMovement)
            .GetField(buttonVariableName)
            .GetValue(player);

        string key = GetAssignedKey(inputName);

        uiText.text = key;
    }

    string GetAssignedKey(string inputName)
    {
#if UNITY_EDITOR
        var obj = new UnityEditor.SerializedObject(
            UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);

        var axes = obj.FindProperty("m_Axes");

        for (int i = 0; i < axes.arraySize; i++)
        {
            var axis = axes.GetArrayElementAtIndex(i);

            if (axis.FindPropertyRelative("m_Name").stringValue == inputName)
            {
                string key = axis.FindPropertyRelative("positiveButton").stringValue;
                if (!string.IsNullOrEmpty(key)) return key;

                string alt = axis.FindPropertyRelative("altPositiveButton").stringValue;
                if (!string.IsNullOrEmpty(alt)) return alt;

                return "None";
            }
        }
#endif

        return "None";
    }
}
