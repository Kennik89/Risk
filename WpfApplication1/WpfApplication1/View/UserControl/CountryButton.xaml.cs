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
using System.Windows.Interactivity;
using System.Windows.Shapes;

namespace Risk.View.UserControl
{
    /// <summary>
    /// Interaction logic for CountryButton.xaml
    /// </summary>
    public partial class CountryButton : System.Windows.Controls.UserControl
    {

        //Implementing drag of a button
        //This is done as in Exercises
        //Defines dependencyproperty
        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        private static void OnXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var db = d as CountryButton;
            if (db == null) return;
            Canvas.SetLeft((CountryButton)d, (double)e.NewValue);
        }

        private static void OnYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var db = d as CountryButton;
            if (db == null) return;
            Canvas.SetLeft((CountryButton)d, (double)e.NewValue);
        }

        //X
        public static DependencyProperty XProperty =
    DependencyProperty.Register("X", typeof(double), typeof(CountryButton), new FrameworkPropertyMetadata(0d, OnXChanged) { BindsTwoWayByDefault = true });

        //Y
        public static DependencyProperty YProperty =
    DependencyProperty.Register("Y", typeof(double), typeof(CountryButton), new FrameworkPropertyMetadata(0d, OnYChanged) { BindsTwoWayByDefault = true });



        private Risk.Model.Shape currentTarget = null;
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
            MouseButtonEventArgs e)
        {

            CountryButton b = (CountryButton)sender;
            Canvas c = (Canvas)b.Parent;

            isDragging = true;

            currentTarget = (Risk.Model.Shape)(((FrameworkElement)e.MouseDevice.Target).DataContext);

            //As inspired by the following:
            /*
            // Here the visual element that the mouse is captured by is retrieved.
            //Should only be set when the element is being dropped. A dependencyobject should be set while dragging.
            //var shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
            // From the shapes visual element, the Shape object which is the DataContext is retrieved.
            //Risk.Model.Shape s = (Risk.Model.Shape)shapeVisualElement.DataContext;
            */


            e.Handled = true;//Marks that the evens has been handled
        }

        /*        public delegate void MouseEventHandler(
                    object sender,
                    MouseEventArgs e
                );*/

        private void CountryButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;

            currentTarget.X = X;//Sets the values of the target shape
            currentTarget.Y = Y;//See above.


            //throw new Exception();
            //Model.Shape s = (Model.Shape)sender;
            //s.X = 50;
        }

        private void CountryButton_MouseMove(object sender, MouseEventArgs e)
        {

            if (isDragging)
            {
                var screenpos = e.GetPosition(this);//This may be unsafe according to msdn.
                //var pos = c.PointFromScreen(screenpos);
    
                //TODO Make boolean check of bounds.
                //TODO Make the grabbing of the point on the object that there is being dragged (Offset).

                X = screenpos.X;
                Y = screenpos.Y;
                Canvas.SetLeft(this, X);
                Canvas.SetBottom(this, Y);
                e.Handled = true;//Trace works, values are set. Must be set to actual object.
            }
            return;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
