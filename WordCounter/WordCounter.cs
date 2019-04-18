﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordCounter
{
    public class FileWordCounter : ICountWords
    {
        public IList<KeyValuePair<string,int>> CountTopWords(string source, int top)
        {
            var text = Preprocess(SourceText(source));            
            var words = Words(text);
            var wordCount = CountWords(words);
            var topWords = Top(wordCount, top);
            return topWords;
        }

        private static string Preprocess(string text)
        {
            text = StandardiseWhitespace(text);
            return text;
        }

        private static List<KeyValuePair<string, int>> Top(Dictionary<string, int> items, int topCount)
        {
            return items.OrderByDescending(x => x.Value)
                        .Take(topCount)
                        .Select(x => new KeyValuePair<string, int>(x.Key, x.Value))
                        .ToList();
        }

        private static Dictionary<string, int> CountWords(List<string> words)
        {
            var wordCount = new Dictionary<string, int>();
            words.ForEach(word =>
            {
                if (word.All(Char.IsLetter))
                {                    
                    AddOrUpdateWordCount(wordCount, word.ToLower());
                }
            });
            return wordCount;
        }

        private static void AddOrUpdateWordCount(Dictionary<string, int> wordCount, string key)
        {
            if (!wordCount.ContainsKey(key))
            {
                wordCount.Add(key, 1);
            }
            else
            {
                wordCount[key]++;
            }
        }

        private static List<string> Words(string text)
        {
            return text.Split(" ").ToList();
        }

        private static string SourceText(string source)
        {
            var filepath = source;
            var text = File.ReadAllText(filepath);
            return text;
        }

        public static string StandardiseWhitespace(string text)
        {
            return string.Join(" ", text.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
