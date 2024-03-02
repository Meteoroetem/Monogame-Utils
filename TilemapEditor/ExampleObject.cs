using System.Text.Json.Serialization;
using GObject;
using Gtk;

namespace TilemapEditor
{
    public class ExampleObject(string label) : ListItem(false, new ConstructArgument[0])
    {
        public string label = label;
    }
}