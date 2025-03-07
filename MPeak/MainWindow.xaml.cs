using Microsoft.Win32;
using System;
using System.Windows;
using MediaToolkit;
using MediaToolkit.Model;
using System.IO;

namespace MPeak
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string videoPath = "";
        private string ffmpegPath = @"ffmpeg/ffmpeg.exe";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSeleccionarArchivo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Archivos MP4 (*.mp4)|*.mp4"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                videoPath = openFileDialog.FileName;
                lblEstado.Text = "Archivo seleccionado: " + Path.GetFileName(videoPath);
            }
        }

        private void btnConvertir_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(videoPath))
            {
                MessageBox.Show("Selecciona un archivo MP4 primero.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string outputPath = Path.ChangeExtension(videoPath, ".mp3");

            try
            {
                var inputFile = new MediaFile { Filename = videoPath };
                var outputFile = new MediaFile { Filename = outputPath };

                using (var engine = new Engine(ffmpegPath))
                {
                    engine.GetMetadata(inputFile);
                    engine.Convert(inputFile, outputFile);
                }

                MessageBox.Show("Conversión completada: " + outputPath, "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                lblEstado.Text = "Conversión completada";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la conversión: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}