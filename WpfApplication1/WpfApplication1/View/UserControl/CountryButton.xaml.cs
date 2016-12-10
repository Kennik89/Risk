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
using Shape = Model.Shape;

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
            DependencyObject parent = VisualTreeHelper.GetParent(db);//Is a contentpresenter
            //Canvas c = (Canvas)VisualTreeHelper.GetParent(parent);//Should really really be a Canvas.
            Canvas.SetLeft(db, (double)e.NewValue);
        }

        private static void OnYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var db = d as CountryButton;
            if (db == null) return;
            DependencyObject parent = VisualTreeHelper.GetParent(db);//Contentpresenter
            //Canvas c = (Canvas)VisualTreeHelper.GetParent(parent);//Should really really be a Canvas.
            Canvas.SetTop(db, (double)e.NewValue);
            //throw new Exception();
        }

        //X
        public static DependencyProperty XProperty =
    DependencyProperty.Register("X", typeof(double), typeof(CountryButton), new FrameworkPropertyMetadata(0d, OnXChanged) { BindsTwoWayByDefault = true });

        //Y
        public static DependencyProperty YProperty =
    DependencyProperty.Register("Y", typeof(double), typeof(CountryButton), new FrameworkPropertyMetadata(0d, OnYChanged) { BindsTwoWayByDefault = true });


        Binding xBind = new Binding("X");
        Binding yBind = new Binding("Y");
        private Point grabOffset;
        private Shape target = null;
        private bool isDragging = false;
        public CountryButton()
        {
            InitializeComponent();
/*            target = (Risk.Model.Shape)this.DataContext;
            //Ensures that the event handler is there.
            AddHandler(FrameworkElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(CountryButton_MouseLeftButtonDown), true);
            AddHandler(FrameworkElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(CountryButton_MouseLeftButtonUp), true);
            AddHandler(FrameworkElement.MouseMoveEvent, new MouseEventHandler(CountryButton_MouseMove), true);

            DataContextChanged += new DependencyPropertyChangedEventHandler(CountryButton_DataContextChanged);
*/            //Datacontextchanged is from stackoverflowechange
        }

        void CountryButton_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
            //Reads from e and changes the data context.
            //Should work, MSDN.
            target = (Shape) e.NewValue;

            X = target.X;
            Y = target.Y;
        }

    private void CountryButton_MouseLeftButtonDown(object sender,//Hvad sker der, hvis der trykkes
            MouseButtonEventArgs e)
        {

            Point thisLoc = e.GetPosition(this);
            grabOffset.X = thisLoc.X - X;
            grabOffset.Y = thisLoc.Y - Y;
            //mousePos.X = System.Windows.Forms.Cursor.Position.X;
            //mousePos.Y = System.Windows.Forms.Cursor.Position.Y;

            CountryButton b = (CountryButton)sender;
            Canvas c = (Canvas)b.Parent;
            
            isDragging = true;





            //currentTarget = (Risk.Model.Shape)(((FrameworkElement)e.MouseDevice.Target).DataContext);

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

            target.X = X;//Sets the values of the target shape
            target.Y = Y;//See above.

            //Canvas.Top = "{Binding Y, Mode=TwoWay, UpdateSourceTrigger = PropertyChanged}"
            //Canvas.Left = "{Binding X, Mode=TwoWay, UpdateSourceTrigger = PropertyChanged}"

            //throw new Exception();
            //Model.Shape s = (Model.Shape)sender;
            //s.X = 50;
        }

        private void CountryButton_MouseMove(object sender, MouseEventArgs e)
        {

            if (isDragging)
            {
                Point screenpos = e.GetPosition(this);//This may be unsafe according to msdn.
                                                      //var pos = c.PointFromScreen(screenpos);
                                                      //TODO Make boolean check of bounds.
                                                      //TODO Make the grabbing of the point on the object that there is being dragged (Offset).

                //SOMEHOW SHOW ANIMATION
                //SOMEHOW EITHER MAKE CANVAS GRAB THIS X OR SET THIS X TO CANVAS WITHOUT JUST UPDATING TARGET
                //target.X = X;//Sets the values of the target shape
                //target.Y = Y;//See above.
                screenpos.X -= grabOffset.X;
                screenpos.Y -= grabOffset.Y;
                Y = screenpos.Y;
                X = screenpos.X;
                target.X = X;
                target.Y = Y;
                Canvas.SetTop(this, Y);
                Canvas.SetLeft(this, X);
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
