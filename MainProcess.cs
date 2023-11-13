using ASNParserApp.DTOs;

namespace ASNParserApp
{
	public class MainProcess
	{
		FileSystemWatcher? watcher;
		Helpers helpers;

		public MainProcess()
		{
			this.helpers = new Helpers();
		}

		public void MonitorFolder()
		{
			watcher = new FileSystemWatcher();
			watcher.Path = @"C:\Users\Alexa\Desktop\VR tasks\asn_files";
			watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
			watcher.Filter = "*.*"; // could be only *.txt or only *.doc or multiple file types, depends on the requirements

			watcher.Created += new FileSystemEventHandler(ParsingProcess);

			watcher.EnableRaisingEvents = true;
		}

		private async void ParsingProcess(object source, FileSystemEventArgs e)
		{
			List<BoxDTO> boxesToSave = new List<BoxDTO>();

			// reading file contents line by line
			using (var reader = new StreamReader(e.FullPath))
			{
				string? line;
				BoxDTO? currentBox = null;

				while ((line = reader.ReadLine()) != null)
				{
					// ignoring empty lines
					if (!string.IsNullOrEmpty(line))
					{
						// splitting line into separate values, ignoring all empty strings
						var values = line.Split(" ").Where(x => !string.IsNullOrEmpty(x)).ToList();

						// indicates that new box starts
						// it is assumed that in the whole file, a single box with certain SupplierIdentifier and Carton box identifier is met only once
						// otherwise there would be a need to check, whether the box already exists in boxesToSave list to append the box content
						if (values.First().Trim() == "HDR")
						{
							// previous processed box should be added to boxesToSave list and cleared
							if (currentBox != null)
							{
								boxesToSave.Add(currentBox);
								currentBox = null;
							}

							try
							{
								currentBox = helpers.CreateNewBox(values);
							}
							catch (Exception ex)
							{
								Console.WriteLine($"New box creation failed: {ex.Message}");
							}
						}

						if (currentBox != null && values.First().Trim() == "LINE")
						{
							try
							{
								currentBox.Contents.Add(helpers.CreateNewBoxContent(values));
							}
							catch (Exception ex)
							{
								Console.WriteLine($"New box content creation and/or append to existing box failed: {ex.Message}");
							}
						}
					}
				}
			}

			if (boxesToSave.Count > 0)
			{
				try
				{
					await helpers.SaveBoxesAndContents(boxesToSave);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Saving to DB failed: {ex.Message}");
				}

				Console.WriteLine($"Successfully parsed file with path: {e.FullPath}.");
				Console.WriteLine($"Parsed and saved {boxesToSave.Count} boxes.");
			}
			else
			{
				Console.WriteLine("The file was empty, no items to save.");
			}
		}
	}
}