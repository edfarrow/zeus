using System;
using System.Collections.Generic;
using System.Security.Principal;
using MongoDB.Bson;
using NUnit.Framework;
using Ormongo;
using Rhino.Mocks;
using Zeus.Tests.Fakes;
using Zeus.Web;

namespace Zeus.Tests
{
	[TestFixture]
	public abstract class ItemTestsBase
	{
		protected MockRepository mocks;

		[SetUp]
		public virtual void SetUp()
		{
			RequestItem.Accessor = new StaticContextAccessor();
			mocks = new MockRepository();
		}

		[TearDown]
		public virtual void TearDown()
		{
			OrmongoConfiguration.DropAllCollections();
			if (mocks != null)
			{
				mocks.ReplayAll();
				mocks.VerifyAll();
			}
		}

		protected virtual T CreateOneItem<T>(string name, ContentItem parent) 
			where T : ContentItem
		{
			T item = (T)Activator.CreateInstance(typeof(T), true);
			item.Name = name;
			item.Title = name;
			item.Parent = parent;
			item.Save();
			return item;
		}

		protected IPrincipal CreatePrincipal(string name, params string[] roles)
		{
			return SecurityUtilities.CreatePrincipal(name, roles);
		}

		private Dictionary<string, object> requestItems;
		protected IWebContext CreateWebContext(bool replay)
		{
			requestItems = new Dictionary<string, object>();
			IWebContext context = mocks.StrictMock<IWebContext>();
			Expect.On(context).Call(context.RequestItems).Return(requestItems).Repeat.Any();

			if (replay)
				mocks.Replay(context);
			return context;
		}
	}
}