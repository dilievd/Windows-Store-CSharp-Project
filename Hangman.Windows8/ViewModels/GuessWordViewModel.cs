using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Hangman.Windows8.Behavior;
using Hangman.Windows8.Data;
using Hangman.Windows8.Utilities;
using Windows.Storage;

namespace Hangman.Windows8.ViewModels
{
    public class GuessWordViewModel : BaseViewModel
    {
        private const char MysteryChar = '*';

        private ApplicationDataContainer roamingSettings;
        private List<CategoryViewModel> categories;
        private ICommand chooseCategoryCommand;
        private ICommand tryLetterCommand;
        private ICommand newWordCommand;
        private CategoryViewModel selectedCategory;
        private string mysteryWord;
        private ObservableCollection<char> mysteryWordChars;
        private ObservableCollection<char> alphabetChars;

        private async void GetCategoriesAsync()
        {
            this.Categories = await DataPersister.GetCategoriesWithWordsAsync();
            this.OnPropertyChanged("Categories");

            Random rand = new Random();
            int categoryIndex = rand.Next(0, this.categories.Count);
            this.selectedCategory = this.categories[categoryIndex];

            GenerateNewWordAndGamefield();
        }

        private void GenerateAlphabet()
        {
            this.AlphabetChars = new List<char>() { 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ъ', 'Ь', 'Ю', 'Я' };
            this.OnPropertyChanged("AlphabetChars");
        }

        private void GenerateImageSource()
        {
            this.CurrentImageSource = "/Assets/Hangman/" + this.TriesRemaining + ".png";
            this.OnPropertyChanged("CurrentImageSource");
        }

        private void GenerateNewWordAndGamefield()
        {
            string word = GameLogic.GetRandomWord(this.SelectedCategory);
            this.mysteryWord = word.ToUpper();
            GenerateAlphabet();

            List<char> wordChars = GenerateMysteryWordChars(word);

            this.SetObservableValues(this.mysteryWordChars, wordChars);

            this.TriesRemaining = 5;
            GenerateImageSource();

            this.OnPropertyChanged("SelectedCategory");
        }
  
        private List<char> GenerateMysteryWordChars(string word)
        {
            List<char> wordChars = new List<char>();
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] != ' ')
                {
                    wordChars.Add(MysteryChar);
                }
                else
                {
                    wordChars.Add(' ');
                }
            }
            return wordChars;
        }

        private void ShowMysteryWordLetters()
        {
            for (int i = 0; i < this.mysteryWordChars.Count; i++)
            {
                this.mysteryWordChars[i] = this.mysteryWord[i];
            }
        }

        private void SetObservableValues<T>(ObservableCollection<T> observableCollection, IEnumerable<T> values)
        {
            if (observableCollection != values)
            {
                if (observableCollection == null)
                {
                    observableCollection = new ObservableCollection<T>();
                }

                observableCollection.Clear();
                foreach (var item in values)
                {
                    observableCollection.Add(item);
                }
            }
        }

        private void HandleChooseCategoryCommand(object obj)
        {
            for (int i = 0; i < this.categories.Count; i++)
            {
                if (this.categories[i].Title == obj.ToString())
                {
                    this.selectedCategory = this.categories[i];
                    this.OnPropertyChanged("SelectedCategory");
                    break;
                }
            }

            GenerateNewWordAndGamefield();
        }

        private void HandleTryLetterCommand(object obj)
        {
            if (this.mysteryWordChars.Count == 0)
            {
                this.SetObservableValues(this.mysteryWordChars, GenerateMysteryWordChars(this.mysteryWord));
            }

            if (this.TriesRemaining == 0)
            {
                return;
            }

            char letter = obj.ToString()[0];
            if (char.IsWhiteSpace(letter))
            {
                return;
            }

            int index = this.alphabetChars.IndexOf(letter);
            this.alphabetChars[index] = ' ';

            bool isLetterInWord = false;
            for (int i = 0; i < this.mysteryWord.Length; i++)
            {
                if (this.mysteryWord[i] == letter)
                {
                    this.mysteryWordChars[i] = letter;
                    isLetterInWord = true;
                }
            }

            if (!isLetterInWord)
            {
                this.TriesRemaining--;
                GenerateImageSource();
            }

            if (this.TriesRemaining == 0)
            {
                ShowMysteryWordLetters();
            }
            else if (this.TriesRemaining > 0 && this.mysteryWordChars.IndexOf(MysteryChar) < 0)
            {
                this.TotalGuessedWords++;
                this.CurrentImageSource = "/Assets/Hangman/win.png";
                this.OnPropertyChanged("CurrentImageSource");
                LiveTileHandler.UpdateTile();
            }
        }

        private void HandleNewWordCommand(object obj)
        {
            GenerateNewWordAndGamefield();
        }

        public ICommand ChooseCategoryCommand
        {
            get
            {
                if (this.chooseCategoryCommand == null)
                {
                    this.chooseCategoryCommand = new RelayCommand(this.HandleChooseCategoryCommand);
                }

                return this.chooseCategoryCommand;
            }
        }

        public ICommand TryLetterCommand
        {
            get
            {
                if (this.tryLetterCommand == null)
                {
                    this.tryLetterCommand = new RelayCommand(this.HandleTryLetterCommand);
                }

                return this.tryLetterCommand;
            }
        }

        public ICommand NewWordCommand
        {
            get
            {
                if (this.newWordCommand == null)
                {
                    this.newWordCommand = new RelayCommand(this.HandleNewWordCommand);
                }

                return this.newWordCommand;
            }
        }

        public int TriesRemaining { get; set; }

        public string CurrentImageSource { get; set; }

        public IEnumerable<char> AlphabetChars
        {
            get
            {
                if (this.alphabetChars == null)
                {
                    this.alphabetChars = new ObservableCollection<char>();
                }
                return this.alphabetChars;
            }
            set
            {
                if (this.alphabetChars == null)
                {
                    this.alphabetChars = new ObservableCollection<char>();
                }
                this.SetObservableValues(this.alphabetChars, value);
            }
        }

        public CategoryViewModel SelectedCategory
        {
            get
            {
                return this.selectedCategory;
            }
            set
            {
                this.selectedCategory = value;
                this.OnPropertyChanged("SelectedCategory");
            }
        }

        public int TotalGuessedWords
        {
            get
            {
                return (int)(roamingSettings.Values["guessedWords"]);
            }
            set
            {
                this.roamingSettings.Values["guessedWords"] = value;
                this.OnPropertyChanged("TotalGuessedWords");
            }
        }

        public IEnumerable<char> MysteryWordChars
        {
            get
            {
                if (this.mysteryWordChars == null)
                {
                    this.mysteryWordChars = new ObservableCollection<char>();
                }
                return this.mysteryWordChars;
            }
            set
            {
                if (this.mysteryWordChars == null)
                {
                    this.mysteryWordChars = new ObservableCollection<char>();
                }
                this.SetObservableValues(this.mysteryWordChars, value);
            }
        }

        public List<CategoryViewModel> Categories
        {
            get { return this.categories; }
            set { this.categories = value; }
        }

        public GuessWordViewModel()
        {
            GetCategoriesAsync();

            this.roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values["guessedWords"] == null)
            {
                roamingSettings.Values["guessedWords"] = 0;
            }

            this.OnPropertyChanged("TotalGuessedWords");
            LiveTileHandler.UpdateTile();
        }
    }
}
