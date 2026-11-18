using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Lab9_UniversityDeployment.Artifacts;
using Lab9_UniversityDeployment.Topology;

namespace Lab9_UniversityDeployment
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("=== 1. ІНІЦІАЛІЗАЦІЯ АРТЕФАКТІВ (UML ARTIFACT DIAGRAM) ===");
            var clientExe = new SoftwareArtifact("UniversityClient.exe", ArtifactType.Executable, "1.0.4");
            var clientConfig = new SoftwareArtifact("appsettings.json", ArtifactType.Configuration, "1.0.0");
            var coreDll = new SoftwareArtifact("UniversityCore.dll", ArtifactType.Library, "1.0.4");
            var edeboDll = new SoftwareArtifact("EdeboConnector.dll", ArtifactType.Library, "2.1.0");
            var dbSchema = new SoftwareArtifact("UniversityDB_Schema.sql", ArtifactType.DatabaseSchema, "1.2.0");

            // Залежності між артефактами («manifest» / «use»)
            clientExe.AddDependency(clientConfig);
            clientExe.AddDependency(coreDll);
            coreDll.AddDependency(edeboDll);
            coreDll.AddDependency(dbSchema);

            var allArtifacts = new[] { clientExe, clientConfig, coreDll, edeboDll, dbSchema };
            foreach (var art in allArtifacts)
            {
                Console.WriteLine($"  * {art}");
                foreach (var dep in art.ManifestsDependencies)
                {
                    Console.WriteLine($"      └── залежить від: {dep.FileName}");
                }
            }

            Console.WriteLine("\n=== 2. РОЗГОРТАННЯ НА ВУЗЛАХ СИСТЕМИ (UML DEPLOYMENT DIAGRAM) ===");
            var clientNode = new DeploymentNode("DeaneryWorkstation-PC", "«device»", "192.168.10.25", ".NET 8.0 Desktop Runtime");
            var appServerNode = new DeploymentNode("UniversityAppServer", "«device»", "10.0.1.10", "ASP.NET Core Kestrel Server");
            var dbServerNode = new DeploymentNode("UniversityDBServer", "«device»", "10.0.1.20", "PostgreSQL 16 DBMS");

            clientNode.DeployArtifact(clientExe);
            clientNode.DeployArtifact(clientConfig);

            appServerNode.DeployArtifact(coreDll);
            appServerNode.DeployArtifact(edeboDll);

            dbServerNode.DeployArtifact(dbSchema);

            Console.WriteLine("\n=== 3. ПЕРЕВІРКА МЕРЕЖЕВИХ КАНАЛІВ ТА ВУЗЛІВ ===");
            var links = new List<CommunicationLink>
            {
                new CommunicationLink(clientNode, appServerNode, "HTTPS / REST API", 443),
                new CommunicationLink(appServerNode, dbServerNode, "PostgreSQL TCP / TLS", 5432)
            };

            foreach (var link in links)
            {
                link.VerifyConnection();
            }

            Console.WriteLine("\n=== 4. ІНФОРМАЦІЯ ПРО ПОТОЧНУ ЗБІРКУ СЕРЕДОВИЩА ВИКОНАННЯ ===");
            var asm = Assembly.GetExecutingAssembly().GetName();
            Console.WriteLine($"  Виконуваний модуль: {asm.Name}.dll | Версія: {asm.Version}");
            Console.WriteLine($"  Середовище CLR:     {Environment.Version} | ОС: {Environment.OSVersion}");

            Console.ReadLine();
        }
    }
}