using System.Collections.Generic;
using System.Linq;
using _2DGame.Scripts.Save;
using UnityEditor;
using UnityEngine;

public class DataPersistentMenu 
{
     [MenuItem("Tools/Data Persistent/Reset default values on saved elements")]
     public static void ResetSave()
     {
         ResetScriptablesValue(GetAllInstances<ScriptableValueFloatSaveable>());
         ResetScriptablesValue(GetAllInstances<ScriptableValueBoolSaveable>());
         ResetScriptablesValue(GetAllInstances<ScriptableValueIntSaveable>());
         ResetScriptablesValue(GetAllInstances<ScriptableValueVector3Saveable>());

         // TODO : normally we have ONE handler, so we need to fix that
         DataPersistentHandler[] dataPersistentHandlers = GetAllInstances<DataPersistentHandler>();

         foreach (DataPersistentHandler dataPersistent in dataPersistentHandlers)
         {
             dataPersistent.SaveAll();
         }
     }

     private static void ResetScriptablesValue(ISave[] array) 
     {
         foreach (ISave scriptableObject in array)
         {
             scriptableObject.OnReset();
             Debug.Log($"Reset default value for : {scriptableObject}");
         }
         
         // Important to save assets, otherwise the editor will not update the file
         AssetDatabase.SaveAssets();
     }
     
     // TODO : Put that in a custom library
     /// <summary>
     /// Get all instances of T (ScriptableObject) in Asset Folder
     /// </summary>
     /// <typeparam name="T"></typeparam>
     /// <returns></returns>
     static T[] GetAllInstances<T>() where T : ScriptableObject
     {
         string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);  //FindAssets uses tags check documentation for more info
         
         int count = guids.Length;
         T[] a = new T[count];
         
         for (int i = 0; i < count; i++)         //probably could get optimized 
         {
             string path = AssetDatabase.GUIDToAssetPath(guids[i]);
             a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
         }

         return a;
     }
}
