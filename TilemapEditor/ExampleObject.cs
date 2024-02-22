using System.Text.Json.Serialization;
using GObject;

namespace TilemapEditor
{
    public class ExampleObject(string label) : GObject.Object(false, new ConstructArgument[0])
    {
        public string label = label;
    }
}