﻿//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Media;

namespace Metro
{
	/// <summary>
	/// Helper class used to traverse the Visual Tree.
	/// </summary>
	public static class TreeHelper
	{
		/// <summary>
		/// Traverses the Visual Tree upwards looking for the ancestor that satisfies the <paramref name="predicate"/>.
		/// </summary>
		/// <param name="dependencyObject">The element for which the ancestor is being looked for.</param>
		/// <param name="predicate">The predicate that evaluates if an element is the ancestor that is being looked for.</param>
		/// <returns>
		/// The ancestor element that matches the <paramref name="predicate"/> or <see langword="null"/>
		/// if the ancestor was not found.
		/// </returns>
		public static DependencyObject FindAncestor(DependencyObject dependencyObject, Func<DependencyObject, bool> predicate)
		{
			if (predicate(dependencyObject))
			{
				return dependencyObject;
			}

			DependencyObject parent = null;
#if SILVERLIGHT
			FrameworkElement frameworkElement = dependencyObject as FrameworkElement;
			if (frameworkElement != null)
			{
				parent = frameworkElement.Parent ?? System.Windows.Media.VisualTreeHelper.GetParent(frameworkElement);
			}
#else
            parent = LogicalTreeHelper.GetParent(dependencyObject);
#endif
			if (parent != null)
			{
				return FindAncestor(parent, predicate);
			}

			return null;
		}

		public static IEnumerable<DependencyObject> FindDescendants(DependencyObject dependencyObject, Func<DependencyObject, bool> predicate)
		{
			if (predicate(dependencyObject))
			{
				yield return dependencyObject;
			}

			var count = VisualTreeHelper.GetChildrenCount(dependencyObject);
			for (int i = 0; i < count; i++)
			{
				foreach (var item in FindDescendants(VisualTreeHelper.GetChild(dependencyObject, i), predicate))
				{
					yield return item;
				}
			}
		}

		/// <summary>
		/// Finds the parent.
		/// </summary>
		/// <param name="element">The element to start with.</param>
		/// <param name="filter">The filter criteria.</param>
		/// <returns></returns>
		public static DependencyObject FindParent(this DependencyObject element, Func<DependencyObject, bool> filter)
		{
			DependencyObject parent = VisualTreeHelper.GetParent(element);

			if (parent != null)
			{
				if (filter(parent))
				{
					return parent;
				}

				return FindParent(parent, filter);
			}

			return null;
		}

		/// <summary>
		/// Finds the parent of the first occurance of the specified type.
		/// </summary>
		/// <typeparam name="T">The type of dependency object to search for.</typeparam>
		/// <param name="childElement">The child element.</param>
		/// <returns>The parent of the first occurance of the specified type.</returns>
		public static T FindParent<T>(this DependencyObject childElement) where T : DependencyObject
		{
			return FindParent<T>(childElement, 0);
		}

		/// <summary>
		/// Finds the parent of the specified type at the specified depth.
		/// </summary>
		/// <typeparam name="T">The type of dependency object to search for.</typeparam>
		/// <param name="childElement">The child element.</param>
		/// <param name="depth">The depth.</param>
		/// <returns>The parent of the specified type at the specified depth.</returns>
		public static T FindParent<T>(this DependencyObject childElement, int depth) where T : DependencyObject
		{
			DependencyObject parent = VisualTreeHelper.GetParent(childElement);
			T parentAsT = parent as T;
			if (parent == null)
			{
				return null;
			}
			else if (parentAsT != null)
			{
				if (depth == 0)
				{
					return parentAsT;
				}

				depth -= depth;
			}

			return FindParent<T>(parent, depth);
		}
	}
}