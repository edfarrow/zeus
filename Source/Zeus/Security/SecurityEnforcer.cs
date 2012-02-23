using System;
using System.Security.Principal;
using Ninject;
using Zeus.Persistence;

namespace Zeus.Security
{
	/// <summary>
	/// Checks against unauthorized requests, and updates of content items.
	/// </summary>
	public class SecurityEnforcer : ContentItemObserver, ISecurityEnforcer, IStartable
	{
		#region Fields

		private readonly ISecurityManager _security;
		private readonly Web.IWebContext _webContext;

		#endregion

		#region Constructor

		public SecurityEnforcer(ISecurityManager security, Web.IWebContext webContext)
		{
			_webContext = webContext;
			_security = security;
		}

		#endregion

		#region Events

		/// <summary>
		/// Is invoked when a security violation is encountered. The security 
		/// exception can be cancelled by setting the cancel property on the event 
		/// arguments.
		/// </summary>
		public event EventHandler<CancelItemEventArgs> AuthorizationFailed;

		#endregion

		#region Methods

		/// <summary>Checks that the current user is authorized to access the current item.</summary>
		public virtual void AuthoriseRequest()
		{
			ContentItem item = _webContext.CurrentPage;

			if (item == null)
				return;

			if (_security.IsAuthorized(item, _webContext.User, Operations.Read))
				return;

			CancelItemEventArgs args = new CancelItemEventArgs(item);
			if (AuthorizationFailed != null)
				AuthorizationFailed.Invoke(this, args);

			if (!args.Cancel)
				//throw new PermissionDeniedException(item, webContext.User);
				throw new UnauthorizedAccessException();
		}

		public override bool BeforeSave(ContentItem document)
		{
			if (!_security.IsAuthorized(document, _webContext.User, Operations.Change))
				throw new PermissionDeniedException(document, _webContext.User, Operations.Change);
			IPrincipal user = _webContext.User;
			if (user != null)
				document.SavedBy = user.Identity.Name;
			else
				document.SavedBy = null;
			return base.BeforeSave(document);
		}

		public override bool BeforeMove(ContentItem document, ContentItem newParent)
		{
			if (!_security.IsAuthorized(document, _webContext.User, Operations.Read) 
				|| !_security.IsAuthorized(newParent, _webContext.User, Operations.Create))
				throw new PermissionDeniedException(document, _webContext.User, Operations.Create);
			return base.BeforeMove(document, newParent);
		}

		public override bool BeforeDestroy(ContentItem document)
		{
			IPrincipal user = _webContext.User;
			if (!_security.IsAuthorized(document, user, Operations.Delete))
				throw new PermissionDeniedException(document, user, Operations.Delete);
			return base.BeforeDestroy(document);
		}

		public override bool BeforeCopy(ContentItem document, ContentItem newParent)
		{
			if (!_security.IsAuthorized(document, _webContext.User, Operations.Read)
				|| !_security.IsAuthorized(document, _webContext.User, Operations.Create))
				throw new PermissionDeniedException(document, _webContext.User, Operations.Create);
			return base.BeforeCopy(document, newParent);
		}

		#endregion

		#region IStartable Members

		public virtual void Start()
		{
			ContentItem.Observers.Add(this);
		}

		public virtual void Stop()
		{
			ContentItem.Observers.Remove(this);
		}

		#endregion
	}
}
