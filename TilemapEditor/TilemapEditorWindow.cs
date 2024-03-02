using GObject;
using Gtk;

namespace TilemapEditor;

public class TilemapEditorWindow : Window{
	[Connect] private readonly Adw.HeaderBar headerBar;
    [Connect] private readonly Adw.SplitButton openButton;
    [Connect] private readonly Button newButton;
    [Connect] private readonly ScrolledWindow sw;
    [Connect] private readonly GridView mainGridView;

    private TilemapEditorWindow(Gtk.Builder builder, string name) : base(builder.GetPointer(name), false){
        builder.Connect(this);  

        var factory = SignalListItemFactory.New();

        static void setupHandle(SignalListItemFactory sender, SignalListItemFactory.SetupSignalArgs args){
            System.Console.WriteLine("Setup!");
            var box = Box.New(Orientation.Horizontal, 2);
            var label = new Value("");
            //((ListItem)args.Object).Item.GetProperty("label", label);
            box.Append(Label.New(label.GetString()));
            box.MarginEnd = 20;
            ((ListItem)args.Object).SetChild(box);
        }
        static void bindHandle(SignalListItemFactory sender, SignalListItemFactory.BindSignalArgs args){
            Label labelWidget = (Label) ((ListItem) args.Object).Child.GetFirstChild();
            labelWidget.SetLabel(((ExampleObject) ((ListItem) args.Object).Item).label);
        }

        factory.OnSetup += setupHandle;
        factory.OnBind += bindHandle;

        var listStore = Gio.ListStore.New(GObject.Type.Object);

        for(int i = 0; i < 5; i++){
            var obj = new ExampleObject($"{i}");
            obj.SetProperty("label", new($"{i}"));
            listStore.Append(obj);
        }
        
        mainGridView.Model = MultiSelection.New(listStore);
        mainGridView.Factory = factory;
    }

    public TilemapEditorWindow() : this(new Builder("MainWindow.ui"), "mainWindow"){
        
    }
}