# Reloaded-GUI

Contains the driving components of the Windows Forms UI interface that backs up Reloaded Mod Loader's Launcher.

This project is tailored entirely for Reloaded, thus may not be the most universal and does take some rather "weird" conventions.

In addition, certain assumptions are made about your GUI elements.

```csharp
// Category Bar Items should be named categoryBar_*
// Regular Items should be named item_*
// Title Bar Items should be named titleBar_*
// Buttons used for decoration should be named box_*
// Buttons (Alternative style) should be named borderless_*
```

## Usage


### Basic

You should first bind your individual windows forms instances by using the bindings class,
which allows for them to automatically re-theme themselves on theme switch.

This should be done in the constructor:

```csharp
Bindings.WindowsForms.Add(this);
```

And should subsequently be removed in the destructor/finalizer.

To activate the current user's Reloaded theme, simply then create an instance of `Reloaded_GUI.Styles.Themes.Theme`
and call the method `LoadCurrentTheme()`, which will automatically theme all added windows to the aforementioned bindings list.

For temporary windows such as dialogs, you should not register the individual Windows Form to the bindings, instead call 
`Reloaded_GUI.Styles.Themes.ApplyTheme.ThemeWindowsForm()` which will apply the current theme to the current window.

### Icons

Should you wish to inherit any images from the individual Reloaded Themes, consider subscribing to the `Bindings.ApplyImages`
delegate, which will fire on theme switch and loads. The delegate will pass a structure containing a copy of the individual images
you can apply to your WinForms elements.
