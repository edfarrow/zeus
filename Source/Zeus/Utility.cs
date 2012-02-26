﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using Ext.Net;
using Zeus.BaseLibrary.ExtensionMethods;
using Zeus.Web.Hosting;

namespace Zeus
{
	/// <summary>
	/// Mixed utility functions used by Zeus.
	/// </summary>
	public static class Utility
	{
		/// <summary>Converts a value to a destination type.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="destinationType">The type to convert the value to.</param>
		/// <returns>The converted value.</returns>
		public static object Convert(object value, Type destinationType)
		{
			if (value != null)
			{
				TypeConverter converter = TypeDescriptor.GetConverter(destinationType);
				if (converter != null && converter.CanConvertFrom(value.GetType()))
					return converter.ConvertFrom(value);
				converter = TypeDescriptor.GetConverter(value.GetType());
				if (converter != null && converter.CanConvertTo(destinationType))
					return converter.ConvertTo(value, destinationType);
				if (destinationType.IsEnum && value is int)
					return Enum.ToObject(destinationType, (int) value);
				if (!destinationType.IsAssignableFrom(value.GetType()))
				{
					if (!(value is IConvertible))
						throw new ZeusException("Cannot convert object of type '{0}' because it does not implement IConvertible", value.GetType());
					if (destinationType.IsNullable())
					{
						NullableConverter nullableConverter = new NullableConverter(destinationType);
						destinationType = nullableConverter.UnderlyingType;
					}
					return System.Convert.ChangeType(value, destinationType);
				}
			}
			return value;
		}

		/// <summary>Converts a value to a destination type.</summary>
		/// <param name="value">The value to convert.</param>
		/// <typeparam name="T">The type to convert the value to.</typeparam>
		/// <returns>The converted value.</returns>
		public static T Convert<T>(object value)
		{
			return (T) Convert(value, typeof(T));
		}

		public static Func<DateTime> CurrentTime = () => DateTime.Now;

		/// <summary>Gets a value from a property.</summary>
		/// <param name="instance">The object whose property to get.</param>
		/// <param name="propertyName">The name of the property to get.</param>
		/// <returns>The value of the property.</returns>
		public static object GetProperty(object instance, string propertyName)
		{
			if (instance == null) throw new ArgumentNullException("instance");
			if (propertyName == null) throw new ArgumentNullException("propertyName");

			Type instanceType = instance.GetType();
			PropertyInfo pi = instanceType.GetProperty(propertyName);

			if (pi == null)
				throw new ZeusException("No property '{0}' found on the instance of type '{1}'.", propertyName, instanceType);

			return pi.GetValue(instance, null);
		}

		public static string GetSafeName(string value)
		{
			return value.ToSafeUrl();
		}

		/// <summary>Checks that the destination isn't below the source.</summary>
		private static bool IsDestinationBelowSource(ContentItem source, ContentItem destination)
		{
			if (source == destination)
				return true;
			foreach (ContentItem ancestor in Find.EnumerateParents(destination))
				if (ancestor == source)
					return true;
			return false;
		}

		/// <summary>Moves an item in a list to a new index.</summary>
		/// <param name="siblings">A list of items where the item to move is listed.</param>
		/// <param name="itemToMove">The item that should be moved (must be in the list)</param>
		/// <param name="newIndex">The new index onto which to place the item.</param>
		/// <remarks>To persist the new ordering one should call <see cref="Utility.UpdateSortOrder"/> and save the returned items. 
		/// If the items returned from the <see cref="ContentItem.GetChildren"/> are moved with this method the 
		/// changes will not be persisted since this is a new list instance.</remarks>
		public static void MoveToIndex(IList<ContentItem> siblings, ContentItem itemToMove, int newIndex)
		{
			siblings.Remove(itemToMove);
			siblings.Insert(newIndex, itemToMove);
		}

		/// <summary>Invokes an event and and executes an action unless the event is cancelled.</summary>
		/// <param name="handler">The event handler to signal.</param>
		/// <param name="item">The item affected by this operation.</param>
		/// <param name="sender">The source of the event.</param>
		/// <param name="finalAction">The default action to execute if the event didn't signal cancel.</param>
		public static void InvokeEvent(EventHandler<CancelItemEventArgs> handler, ContentItem item, object sender, Action<ContentItem> finalAction)
		{
			if (handler != null)
			{
				CancelItemEventArgs args = new CancelItemEventArgs(item, finalAction);

				handler.Invoke(sender, args);

				if (!args.Cancel)
					args.FinalAction(args.AffectedItem);
			}
			else
				finalAction(item);
		}

		/// <summary>Invokes an event and and executes an action unless the event is cancelled.</summary>
		/// <param name="handler">The event handler to signal.</param>
		/// <param name="source">The item affected by this operation.</param>
		/// <param name="destination">The destination of this operation.</param>
		/// <param name="sender">The source of the event.</param>
		/// <param name="finalAction">The default action to execute if the event didn't signal cancel.</param>
		/// <returns>The result of the action (if any).</returns>
		public static ContentItem InvokeEvent(EventHandler<CancelDestinationEventArgs> handler, object sender, ContentItem source, ContentItem destination, Func<ContentItem, ContentItem, ContentItem> finalAction)
		{
			if (handler != null)
			{
				CancelDestinationEventArgs args = new CancelDestinationEventArgs(source, destination, finalAction);

				handler.Invoke(sender, args);

				if (args.Cancel)
					return null;

				return args.FinalAction(args.AffectedItem, args.Destination);
			}

			return finalAction(source, destination);
		}

		public static string GetUrlSafeString(string value)
		{
			value = value.ToLower().Replace(' ', '-');
			return Regex.Replace(value, @"[^a-zA-Z0-9\-]", string.Empty);
		}

		private static readonly ResourceManager _resourceManager = new ResourceManager();
		public static string GetCooliteIconUrl(Icon icon)
		{
			return _resourceManager.GetIconUrl(icon);
		}

		public static string GetClientResourceUrl(Assembly assembly, string relativePath)
		{
			return Context.Current.Resolve<IEmbeddedResourceManager>().GetClientResourceUrl(assembly, relativePath);
		}

		public static string GetClientResourceUrl(Type type, string relativePath)
		{
			return GetClientResourceUrl(type.Assembly, relativePath);
		}
	}
}
