using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Editor.Models
{
    /// <summary>
    /// Interaction logic for Texture.xaml
    /// </summary>
    public partial class Texture : UserControl
    {
        public byte ID { get; }

        public Texture(byte id, Bitmap img)
        {
            InitializeComponent();
            MouseLeftButtonDown += Control_MouseClick;
            ID = id;
            imgTexture.Source = Extensions.ImageSourceForBitmap(img);
            brdSelected.BorderBrush = null;
        }

        // Event fires when the MouseClick event fires for the UC or any of its child controls.
        public event EventHandler<EventArgs> WasClicked;

        private void Control_MouseClick(object sender, MouseButtonEventArgs e)
        {
            var wasClicked = WasClicked;
            if (wasClicked != null)
            {
                WasClicked(this, EventArgs.Empty);
            }
            // Select this UC on click.
            IsSelected = true;
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                brdSelected.BorderBrush = IsSelected ? new SolidColorBrush(Colors.Red) : null;
            }
        }
    }
}