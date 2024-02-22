using System.Reflection.Emit;
using Adw;
using Gio;
using GObject;
//using Gtk;

namespace TilemapEditor;

public class TilemapEditorWindow : Window{
	[Gtk.Connect] private readonly HeaderBar headerBar;
    [Gtk.Connect] private readonly SplitButton openButton;
    [Gtk.Connect] private readonly Gtk.Button newButton;
    [Gtk.Connect] private readonly Gtk.ScrolledWindow sw;

    [Gtk.Connect] public Gtk.ListView mainGridView;


    private TilemapEditorWindow(Gtk.Builder builder, string name) : base(builder.GetPointer(name), false){
        builder.Connect(this);
    }

    public TilemapEditorWindow() : this(new Gtk.Builder("MainWindow.ui"), "mainWindow"){
        var list = Gio.ListStore.New(GObject.Type.Object);
        for(int i = 0; i < 5; i++){
            var obj = new ExampleObject($"{i}"); //GObject.Object.NewValist(GObject.Type.Object, "label", i);
            Console.WriteLine(obj);
            list.Append(obj);
        }
        var factory = Gtk.SignalListItemFactory.New();
        factory.OnSetup += (sender, args) => {
            var button = Gtk.Button.New();
            button.Label = ((ExampleObject)args.Object).label;
            ((Gtk.ListItem)args.Object).SetChild(button);
        };
        mainGridView = Gtk.ListView.New(Gtk.NoSelection.New((ListModel)list),factory);
    }
}