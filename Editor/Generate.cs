using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldGeneration_Base), true)]
public class Generate : Editor {
    public override void OnInspectorGUI() {
        WorldGeneration_Base mapGen = (WorldGeneration_Base) target;

        if (DrawDefaultInspector ()) {
            if (mapGen.autoGenerate) {
                mapGen.ResetTiles();
                mapGen.Generate();
            }
        }

        if (GUILayout.Button ("Generate")) {
            mapGen.ResetTiles();
            mapGen.Generate();
        }
    }
}
