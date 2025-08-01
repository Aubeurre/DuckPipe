using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class IconComboBox : ComboBox
{
    public Dictionary<string, Image> IconMap { get; set; } = new();

    public IconComboBox()
    {
        DrawMode = DrawMode.OwnerDrawFixed;
        DropDownStyle = ComboBoxStyle.DropDownList;

        this.DrawItem += IconComboBox_DrawItem;
    }

    private void IconComboBox_DrawItem(object sender, DrawItemEventArgs e)
    {
        e.DrawBackground();

        if (e.Index >= 0)
        {
            string text = Items[e.Index].ToString();
            Image? icon = IconMap.ContainsKey(text) ? IconMap[text] : null;

            if (icon != null)
            {
                e.Graphics.DrawImage(icon, e.Bounds.Left + 2, e.Bounds.Top + 2, 16, 16);
                e.Graphics.DrawString(text, Font, Brushes.White, e.Bounds.Left + 22, e.Bounds.Top + 2);
            }
            else
            {
                e.Graphics.DrawString(text, Font, Brushes.White, e.Bounds.Left + 4, e.Bounds.Top + 2);
            }
        }

        e.DrawFocusRectangle();
    }
}
