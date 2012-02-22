using NUnit.Framework;

namespace Zeus.Tests
{
	[TestFixture]
	public class ContentItemTests
	{
		private class MyWebsite : ContentItem
		{

		}

		private class MyPage : ContentItem
		{
			public virtual string MyProperty
			{
				get { return (string)(GetDetail("MyProperty") ?? null); }
				set { SetDetail("MyProperty", value); }
			}
		}

		#region Sanity checks on AncestryDocument<T> properties

		[Test]
		public void CanGetAndSetParentOnSameClass()
		{
			// Arrange.
			var root = ContentItem.Create(new MyPage());
			var child = ContentItem.Create(new MyPage { Parent = root });

			// Act.
			var result = child.Parent;

			// Assert.
			Assert.That(result, Is.EqualTo(root));
		}

		[Test]
		public void CanGetAndSetParentOnDifferentClasses()
		{
			// Arrange.
			var root = ContentItem.Create(new MyWebsite());
			var child1 = ContentItem.Create(new MyPage { Parent = root });
			var child2 = ContentItem.Create(new MyPage { Parent = root });

			// Act / Assert.
			Assert.That(root.Children, Contains.Item(child1).And.Contains(child2));
			Assert.That(child1.Parent, Is.EqualTo(root));
			Assert.That(child2.Parent, Is.EqualTo(root));
		}

		#endregion
	}
}