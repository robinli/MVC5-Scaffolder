using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Scaffolding;

namespace Happy.Scaffolding.MVC.Utils
{
    internal class ProjectItemUtils
    {
        public static readonly string PathSeparator = Path.DirectorySeparatorChar.ToString();



        public static string GetProjectRelativePath(ProjectItem projectItem)
        {
            Project project = projectItem.ContainingProject;
            string projRelativePath = null;

            string rootProjectDir = project.GetFullPath();
            rootProjectDir = EnsureTrailingBackSlash(rootProjectDir);
            string fullPath = projectItem.GetFullPath();

            if (!String.IsNullOrEmpty(rootProjectDir) && !String.IsNullOrEmpty(fullPath))
            {
                projRelativePath = MakeRelativePath(fullPath, rootProjectDir);
            }

            return projRelativePath;
        }


        public static string EnsureTrailingBackSlash(string str)
        {
            if (str != null && !str.EndsWith(PathSeparator, StringComparison.Ordinal))
            {
                str += PathSeparator;
            }
            return str;
        }


        public static string MakeRelativePath(string fullPath, string basePath)
        {
            string tempBasePath = basePath;
            string tempFullPath = fullPath;
            StringBuilder relativePath = new StringBuilder();

            if (!tempBasePath.EndsWith(PathSeparator, StringComparison.OrdinalIgnoreCase))
            {
                tempBasePath += PathSeparator;
            }

            while (!String.IsNullOrEmpty(tempBasePath))
            {
                if (tempFullPath.StartsWith(tempBasePath, StringComparison.OrdinalIgnoreCase))
                {
                    relativePath.Append(fullPath.Remove(0, tempBasePath.Length));
                    if (String.Equals(relativePath.ToString(), PathSeparator, StringComparison.OrdinalIgnoreCase))
                    {
                        relativePath.Clear();
                    }
                    return relativePath.ToString();
                }
                else
                {
                    tempBasePath = tempBasePath.Remove(tempBasePath.Length - 1);
                    int lastIndex = tempBasePath.LastIndexOf(PathSeparator, StringComparison.OrdinalIgnoreCase);
                    if (-1 != lastIndex)
                    {
                        tempBasePath = tempBasePath.Remove(lastIndex + 1);
                        relativePath.Append("..");
                        relativePath.Append(PathSeparator);
                    }
                    else
                    {
                        return fullPath;
                    }
                }
            }

            return fullPath;
        }





    }
}
