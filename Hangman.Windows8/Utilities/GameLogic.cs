using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangman.Windows8.ViewModels;

namespace Hangman.Windows8.Utilities
{
    public class GameLogic
    {
        internal static string GetRandomWord(CategoryViewModel category)
        {
            Random rand = new Random();
            int randomIndex = rand.Next(0, category.Items.Count());
            string randomWord = category.Items[randomIndex];

            return randomWord.ToUpper();
        }
    }
}
