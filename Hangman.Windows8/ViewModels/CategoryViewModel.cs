using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hangman.Windows8.ViewModels
{
    public class CategoryViewModel
    {
        public string Title { get; set; }
        public List<string> Items { get; set; }

        public static Expression<Func<XElement, CategoryViewModel>> FromXElement
        {
            get
            {
                return element =>
                    new CategoryViewModel()
                    {
                        Title = element.Element("title").Value,
                        Items = element.Element("items")
                            .Elements("item")
                            .Select(item => item.Value)
                            .ToList()
                    };
            }
        }
    }
}
