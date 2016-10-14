namespace Framework
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// <see cref="IEnumerable{T}"/> extension methods.
    /// </summary>
    public static class EnumerableExtensions
    {
        #region Public Static Methods

        /// <summary>
        /// Iterates over all the items in the collection and executes the specified action for each item.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="action">The action to perform on each item.</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// Traverses a tree using breadth first traversal.
        /// </summary>
        /// <typeparam name="T">Type of the items to traverse.</typeparam>
        /// <param name="source">The root items for the traversal, which are always included in the result of the traversal.</param>
        /// <param name="traverser">The traversing function that is applied to the current item of the type <typeparamref name="T" />.</param>
        /// <returns>A flattened enumeration of the traversal, lazily evaluated.</returns>
        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> traverser)
        {
            return Traverse(source, TraverseKind.BreadthFirst, traverser);
        }

        /// <summary>
        /// Traverses a tree using the given traversal <paramref name="kind"/>.
        /// </summary>
        /// <typeparam name="T">Type of the items to traverse.</typeparam>
        /// <param name="source" this="true">The root items for the traversal, which are always included in the result of the traversal.</param>
        /// <param name="kind">Traversal style to use. See <see cref="TraverseKind"/>.</param>
        /// <param name="traverser">The traversing function that is applied to the current item of the type <typeparamref name="T"/>.</param>
        /// <returns>A flattened enumeration of the traversal, lazily evaluated.</returns>
        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> source, TraverseKind kind, Func<T, IEnumerable<T>> traverser)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (traverser == null)
            {
                throw new ArgumentNullException("traverser");
            }

            if (kind == TraverseKind.BreadthFirst)
            {
                return source.TraverseBreadthFirst(traverser);
            }

            return source.TraverseDepthFirst(traverser);
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Traverses a tree using breadth first search.
        /// </summary>
        /// <typeparam name="T">Type of the items to traverse.</typeparam>
        /// <param name="source">The root items for the traversal, which are always included in the result of the traversal.</param>
        /// <param name="traverser">The traversing function that is applied to the current item of the type <typeparamref name="T" />.</param>
        /// <returns>A flattened enumeration of the traversal, lazily evaluated.</returns>
        private static IEnumerable<T> TraverseBreadthFirst<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> traverser)
        {
            var queue = new Queue<T>();

            foreach (var item in source)
            {
                queue.Enqueue(item);
            }

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                yield return current;

                var children = traverser(current);
                if (children != null)
                {
                    foreach (var child in traverser(current))
                    {
                        queue.Enqueue(child);
                    }
                }
            }
        }

        /// <summary>
        /// Traverses a tree using depth first search.
        /// </summary>
        /// <typeparam name="T">Type of the items to traverse.</typeparam>
        /// <param name="source">The root items for the traversal, which are always included in the result of the traversal.</param>
        /// <param name="traverser">The traversing function that is applied to the current item of the type <typeparamref name="T" />.</param>
        /// <returns>A flattened enumeration of the traversal, lazily evaluated.</returns>
        private static IEnumerable<T> TraverseDepthFirst<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> traverser)
        {
            var stack = new Stack<T>();

            foreach (var item in source)
            {
                stack.Push(item);
            }

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current;

                var children = traverser(current);
                if (children != null)
                {
                    foreach (var child in traverser(current))
                    {
                        stack.Push(child);
                    }
                }
            }
        } 

        #endregion
    }
}
