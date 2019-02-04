using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SkeletonScripts
{
    [CreateAssetMenu(fileName = "SkeletonScriptReplaceDefinition", menuName = "Skeleton Script/Find And Replace Settings")]
    public class SkeletonScriptReplace : ScriptableObject
    {
        public bool enableReplaceList = true;
        public List<FindAndReplace> ReplaceList = new List<FindAndReplace>();
        public bool enableReplaceWithFileNameList = true;
        public List<string> ReplaceWithFileNameList = new List<string>() { "#SCRIPTNAME#" };
        [System.Serializable]
        public struct FindAndReplace
        {
            public string Find;
            public string Replace;
        }
        private string Replace(string SourceLine, string Find, string Replace)
        {
            if (Find != Replace && Find != "" && Find != null)
            {
                string result = SourceLine;
                while (result.Contains(Find))
                {
                    int Place = result.IndexOf(Find);
                    result = result.Remove(Place, Find.Length).Insert(Place, Replace);
                }
                return result;
            }
            return SourceLine;

        }
        public string Replace(string Source, string fileName = null)
        {
            if (Source != null && Source != "")
            {
                foreach (FindAndReplace findAndReplace in ReplaceList)
                {
                    Source = Replace(Source, findAndReplace.Find, findAndReplace.Replace);
                }
                if (enableReplaceWithFileNameList && fileName != null && fileName != "")
                {
                    foreach (string find in ReplaceWithFileNameList)
                    {
                        Source = Replace(Source, find, fileName);
                    }
                }
            }
            return Source;
        }
    }
}