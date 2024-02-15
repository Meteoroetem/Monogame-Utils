using Adw;

var application = Application.New("org.gir.core", Gio.ApplicationFlags.FlagsNone);
application.OnActivate += (sender, args) =>
{
    //var window = Gtk.ApplicationWindow.New((Gtk.Application) sender);
    var window = new TilemapEditor.TilemapEditorWindow
    {
        Application = (Application) sender,
        Title = "MonoGame Tilemap Editor"
    };
    window.SetDefaultSize(1000, 700);
    window.Show();
};
return application.RunWithSynchronizationContext();