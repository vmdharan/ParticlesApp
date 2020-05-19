using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ParticlesApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPageWin2D : Page
    {
        // Random number generator
        public static Random rand = new Random();

        // Define the particle structure
        public struct Particles
        {
            public double newLeft;
            public double newTop;
            public double hDir;
            public double vDir;
            public Color col1;
        }

        private const int MAXWIDTH = 1280;
        private const int MAXHEIGHT = 720;

        private Particles[] particles;          // Array of particles
        private const int maxParticles = 64;    // Number of particles to draw
        private float radius = 32.0f;           // Particle size

        // Store the width and height limits
        private int wComponent;
        private int hComponent;

        public MainPageWin2D()
        {
            this.InitializeComponent();
        }

        private void canvasWin2d_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            CanvasCommandList ccl = new CanvasCommandList(sender);
            using(CanvasDrawingSession cds = ccl.CreateDrawingSession())
            {
                // Loop over all the particles.
                for (int i = 0; i < maxParticles; i++)
                {
                    // Draw
                    cds.FillEllipse(
                        (float)particles[i].newLeft, (float)particles[i].newTop,
                        radius, radius, particles[i].col1
                    );
                }
            }

            GaussianBlurEffect gb = new GaussianBlurEffect();
            gb.Source = ccl;
            gb.BlurAmount = 5.0f;
            args.DrawingSession.DrawImage(gb);
        }

        private void canvasWin2d_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            // Loop over all the particles.
            for (int i = 0; i < maxParticles; i++)
            {
                // Update the horizontal position.
                particles[i].newLeft += particles[i].hDir;

                // If outside the bounds, then invert the direction of motion.
                if (particles[i].newLeft <= 0)
                {
                    particles[i].hDir = 1;
                }
                else if (particles[i].newLeft >= wComponent)
                {
                    particles[i].hDir = -1;
                }

                // Update the vertical position.
                particles[i].newTop += particles[i].vDir;

                // If outside the bounds, then invert the direction of motion.
                if (particles[i].newTop <= 0)
                {
                    particles[i].vDir = 1;
                }
                else if (particles[i].newTop >= hComponent)
                {
                    particles[i].vDir = -1;
                }
            }
        }

        private void canvasWin2d_CreateResources(CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            particles = new Particles[maxParticles];

            // Create an array of random colours.
            Color[] scb = new Color[10];

            scb[0] = Color.FromArgb(180, 124, 212, 85);
            scb[1] = Color.FromArgb(180, 124, 85, 212);
            scb[2] = Color.FromArgb(180, 85, 124, 212);
            scb[3] = Color.FromArgb(180, 212, 124, 85);
            scb[4] = Color.FromArgb(180, 212, 85, 124);
            scb[5] = Color.FromArgb(180, 85, 212, 124);
            scb[6] = Color.FromArgb(180, 24, 21, 85);
            scb[7] = Color.FromArgb(180, 24, 85, 21);
            scb[8] = Color.FromArgb(180, 85, 21, 24);
            scb[9] = Color.FromArgb(180, 85, 85, 124);

            // Set the x and y bounds.
            wComponent = MAXWIDTH - (int)radius;
            hComponent = MAXHEIGHT - (int)radius;

            // Loop over all the particles and draw them on the canvas.
            for (int i = 0; i < maxParticles; i++)
            {
                particles[i].newLeft = rand.Next(0, wComponent);
                particles[i].newTop = rand.Next(0, hComponent);
                particles[i].col1 = scb[rand.Next(0, 9)];

                //particles[i].hDir = rand.Next(1, 2) * -1;
                //particles[i].vDir = rand.Next(1, 2) * -1;

                particles[i].hDir = rand.Next(0, 2) == 0 ? -1 : 1;
                particles[i].vDir = rand.Next(0, 2) == 0 ? -1 : 1;
            }
        }
    }
}
