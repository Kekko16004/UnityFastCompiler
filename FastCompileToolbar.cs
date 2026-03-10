using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

public static class FastCompileToolbar
{
    [MainToolbarElement("FastCompile/CompileButton",
        defaultDockPosition = MainToolbarDockPosition.Left)]
    public static MainToolbarElement CompileButton()
    {
        string iconName = FastCompileManager.IsEnabled ? "Refresh" : "Refresh@2x";
        var icon = EditorGUIUtility.IconContent(iconName).image as Texture2D;

        string tooltip = FastCompileManager.IsEnabled
            ? "Compile Now (Manual Mode ENABLED)"
            : "Compile Now (Manual Mode DISABLED - Click Tools to enable)";

        var content = new MainToolbarContent(icon)
        {
            tooltip = tooltip
        };

        var button = new MainToolbarButton(content, () =>
        {
            FastCompileManager.CompileNow();
        });

        return button;
    }
}
