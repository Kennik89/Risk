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
    /// Interaction logic for CountryButton.xaml
    /// </summary>
    public partial class CountryButton : System.Windows.Controls.UserControl
    {
        private bool isDragging = false;
        public CountryButton()
        {
            InitializeComponent();
            //Ensures that the event handler is there.
            AddHandler(FrameworkElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(CountryButton_MouseLeftButtonDown), true);
            AddHandler(FrameworkElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(CountryButton_MouseLeftButtonUp), true);
            AddHandler(FrameworkElement.MouseMoveEvent, new MouseEventHandler(CountryButton_MouseMove), true);
        }


        private void CountryButton_MouseLeftButtonDown(object sender,//Hvad sker der, hvis der trykkes
            MouseButtonEventArgs e){

            isDragging = true;

            //throw new System.Exception("Button click");//Tests if event is thrown
            //CountryButton.startDragDrop(this, System.Windows.Forms.DragDropEffects.Move);
            e.Handled = true;//Marks that the evens has been handled
            }

/*        public delegate void MouseEventHandler(
            object sender,
            MouseEventArgs e
        );*/

        private void CountryButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            //throw new Exception();
            //Model.Shape s = (Model.Shape)sender;
            //s.X = 50;
        }

        private void CountryButton_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                throw new Exception();
            }
            return;
        }
    }


}
