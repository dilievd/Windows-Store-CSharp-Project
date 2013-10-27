using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Hangman.Windows8.ViewModels;

namespace Hangman.Windows8.Data
{
    public static class DataPersister
    {
        private const string WordsDocumentPath = "words.xml";

        public static Task<List<CategoryViewModel>> GetCategoriesWithWordsAsync()
        {
            return Task.Run(
                () => GetCategoriesWithWords()
                );

        }

        private static List<CategoryViewModel> GetCategoriesWithWords()
        {
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            XDocument xmlDoc = XDocument.Load(folder.Path + "\\" + WordsDocumentPath);
            XElement categoriesRoot = xmlDoc.Root;
            var categoriesElements = categoriesRoot.Elements("category");
            var categoriesViewModels = categoriesElements.AsQueryable()
                                        .Select(CategoryViewModel.FromXElement);

            return categoriesViewModels.ToList();
        }
    }
}
