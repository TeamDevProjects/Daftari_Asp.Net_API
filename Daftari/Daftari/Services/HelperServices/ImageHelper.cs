
namespace Daftari.Services.Images
{
	public static class ImageHelper
	{
		public class clsImageData
		{
			public byte[] ImageData {get;set;} = null!;
			public string ImageType { get; set; } = null!; 
		}

		public static async Task<clsImageData> HandelImageServices(IFormFile FormImage)
		{
			var imageDataObj = new clsImageData();
			if (FormImage != null)
			{
				try
				{
					long fileSizeLimit = 10 * 1024 * 1024; // 10 MB size limit
					if (FormImage.Length > fileSizeLimit)
					{
						throw new Exception("File size exceeds the allowed limit.");
					}

					using (var memoryStream = new MemoryStream())
					{
						await FormImage.CopyToAsync(memoryStream);
						imageDataObj.ImageData = memoryStream.ToArray();  // Convert to byte array
						imageDataObj.ImageType = FormImage.ContentType;  // Set MediaType from the file
					}
				}
				catch (Exception ex)
				{

					// Log the exception (e.g., using a logging framework)
					throw new Exception($"File upload failed: {ex.Message}");


				}
			}

			// Check if MediaData is still null and set MediaType to "None"
			if (imageDataObj.ImageData == null) imageDataObj.ImageType = null!;

			// Validate MediaType if necessary
			var validMediaTypes = new[] { "image/jpeg", "image/png", null };
			if (!validMediaTypes.Contains(imageDataObj.ImageType))
			{
				throw new Exception("Invalid media type. Only specific file types are allowed.");
			}

			return imageDataObj;
		}
	}
}
