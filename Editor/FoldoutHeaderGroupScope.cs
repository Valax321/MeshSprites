using System;
using UnityEditor;
using UnityEngine;

namespace Valax321.MeshSprites.Editor
{
    internal class FoldoutHeaderGroupScope : IDisposable
    {
        public bool show { get; }

        private readonly string m_groupKey;
        private readonly bool m_unfolded;

        public FoldoutHeaderGroupScope(string content, string groupKey, GUIStyle style = null, 
            Action<Rect> menuAction = null, GUIStyle menuIcon = null)
        {
            m_groupKey = $"{groupKey}:show";
            m_unfolded = EditorPrefs.GetBool(m_groupKey, false);
            EditorGUI.indentLevel++;
            show = EditorGUILayout.BeginFoldoutHeaderGroup(m_unfolded, content, style, menuAction, menuIcon);
        }
        
        public void Dispose()
        {
            EditorGUI.indentLevel--;
            
            if (show != m_unfolded)
                EditorPrefs.SetBool(m_groupKey, show);
            
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }
}