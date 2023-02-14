using UnityEditor;

//This basically enables and disables the capacity to edit the scriptable objects on the inspector
[CustomEditor(typeof(ChunkBuilder))]
public class ScriptEditor : Editor
{
    ChunkBuilder builder;
    Editor editorData;
    Editor editorNoise;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrawSettingsEditor(builder.Data,ref builder.DataState, ref editorData);
        DrawSettingsEditor(builder.Noise,ref builder.NoiseState, ref editorNoise);
    }

    private void DrawSettingsEditor(UnityEngine.Object settings,ref bool State, ref Editor editor)
    {
        if(settings != null)
        {
            using var check = new EditorGUI.ChangeCheckScope();
            State = EditorGUILayout.InspectorTitlebar(State, settings);
            if (State)
            {
                CreateCachedEditor(settings, null, ref editor);
                editor.OnInspectorGUI();
            }
        }
    }
    private void OnEnable()
    {
        builder = (ChunkBuilder)target;
    }
}
