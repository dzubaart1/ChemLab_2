using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngineerLab.Activities;
using BioEngineerLab.UI.Components;
using UnityEngine;

namespace BioEngineerLab.Core
{
    public class UIComponentsService : IService
    {
        private List<SliderComponent> _sliders;
        private List<DragLine> _dragLines;

        public Task Initialize()
        {
            _sliders = new List<SliderComponent>();
            _dragLines = new List<DragLine>();
            
            return Task.CompletedTask;
        }

        public void Destroy()
        {
        }
        
        public void RegisterDragLine(DragLine dragLine)
        {
            _dragLines.Add(dragLine);
        }

        public DragLine GetDragLineByType(DragLineType dragLineType)
        {
            foreach (var dragLine in _dragLines)
            {
                if (dragLine.DragLineType == dragLineType)
                {
                    return dragLine;
                }
            }

            return null;
        }

        public void RegisterSlider(SliderComponent sliderComponent)
        {
            _sliders.Add(sliderComponent);
        }

        public SliderComponent GetSliderByType(SliderType sliderType)
        {
            foreach (var slider in _sliders)
            {
                if (slider.SliderType == sliderType)
                {
                    return slider;
                }
            }

            return null;
        }
    }
}