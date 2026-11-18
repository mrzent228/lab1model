using System;
using System.Collections.Generic;
using Lab9_UniversityDeployment.Artifacts;

namespace Lab9_UniversityDeployment.Topology
{
    // UML «device» / «execution environment» — вузол розгортання
    public class DeploymentNode
    {
        public string NodeName { get; }
        public string Stereotype { get; }
        public string IpAddress { get; }
        public string ExecutionEnvironment { get; }
        public List<SoftwareArtifact> DeployedArtifacts { get; } = new List<SoftwareArtifact>();

        public DeploymentNode(string nodeName, string stereotype, string ipAddress, string executionEnvironment)
        {
            NodeName = nodeName;
            Stereotype = stereotype;
            IpAddress = ipAddress;
            ExecutionEnvironment = executionEnvironment;
        }

        public void DeployArtifact(SoftwareArtifact artifact)
        {
            DeployedArtifacts.Add(artifact);
            Console.WriteLine($"  [Deploy] На вузол '{NodeName}' ({IpAddress}) розгорнуто {artifact}");
        }
    }

    // UML Communication Path — мережевий канал між вузлами
    public class CommunicationLink
    {
        public DeploymentNode Source { get; }
        public DeploymentNode Target { get; }
        public string Protocol { get; }
        public int Port { get; }

        public CommunicationLink(DeploymentNode source, DeploymentNode target, string protocol, int port)
        {
            Source = source;
            Target = target;
            Protocol = protocol;
            Port = port;
        }

        public void VerifyConnection()
        {
            Console.WriteLine($"  [Ping OK] {Source.NodeName} ({Source.IpAddress}) ===[{Protocol} : {Port}]===> {Target.NodeName} ({Target.IpAddress})");
        }
    }
}