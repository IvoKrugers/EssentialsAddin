using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GLib;
using MonoDevelop.Ide;

namespace EssentialsAddin.GitHub
{
    public static class GitHelper
    {
        private static string GetGitDir(string subdir)
        {
            var r = Path.GetPathRoot(subdir);
            while (!string.IsNullOrEmpty(subdir) && subdir != r)
            {
                var gd = Path.Combine(subdir, ".git");
                if (Directory.Exists(gd))
                    return gd;
                subdir = Path.GetDirectoryName(subdir);
            }
            return null;
        }

        public static string GetCurrentBranch()
        {
            var solution = IdeApp.Workspace.CurrentSelectedSolution;
            if (solution is null)
                return null;

            var gitDir = GetGitDir(solution.FileName);

            if (gitDir is null)
                return null;

            var file = new GitConfigFile();
            file.LoadFile(Path.Combine(gitDir, "config"));

            string head = File.ReadAllText(Path.Combine(gitDir, "HEAD")).Trim();
            string branch;
            string remote = null;
            if (head.StartsWith("ref: refs/heads/"))
            {
                branch = head.Substring(16);
                var sec = file.Sections.FirstOrDefault(s => s.Type == "branch" && s.Name == branch);
                if (sec != null)
                    remote = sec.GetValue("remote");
            }
            else
            {
                branch = head;
            }
            if (string.IsNullOrEmpty(remote))
                remote = "origin";

            return branch;
        }

        internal static string GetUrl()
        {
            var doc = IdeApp.Workbench.ActiveDocument;
            if (doc is null)
                return null;

            var gitDir = GetGitDir(doc.FileName);

            if (gitDir == null)
                return null;

            var rootDir = Path.GetDirectoryName(gitDir);

            var gitModulesFile = Path.Combine(rootDir, ".gitmodules");
            GitConfigSection submoduleSection = null;
            if (File.Exists(gitModulesFile))
            {
                var modulesConfig = new GitConfigFile();
                modulesConfig.LoadFile(gitModulesFile);
                foreach (var section in modulesConfig.Sections)
                {
                    //Checking if file is inside submodule folder
                    if (section.Type == "submodule" &&
                        section.GetValue("path") != null &&
                        Path.GetFullPath(doc.FileName).StartsWith(Path.Combine(rootDir, Path.Combine(section.GetValue("path").Split('/'))), StringComparison.Ordinal))
                    {
                        gitDir = Path.Combine(gitDir, "modules", Path.Combine(section.GetValue("path").Split('/')));
                        submoduleSection = section;
                        break;
                    }
                }
            }

            var file = new GitConfigFile();
            file.LoadFile(Path.Combine(gitDir, "config"));

            string head = File.ReadAllText(Path.Combine(gitDir, "HEAD")).Trim();
            string branch;
            string remote = null;
            if (head.StartsWith("ref: refs/heads/"))
            {
                branch = head.Substring(16);
                var sec = file.Sections.FirstOrDefault(s => s.Type == "branch" && s.Name == branch);
                if (sec != null)
                    remote = sec.GetValue("remote");
            }
            else
            {
                branch = head;
            }
            if (string.IsNullOrEmpty(remote))
                remote = "origin";
            var rref = file.Sections.FirstOrDefault(s => s.Type == "remote" && s.Name == remote);
            if (rref == null)
                return null;

            var url = rref.GetValue("url");
            if (url.EndsWith(".git"))
                url = url.Substring(0, url.Length - 4);

            string host;

            int k = url.IndexOfAny(new[] { ':', '@' });
            if (k != -1 && url[k] == '@')
            {
                k++;
                int i = url.IndexOf(':', k);
                if (i != -1)
                    host = url.Substring(k, i - k);
                else
                    return null;
            }
            else
            {
                Uri uri;
                if (Uri.TryCreate(url, UriKind.Absolute, out uri))
                    host = uri.Host;
                else
                    return null;
            }

            int j = url.IndexOf(host);
            var repo = url.Substring(j + host.Length + 1);

            var fileRootDir = submoduleSection == null ? rootDir : Path.Combine(rootDir, Path.Combine(submoduleSection.GetValue("path").Split('/')));
            string subdir = doc.FileName.ToRelative(fileRootDir);
            subdir = subdir.Replace('\\', '/');
            string tline;
            if (doc.Editor.SelectionRange.Offset != doc.Editor.SelectionRange.EndOffset)
            {
                var line1 = doc.Editor.OffsetToLineNumber(doc.Editor.SelectionRange.Offset);
                var line2 = doc.Editor.OffsetToLineNumber(doc.Editor.SelectionRange.EndOffset);
                tline = "L" + line1.ToString();
                if (line1 != line2)
                    tline += "-L" + line2;
            }
            else
            {
                tline = "L" + doc.Editor.CaretLine;
            }
            return "https://" + host + "/" + repo + "/blob/" + branch + "/" + subdir + "#" + tline;
        }

        private static GitConfigFile LoadConfigFile()
        {
            var solution = IdeApp.Workspace.CurrentSelectedSolution;
            if (solution is null)
                return null;

            var gitDir = GetGitDir(solution.FileName);

            if (gitDir == null)
                return null;

            var file = new GitConfigFile();
            file.LoadFile(Path.Combine(gitDir, "config"));
            return file;
        }

        internal static List<string> GetLocalBranches()
        {
            var file = LoadConfigFile();

            var list = file?.Sections?
                .Where(s => s.Type == "branch")
                .Select(s => s.Name).ToList();
            return list ?? new List<string>();
        }
    }
}

