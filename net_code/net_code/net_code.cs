// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Linq;

namespace dotnet_maintainability_test
{
    // This file intentionally contains many maintainability problems
    // (very long methods, deep nesting, many responsibilities, global mutable state,
    // long parameter lists, magic numbers, complex boolean expressions) so SonarQube
    // will report maintainability issues. It avoids duplicate code blocks and
    // obvious security vulnerabilities.
    internal static class Program
    {
        // Global mutable state - makes reasoning harder
        public static int GlobalCounter = 0;
        public static string GlobalMode = "default";

        private static void Main(string[] args)
        {
            // Entrypoint that delegates to a very large routine
            try
            {
                var runner = new Monolith();
                runner.Run(args ?? Array.Empty<string>(), 42, true, "alpha", new List<int> { 1, 2, 3 }, DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                // Keep minimal handling to avoid security concerns while still being realistic
                Console.WriteLine("Unhandled: " + ex.GetType().Name + ": " + ex.Message);
            }
        }
    }

    // A single large class that does many different things
    public sealed class Monolith
    {
        // Many public fields to increase class surface
        public int A;
        public int B;
        public int C;
        public string Mode = "unset";

        // A method with a very long signature and many responsibilities
        public void Run(string[] inputs, int magicNumber, bool flag, string name, List<int> items, DateTime when)
        {
            // Lots of local variables with unclear purpose
            var result = 0;
            var state = new Dictionary<string, object>();
            var index = 0;
            var errors = new List<string>();
            var accumulated = string.Empty;
            var depth = 0;

            // Update global state - side-effects everywhere
            Program.GlobalCounter += magicNumber;
            Program.GlobalMode = name ?? Program.GlobalMode;

            // Very long method body with deep nesting and many branches
            foreach (var input in inputs)
            {
                depth++;
                if (depth % 2 == 0)
                {
                    for (var i = 0; i < items.Count; i++)
                    {
                        index = ProcessItem(items[i], index, ref result);
                        if (index < 0)
                        {
                            errors.Add("NegativeIndex");
                            // Continue with another nested check
                            for (var j = i; j >= 0; j--)
                            {
                                if (j % 3 == 0)
                                {
                                    accumulated += (j * magicNumber).ToString();
                                }
                                else if (j % 3 == 1)
                                {
                                    accumulated += "-" + j;
                                }
                                else
                                {
                                    accumulated += ":" + (j + index);
                                }
                            }
                        }
                        else
                        {
                            // Complex but deterministic calculation
                            result += ComputeComplex(result, index, flag, name.Length, when.Year, items.Count);
                        }

                        // Large switch with many cases to increase cognitive complexity
                        switch (DetermineMode(flag, name, index))
                        {
                            case ModeKind.Alpha:
                                A = index + 1;
                                break;
                            case ModeKind.Beta:
                                B = index * 2;
                                goto case ModeKind.Gamma;
                            case ModeKind.Gamma:
                                C = index - 3;
                                break;
                            case ModeKind.Delta:
                                // intentionally empty
                                break;
                            default:
                                // nested conditional with complex boolean logic
                                if ((flag && index > 0) || (!flag && name != null && name.StartsWith("a") && items.Count > 0))
                                {
                                    result += 7;
                                }
                                else if (index == 0 && name?.Length > 0)
                                {
                                    result -= 1;
                                }
                                else
                                {
                                    result = result ^ index;
                                }
                                break;
                        }

                        // Another nested try/catch inside loop to increase complexity
                        try
                        {
                            state[$"key_{i}"] = Transform(index, accumulated, when);
                        }
                        catch (Exception ex)
                        {
                            errors.Add(ex.GetType().Name + ":" + ex.Message);
                        }
                    }
                }
                else
                {
                    // Different path with its own deeply nested structures
                    if (input == null)
                    {
                        errors.Add("NullInput");
                        continue;
                    }

                    var tokens = input.Split(new[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var t in tokens)
                    {
                        depth += t.Length % 5;
                        if (t.Contains("skip"))
                        {
                            continue;
                        }

                        if (t.Length > 2)
                        {
                            result += t.Length * magicNumber;
                        }
                        else
                        {
                            result -= magicNumber / (t.Length + 1);
                        }

                        // Inline lambda expressions mixed with imperative code
                        Func<int, int> mapper = x => x * (t.Length % 3 + 1) - (t[0] % 2);
                        var mapped = mapper(depth);
                        result += mapped;

                        // Deep conditional complexity
                        if ((mapped > 10 && !flag) || (mapped < -5 && flag && name.Length > 0))
                        {
                            result = HandleSpecial(result, mapped, ref accumulated, ref depth);
                        }
                    }
                }
            }

            // Final aggregation with many magic numbers and unclear rules
            var final = ((result * 31) + (Program.GlobalCounter % 97) - (A + B + C) + accumulated.Length) % 1000;
            final += errors.Count * 13;

            // Expose a lot of state via console to avoid dead code elimination
            Console.WriteLine("Final:" + final + " R:" + result + " G:" + Program.GlobalCounter + " M:" + Program.GlobalMode);
            foreach (var kv in state.Take(5))
            {
                Console.WriteLine(kv.Key + "=" + kv.Value);
            }

            // store some fields to increase class surface and hidden state
            this.Mode = Program.GlobalMode + "-processed";
            this.B = final;
        }

        private static int ProcessItem(int value, int index, ref int result)
        {
            // Some arithmetic and mutation
            index += value % 7;
            result += (value * 3) % 11;
            if (value % 2 == 0)
            {
                index = -index;
            }

            // multiple return paths
            if (index < -10)
            {
                return -1;
            }

            return index;
        }

        private static int ComputeComplex(int r, int i, bool f, int length, int year, int itemCount)
        {
            // long expression with magic numbers
            var t = r * 3 + i * 7 - (f ? 13 : 5) + length * year % 100 - itemCount * 9;
            if (t % 2 == 0)
            {
                return t / 2;
            }

            return (t * 3 + 1) / 2;
        }

        private static ModeKind DetermineMode(bool flag, string name, int index)
        {
            // Hard to follow boolean logic and fall-through
            if (flag && index % 5 == 0)
            {
                return ModeKind.Beta;
            }

            if (!flag && !string.IsNullOrEmpty(name) && name.Length > 3)
            {
                return ModeKind.Alpha;
            }

            if (index < 0)
            {
                return ModeKind.Gamma;
            }

            return ModeKind.Unknown;
        }

        private static object Transform(int index, string accumulated, DateTime when)
        {
            // Complex object creation with anonymous types and conditional content
            if (string.IsNullOrEmpty(accumulated))
            {
                return new { Index = index, Timestamp = when, Note = "empty" };
            }

            return new Dictionary<string, object>
            {
                ["index"] = index,
                ["len"] = accumulated.Length,
                ["snippet"] = accumulated.Length > 10 ? accumulated.Substring(0, 10) : accumulated,
                ["when"] = when
            };
        }

        private static int HandleSpecial(int result, int mapped, ref string accumulated, ref int depth)
        {
            // Mutates multiple out parameters and performs confusing math
            depth += mapped % 4;
            if (mapped < 0)
            {
                accumulated += "!" + mapped;
                return result - (mapped * 2);
            }

            accumulated = mapped.ToString() + accumulated;
            return result + (mapped * 5) - depth;
        }
    }

    public enum ModeKind
    {
        Unknown = 0,
        Alpha = 1,
        Beta = 2,
        Gamma = 3,
        Delta = 4
    }
}

