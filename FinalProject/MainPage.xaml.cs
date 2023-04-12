using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FinalProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
         
        }
        public MainPage()
        {
            this.InitializeComponent();

            Window.Current.CoreWindow.KeyDown += Canvas_KeyDown;
            Window.Current.CoreWindow.KeyUp += Canvas_KeyUp;
        }

        Tank tank;
        private CanvasBitmap tankimage;
        private void Canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            tank.Draw(args.DrawingSession);
        }

        private void Canvas_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            tank.Update();
        }
        private void Canvas_CreateResources(CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResources(sender).AsAsyncAction());
        }

        async Task CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl sender)
        {
            tankimage = await CanvasBitmap.LoadAsync(sender, "Assets/demoTank.png");
            tank = new Tank(50,50,20,tankimage);
        }



        
        private void Canvas_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs e)
        {
            if (e.VirtualKey == Windows.System.VirtualKey.Left)
            {
                tank.TravelingLeftward= true;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Right)
            {
                tank.TravelingRightward = true;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Up)
            {
                tank.TravelingUpward = true;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Down)
            {
                tank.TravelingDownward = true;
            }
        }

        private void Canvas_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs e)
        {
            if (e.VirtualKey == Windows.System.VirtualKey.Left)
            {
                tank.TravelingLeftward = false;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Right)
            {
                tank.TravelingRightward = false;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Up)
            {
                tank.TravelingUpward = false;
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Down)
            {
                tank.TravelingDownward = false;
            }
        }

        
    }
}
