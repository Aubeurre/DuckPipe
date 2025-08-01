using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms;
using System.Drawing;

public class CustomTabControl : TabControl
{
    public Color SelectedTabColor { get; set; } = Color.FromArgb(60, 60, 60);
    public Color UnselectedTabColor { get; set; } = Color.FromArgb(50, 50, 50);
    public Color SelectedTextColor { get; set; } = Color.White;
    public Color UnselectedTextColor { get; set; } = Color.White;
    public Color BackgroundColor { get; set; } = Color.FromArgb(40, 40, 40);
    public CustomTabControl()
    {
        // Désactive l’affichage des onglets en fixant la taille de l’item à zéro (0)
        this.SizeMode = TabSizeMode.Fixed;
        this.ItemSize = new Size(0, 1);
        this.Multiline = true; // Permet la modification de la taille des onglets
    }

    // Override pour empêcher l’affichage des onglets
    protected override void WndProc(ref Message m)
    {
        const int TCM_ADJUSTRECT = 0x1328;

        if (m.Msg == TCM_ADJUSTRECT && !DesignMode)
        {
            // On ne modifie pas le rectangle de la zone client pour cacher les onglets
            m.Result = (IntPtr)1;
            return;
        }
        base.WndProc(ref m);
    }
}