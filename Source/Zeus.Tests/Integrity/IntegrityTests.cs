using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Zeus.BaseLibrary.Reflection;
using Zeus.ContentProperties;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Persistence;
using Zeus.Tests.Integrity.ContentTypes;
using Zeus.Web;

namespace Zeus.Tests.Integrity
{
	[TestFixture]
	public class IntegrityTests : ItemTestsBase
	{
		private IPersister persister;
		private IContentTypeManager definitions;
		private IUrlParser parser;
		private IntegrityManager integrityManger;

		private IEventRaiser moving;
		private IEventRaiser copying;
		private IEventRaiser deleting;
		private IEventRaiser saving;

		#region SetUp

		[SetUp]
		public override void SetUp()
		{
			base.SetUp();

			CreatePersister();

			parser = mocks.StrictMock<IUrlParser>();

			ITypeFinder typeFinder = CreateTypeFinder();
			ContentTypeBuilder builder = new ContentTypeBuilder(typeFinder, 
				new EditableHierarchyBuilder<IEditor>(), new AttributeExplorer<IEditor>(),
				new AttributeExplorer<IContentProperty>(), new AttributeExplorer<IEditorContainer>());
			IItemNotifier notifier = mocks.DynamicMock<IItemNotifier>();
			mocks.Replay(notifier);
			definitions = new ContentTypeManager(builder, notifier);
			integrityManger = new IntegrityManager(definitions, parser);
			IntegrityEnforcer enforcer = new IntegrityEnforcer(persister, integrityManger);
			enforcer.Start();
		}

		private ITypeFinder CreateTypeFinder()
		{
			IAssemblyFinder assemblyFinder = mocks.StrictMock<IAssemblyFinder>();
			Expect.On(assemblyFinder)
				.Call(assemblyFinder.GetAssemblies())
				.Return(new[] {typeof (AlternativePage).Assembly})
				.Repeat.Any();

			ITypeFinder typeFinder = mocks.StrictMock<ITypeFinder>();
			Expect.On(typeFinder)
				.Call(typeFinder.Find(typeof (ContentItem)))
				.Return(new[]
				        	{
				        		typeof (AlternativePage),
				        		typeof (AlternativeStartPage),
				        		typeof (Page),
				        		typeof (Root),
				        		typeof (StartPage),
				        		typeof (SubPage)
				        	});
			mocks.Replay(typeFinder);
			return typeFinder;
		}

		private void CreatePersister()
		{
			mocks.Record();
			persister = mocks.DynamicMock<IPersister>();

			persister.ItemMoving += null;
			moving = LastCall.IgnoreArguments().Repeat.Any().GetEventRaiser();

			persister.ItemCopying += null;
			copying = LastCall.IgnoreArguments().Repeat.Any().GetEventRaiser();

			persister.ItemDeleting += null;
			deleting = LastCall.IgnoreArguments().Repeat.Any().GetEventRaiser();

			persister.ItemSaving += null;
			saving = LastCall.IgnoreArguments().Repeat.Any().GetEventRaiser();

			mocks.Replay(persister);
		}

		#endregion

		#region Move

		[Test]
		public void CanMoveItem()
		{
			StartPage startPage = new StartPage();
			Page page = new Page();
			bool canMove = integrityManger.CanMove(page, startPage);
			Assert.IsTrue(canMove, "The page couldn't be moved to the destination.");
		}

		[Test]
		public void CanMoveItemEvent()
		{
			StartPage startPage = new StartPage();
			Page page = new Page();

			moving.Raise(persister, new CancelDestinationEventArgs(page, startPage));
		}

		[Test]
		public void CannotMoveItemOntoItself()
		{
			Page page = new Page();
			bool canMove = integrityManger.CanMove(page, page);
			Assert.IsFalse(canMove, "The page could be moved onto itself.");
		}

		[Test, ExpectedException(typeof(DestinationOnOrBelowItselfException))]
		public void CannotMoveItemOntoItselfEvent()
		{
			Page page = new Page();
			moving.Raise(persister, new CancelDestinationEventArgs(page, page));
		}

		[Test]
		public void CannotMoveItemBelowItself()
		{
			var page1 = ContentItem.Create(new Page());
			var page2 = CreateOneItem<Page>("Rutger", page1);

			bool canMove = integrityManger.CanMove(page1, page2);
			Assert.IsFalse(canMove, "The page could be moved below itself.");
		}

		[Test, ExpectedException(typeof(DestinationOnOrBelowItselfException))]
		public void CannotMoveItemBelowItselfEvent()
		{
			var page = ContentItem.Create(new Page());
			Page page2 = CreateOneItem<Page>("Rutger", page);

			moving.Raise(persister, new CancelDestinationEventArgs(page, page2));
		}

		[Test]
		public void CannotMoveIfNameIsOccupied()
		{
			StartPage startPage = CreateOneItem<StartPage>("start", null);
			Page page2 = CreateOneItem<Page>("Sasha", startPage);
			Page page3 = CreateOneItem<Page>("Sasha", null);

			bool canMove = integrityManger.CanMove(page3, startPage);
			Assert.IsFalse(canMove, "The page could be moved even though the name was occupied.");
		}

		[Test, ExpectedException(typeof(NameOccupiedException))]
		public void CannotMoveIfNameIsOccupiedEvent()
		{
			StartPage startPage = CreateOneItem<StartPage>("start", null);
			Page page2 = CreateOneItem<Page>("Sasha", startPage);
			Page page3 = CreateOneItem<Page>("Sasha", null);

			moving.Raise(persister, new CancelDestinationEventArgs(page3, startPage));
		}

		[Test]
		public void CannotMoveIfTypeIsntAllowed()
		{
			StartPage startPage = new StartPage();
			Page page = new Page();

			bool canMove = integrityManger.CanMove(startPage, page);
			Assert.IsFalse(canMove, "The start page could be moved even though a page isn't an allowed destination.");
		}

		[Test, ExpectedException(typeof(NotAllowedParentException))]
		public void CannotMoveIfTypeIsntAllowedEvent()
		{
			StartPage startPage = new StartPage();
			Page page = new Page();

			moving.Raise(persister, new CancelDestinationEventArgs(startPage, page));
		}

		#endregion

		#region Copy

		[Test]
		public void CanCopyItem()
		{
			StartPage startPage = new StartPage();
			Page page = new Page();
			bool canCopy = integrityManger.CanCopy(page, startPage);
			Assert.IsTrue(canCopy, "The page couldn't be copied to the destination.");
		}

		[Test]
		public void CanCopyItemEvent()
		{
			StartPage startPage = new StartPage();
			Page page = new Page();

			copying.Raise(persister, new CancelDestinationEventArgs(page, startPage));
		}

		[Test]
		public void CannotCopyIfNameIsOccupied()
		{
			StartPage startPage = CreateOneItem<StartPage>("start", null);
			Page page2 = CreateOneItem<Page>("Sasha", startPage);
			Page page3 = CreateOneItem<Page>("Sasha", null);

			bool canCopy = integrityManger.CanCopy(page3, startPage);
			Assert.IsFalse(canCopy, "The page could be copied even though the name was occupied.");
		}

		[Test, ExpectedException(typeof(NameOccupiedException))]
		public void CannotCopyIfNameIsOccupiedEvent()
		{
			StartPage startPage = CreateOneItem<StartPage>("start", null);
			Page page2 = CreateOneItem<Page>("Sasha", startPage);
			Page page3 = CreateOneItem<Page>("Sasha", null);

			copying.Raise(persister, new CancelDestinationEventArgs(page3, startPage));
		}

		[Test]
		public void CannotCopyIfTypeIsntAllowed()
		{
			StartPage startPage = new StartPage();
			Page page = new Page();

			bool canCopy = integrityManger.CanCopy(startPage, page);
			Assert.IsFalse(canCopy, "The start page could be copied even though a page isn't an allowed destination.");
		}

		[Test, ExpectedException(typeof(NotAllowedParentException))]
		public void CannotCopyIfTypeIsntAllowedEvent()
		{
			StartPage startPage = new StartPage();
			Page page = new Page();

			copying.Raise(persister, new CancelDestinationEventArgs(startPage, page));
		}

		#endregion

		#region Delete

		[Test]
		public void CanDelete()
		{
			Page page = new Page();

			mocks.Record();
			Expect.On(parser).Call(parser.IsRootOrStartPage(page)).Return(false);
			mocks.Replay(parser);

			bool canDelete = integrityManger.CanDelete(page);
			Assert.IsTrue(canDelete, "Page couldn't be deleted");

			mocks.Verify(parser);
		}

		[Test]
		public void CanDeleteEvent()
		{
			Page page = new Page();

			mocks.Record();
			Expect.On(parser).Call(parser.IsRootOrStartPage(page)).Return(false);
			mocks.Replay(parser);

			deleting.Raise(persister, new CancelItemEventArgs(page));

			mocks.Verify(parser);
		}

		[Test]
		public void CannotDeleteStartPage()
		{
			StartPage startPage = new StartPage();

			mocks.Record();
			Expect.On(parser).Call(parser.IsRootOrStartPage(startPage)).Return(true);
			mocks.Replay(parser);

			bool canDelete = integrityManger.CanDelete(startPage);
			Assert.IsFalse(canDelete, "Start page could be deleted");

			mocks.Verify(parser);
		}

		[Test, ExpectedException(typeof(CannotDeleteRootException))]
		public void CannotDeleteStartPageEvent()
		{
			StartPage startPage = new StartPage();

			mocks.Record();
			Expect.On(parser).Call(parser.IsRootOrStartPage(startPage)).Return(true);
			mocks.Replay(parser);

			deleting.Raise(persister, new CancelItemEventArgs(startPage));
			mocks.Verify(parser);
		}

		#endregion

		#region Save

		[Test]
		public void CanSave()
		{
			StartPage startPage = new StartPage();

			bool canSave = integrityManger.CanSave(startPage);
			Assert.IsTrue(canSave, "Couldn't save");
		}

		[Test]
		public void CanSaveEvent()
		{
			StartPage startPage = new StartPage();

			saving.Raise(persister, new CancelItemEventArgs(startPage));
		}

		[Test]
		public void CannotSaveNotLocallyUniqueItem()
		{
			StartPage startPage = CreateOneItem<StartPage>("start", null);

			Page page2 = CreateOneItem<Page>("Sasha", startPage);
			Page page3 = CreateOneItem<Page>("Sasha", startPage);

			bool canSave = integrityManger.CanSave(page3);
			Assert.IsFalse(canSave, "Could save even though the item isn't the only sibling with the same name.");
		}

		[Test]
		public void LocallyUniqueItemThatWithoutNameYet()
		{
			//StartPage startPage = CreateOneItem<StartPage>("start", null);

			//Page page2 = CreateOneItem<Page>(null, startPage);
			//Page page3 = CreateOneItem<Page>("Sasha", startPage);
			var startPage = ContentItem.Create(new StartPage { Name = "start" });

			var page2 = ContentItem.Create(new Page { Name = null, Parent = startPage });
			var page3 = ContentItem.Create(new Page { Name = "Sasha", Parent = startPage });

			bool isUnique = integrityManger.IsLocallyUnique("Sasha", page2);
			Assert.IsFalse(isUnique, "Shouldn't have been locally unique.");
		}

		[Test, ExpectedException(typeof(NameOccupiedException))]
		public void CannotSaveNotLocallyUniqueItemEvent()
		{
			StartPage startPage = CreateOneItem<StartPage>("start", null);

			Page page2 = CreateOneItem<Page>("Sasha", startPage);
			Page page3 = CreateOneItem<Page>("Sasha", startPage);

			saving.Raise(persister, new CancelItemEventArgs(page3));
		}

		[Test]
		public void CannotSaveUnallowedItem()
		{
			Page page = CreateOneItem<Page>("John", null);
			StartPage startPage = CreateOneItem<StartPage>("Leonidas", page);

			bool canSave = integrityManger.CanSave(startPage);
			Assert.IsFalse(canSave, "Could save even though the start page isn't below a page.");
		}

		[Test, ExpectedException(typeof(NotAllowedParentException))]
		public void CannotSaveUnallowedItemEvent()
		{
			Page page = CreateOneItem<Page>("John", null);
			StartPage startPage = CreateOneItem<StartPage>("Leonidas", page);

			saving.Raise(persister, new CancelItemEventArgs(startPage));
		}

		#endregion

		#region Security

		[Test]
		public void UserCanEditAccessibleDetail()
		{
			ContentType definition = definitions.GetContentType(typeof(Page));
			Assert.AreEqual(1,
				definition.GetEditors(SecurityUtilities.CreatePrincipal("UserNotInTheGroup", "ACertainGroup")).
				Count);
		}

		[Test]
		public void UserCannotEditInaccessibleDetail()
		{
			ContentType definition = definitions.GetContentType(typeof(Page));
			Assert.AreEqual(0,
				definition.GetEditors(SecurityUtilities.CreatePrincipal("UserNotInTheGroup", "Administrator")).
				Count);
		}

		#endregion
	}
}