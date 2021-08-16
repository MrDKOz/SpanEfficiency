using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.CodeAnalysis.Operations;

namespace SpanEfficiency
{
    internal class Program
    {
        private static void Main()
        {
            MakeAChoice();
        }

        /// <summary>
        /// Requests and handles the Users choice of action.
        /// </summary>
        private static void MakeAChoice()
        {
            while (true)
            {
                Console.WriteLine($"What do you want to do?{Environment.NewLine} " +
                                  $"1) Run all functions{Environment.NewLine} " +
                                  $"2) Run Benchmark{Environment.NewLine} " +
                                  "3) Exit");
                Console.Write("Your choice: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        var colosseum = new Coliseum();

                        Console.WriteLine("============= START RESULTS =============");
                        Console.WriteLine($"> Using Span{Environment.NewLine} " +
                                          $">> All country codes  : {string.Join(", ", colosseum.FetchAllCountryCodesUsingSpan())}{Environment.NewLine} " +
                                          $">> First country code : {string.Join(", ", colosseum.FetchSingleCountryCodeUsingSpan())}");
                        Console.WriteLine();
                        Console.WriteLine($"> Using Substring{Environment.NewLine} " +
                                          $">> All country codes  : {string.Join(", ", colosseum.FetchAllCountryCodesUsingSubstring())}{Environment.NewLine} " +
                                          $">> First country code : {string.Join(", ", colosseum.FetchSingleCountryCodeUsingSubstring())}");
                        Console.WriteLine("============== END RESULTS ==============");
                        continue;
                    case "2":
                        BenchmarkRunner.Run<Coliseum>();
                        continue;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Not an option, try again.");
                        continue;
                }
            }
        }

        [MemoryDiagnoser]
        public class Coliseum
        {
            private readonly string[] _accountIds = { "OSBO44", "SMIT01", "CONL353", "POPE379", "KIMJ850" };

            /// <summary>
            /// Fetches all Country codes from all strings within _accountIds, by using the 'Substring' method.
            /// As strings are immutable, this causes each substring to be added to the heap as a new entry. 
            /// </summary>
            /// <returns>An int array of all found country codes.</returns>
            [Benchmark]
            public int[] FetchAllCountryCodesUsingSubstring()
            {
                var countryIds = new int[_accountIds.Length];

                for (var i = 0; i < countryIds.Length; i++)
                {
                    var accountId = _accountIds[i];

                    countryIds[i] = int.Parse(accountId[4..]);
                }

                return countryIds;
            }
        
            /// <summary>
            /// Fetches all Country codes from all strings within _accountIds, by using the 'Span' method.
            /// 'ReadOnlySpan' provides us with a window to the allocated memory on the heap and uses a slice
            /// to fetch our country code. Slices are stored on the stack without adding anything to the heap.
            /// </summary>
            /// <returns>An int array of all found country codes.</returns>
            [Benchmark]
            public int[] FetchAllCountryCodesUsingSpan()
            {
                var countryIds = new int[_accountIds.Length];

                for (var i = 0; i < _accountIds.Length; i++)
                {
                    ReadOnlySpan<char> accountIdAsSpan = _accountIds[i];

                    countryIds[i] = int.Parse(accountIdAsSpan[4..]);
                }

                return countryIds;
            }

            /// <summary>
            /// Performs the same operation as 'FetchAllCountryCodesUsingSubstring' but only on the first entry
            /// within the _accountsIds array.
            /// </summary>
            /// <returns>The country code of the first entry within the _accountIds array.</returns>
            [Benchmark]
            public int FetchSingleCountryCodeUsingSubstring()
            {
                return int.Parse(_accountIds[0][4..]);
            }

            /// <summary>
            /// Performs the same operation as 'FetchAllCountryCodesUsingSpan' but only on the first entry
            /// within the _accountsIds array.
            /// </summary>
            /// <returns>The country code of the first entry within the _accountIds array.</returns>
            [Benchmark]
            public int FetchSingleCountryCodeUsingSpan()
            {
                ReadOnlySpan<char> accountId = _accountIds[0];

                return int.Parse(accountId[4..]);
            }
        }
    }
}