using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Risk.Model;
using Risk.Command;
using Risk.Serialization;

namespace Risk.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<Shape> Shapes { get; set; }

        public ObservableCollection<Line> Lines { get; set; }

        private UndoRedoController undoRedoController = UndoRedoController.Instance;


        private bool isAddingLine;
        // Used for saving the shape that a line is drawn from, while it is being drawn.
        private Shape addingLineFrom;

        public double ModeOpacity => isAddingLine ? 0.4 : 1.0;
        // Saves the initial point that the shape has during a move operation.
        private Point initialShapePosition;
        // Saves the initial point that the mouse has during a move operation.
        private Point initialMousePosition;
        private bool isDragging = false;

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
            ////if (IsInDesignMode)

            Shapes = new ObservableCollection<Shape>();
            Lines = new ObservableCollection<Line>();

            UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);

            AddShapeCommand = new RelayCommand(AddShape);
            AddLineCommand = new RelayCommand(AddLine);
            DeleteCommand = new RelayCommand(Delete);
            //RemoveLinesCommand = new RelayCommand<IList>(RemoveLines, CanRemoveLines);

            // The commands are given the methods they should use to execute, and find out if they can execute.
            /*
             * 
             * 
             * MouseDownShapeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownShape);
             * MouseMoveShapeCommand = new RelayCommand<MouseEventArgs>(MouseMoveShape);
             * MouseUpShapeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpShape);
             */
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
            undoRedoController.Clear();
        }

        private void LoadMap()
        {
            throw new NotImplementedException();
        }

        private void SaveMap()
        {
            throw new NotImplementedException();
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
            ////{

            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

        }

        private void AddLine()
        {
            isAddingLine = true;
            RaisePropertyChanged(() => ModeOpacity);
        }

        private void AddShape()
        {
            Random rand = new Random();
            undoRedoController.AddAndExecute(new AddShapeCommand(Shapes, new Shape(rand.Next(0, 500), rand.Next(0, 1000), 50, 50)));
            //undoRedoController.AddAndExecute(new AddShapeCommand(Shapes, new Shape(100,100,100,100)));
            //undoRedoController.AddAndExecute(new AddShapeCommand(Shapes, new Shape()));
            //Shapes[0].X = 3;
        }
        private void Delete()
        {
            throw new NotImplementedException();
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



        private void MouseDownShape(MouseButtonEventArgs e)//Mouse is pressed down
            //Maybe check for which button?

        {

            //CountryButton b = (CountryButton)sender;
            //            Canvas c = (Canvas)b.Parent;

            // Used for adding a Line.
            Console.WriteLine("Test: MouseDownShape happened");
            if (isAddingLine)
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
            //, RelativeSource={RelativeSource FindAncestor, AncestorType={x:Type Window}}
            // Because a MouseUp event has happened and a Line is currently being drawn, 
            //  the Shape that the Line is drawn from or to has been selected, and is here retrieved from the event parameters.
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //<<<<<<< HEAD
                // Now that it has been established that the Line adding operation has been completed succesfully by the user, 
                //  a Line is added using an 'AddLineCommand', with a new Line given between the two shapes chosen.
                //undoRedoController.AddAndExecute(new AddLineCommand(Lines, new Line() { From = addingLineFrom, To = shape }));
                // The property used for visually indicating that a Line is being Drawn is cleared, 
                //  so the View can return to its original and default apperance.
                //                    addingLineFrom.IsSelected = false;
                // The 'isAddingLine' and 'addingLineFrom' variables are cleared, 
                //  so the MainViewModel is ready for another Line adding operation.
                //                    isAddingLine = false;
                //                    addingLineFrom = null;
                // The property used for visually indicating which Shape has already chosen are choosen is cleared, 
                //  so the View can return to its original and default apperance.
                //                    RaisePropertyChanged(() => ModeOpacity);
                //=======
                Console.WriteLine("Test: Mouse in Downstate");
                var shape = (Risk.Model.Shape)(((FrameworkElement)e.MouseDevice.Target).DataContext); //TargetShape(e);

                // This checks if this is the first Shape chosen during the Line adding operation, 
                //  by looking at the addingLineFrom variable, which is empty when no Shapes have previously been choosen.
                // If this is the first Shape choosen, and if so, the Shape is saved in the AddingLineFrom variable.
                //  Also the Shape is set as selected, to make it look different visually.
                if (addingLineFrom == null) { addingLineFrom = shape; addingLineFrom.IsSelected = true; }
                // If this is not the first Shape choosen, and therefore the second, 
                //  it is checked that the first and second Shape are different.
                else if (addingLineFrom.Number != shape.Number)
                {
                    // Now that it has been established that the Line adding operation has been completed succesfully by the user, 
                    //  a Line is added using an 'AddLineCommand', with a new Line given between the two shapes chosen.
                    undoRedoController.AddAndExecute(new AddLineCommand(Lines, new Line() { From = addingLineFrom, To = shape }));
                    // The property used for visually indicating that a Line is being Drawn is cleared, 
                    //  so the View can return to its original and default apperance.
                    addingLineFrom.IsSelected = false;
                    // The 'isAddingLine' and 'addingLineFrom' variables are cleared, 
                    //  so the MainViewModel is ready for another Line adding operation.
                    isAddingLine = false;
                    addingLineFrom = null;
                    // The property used for visually indicating which Shape has already chosen are choosen is cleared, 
                    //  so the View can return to its original and default apperance.
                    RaisePropertyChanged(() => ModeOpacity);
                }
                //>>>>>>> refs/remotes/origin/master
            }
        }

        private void startDrag(MouseButtonEventArgs e)
        {
            // The Shape is gotten from the mouse event.
            var shape = TargetShape(e);
            // The mouse position relative to the target of the mouse event.
            var mousePosition = RelativeMousePosition(e);

            // When the shape is moved with the mouse, the MouseMoveShape method is called many times, 
            //  for each part of the movement.
            // Therefore to only have 1 Undo/Redo command saved for the whole movement, the initial position is saved, 
            //  during the start of the movement, so that it together with the final position, 
            //  from when the mouse is released, can become one Undo/Redo command.
            // The initial shape position is saved to calculate the offset that the shape should be moved.
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

            // Now that the Move Shape operation is over, the Shape is moved to the final position, 
            //  by using a MoveNodeCommand to move it.
            // The MoveNodeCommand is given the offset that it should be moved relative to its original position, 
            //  and with respect to the Undo/Redo functionality the Shape has only been moved once, with this Command.
            undoRedoController.AddAndExecute(new MoveShapeCommand(shape, mousePosition.X - initialMousePosition.X, mousePosition.Y - initialMousePosition.Y));

            // The mouse is released, as the move operation is done, so it can be used by other controls.
            e.MouseDevice.Target.ReleaseMouseCapture();
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