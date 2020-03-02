using System.Collections.Generic;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Projects;
using Newtonsoft.Json;

namespace Diagram.Storage
{
	public class DiagramStorage
	{
		public static DiagramStorage Instance => instance ?? (instance = new DiagramStorage());
		static DiagramStorage instance;

		const string FILE_NAME = "diagram.json";

		JsonSerializer serializer;

		DiagramStorage()
		{
			serializer = new JsonSerializer();
		}

		public StoredDiagramDetails GetProjectDetails(Project project)
		{
			var storedDetails = ReadStoredDetails(project);
			if (!storedDetails.ContainsKey(project.ItemId))
				return new StoredDiagramDetails();

			return JsonConvert.DeserializeObject<StoredDiagramDetails>(storedDetails[project.ItemId]);
		}

		public void SaveProjectDetails(Project project, StoredDiagramDetails details)
		{
			var storedDetails = ReadStoredDetails(project);
			storedDetails[project.ItemId] = JsonConvert.SerializeObject(details);

			using (StreamWriter writer = File.CreateText(JsonFilePath(project)))
				new JsonSerializer().Serialize(writer, storedDetails);
		}

		Dictionary<string, string> ReadStoredDetails(Project project)
		{
			using (StreamReader reader = File.OpenText(JsonFilePath(project)))
			{
				var data = (Dictionary<string, string>)serializer.Deserialize(reader, typeof(Dictionary<string, string>));
				return data ?? new Dictionary<string, string>();
			}
		}

		string JsonFilePath(Project project)
		{
			var path = project.ParentSolution.BaseDirectory.ToAbsolute(new FilePath());
			path = Path.Combine(path, FILE_NAME);

			if (!File.Exists(path))
				File.CreateText(path).Close();

			return path;
		}
	}
}
