using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEditor;
namespace SkeletonScripts
{
    [CreateAssetMenu(fileName = "SkeletonScriptDefinition", menuName = "Skeleton Script/Definition")]
    public class SkeletonScript : ScriptableObject
    {
        public Object rawFile;
        public string extention = ".";
        [TextArea]
        public string lines;
        public void GetInfoFrom(string path)
        {
            lines = "";
            extention = "";
            if (File.Exists(path))
            {
                int counter = 0;
                string line;
                System.IO.StreamReader file =
                    new System.IO.StreamReader(path);
                while ((line = file.ReadLine()) != null)
                {
                    lines += line + "\n";
                    counter++;
                }
                file.Close();
            }
        }
        public void CreateFile(string path, SkeletonScriptReplace skeletonScriptReplace = null, string nameOfFile = null)
        {
            if (!File.Exists(path))
            {
                // Create a new file   
                StreamWriter sw = File.CreateText(path + "\\" + nameOfFile + extention);
                if (skeletonScriptReplace != null)
                {
                    string aLine = null;
                    StringReader strReader = new StringReader(lines);
                    while ((aLine = strReader.ReadLine()) != null)
                    {
                        sw.WriteLine(skeletonScriptReplace.Replace(aLine, Path.GetFileNameWithoutExtension(nameOfFile)));
                    }

                }
                sw.Close();
            }
        }

        public string GetPreview(SkeletonScriptReplace skeletonScriptReplace = null, string nameOfFile = null)
        {
            string retVal = "";
            if (skeletonScriptReplace != null)
            {
                string aLine = null;
                StringReader strReader = new StringReader(lines);
                while ((aLine = strReader.ReadLine()) != null)
                {
                    retVal += skeletonScriptReplace.Replace(aLine, Path.GetFileNameWithoutExtension(nameOfFile)) + "\n";
                }
            }
            return retVal;
        }
        [ContextMenu("Update info from file")]
        public void UpdateInfo()
        {
            if (rawFile != null)
            {
                string path = AssetDatabase.GetAssetPath(rawFile);
                extention = Path.GetExtension(path);
                System.IO.StreamReader file = new System.IO.StreamReader(Path.GetFullPath(path));
                string line;
                lines = "";
                while ((line = file.ReadLine()) != null)
                {
                    lines += line + "\n";
                }
                file.Close();
            }
            else
            {

            }
        }
    }
}