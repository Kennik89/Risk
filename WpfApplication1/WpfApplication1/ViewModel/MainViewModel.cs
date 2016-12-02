using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Risk.Model;
using Risk.Command;
using Risk.LoadSave;
using System.Threading;

namespace Risk.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<Shape> Shapes { get; set; }
        public ObservableCollection<Line> Lines { get; set; }

        // Enable to call the methods from those classes
        private _serializer _serializer = _serializer.Instance;
        private UndoRedoController _undoRedoController = UndoRedoController.Instance;

        private bool _isAddingLine;
        // Used for saving the shape that a line is drawn from, while it is being drawn.
        private Shape _addingLineFrom;

        public double ModeOpacity => _isAddingLine ? 0.4 : 1.0;
        // Saves the initial point that the shape has during a move operation.
        private Point initialShapePosition;
        // Saves the initial point that the mouse has during a move operation.
        private Point initialMousePosition;
        private bool isDragging = false;
        public Shape currentlySelected;


        /*  UNDO/REDO   */
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }

        /*  MAP FUNCTIONALITIES */
        public ICommand AddShapeCommand { get; }
        public ICommand AddLineCommand { get; }
        public ICommand DeleteCommand { get; }

        /*  MOUSE CONTROLLER    */
        public ICommand MouseDownShapeCommand { get; }
        public ICommand MouseMoveShapeCommand { get; }
        public ICommand MouseUpShapeCommand { get; }

        /*  FILE MENU CONTROLLER */
        public ICommand NewMapCommand { get; }
        public ICommand LoadMapCommand { get; }
        public ICommand SaveMapCommand { get; }
        public ICommand ExitCommand { get; }

        /*  GAME CONTROLLER */
        public ICommand StartCommand { get; }

        /* EDIT CONTROLLER */
        public ICommand CutCommand { get; }
        public ICommand CopyCommand { get; }
        public ICommand PasteCommand { get; }


        public MainViewModel()
        {

            Shapes = new ObservableCollection<Shape>();
            Lines = new ObservableCollection<Line>();

            UndoCommand = new RelayCommand(_undoRedoController.Undo, _undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(_undoRedoController.Redo, _undoRedoController.CanRedo);

            AddShapeCommand = new RelayCommand(AddShape);
            AddLineCommand = new RelayCommand(AddLine);
            DeleteCommand = new RelayCommand(Delete);
            
            MouseDownShapeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownShape);
            MouseMoveShapeCommand = new RelayCommand<MouseEventArgs>(MouseMoveShape);
            MouseUpShapeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpShape);

            NewMapCommand = new RelayCommand(NewMap);
            LoadMapCommand = new RelayCommand(LoadMap);
            SaveMapCommand = new RelayCommand(SaveMap);
            ExitCommand = new RelayCommand(Exit);
            StartCommand = new RelayCommand(StartMap);
            CutCommand = new RelayCommand(Cut);
            CopyCommand = new RelayCommand(Copy);
            PasteCommand = new RelayCommand(Paste);
        }

        private void NewMap()
        {
            Lines.Clear();
            Shapes.Clear();
            _undoRedoController.Clear();
        }

        private void LoadMap()
        {
            //NewMap();
            // TODO: Need to test first
            if (_serializer.Load(Shapes, Lines))
            {
                //Clearer undo-redo-stacken. Shapes og Lines bliver clearet i load,
                //Da disse ikke må cleares efter alt er loadet, men kun skal cleares
                //Hvis loaden er succesfuld.
                _undoRedoController.Clear();
            }
        }

        private void saveThread()
        {
            _serializer.Save(Shapes, Lines);
        }

        private void SaveMap()
        {
            // TODO: Need to test first
            try
            {
                ThreadStart save = new ThreadStart(saveThread);
                Thread s = new Thread(save);
                s.SetApartmentState(ApartmentState.STA);
                s.Start();
                /*Console.WriteLine("Main Thread waiting 10s");
                System.Threading.Thread.Sleep(10000);
                Console.WriteLine("Main Thread waited 10s");
                */
            }

            catch (SerializationException serExc)
            {
                Console.WriteLine("Serialization Failed");
                Console.WriteLine(serExc.Message);
            }
            catch (Exception exc)
            {
                Console.WriteLine(
                "The serialization operation failed: {0} StackTrace: {1}",
                exc.Message, exc.StackTrace);
            }

            finally
            {
                Console.WriteLine("Serization Successed");
            }
        }

        private void Exit()
        {
            throw new NotImplementedException();
        }

        private void StartMap()
        {
            throw new NotImplementedException();
        }

        private void Cut()
        {
            throw new NotImplementedException();
        }

        private void Copy()
        {
            throw new NotImplementedException();
        }

        private void Paste()
        {
            throw new NotImplementedException();

            //MouseDownShapeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownShape);
        }

        private void AddLine()
        {
            _isAddingLine = true;
            RaisePropertyChanged(() => ModeOpacity);
        }

        private void AddShape()
        {
            _undoRedoController.AddAndExecute(new AddShapeCommand(Shapes, new Shape()));
            //undoRedoController.AddAndExecute(new AddShapeCommand(Shapes, new Shape(100,100,100,100)));
        }
        private void Delete()
        {
            throw new NotImplementedException();

            //DELETE CURRENTLYSELECTED

        }

        private Line TargetLine(MouseEventArgs e)
        {
            // Here the visual element that the mouse is captured by is retrieved.
            var shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
            // From the shapes visual element, the Shape object which is the DataContext is retrieved.
            return (Line)shapeVisualElement.DataContext;
        }


        private Shape TargetShape(MouseEventArgs e)
        {
            // Here the visual element that the mouse is captured by is retrieved.
            var shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
            // From the shapes visual element, the Shape object which is the DataContext is retrieved.
            return (Shape)shapeVisualElement.DataContext;
        }
        /* public void CountryButton()
         {
             //Ensures that the event handler is there.
             AddHandler(FrameworkElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(MouseDownShape), true);
                   }*/

        private void MouseUpShape(MouseButtonEventArgs e)
        {
            if (isDragging)
            {
                endDrag(e);
                currentlySelected = TargetShape(e);
            }
        }

        private void MouseMoveShape(MouseEventArgs e)//If the mouse is getting moved
        {
            if (isDragging)
            {
                // The Shape is gotten from the mouse event.
                var shape = TargetShape(e);
                // The mouse position relative to the target of the mouse event.
                var mousePosition = RelativeMousePosition(e);

                // The Shape is moved by the offset between the original and current mouse position.
                // The View (GUI) is then notified by the Shape, that its properties have changed.
                shape.X = initialShapePosition.X + (mousePosition.X - initialMousePosition.X);
                shape.Y = initialShapePosition.Y + (mousePosition.Y - initialMousePosition.Y);
            }
        }




        public void MouseDownShape(MouseButtonEventArgs e)
        {

            //CountryButton b = (CountryButton)sender;
            //            Canvas c = (Canvas)b.Parent;

            // Used for adding a Line.
            Console.WriteLine("Test: MouseDownShape happened");
            if (_isAddingLine)
            {
                addLineClick(e);
            }
            // Used for moving a Shape.
            else
            {
                //Moving shape, if there is no line being added.
                startDrag(e);

            }
        }

        private void addLineClick(MouseButtonEventArgs e)//A click that adds a line
        {
    Console.WriteLine("Test: MouseDownShape happened and isAddingLine==true");
   if (e.LeftButton == MouseButtonState.Pressed)
    {
        Console.WriteLine("Test: Mouse in Downstate");
        var shape = (Risk.Model.Shape)(((FrameworkElement)e.MouseDevice.Target).DataContext); //TargetShape(e);

        if (_addingLineFrom == null) { _addingLineFrom = shape; _addingLineFrom.IsSelected = true; }
        // If this is not the first Shape choosen, and therefore the second, 
        //  it is checked that the first and second Shape are different.
        else if (_addingLineFrom.UID != shape.UID)
        {
            _undoRedoController.AddAndExecute(new AddLineCommand(Lines, new Line() { From = _addingLineFrom, To = shape }));
            _addingLineFrom.IsSelected = false;
                    // The 'isAddingLine' and 'addingLineFrom' variables are cleared
            _isAddingLine = false;
            _addingLineFrom = null;
            // The property used for visually indicating which Shape has already chosen are choosen is cleared, 
            //  so the View can return to its original and default apperance.
            RaisePropertyChanged(() => ModeOpacity);
        }
    }
}

        private void startDrag(MouseButtonEventArgs e)
        {
            // The Shape is gotten from the mouse event.
            var shape = TargetShape(e);
            // The mouse position relative to the target of the mouse event.
            var mousePosition = RelativeMousePosition(e);

            initialMousePosition = mousePosition;
            initialShapePosition = new Point(shape.X, shape.Y);

            // The mouse is captured, so the current shape will always be the target of the mouse events, 
            //  even if the mouse is outside the application window.
            e.MouseDevice.Target.CaptureMouse();

            isDragging = true;
        }

        private void endDrag(MouseButtonEventArgs e)
        {
            // The Shape is gotten from the mouse event.
            var shape = TargetShape(e);
            // The mouse position relative to the target of the mouse event.
            var mousePosition = RelativeMousePosition(e);
            // The Shape is moved back to its original position, so the offset given to the move command works.
            shape.X = initialShapePosition.X;
            shape.Y = initialShapePosition.Y;

            _undoRedoController.AddAndExecute(new MoveShapeCommand(shape, mousePosition.X - initialMousePosition.X, mousePosition.Y - initialMousePosition.Y));

            // The mouse is released, as the move operation is done, so it can be used by other controls.
            e.MouseDevice.Target.ReleaseMouseCapture();
            //Indicates that drag has ended.
            isDragging = false;
        }

        private Point RelativeMousePosition(MouseEventArgs e)
        {
            // Here the visual element that the mouse is captured by is retrieved.
            var shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
            // The canvas holding the shapes visual element, is found by searching up the tree of visual elements.
            var canvas = FindParentOfType<Canvas>(shapeVisualElement);
            // The mouse position relative to the canvas is gotten here.
            return Mouse.GetPosition(canvas);
        }

        private static T FindParentOfType<T>(DependencyObject o)
        {
            dynamic parent = VisualTreeHelper.GetParent(o);
            return parent.GetType().IsAssignableFrom(typeof(T)) ? parent : FindParentOfType<T>(parent);
        }
    }
}