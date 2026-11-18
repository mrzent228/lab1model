using System;
using System.Collections.Generic;

namespace Lab9_UniversityDeployment.Artifacts
{
    public enum ArtifactType
    {
        Executable,     // .exe
        Library,        // .dll
        Configuration,  // .json
        DatabaseSchema  // .sql / DB
    }

    // UML «artifact» — фізичний файл або компонент розгортання
    public class SoftwareArtifact
    {
        public string FileName { get; }
        public ArtifactType Type { get; }
        public string Version { get; }
        public List<SoftwareArtifact> ManifestsDependencies { get; } = new List<SoftwareArtifact>();

        public SoftwareArtifact(string fileName, ArtifactType type, string version)
        {
            FileName = fileName;
            Type = type;
            Version = version;
        }

        public void AddDependency(SoftwareArtifact artifact)
        {
            ManifestsDependencies.Add(artifact);
        }

        public override string ToString() => $"«artifact» {FileName} (v{Version}, {Type})";
    }
}