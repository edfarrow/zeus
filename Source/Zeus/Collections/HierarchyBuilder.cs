using System;
using System.Collections.Generic;
using Zeus.Linq;

namespace Zeus.Collections
{
	public abstract class HierarchyBuilder
	{
		public Func<IEnumerable<ContentItem>, IEnumerable<ContentItem>> Filter { get; set; }

		public abstract HierarchyNode<ContentItem> Build();

		public HierarchyNode<ContentItem> Build(Func<IEnumerable<ContentItem>, IEnumerable<ContentItem>> filter)
		{
			Filter = filter;
			return Build();
		}

		protected virtual IEnumerable<ContentItem> GetChildren(ContentItem currentItem)
		{
			IEnumerable<ContentItem> children = currentItem.Children.Accessible();
			if (Filter != null)
				children = Filter(children);
			return children;
		}
	}
}
