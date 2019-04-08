using System;
using System.Linq;
using System.Windows.Controls;
using static Editor.MainWindow;

namespace Editor.Controls
{
    /// <summary>
    /// Interaction logic for LayerSelector.xaml
    /// </summary>
    public partial class LayerSelector : UserControl
    {
        public LayerSelector()
        {
            InitializeComponent();

            bool firstLayer = true;

            foreach (Layer layer in Enum.GetValues(typeof(Layer)))
            {
                RadioButton rb = new RadioButton();
                rb.Content = layer.ToString();
                rb.Tag = layer;
                rb.GroupName = "grpLayer";
                rb.IsChecked = firstLayer;

                pnlLayers.Children.Add(rb);
                if (firstLayer) firstLayer = false;
            }
        }

        public Layer GetSelectedLayer()
        {
            return (Layer)pnlLayers.Children.OfType<RadioButton>().Where(o => o.IsChecked == true).FirstOrDefault().Tag;
        }
    }
}