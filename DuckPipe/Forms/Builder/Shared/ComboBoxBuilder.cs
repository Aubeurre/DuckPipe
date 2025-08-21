// UI/Builders/ComboBoxBuilder.cs
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;

namespace DuckPipe.Forms.Builder.Shared
{
    public static class ComboBoxBuilder
    {
        // Remplit un IconComboBox de statuts + mappe les icônes
        public static void PopulateStatusIconCombo(IconComboBox comboBox, Dictionary<string, string> statusIcons)
        {
            comboBox.BeginUpdate();
            try
            {
                comboBox.Items.Clear();
                comboBox.Items.AddRange(statusIcons.Keys.ToArray());

                var iconMap = new Dictionary<string, Image>();
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;

                foreach (var kv in statusIcons)
                {
                    string fullPath = Path.Combine(baseDir, kv.Value);
                    if (File.Exists(fullPath))
                        iconMap[kv.Key] = Image.FromFile(fullPath);
                }

                comboBox.IconMap = iconMap;
                if (comboBox.Items.Count > 0 && comboBox.SelectedIndex == -1)
                    comboBox.SelectedIndex = 0;
            }
            finally { comboBox.EndUpdate(); }
        }

        // Remplit une ComboBox classique pour les users
        public static void PopulateUserCombo(ComboBox comboBox, IEnumerable<string> users)
        {
            comboBox.BeginUpdate();
            try
            {
                comboBox.Items.Clear();
                foreach (var u in users) comboBox.Items.Add(u);
                if (comboBox.Items.Count > 0 && comboBox.SelectedIndex == -1)
                    comboBox.SelectedIndex = 0;
            }
            finally { comboBox.EndUpdate(); }
        }
    }
}
