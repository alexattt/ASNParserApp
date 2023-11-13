using ASNParserApp.DbAccess;
using ASNParserApp.DbModels;
using ASNParserApp.DTOs;

namespace ASNParserApp
{
	public class Helpers
	{
		AppDbContext dbContext;

        public Helpers()
        {
            this.dbContext = new AppDbContext();
        }

        public BoxDTO CreateNewBox(List<string> boxParams)
		{
			var result = new BoxDTO();

			// instead of try-catch, which is mostly meant to catch IndexOutOfRangeException
			// here could have been some more advanced validation, in case it is assumed that ASN message always has the same structure
			// maybe Identifier is missing, but SupplierIdentifier is not, in that case some warning message could have been logged
			// or if that is allowed, Identifier could be replaced with string.Empty
			try
			{
				result.SupplierIdentifier = boxParams[1].Trim();
				result.Identifier = boxParams[2].Trim();
			}
			catch (Exception ex)
			{
				throw new Exception($"Box creation failed: {ex.Message}");
			}

			return result;
		}

		public BoxContentDTO CreateNewBoxContent(List<string> boxParams)
		{
			var result = new BoxContentDTO();

			try
			{
				result.PoNumber = boxParams[1].Trim();
				result.Isbn = boxParams[2].Trim();
				result.Quantity = int.Parse(boxParams[3].Trim());
			}
			catch (Exception ex)
			{
				throw new Exception($"Box content creation failed: {ex.Message}");
			}

			return result;
		}

		public async Task SaveBoxesAndContents(List<BoxDTO> boxesToSave)
		{
			foreach (var boxToSaveDTO in boxesToSave)
			{
				var boxToSave = new Box
				{
					SupplierIdentifier = boxToSaveDTO.SupplierIdentifier,
					Identifier = boxToSaveDTO.Identifier,
				};

				//first, add the box itself
				await dbContext.Boxes.AddAsync(boxToSave);

				//save box contents
				foreach (var boxContent in boxToSaveDTO.Contents)
				{
					await dbContext.BoxContents.AddAsync(new BoxContent
					{
						PoNumber = boxContent.PoNumber,
						Isbn = boxContent.Isbn,
						Quantity = boxContent.Quantity,
						Box = boxToSave,
						BoxID = boxToSave.ID,
					});
				}

				await dbContext.SaveChangesAsync();
			}
		}
	}
}
