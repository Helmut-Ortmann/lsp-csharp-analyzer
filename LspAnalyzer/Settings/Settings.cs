using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace LspAnalyzer.Settings
{
    public class Settings
    {
        string _settingsPath;
        string _settingsFacturyPath;
        string _settingsBackupPath;

        public SettingsItem SettingsItem { get; set; }

        public Settings()
        {


            _settingsPath = Path.Combine((Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)), "ho\\LspAnalyzer\\Settings.Json");
            _settingsFacturyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.Json");
            _settingsBackupPath = $"{_settingsPath}.tmp";

            if (!Directory.Exists(Path.GetDirectoryName(_settingsPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(_settingsPath));

            if (!File.Exists(_settingsFacturyPath))
            {
                MessageBox.Show($"Expected factury settings: '{_settingsFacturyPath}' ",@"No factury settings found");
            }
            // Load factury settings
            if (!File.Exists(_settingsPath))
            {
                File.Copy(_settingsFacturyPath, _settingsPath);
            }
            LoadJason();
        }

        private void LoadJason()
        {
            try
            {
                    SettingsItem = JsonConvert.DeserializeObject<SettingsItem>(File.ReadAllText(_settingsPath));
            }
            catch (Exception e)
            {
                MessageBox.Show($"Path: '{_settingsPath}'\r\n{e}","Can't read 'Settings.Json', break!!");
               
            }
            
        }


        /// <summary>
        /// Backup settings.json
        /// </summary>
        public void JsonBackup()
        {
            var result = MessageBox.Show(
                $@"LspAnalyzer deletes 'Settings.json'. After restart, you have the initial settings.

You find the factury settings in: '{_settingsFacturyPath}':
You find the current settings in: '{_settingsPath}':
You find a backup of the current settings in: '{_settingsBackupPath}':
", "Do you want to reset your configuration?", MessageBoxButtons.OKCancel);

            if (result == DialogResult.OK) {

                File.Copy(SettingsPath, SettingsBackUpPath, true);
                File.Delete(SettingsPath);
                MessageBox.Show(
                    $@"{SettingsPath} saved to {SettingsBackUpPath}",
                    "Configuration reset to default. Please Restart!");
            }
        }

        public string SettingsPath
        {
            get => _settingsPath;
            set => _settingsPath = value;
        }

        public string SettingsFacturyPath
        {
            get => _settingsFacturyPath;
            set => _settingsFacturyPath = value;
        }

        public string SettingsBackUpPath
        {
            get => _settingsBackupPath;
            set => _settingsBackupPath = value;
        }
    }
}
