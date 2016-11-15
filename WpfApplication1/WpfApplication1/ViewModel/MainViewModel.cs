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

namespace Risk.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<Shape> Shapes { get; set; }

        public ObservableCollection<Line> Lines { get; set; }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            Shapes = new ObservableCollection<Shape>() {
                new Shape() { X = 0, Y = 0, Width = 80, Height = 80 },
                new Shape() { X = 200, Y = 200, Width = 100, Height = 100 }
            };
            Lines = new ObservableCollection<Line>() {
                new Line() { From = Shapes.ElementAt(0), To = Shapes.ElementAt(1) }
            };
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }
    { 
        private UndoRedoController undoRedoController = UndoRedoController.Instance;
    
        /*
         * public ObservableCollection<Shape> Shapes { get; set; }
        /*
         * public ObservableCollection<Line> Lines { get; set; }
        */
        // Commands that the UI can be bound to.
        // These are read-only properties that can only be set in the constructor.
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }

        // Commands that the UI can be bound to.
        public ICommand AddShapeCommand { get; }
        public ICommand RemoveShapeCommand { get; }
        public ICommand AddLineCommand { get; }
        public ICommand RemoveLinesCommand { get; }

        public MainViewModel()
        {
            // Here the list of Shapes is filled with 2 Nodes. 
            // The "new Type() { prop1 = value1, prop2 = value }" syntax is called an Object Initializer, which creates an object and sets its values.

            // Also a constructor could be created for the Shape class that takes the parameters (X, Y, Width and Height), 
            //  and the following could be done:
            // new Shape(30, 40, 80, 80);
            /*
             * Shapes = new ObservableCollection<Shape>() {
             *     new Shape() { X = 30, Y = 40, Width = 80, Height = 80 },
             *     new Shape() { X = 140, Y = 230, Width = 100, Height = 100 }
             * };
             */
            // Here the list of Lines i filled with 1 Line that connects the 2 Shapes in the Shapes collection.
            // ElementAt() is an Extension Method, that like many others can be used on all types of collections.
            // It works just like the "Shapes[0]" syntax would be used for arrays.
            /*
             * Lines = new ObservableCollection<Line>() {
             *   new Line() { From = Shapes.ElementAt(0), To = Shapes.ElementAt(1) }
             *};
             */
            // The commands are given the methods they should use to execute, and find out if they can execute.
            // For these commands the methods are not part of the MainViewModel, but part of the UndoRedoController.
            // Her vidersendes metode kaldne til UndoRedoControlleren.
            UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);

            // The commands are given the methods they should use to execute, and find out if they can execute.
            /*
             * AddShapeCommand = new RelayCommand(AddShape);
             * RemoveShapeCommand = new RelayCommand<IList>(RemoveShape, CanRemoveShape);
             * AddLineCommand = new RelayCommand(AddLine);
             * RemoveLinesCommand = new RelayCommand<IList>(RemoveLines, CanRemoveLines);
             */

            // The commands are given the methods they should use to execute, and find out if they can execute.
            /*
             * MouseDownShapeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownShape);
             * MouseMoveShapeCommand = new RelayCommand<MouseEventArgs>(MouseMoveShape);
             * MouseUpShapeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpShape);
             */    
    }

    }
}