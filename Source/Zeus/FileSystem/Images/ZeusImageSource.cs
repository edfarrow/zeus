using System.Collections.Generic;
using MongoDB.Bson;
using Ormongo;
using SoundInTheory.DynamicImage.Caching;
using SoundInTheory.DynamicImage.Sources;
using SoundInTheory.DynamicImage.Util;

namespace Zeus.FileSystem.Images
{
	public class OrmongoImageSource : ImageSource
	{
		public ObjectId AttachmentID
		{
			get { return (ObjectId) (this["AttachmentID"] ?? ObjectId.Empty); }
			set { this["AttachmentID"] = value; }
		}

		public OrmongoImageSource(Attachment attachment)
		{
			AttachmentID = attachment.ID;
		}

		public OrmongoImageSource(ObjectId attachmentID)
		{
			AttachmentID = attachmentID;
		}

		public override FastBitmap GetBitmap()
		{
			Attachment attachment = Attachment.Find(AttachmentID);
			var bytes = new byte[attachment.Content.Length];
			attachment.Content.Read(bytes, 0, bytes.Length);

			return new FastBitmap(bytes);
		}

		public override void PopulateDependencies(List<Dependency> dependencies)
		{
			dependencies.Add(new Dependency
			{
				Text1 = AttachmentID.ToString(),
				Text2 = string.Empty,
				Text3 = string.Empty,
				Text4 = string.Empty,
			});
		}
	}
}