﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Labyrinth
{
    /// <summary>
    /// Contains various utility methods used throughout the project
    /// </summary>
    public class Utils
    {
        public static Random Random { get; set; } = new Random();

        /// <summary>
        /// Creates a 2D array filled with a given value
        /// </summary>
        /// <param name="size">The size (for both dimensions) of the array</param>
        /// <param name="value">The value with which to fill the matrix</param>
        /// <returns>The created matrix</returns>
        public static T[][] InitializeMatrix<T>(int size, T value)
        {
            return Enumerable.Range(0, size).Select(x => Enumerable.Repeat(value, size).ToArray()).ToArray();
        }

        /// <summary>
        /// Retrieves a random item from a container
        /// </summary>
        /// <param name="list">The container</param>
        /// <returns>An item from the container</returns>
        public static T GetRandomFromList<T>(IEnumerable<T> list)
        {
            return list.ElementAt(Random.Next(list.Count()));
        }

        /// <summary>
        /// Retrieves a random item from a 2D array
        /// </summary>
        /// <param name="matrix">The matrix</param>
        /// <returns>An item from the matrix</returns>
        public static T GetRandomFromMatrix<T>(T[][] matrix)
        {
            return GetRandomFromList(GetRandomFromList(matrix));
        }

        /// <summary>
        /// Returns a random number between 0 and 1
        /// </summary>
        /// <returns>A <see cref="float"/> between 0 and 1</returns>
        public static float GetRandomPercent()
        {
            int range = 10000;
            return Random.Next(range) / (float)range;
        }

        /// <summary>
        /// Makes a check for a binary event based on a given probability
        /// </summary>
        /// <param name="odds">A <see cref="float"/> between 0 and 1 representing the percent chance of a "success"</param>
        /// <returns>True if the random number generated is within the given odds; false otherwise</returns>
        public static bool Roll(float odds)
        {
            return GetRandomPercent() < odds;
        }

        /// <summary>
        /// Returns an article word ("a" or "an") as appropriate for the given word
        /// </summary>
        /// <param name="word">The word which the article will proceed</param>
        /// <param name="capitalize">Whether the article should be capitalized</param>
        /// <returns>"an" if the word begins with a vowel; "a" otherwise</returns>
        public static string AnOrA(string word, bool capitalize)
        {
            string article = capitalize ? "A" : "a";

            bool isVowel = "aeiouAEIOU".Contains(word[0]);

            if (isVowel)
                article += 'n';

            return article;
        }

        /// <summary>
        /// Returns a masculine or feminine pronoun.
        /// </summary>
        /// <param name="male">
        /// True if the pronoun corresponds to a male. 
        /// False if the pronoun corresponds to a female.
        /// null if the pronoun should be random.</param>
        /// <param name="capitalize">Whether the article should be capitalized</param>
        /// <returns>He, She, he, or she</returns>
        public static string HeOrShe(bool? male, bool capitalize)
        {
            male ??= Roll(0.5f); // Cool operator... returns the left-hand operand if it's not null, else returns the right-hand operand
            string pronoun = male.Value ? "he" : "she";

            return capitalize ? Capitalize(pronoun) : pronoun;
        }

        /// <summary>
        /// Capitalizes the first letter of a string
        /// </summary>
        /// <returns>The capitalized string, or the original string if it couldn't be capitalized</returns>
        public static string Capitalize(string word)
        {
            string capitalizedWord = word;

            if (word != null && word.Length > 0)
            {
                capitalizedWord = word.First().ToString().ToUpper() + word[1..];
            }

            return capitalizedWord;
        }

        /// <summary>
        /// Creates an action dictionary given an arbitrary list of items.
        /// </summary>
        /// <typeparam name="T">The type of the items in the list. Cannot have more than 9 items.</typeparam>
        /// <param name="options">The list of items.</param>
        /// <param name="nameFunction">The function to use to determine the name of each item.</param>
        /// <returns>An action dictionary.s</returns>
        public static Dictionary<char, string> CreateActions<T>(IList<T> options, Func<T, string> nameFunction = null)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            else if (options.Count > 9)
                throw new ArgumentOutOfRangeException(nameof(options), $"{nameof(options)} cannot contain more than 9 items. Length: {options.Count}");

            // If no function is given for determining the name of each option, use ToString by default
            nameFunction ??= t => t.ToString();

            Dictionary<char, string> actions = new();

            for (int i = 0; i < options.Count; i++)
                actions.Add((i + 1).ToString().First(), nameFunction(options[i]));

            return actions;
        }

        public static IEnumerable<T> GetEnumValues<T>()
            where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
