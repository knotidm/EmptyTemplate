/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace InfinityCode.ProjectContextActions.Actions
{
    [InitializeOnLoad]
    public static class CreateMaterial
    {
        static CreateMaterial()
        {
            ItemDrawer.Register(ItemDrawers.CreateMaterial, DrawButton, 10);
        }
        
        private static void Create(ProjectItem item)
        {
            Selection.activeObject = item.asset;

            Shader shader;

#if UNITY_6000_0_OR_NEWER
            RenderPipelineAsset rp = GraphicsSettings.defaultRenderPipeline;
#else
            RenderPipelineAsset rp = GraphicsSettings.renderPipelineAsset;
#endif
            if (rp != null)
            {
                if (rp.GetType().Name.Contains("HDRenderPipelineAsset"))
                {
                    shader = Shader.Find("HDRenderPipeline/Lit");
                }
                else
                {
                    shader = Shader.Find("Universal Render Pipeline/Lit");
                }
            }
            else shader = Shader.Find("Standard"); 
            
            Material material = new Material(shader);
            ProjectWindowUtil.CreateAsset(material, "New Material.mat");
        }

        private static void DrawButton(ProjectItem item)
        {
            if (!item.isFolder) return;
            if (!item.hovered) return;
            if (!item.path.StartsWith("Assets")) return;
            if (!item.path.Contains("Material")) return;

            Rect r = item.rect;
            r.xMin = r.xMax - 18;
            r.height = 16;

            item.rect.xMax -= 18;

            GUIContent content = TempContent.Get(EditorIconContents.Material.image, "Create Material");
            if (GUI.Button(r, content, GUIStyle.none)) Create(item);
        }
    }
}