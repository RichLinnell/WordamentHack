using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace WordamentHack
{
    internal class Program
    {
        private static readonly HashSet<string> words = new HashSet<string>();
        private static readonly List<string> found = new List<string>();

        private static void Main(string[] args)
        {
            Console.WriteLine("WORDY - READING WORDS");
            var logFile = File.ReadAllLines("words.txt");
            foreach (var word in logFile)
            {
                if(word.Length<3) continue;
                words.Add(word.ToUpper());
            }
            Console.WriteLine($"{words.Count} read");
            var grid = new char[4, 4];
            Console.WriteLine("input the grid 16");
            Console.WriteLine("---------");
            for (var r = 0; r < 4; r++)
            {
                Console.Write("|");
                for (var c = 0; c < 4; c++)
                {
                    var key = Console.ReadKey();
                    grid[r, c] = char.ToUpper(key.KeyChar);
                    Console.Write("|");
                }
                Console.WriteLine("");
                Console.WriteLine("---------");
            }

            var sw = new Stopwatch();
            var rowTimes = new List<long>();
            var cellTimes = new List<long>();
            long ct = 0;
            long rt = 0;

            sw.Start();

            for (var r = 0; r < 4; r++)
            {
                for (var c = 0; c < 4; c++)
                {
                    var used = new BitArray(16);
                    var word = "";
                    MakeWord(words, grid, r, c, word, used);
                    cellTimes.Add(sw.ElapsedMilliseconds - ct);
                    ct = sw.ElapsedMilliseconds;
                }
                rowTimes.Add(sw.ElapsedMilliseconds - rt);
                rt = sw.ElapsedMilliseconds;
            }

            var p = Process.GetProcessesByName("Microsoft Ultimate Word Games")[0];
            var handle = p.MainWindowHandle;
            SetForegroundWindow(handle);
            Console.WriteLine("Give Wordament app focus");
            Console.Write("Hacking in ");
            for (int i = 5; i > 0; i--)
            {
                Console.Write(i);
                Thread.Sleep(1000);
            }
            Console.WriteLine();
            var itemNo = 1;
            foreach (var word in found.Distinct().OrderByDescending(w => w.Length))
            {
                SendKeys.SendWait(word);
                SendKeys.SendWait("\n");
            }

            Console.WriteLine("---------");

            Console.WriteLine("Stats :");
            Console.WriteLine($"  RowTime = {rowTimes.Average()} Ave. {rowTimes.Max()} Max.");
            Console.WriteLine($"  CellTime = {cellTimes.Average()} Ave. {cellTimes.Max()} Max.");

            Console.WriteLine("I'm Done");
            Console.ReadLine();
        }

        [DllImport("User32.dll")]
        private static extern int SetForegroundWindow(IntPtr pointer);

        private static void MakeWord(
            HashSet<string> possibles,
            char[,] grid,
            int row,
            int col,
            string word,
            BitArray used)
        {
            var newword = word + grid[row, col];
            if (!possibles.Any(w => w.StartsWith(newword))) return;
            if (possibles.Contains(newword))
            {
                found.Add(newword);
                words.Remove(newword);
                possibles.Remove(newword);
            }
            ;
            var copyArray = new BitArray(used);
            copyArray[row * 4 + col] = true;
            var newList = new HashSet<string>(possibles.Where(w => w.StartsWith(newword)));
            for (var r = -1; r < 2; r++)
            {
                if (row + r < 0 || row + r > 3) continue;
                for (var c = -1; c < 2; c++)
                {
                    if (col + c < 0 || col + c > 3) continue;
                    if (r == 0 && c == 0) continue;
                    if (used[(row + r) * 4 + col + c]) continue;
                    MakeWord(newList, grid, row + r, col + c, newword, copyArray);
                }
            }
        }
    }
}