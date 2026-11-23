using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC2022.Modules;

public class Day07 : DayBase
{
    public override bool Ignore => true;
    private const string cdPrefix = "$ cd ";
    private const string cdBack = "..";
    private const string lsPrefix = "$ ls";
    private const string dirPrefix = "dir ";
    private static Regex fileRegex = new Regex(@"^(\d+) (.*)$");

    private List<TerminalDirectory> GetDirectories(List<string> terminalLines)
    {
        
        var _directories = new List<TerminalDirectory>();
        var currentPath = string.Empty;
        var currentDirectory = new TerminalDirectory();
        foreach (var terminalLine in terminalLines)
        {
            if (terminalLine.StartsWith(cdPrefix))
            {
                var cdArg = terminalLine.Replace(cdPrefix, "");
                if (cdArg == cdBack)
                {
                    var currentDirectoryParts = currentPath.Split('/', StringSplitOptions.RemoveEmptyEntries).ToList();
                    currentPath = "/" + string.Join("/", currentDirectoryParts.Take(currentDirectoryParts.Count - 1));
                }
                else
                    currentPath = currentPath.TrimEnd('/') + "/" + $"{(cdArg == "/" ? "" : cdArg)}";

                currentDirectory = _directories.FirstOrDefault(x => x.FullPath == currentPath);
                if (currentDirectory == null)
                {
                    _directories.Add(new TerminalDirectory
                    {
                        FullPath = currentPath,
                        Name = cdArg
                    });
                    currentDirectory = _directories.FirstOrDefault(x => x.FullPath == currentPath);
                }
            } 
            else if (!terminalLine.StartsWith(lsPrefix))
            {
                if (terminalLine.StartsWith(dirPrefix))
                {
                    var dirArg = terminalLine.Replace(dirPrefix, "");
                    var dirFullPath = currentPath.TrimEnd('/') + $"/{dirArg}";
                    if (_directories.All(x => x.FullPath != dirFullPath)) 
                        _directories.Add(new TerminalDirectory
                        {
                            FullPath = dirFullPath,
                            Name = dirArg
                        });
                    currentDirectory.Directories.Add(_directories.FirstOrDefault(x => x.FullPath == dirFullPath));
                }
                else
                {
                    var match = fileRegex.Match(terminalLine);
                    currentDirectory.Files.Add(new TerminalFile
                    {
                        Name = match.Groups[2].Value,
                        Size = long.Parse(match.Groups[1].Value)
                    });
                }
            }
        }

        return _directories;
    }
    
    public override dynamic Part1()
    {
        var directories = GetDirectories(get_input().ToList());
        var maxSize = 100000L;
        var directoriesOfIdealSize = directories.Where(x => x.Size <= maxSize).ToList();
        var totalIdealSizes = directoriesOfIdealSize.Sum(x => x.Size);
        
        return new { sizeToBeDeleted = totalIdealSizes };
    }

    public override dynamic Part2()
    {
        var directories = GetDirectories(get_input().ToList());
        var totalAvailableSpace = 70000000L;
        var unusedSpaceNeeded = 30000000L;
        var spaceUsed = directories.FirstOrDefault().Size;
        var unusedSpace = totalAvailableSpace - spaceUsed;
        var minSizeToBeDeleted = unusedSpaceNeeded - unusedSpace;
        var directoryToDelete = directories.Where(x => x.Size >= minSizeToBeDeleted)
            .MinBy(x => x.Size);
        
        
        return new { directoryToDelete.FullPath, directoryToDelete.Size };
    }

    private class TerminalDirectory
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public List<TerminalFile> Files { get; set; } = new List<TerminalFile>();
        public List<TerminalDirectory> Directories { get; set; } = new List<TerminalDirectory>();
        public long Size => Files.Sum(x => x.Size) + Directories.Sum(x => x.Size);
    }
    
    private class TerminalFile
    {
        public string Name { get; set; }
        public long Size { get; set; }
        
    }
}