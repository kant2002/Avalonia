using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using ReactiveUI;

namespace RenderDemo.Pages
{
    public class XDataGridHeaderCell : ContentControl
    {
        private Thumb _rightThumbResizer;
        internal ContentControl _contentControl;

        public XDataGridHeaderCell()
        {
            this.TemplateApplied += TemplateAppliedCore;
            this.Cursor = new Cursor(StandardCursorType.SizeWestEast);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var desc = (Content as XDataGridHeaderDescriptor);

            if (_contentControl != null || desc != null)
            {
                var content = (Content as XDataGridHeaderDescriptor);
                content.HeaderWidth = _contentControl.Bounds.Width;
            }

            return base.MeasureOverride(availableSize);

        }

        private void TemplateAppliedCore(object sender, TemplateAppliedEventArgs e)
        {

            this._contentControl = e.NameScope.Find<ContentControl>("PART_ContentControl");

            this._rightThumbResizer = e.NameScope.Find<Thumb>("PART_RightThumbResizer");

            var content = (Content as XDataGridHeaderDescriptor);
            content.HeaderWidth = _contentControl.Bounds.Width;


            if (_rightThumbResizer == null) return;
            this._rightThumbResizer.DragDelta += ResizerDragDelta;
            this._rightThumbResizer.DragStarted += ResizerDragStarted;


        }

        private void ResizerDragStarted(object sender, VectorEventArgs e)
        {
            if (Double.IsNaN(_contentControl.Width))
            {
                _contentControl.Width = _contentControl.Bounds.Width;
            }
        }

        private void ResizerDragDelta(object sender, VectorEventArgs e)
        {
            var newW = _contentControl.Width + e.Vector.X;

            if (newW <= 0) return;

            _contentControl.Width = newW;
        }
    }
}