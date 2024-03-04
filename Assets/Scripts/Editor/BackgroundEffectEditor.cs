using UI;
using UnityEditor;

[CustomEditor(typeof(BackgroundEffect))]
public class BackgroundEffectEditor : Editor
{
    private SerializedProperty _mode;
    private SerializedProperty _parallaxEffectMultiplier;
    private SerializedProperty _parallaxDirection;
    private SerializedProperty _panningSpeed;
    private SerializedProperty _panningDirection;
    
    private void OnEnable()
    {
        _mode = serializedObject.FindProperty("mode");
        _parallaxEffectMultiplier = serializedObject.FindProperty("parallaxEffectMultiplier");
        _parallaxDirection = serializedObject.FindProperty("parallaxDirection");
        _panningSpeed = serializedObject.FindProperty("panningSpeed");
        _panningDirection = serializedObject.FindProperty("panningDirection");
    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(_mode);
        
        switch ((BackgroundEffect.BackgroundMode)_mode.enumValueIndex)
        {
            case BackgroundEffect.BackgroundMode.Parallax:
                EditorGUILayout.PropertyField(_parallaxEffectMultiplier);
                EditorGUILayout.PropertyField(_parallaxDirection);
                break;
            case BackgroundEffect.BackgroundMode.Panning:
                EditorGUILayout.PropertyField(_panningSpeed);
                EditorGUILayout.PropertyField(_panningDirection);
                break;
            default:
                EditorGUILayout.PropertyField(_parallaxEffectMultiplier);
                EditorGUILayout.PropertyField(_parallaxDirection);
                break;
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}