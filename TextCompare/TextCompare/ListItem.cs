namespace TextCompare
{
    public class ListItem
    {
        public string Display { get; set; }
        public string Value { get; set; }

        public ListItem(string displayString, string valueString)
        {
            this.Display = displayString;
            this.Value = valueString;
        }
    }
}