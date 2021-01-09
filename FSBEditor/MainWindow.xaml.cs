using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace FSBEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FSBFile fsb = null;
        FSBEntry currentFsbEntry;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFSB_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "FMOD Sound Bank|*.fsb";
            //openFile.InitialDirectory = Directory.GetCurrentDirectory();
            openFile.CheckFileExists = true;
            openFile.CheckPathExists = true;
            openFile.RestoreDirectory = true;

            if (openFile.ShowDialog() == true)
            {
                try
                {
                    fsb = new FSBFile();
                    fsb.ReadFile(openFile.FileName);
                    window.Title = string.Format("FMOD Sound Bank Editor ({0})", Path.GetFileName(openFile.FileName));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }

                lstFsb.Items.Clear();

                int iter = 1;
                foreach (FSBEntry entry in fsb.fsbEntries)
                {
                    lstFsb.Items.Add(string.Format("{0} - {1}", iter, entry.name));

                    iter++;
                }

                lstFsb.SelectedIndex = 0;
                ToggleFields(true);
            }
        }

        private void lstFsb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstFsb.SelectedIndex != -1)
            {
                currentFsbEntry = fsb.fsbEntries[lstFsb.SelectedIndex];

                RefreshFields();

                if (currentFsbEntry.xmaName != "")
                {
                    lblCurrentXma.Content = string.Format("Current XMA file: {0}", currentFsbEntry.xmaName);
                }
                else
                {
                    lblCurrentXma.Content = "";
                }
            }

            if (fsb == null)
            {
                btnDel.IsEnabled = false;
            }
            else
            {
                btnDel.IsEnabled = true;
            }
        }

        private void ToggleFields(bool active)
        {
            foreach (TextBox textBox in grdFSB.Children.OfType<TextBox>())
            {
                if (!textBox.Name.Contains("RO"))
                {
                    textBox.IsEnabled = active;
                }
            }

            btnImportXma.IsEnabled = active;
        }

        private void RefreshFields(int indexToUpdate = -1, FSBEntry fsbEntry = null)
        {
            txtName.Text = currentFsbEntry.name;
            txtFileSizeRO.Text = currentFsbEntry.streamSize.ToString();
            txtSampleRateRO.Text = currentFsbEntry.sampleRate.ToString();
            txtSamplesRO.Text = currentFsbEntry.numSamples.ToString();
            txtStartSample.Text = currentFsbEntry.loopStartSample.ToString();
            txtEndSample.Text = currentFsbEntry.loopEndSample.ToString();
            txtPan.Text = currentFsbEntry.pan.ToString();
            txtChannelsRO.Text = currentFsbEntry.numChannels.ToString();
            txtCodecRO.Text = currentFsbEntry.codec == 1 ? "XMA" : "Unknown";
            txtVolume.Text = currentFsbEntry.volume.ToString();

            if (indexToUpdate != -1)
            {
                lstFsb.SelectedIndex = 0;
                lstFsb.Items[indexToUpdate] = string.Format("{0} - {1}", indexToUpdate + 1, fsbEntry.name);
                lstFsb.SelectedIndex = indexToUpdate;
            }
        }

        void RenumberEntries(int returnIndex = -1)
        {
            for (int i = 0; i < lstFsb.Items.Count; i++)
            {
                lstFsb.Items[i] = string.Format("{0} - {1}", i+1, fsb.fsbEntries[i].name);
            }
            
            if (returnIndex != -1)
            {
                lstFsb.SelectedIndex = returnIndex;
            }
        }

        private void ClearFields()
        {
            foreach (TextBox textBox in grdFSB.Children.OfType<TextBox>())
            {
                textBox.Text = "";
            }
        }

        private void btnImportXma_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "XMA Audio File|*.xma";
            //openFile.InitialDirectory = Directory.GetCurrentDirectory();
            openFile.CheckFileExists = true;
            openFile.CheckPathExists = true;
            openFile.RestoreDirectory = true;

            if (openFile.ShowDialog() == true)
            {
                try
                {
                    currentFsbEntry = fsb.ReadXMA(openFile.FileName);
                    fsb.fsbEntries[lstFsb.SelectedIndex].xmaName = Path.GetFileNameWithoutExtension(openFile.FileName);
                    fsb.fsbEntries[lstFsb.SelectedIndex] = currentFsbEntry;

                    RefreshFields(lstFsb.SelectedIndex, currentFsbEntry);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }

                lblCurrentXma.Content = string.Format("Current XMA file: {0}", currentFsbEntry.xmaName);
            }
        }

        private void mnuSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "FMOD Sound Bank|*.fsb";
            //saveFile.InitialDirectory = Directory.GetCurrentDirectory();
            saveFile.RestoreDirectory = true;

            if (saveFile.ShowDialog() == true)
            {
                fsb.WriteFile(saveFile.FileName);
                HarmonicTuning.WriteXML(Path.GetDirectoryName(saveFile.FileName), Path.GetFileNameWithoutExtension(saveFile.FileName), fsb);
            }
        }

        private void mnuNew_Click(object sender, RoutedEventArgs e)
        {
            ToggleFields(false);
            ClearFields();

            lstFsb.Items.Clear();
            btnAdd.IsEnabled = true;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (fsb == null)
            {
                fsb = new FSBFile();
            }

            FSBEntry newFsb = new FSBEntry();
            fsb.fsbEntries.Add(newFsb);
            currentFsbEntry = newFsb;

            lstFsb.Items.Add(string.Format("{0} - {1}", fsb.fsbEntries.Count, newFsb.name));

            ToggleFields(true);

            lstFsb.SelectedIndex = lstFsb.Items.Count - 1;
            btnDel.IsEnabled = true;
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (fsb.fsbEntries.Count == 1)
            {
                lstFsb.Items.Clear();
                fsb.fsbEntries = new List<FSBEntry>();
                btnDel.IsEnabled = false;
                ToggleFields(false);
            }

            if (lstFsb.SelectedIndex != -1)
            {
                fsb.fsbEntries.RemoveAt(lstFsb.SelectedIndex);
                lstFsb.Items.RemoveAt(lstFsb.SelectedIndex);
                lstFsb.SelectedIndex = lstFsb.Items.Count - 1;
            }

            RenumberEntries();
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            currentFsbEntry.name = txtName.Text;

            fsb.fsbEntries[lstFsb.SelectedIndex] = currentFsbEntry;

            RenumberEntries(lstFsb.SelectedIndex);
        }

        private void txtStartSample_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(txtStartSample.Text, out int i))
            {
                currentFsbEntry.loopStartSample = int.Parse(txtStartSample.Text);

                fsb.fsbEntries[lstFsb.SelectedIndex] = currentFsbEntry;

                RenumberEntries(lstFsb.SelectedIndex);
            }
        }

        private void txtEndSample_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(txtEndSample.Text, out int i))
            {
                currentFsbEntry.loopEndSample = int.Parse(txtEndSample.Text);

                fsb.fsbEntries[lstFsb.SelectedIndex] = currentFsbEntry;

                RenumberEntries(lstFsb.SelectedIndex);
            }
        }

        private void txtPan_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (short.TryParse(txtPan.Text, out short i))
            {
                currentFsbEntry.pan = short.Parse(txtPan.Text);

                fsb.fsbEntries[lstFsb.SelectedIndex] = currentFsbEntry;

                RenumberEntries(lstFsb.SelectedIndex);
            }
        }

        private void txtVolume_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(txtVolume.Text, out int i))
            {
                currentFsbEntry.volume = int.Parse(txtVolume.Text);

                fsb.fsbEntries[lstFsb.SelectedIndex] = currentFsbEntry;

                RenumberEntries(lstFsb.SelectedIndex);
            }
        }
    }
}
