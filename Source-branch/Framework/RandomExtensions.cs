namespace Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// <see cref="Random"/> extension methods.
    /// </summary>
    public static class RandomExtensions
    {
        #region Choose

        /// <summary>
        /// Randomly chooses to execute one of the specified functions.
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <param name="functions">The functions.</param>
        public static void Choose(this Random random, params Action[] functions)
        {
            ChooseItem(random, functions).Invoke();
        }

        /// <summary>
        /// Randomly chooses to execute one of the specified functions.
        /// </summary>
        /// <typeparam name="T">The type of the value the functions return.</typeparam>
        /// <param name="random">The random generator.</param>
        /// <param name="functions">The functions.</param>
        /// <returns>The result of the executed function.</returns>
        public static T Choose<T>(this Random random, params Func<T>[] functions)
        {
            return ChooseItem(random, functions).Invoke();
        }

        #endregion

        #region ChooseItem

        /// <summary>
        /// Gets the Next random item from the collection.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <param name="random">The random generator.</param>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>The next random element from the enumerable.</returns>
        public static T ChooseItem<T>(this Random random, IEnumerable<T> enumerable)
        {
            return ChooseItem(random, enumerable.ToList());
        }

        /// <summary>
        /// Gets the Next random item from the collection.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <param name="random">The random generator.</param>
        /// <param name="list">The list to choose from.</param>
        /// <returns>The next random element from the collection.</returns>
        public static T ChooseItem<T>(this Random random, IList<T> list)
        {
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            return list[random.Next(0, list.Count)];
        }

        /// <summary>
        /// Gets the Next random item from the collection.
        /// </summary>
        /// <typeparam name="TKey">The type of the key elements in the collection.</typeparam>
        /// <typeparam name="TValue">The type of the value elements in the collection.</typeparam>
        /// <param name="random">The random generator.</param>
        /// <param name="dictionary">The dictionary to choose from.</param>
        /// <returns>
        /// The next random element from the collection.
        /// </returns>
        public static KeyValuePair<TKey, TValue> ChooseItem<TKey, TValue>(
            this Random random, 
            IDictionary<TKey, TValue> dictionary)
        {
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }

            TKey key = dictionary.Keys.ToList()[random.Next(0, dictionary.Count)];

            return new KeyValuePair<TKey, TValue>(key, dictionary[key]);
        }

        #endregion

        #region ChooseItems

        /// <summary>
        /// Gets a random sample of all items from the collection. Allows duplicates by default.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <param name="random">The random generator.</param>
        /// <param name="source">The source collection.</param>
        /// <returns>
        /// A collection of samples from the source collection.
        /// </returns>
        public static IEnumerable<T> ChooseItems<T>(this Random random, IEnumerable<T> source)
        {
            return ChooseItems(random, source, source.ToList().Count, true);
        }

        /// <summary>
        /// Gets a random sample of items from the collection with the specified length.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <param name="random">The random generator.</param>
        /// <param name="source">The source collection.</param>
        /// <param name="allowDuplicates">if set to <c>true</c> allow duplicates from the source.</param>
        /// <returns>
        /// A collection of samples from the source collection.
        /// </returns>
        public static IEnumerable<T> ChooseItems<T>(this Random random, IEnumerable<T> source, bool allowDuplicates)
        {
            return ChooseItems(random, source, source.ToList().Count, allowDuplicates);
        }

        /// <summary>
        /// Gets a random sample of items from the collection with the specified length. Allows 
        /// duplicates by default.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <param name="random">The random generator.</param>
        /// <param name="source">The source collection.</param>
        /// <param name="count">The number of items in the sample.</param>
        /// <returns>
        /// A collection of samples from the source collection.
        /// </returns>
        public static IEnumerable<T> ChooseItems<T>(this Random random, IEnumerable<T> source, int count)
        {
            return ChooseItems(random, source, count, true);
        }

        /// <summary>
        /// Gets a random sample of items from the collection with the specified length.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <param name="random">The random generator.</param>
        /// <param name="source">The source collection.</param>
        /// <param name="count">The number of items in the sample.</param>
        /// <param name="allowDuplicates">if set to <c>true</c> allow duplicates from the source.</param>
        /// <returns>
        /// A collection of samples from the source collection.
        /// </returns>
        public static IEnumerable<T> ChooseItems<T>(
            this Random random,
            IEnumerable<T> source,
            int count,
            bool allowDuplicates)
        {
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            List<T> buffer = new List<T>(source);

            if (count > buffer.Count)
            {
                throw new ArgumentOutOfRangeException(
                    "count",
                    string.Format("Count is more than the size of the collection. Count:<{0}>", count));
            }

            if (count > 0)
            {
                for (int i = 1; i <= count; ++i)
                {
                    int randomIndex = random.Next(buffer.Count);

                    yield return buffer[randomIndex];

                    if (!allowDuplicates)
                    {
                        buffer.RemoveAt(randomIndex);
                    }
                }
            }
        }

        #endregion

        #region NextBool

        /// <summary>
        /// Generates a random <see cref="bool"/> value.
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <returns>The random <see cref="bool"/> value.</returns>
        public static bool NextBool(this Random random)
        {
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            return random.NextDouble() > 0.5;
        }

        #endregion

        #region NextByte

        /// <summary>
        /// Generates a random <see cref="byte"/> value.
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <returns>The random <see cref="byte"/> value.</returns>
        public static byte NextByte(this Random random)
        {
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            byte[] bytes = new byte[1];

            random.NextBytes(bytes);

            return bytes[0];
        }

        #endregion

        #region NextDateTime

        /// <summary>
        /// Returns the next random <see cref="DateTime"/> between the specified dates.
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <param name="from">The <see cref="DateTime"/> to start from.</param>
        /// <param name="to">The <see cref="DateTime"/> to end at.</param>
        /// <returns>A random <see cref="DateTime"/> between the specified dates.</returns>
        public static DateTime NextDateTime(this Random random, DateTime from, DateTime to)
        {
            return new DateTime(NextLong(random, from.Ticks, to.Ticks));
        }

        #endregion

        #region NextDecimal

        /// <summary>
        /// Returns a random <see cref="decimal"/> between <see cref="decimal.MinValue"/> and <see cref="decimal.MaxValue"/>.
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <returns>
        /// A random <see cref="decimal"/> between <see cref="decimal.MinValue"/> and <see cref="decimal.MaxValue"/>.
        /// </returns>
        public static decimal NextDecimal(this Random random)
        {
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            byte scale = (byte)random.Next(29);
            bool sign = random.Next(2) == 1;

            return new decimal(
                random.NextInt32(),
                random.NextInt32(),
                random.NextInt32(),
                sign,
                scale);
        }

        /// <summary>
        /// Returns a random <see cref="decimal"/> between 0 and the specified maximum value.
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <param name="maximum">The maximum.</param>
        /// <returns>
        /// A random <see cref="decimal"/> between 0 and the specified maximum value.
        /// </returns>
        public static decimal NextDecimal(this Random random, decimal maximum)
        {
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            if (maximum < 0)
            {
                throw new ArgumentOutOfRangeException("maximum");
            }

            return Math.Abs(NextDecimal(random)) % maximum;
        }

        /// <summary>
        /// Returns a random <see cref="decimal"/> between the specified minimum and maximum values.
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <returns>
        /// A random <see cref="decimal"/> between specified minimum and maximum values.
        /// </returns>
        public static decimal NextDecimal(this Random random, decimal minimum, decimal maximum)
        {
            return (Math.Abs(NextDecimal(random)) % (maximum - minimum)) + minimum;
        }

        #endregion

        #region NextDouble

        /// <summary>
        /// Returns a random <see cref="double"/> between 0 and the specified maximum value.
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <param name="maximum">The maximum.</param>
        /// <returns>
        /// A random <see cref="double"/> between 0 and the specified maximum value.
        /// </returns>
        public static double NextDouble(this Random random, double maximum)
        {
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            if (maximum < 0)
            {
                throw new ArgumentOutOfRangeException("maximum");
            }

            return random.NextDouble() * maximum;
        }

        /// <summary>
        /// Returns a random <see cref="double"/> between the specified minimum and maximum values.
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <returns>
        /// A random <see cref="double"/> between specified minimum and maximum values.
        /// </returns>
        public static double NextDouble(this Random random, double minimum, double maximum)
        {
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            return (random.NextDouble() * (maximum - minimum)) + minimum;
        }

        #endregion

        #region NextEnum

        /// <summary>
        /// Gets the next randomly generated enumeration of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="random">The random generator.</param>
        /// <returns>The random enumeration value.</returns>
        //public static T NextEnum<T>(this Random random) where T : struct
        //{
        //    Array enumValues = Enum.GetValues(typeof(T));

        //    return (T)enumValues.GetValue(random.Next(enumValues.Length));
        //}

        #endregion

        #region NextInt32

        /// <summary>
        /// Returns a random number between <see cref="int.MinValue"/> and <see cref="int.MaxValue"/>.
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <returns>A random number between <see cref="int.MinValue"/> and <see cref="int.MaxValue"/>.</returns>
        public static int NextInt32(this Random random)
        {
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            unchecked
            {
                int firstBits = random.Next(0, 1 << 4) << 28;
                int lastBits = random.Next(0, 1 << 28);
                return firstBits | lastBits;
            }
        }

        #endregion

        #region NextLong

        /// <summary>
        /// Returns a random <see cref="long"/> between <see cref="long.MinValue"/> and <see cref="long.MaxValue"/>.
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <returns>
        /// A random <see cref="long"/> between <see cref="long.MinValue"/> and <see cref="long.MaxValue"/>.
        /// </returns>
        public static long NextLong(this Random random)
        {
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            byte[] bytes = new byte[8];
            random.NextBytes(bytes);

            return BitConverter.ToInt64(bytes, 0);
        }

        /// <summary>
        /// Returns a random <see cref="long"/> between 0 and the specified maximum value.
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <param name="maximum">The maximum.</param>
        /// <returns>
        /// A random <see cref="long"/> between 0 and the specified maximum value.
        /// </returns>
        public static long NextLong(this Random random, long maximum)
        {
            return Math.Abs(random.NextLong()) % (maximum + 1);
        }

        /// <summary>
        /// Returns a random <see cref="long"/> between the specified minimum and maximum values.
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <returns>
        /// A random <see cref="long"/> between specified minimum and maximum values.
        /// </returns>
        public static long NextLong(this Random random, long minimum, long maximum)
        {
            return (Math.Abs(random.NextLong()) % (maximum - minimum + 1)) + minimum;
        }

        #endregion

        #region NextString

        /// <summary>
        /// Generates a random <see cref="string"/> containing uppercase and lowercase letters of 
        /// the alphabet and with the specified length.
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <param name="length">The length of the <see cref="string"/> to generate.</param>
        /// <returns>The random <see cref="string"/>.</returns>
        public static string NextString(this Random random, int length)
        {
            return NextString(random, length, CharacterSet.AlphabetBoth);
        }

        /// <summary>
        /// Generates a random <see cref="string"/> of the specified length.
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <param name="length">The length of the <see cref="string"/> to generate.</param>
        /// <param name="characterRange">A <see cref="string"/> containing a set of allowed
        /// characters.</param>
        /// <returns>The random <see cref="string"/>.</returns>
        public static string NextString(this Random random, int length, string characterRange)
        {
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }

            if (characterRange == null)
            {
                throw new ArgumentNullException("characterRange");
            }

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; stringBuilder.Length < length; i++)
            {
                int randomNumber = random.Next(0, characterRange.Length);

                stringBuilder.Append(characterRange[randomNumber]);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Generates a random <see cref="string"/> using the specified pattern.
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <param name="pattern">The pattern, where '#' represents a number and '?' a letter.</param>
        /// <returns>The random <see cref="string"/>.</returns>
        public static string NextString(this Random random, string pattern)
        {
            return Regex.Replace(
                pattern,
                "[#?]",
                match => ((match.ToString() == "#")
                              ? NextNumericString(random, 1)
                              : NextAlphaString(random, 1)));
        }

        #endregion

        #region NextAlphaString

        /// <summary>
        /// Generates a random <see cref="string"/> composed of letter of the alphabet of the specified length.
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <param name="length">The length of the <see cref="string"/> to generate.</param>
        /// <returns>The random numeric <see cref="string"/> composed of letter of the alphabet.</returns>
        public static string NextAlphaString(this Random random, int length)
        {
            return NextString(random, length, CharacterSet.AlphabetLowercase);
        }

        #endregion

        #region NextAlphanumericString

        /// <summary>
        /// Generates a random alpha-numeric <see cref="string"/> of the specified length.
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <param name="length">The length of the <see cref="string"/> to generate.</param>
        /// <returns>The random alpha-numeric <see cref="string"/>.</returns>
        public static string NextAlphanumericString(this Random random, int length)
        {
            return NextString(random, length, CharacterSet.AlphabetLowercase + CharacterSet.Numbers);
        }

        #endregion

        #region NextNumericString

        /// <summary>
        /// Generates a random numeric <see cref="string"/> of the specified length.
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <param name="length">The length of the <see cref="string"/> to generate.</param>
        /// <returns>The random numeric <see cref="string"/>.</returns>
        public static string NextNumericString(this Random random, int length)
        {
            return NextString(random, length, CharacterSet.Numbers);
        }

        #endregion

        #region NextTimeSpan

        /// <summary>
        /// Returns the next random <see cref="TimeSpan"/> between the specified minimum and maximum values.
        /// </summary>
        /// <param name="random">The random generator.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>
        /// A random <see cref="TimeSpan"/> between the specified minimum and maximum values.
        /// </returns>
        public static TimeSpan NextTimeSpan(this Random random, TimeSpan minValue, TimeSpan maxValue)
        {
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            return new TimeSpan(NextLong(random, minValue.Ticks, maxValue.Ticks));
        }

        #endregion

        #region Shuffle

        /// <summary>
        /// Shuffles the specified list of items.
        /// </summary>
        /// <typeparam name="T">The type of the items in the list.</typeparam>
        /// <param name="random">The random generator.</param>
        /// <param name="collection">The collection to shuffle.</param>
        /// <returns>The shuffled list.</returns>
        public static IEnumerable<T> Shuffle<T>(this Random random, IEnumerable<T> collection)
        {
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            List<T> tempList = new List<T>(collection);
            List<T> shuffledList = new List<T>();

            while (tempList.Count > 0)
            {
                int index = random.Next(0, tempList.Count);

                shuffledList.Add(tempList[index]);
                tempList.RemoveAt(index);
            }

            return shuffledList;
        }

        #endregion
    }
}
