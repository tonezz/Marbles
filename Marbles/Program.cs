using System;
using System.Collections.Generic;
using System.Linq;

namespace Marbles
{
    public class Program
    {
        /*
        The Problem
        
        John has a son named Bob. Bob has a marble collection and has names for all the marbles.
        Each marble has a weight of ≤1 ounce and is one of the following colors: 
        Red, Orange, Yellow, Green, Blue, Indigo, Violet (ROYGBIV).
        
        John would like to line up the marbles in ROYGBIV order for Bob.

        John wants to keep all of the marbles that weigh ≥0.5 ounces and have names that are
        palindromes, discarding the rest.
        
        The Assignment
        
        Write a function that takes in a collection of Marbles and returns the filtered & ordered list 
        of marbles back. The palindrome should ignore capitalization and punctuation, 
        so "Bob o’Bob” is considered a palindrome.
        
        In your written answer please include a comment describing the time and space complexity of
        your algorithm.
        
        Additionally, please describe your deployment strategy to host this workload in (any) cloud
        platform. We would like this to be accessible, repeatable, and modular (example: as a
        serverless function or REST service... etc). Discuss any deployment technologies and
        automation strategies you would wrap around the function.
         */

        static void Main(string[] args)
        {
            var marbles = new List<Tuple<int, string, string, double>>
            {
                new(1, "blue", "Bob", 0.5),
                new(2, "red", "John Smith", 0.25),
                new(3, "violet", "Bob O'Bob", 0.5),
                new(4, "indigo", "Bob Dad-Bob", 0.75),
                new(5, "yellow", "John", 0.5),
                new(6, "orange", "Bob", 0.25),
                new(7, "blue", "Smith", 0.5),
                new(8, "blue", "Bob", 0.25),
                new(9, "green", "Bobb Ob", 0.75),
                new(10, "blue", "Bob", 0.5)
            };
            var processedMarbles = ProcessMarbles(marbles);
            foreach (var marble in processedMarbles)
            {
                Console.WriteLine($"id: {marble.Item1}, color: \"{marble.Item2}\", name: \"{marble.Item3}\", weight: {marble.Item4}");
            }

            /*
            Output:
            id: 9, color: "green", name: "Bobb Ob", weight: 0.75
            id: 1, color: "blue", name: "Bob", weight: 0.5
            id: 10, color: "blue", name: "Bob", weight: 0.5
            id: 4, color: "indigo", name: "Bob Dad-Bob", weight: 0.75
            id: 3, color: "violet", name: "Bob O'Bob", weight: 0.5
             */
        }

        /// <summary>
        /// First, filter the marbles by weight, because double comparison is cheap.
        /// Then, filter by palindrome.
        /// Finally, sort the fitlered data using LINQ OrderBy() that implements QuickSort.
        ///
        /// The FilterByWeight, FilterByPalindrome, and SortByCustomOrder methods may all
        /// be made into serverless functions that process data in parallel.
        ///
        /// FilterByWeight and FilterByPalindrome may be invoked as the data is inputted into the system,
        /// or transferred to the application that requests filtered data.
        ///
        /// SortByCustomOrder should only run on pre-filtered data.
        /// </summary>
        private static List<Tuple<int, string, string, double>> ProcessMarbles(
            List<Tuple<int, string, string, double>> marbles)
        {
            // Discard marbles with weight less than 0.5 oz.
            marbles = FilterByWeight(marbles);

            // Discard marbles that aren't a palindrome.
            marbles = FilterByPalindrome(marbles);

            // Sort based on color.
            var colorComparer = new ColorComparer(StringComparer.CurrentCulture);
            marbles = SortByCustomOrder(marbles, colorComparer);

            return marbles;
        }

        /// <summary>LINQ Where() is O(n) time complexity and O(1) space complexity.</summary>
        private static List<Tuple<int, string, string, double>> FilterByWeight(
            IEnumerable<Tuple<int, string, string, double>> marbles)
        {
            return marbles.Where(marble => marble.Item4 >= 0.5).ToList();
        }

        /// <summary>LINQ Where() is O(n) time complexity and O(1) space complexity.</summary>
        private static List<Tuple<int, string, string, double>> FilterByPalindrome(
            IEnumerable<Tuple<int, string, string, double>> marbles)
        {
            return marbles.Where(marble => IsPalindrome(marble.Item3)).ToList();
        }

        /// <summary>Ignores case, punctuation, and whitespaces.</summary>
        public static bool IsPalindrome(string name)
        {
            var charArray = name.Where(c => !char.IsPunctuation(c) &&
                                                              !char.IsWhiteSpace(c)).ToArray();
            var reversedName = new string(charArray.Reverse().ToArray());

            var processedName = new string(charArray);
            return processedName.Equals(reversedName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>LINQ OrderBy() performs QuickSort in O(nlogn) time and O(logn) space complexity.</summary>
        private static List<Tuple<int, string, string, double>> SortByCustomOrder(
            IEnumerable<Tuple<int, string, string, double>> marbles, IComparer<string> comparer)
        {
            return marbles.OrderBy(marble => marble.Item2, comparer).ToList();
        }
    }

    public class ColorComparer : IComparer<string>
    {
        private static IComparer<string> _baseComparer;

        public ColorComparer(IComparer<string> comparer)
        {
            _baseComparer = comparer;
        }

        /// <summary>ROYGBIV</summary>
        public int Compare(string? x, string? y)
        {
            if (_baseComparer.Compare(x, y) == 0) return 0;

            if (_baseComparer.Compare(x, "red") == 0) return -1;
            if (_baseComparer.Compare(y, "red") == 0) return 1;

            if (_baseComparer.Compare(x, "orange") == 0) return -1;
            if (_baseComparer.Compare(y, "orange") == 0) return 1;

            if (_baseComparer.Compare(x, "yellow") == 0) return -1;
            if (_baseComparer.Compare(y, "yellow") == 0) return 1;

            if (_baseComparer.Compare(x, "green") == 0) return -1;
            if (_baseComparer.Compare(y, "green") == 0) return 1;

            if (_baseComparer.Compare(x, "blue") == 0) return -1;
            if (_baseComparer.Compare(y, "blue") == 0) return 1;

            if (_baseComparer.Compare(x, "indigo") == 0) return -1;
            if (_baseComparer.Compare(y, "indigo") == 0) return 1;

            if (_baseComparer.Compare(x, "violet") == 0) return -1;
            if (_baseComparer.Compare(y, "violet") == 0) return 1;

            return _baseComparer.Compare(x, y);
        }
    }
}