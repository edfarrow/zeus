using NUnit.Framework;
using Zeus.BaseLibrary.ExtensionMethods;

namespace Zeus.BaseLibrary.Tests.ExtensionMethods
{
	[TestFixture]
	public class StringTests
	{
		[Test]
		public void Test_Left_Length_Is_LessThanStringLength()
		{
			const string myString = "This is a test string.";
			string leftPart = myString.Left(6);
			Assert.AreEqual("This i", leftPart);
		}

		[Test]
		public void Test_Left_Length_Is_GreaterThanStringLength()
		{
			const string myString = "This is a test string.";
			string leftPart = myString.Left(300);
			Assert.AreEqual("This is a test string.", leftPart);
		}

		[Test]
		public void Test_Left_EmptyString()
		{
			const string myString = "";
			string leftPart = myString.Left(6);
			Assert.AreEqual("", leftPart);
		}

		[Test]
		public void CanUrlEncodeString()
		{
			Assert.AreEqual("my-safe_url-is-great", " My  SAFE_Url is-great".ToSafeUrl());
		}

		[Test]
		public void CanUrlEncodeAccentedString()
		{
			Assert.AreEqual("%c3%a1cc%c3%a8nt", "áccènt".ToSafeUrl());
		}
	}
}