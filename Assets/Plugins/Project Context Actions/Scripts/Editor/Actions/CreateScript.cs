﻿/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.IO;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.ProjectContextActions.Actions
{
    [InitializeOnLoad]
    public static class CreateScript
    {
        static CreateScript()
        {
            ItemDrawer.Register(ItemDrawers.CreateScript, DrawButton, 10);
        }
        
        private static void AppendBuiltInTemplates(ProjectItem item, GenericMenu menu)
        {
            menu.AddItem(new GUIContent("C# Script"), false, OnCreateScript, new object[] { item.asset, "C# Script" });
            menu.AddItem(new GUIContent("C# ScriptableObject"), false, OnCreateScript, new object[] { item.asset, "C# ScriptableObject" });
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("C# Class"), false, OnCreateScript, new object[] { item.asset, "C# Class" });
            menu.AddItem(new GUIContent("C# Interface"), false, OnCreateScript, new object[] { item.asset, "C# Interface" });
            menu.AddItem(new GUIContent("C# Abstract Class"), false, OnCreateScript, new object[] { item.asset, "C# Abstract Class" });
            menu.AddItem(new GUIContent("C# Struct"), false, OnCreateScript, new object[] { item.asset, "C# Struct" });
            menu.AddItem(new GUIContent("C# Enum"), false, OnCreateScript, new object[] { item.asset, "C# Enum" });
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("C# Custom Editor Script"), false, OnCreateScript, new object[] { item.asset, "C# Custom Editor" });
            menu.AddItem(new GUIContent("C# Custom Property Drawer"), false, OnCreateScript, new object[] { item.asset, "C# Custom Property Drawer" });
            menu.AddItem(new GUIContent("C# Editor Window Script"), false, OnCreateScript, new object[] { item.asset, "C# Editor Window" });
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("C# Test Script"), false, OnCreateScript, new object[] { item.asset, "C# Test Script" });
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Assembly Definition"), false, OnCreateScript, new object[] { item.asset, "Assembly Definition" });
            menu.AddItem(new GUIContent("Assembly Definition Reference"), false, OnCreateScript, new object[] { item.asset, "Assembly Definition Reference" });
        }

        private static void AppendUserTemplates(ProjectItem item, GenericMenu menu)
        {
            string[] files = Directory.GetFiles("Assets/", "*.txt", SearchOption.AllDirectories);
            if (files.Length == 0) return;
            
            menu.AddSeparator("");
            
            string scriptTemplatesFolder = Utils.ScriptTemplatesFolder.Replace('/', '\\');

            foreach (string file in files)
            {
                if (!file.EndsWith(".cs.txt")) continue;
                
                if (file.Replace('/', '\\').StartsWith(scriptTemplatesFolder)) continue;
                
                string name = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(file));
                menu.AddItem(new GUIContent("User Templates/" + name), false, CreateUserScript, new object[] { item.asset, file });
            }
        }
        
        private static void CreateUserScript(object userdata)
        {
            object[] data = (object[])userdata;
            Object asset = data[0] as Object;
            string path = (string)data[1];
            
            Selection.activeObject = asset;
            string defaultName = Path.GetFileNameWithoutExtension(path);
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(path, defaultName);
        }

        private static void DrawButton(ProjectItem item)
        {
            if (!item.isFolder) return;
            if (!item.hovered) return;
            if (!item.path.StartsWith("Assets")) return;
            if (!item.path.Contains("Scripts")) return;

            Rect r = item.rect;
            r.xMin = r.xMax - 18;
            r.height = 16;

            item.rect.xMax -= 18;

            ButtonEvent be = GUILayoutUtils.Button(r, TempContent.Get(EditorIconContents.CsScript.image, "Create Script\n(Right click to select a template)"), GUIStyle.none);
            if (be == ButtonEvent.Click) ProcessClick(item);
        }

        private static void OnCreateScript(object userdata)
        {
            object[] data = (object[])userdata;
            Object asset = data[0] as Object;
            string name = (string)data[1];
            string path = null;

            string[] files = Directory.GetFiles(Utils.ScriptTemplatesFolder, "*.txt", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                if (Path.GetFileName(file).StartsWith(name + "-"))
                {
                    path = file;
                    break;
                }
            }
            
            if (path == null) return;

            Selection.activeObject = asset;
            string defaultName = Path.GetFileNameWithoutExtension(path).Substring(name.Length + 1);
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(path, defaultName);
        }

        private static void ProcessClick(ProjectItem item)
        {
            if (Event.current.button == 0)
            {
                OnCreateScript(new object[] { item.asset, "C# Script" });
            }
            else if (Event.current.button == 1)
            {
                ShowTemplatesMenu(item);
            }
        }

        private static void ShowTemplatesMenu(ProjectItem item)
        {
            GenericMenu menu = new GenericMenu();

            AppendBuiltInTemplates(item, menu);
            AppendUserTemplates(item, menu);

            menu.ShowAsContext();
        }
    }
}