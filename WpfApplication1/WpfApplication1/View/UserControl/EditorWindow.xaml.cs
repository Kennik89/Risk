using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Risk.View.UserControl
{
    /// <summary>
    /// Interaction logic for Canvas.xaml
    /// </summary>
    public partial class EditorWindow : System.Windows.Controls.UserControl
    {
        public EditorWindow()
        {
            InitializeComponent();
        }

        private void CountryButton_MouseMove(object sender, MouseEventArgs e)
        {
            CountryButtonEditor CB = sender as CountryButtonEditor;
            if (CB != null && e.LeftButton == MouseButtonState.Pressed)
            {
                /*DragDrop.DoDragDrop(CB,
                                     CB.Fill.ToString(),
                                     DragDropEffects.Copy);*/
            }
        }
    }
}
