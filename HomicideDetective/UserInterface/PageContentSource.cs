namespace HomicideDetective.UserInterface
{
    public class PageContentSource : IPrintable
    {
        public string Title { get; set; }
        public string Content { get; set; }
        
        public PageContentSource(string title)
        {
            Title = title;
            Content = "";
        }
        
        public PageContentSource(string title, string content)
        {
            Title = title;
            Content = content;
        }
        public string GetPrintableString()
            => $"{Title}\r\n\r\n{Content}";
    }
}