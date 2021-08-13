using System.Collections.Generic;

namespace HomicideDetective.UserInterface
{
    public class PageContentSource : IPrintable
    {
        public string Title { get; set; }
        public List<string> Contents { get; set; }
        public List<string> TemporaryContents { get; set; }
        
        public PageContentSource(string title)
        {
            Title = title;
            Contents = new();
            TemporaryContents = new();
        }
        
        public PageContentSource(string title, string content) : this(title)
        {
            Contents.Add(content);
        }
        
        public PageContentSource(string title, string content, string temporaryContent) : this(title, content)
        {
            TemporaryContents.Add(temporaryContent);
        }

        public string GetPrintableString()
        {
            var sep = "\r\n\r\n";
            var contents = "";
            foreach (var content in Contents)
                contents += content + sep;

            var additional = "";
            foreach (var content in TemporaryContents)
                additional += content + sep;

            return Title + sep + contents + sep + additional;
        }
            
    }
}